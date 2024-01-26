// Ignore Spelling: Fiserv

using EdiMonthlyReportGenerator.Models;
using EdiMonthlyReportGenerator.Services.Implements;
using EdiMonthlyReportGenerator.Services.Interfaces;
using EDIMonthlyReportGenerator.Models;
using Serilog;

namespace EdiMonthlyReportGenerator.Strategies
{
    /// <summary>
    /// Abstract base class for EdiReportBase output file format.
    /// Can be inherited by different implementations in future
    /// Schedule: 6.30 am, 21st day (or next business day) of every month
    /// On-demand run allowed based on config key
    /// </summary>
    public abstract class EdiReportBase : IGenerator
    {
        #region Field(s)
        public abstract string OutputFileType { get; }
        private ILogger _logger;
        private string _inputFileWithPath;
        private string? rejectedFilePath;
        private IConfigurationService _configurationService;
        protected readonly IDataWarehouseService _dataWarehouseService;
        private IEmailService emailService;
        private ITextFileService textFileService;
        private IFileSystemService fileSystemService;
        public List<ISAHeader> isaHeaders = new();
        public ISAHeader isaHeader = new();
        public List<EDIRecord> ediRecords = new();
        public EDIRecord ediRecord;
        #endregion

        #region Constructor(s)   
        protected EdiReportBase(IConfigurationService configurationService, IDataWarehouseService dataWarehouseService, IEmailService emailService, ITextFileService textFileService, IFileSystemService fileSystemService)
        {
            _configurationService = configurationService;
            _dataWarehouseService = dataWarehouseService;
            this.emailService = emailService;
            this.textFileService = textFileService;
            this.fileSystemService = fileSystemService;
            _logger = Log.ForContext(nameof(OutputFileType), OutputFileType); ;
        }
        #endregion

        #region Public Method(s)
        public Result Generate(OutputFileProperties outputFileProperties)
        {
            Result result = new();
            var currentDate = DateTime.UtcNow.AddHours(-8);
            var outputFileWithPath = string.Empty;
            long fileSizeInBytes = 0;
            List<string>? inputRecords;

            try
            {
                if (_configurationService.AppSettings.IsManualTrigger)
                {
                    // Proceed with execution without checking business day logic.
                    _logger.Information("Manual trigger enabled. Ignoring business day check.");
                }
                else // Scheduled trigger. Run only if its 21st business day of month
                {
                    var businessDay = _dataWarehouseService.IsBusinessDay(currentDate);
                    _logger.Information("Manual trigger disabled. {currentDate}, {businessDay}", currentDate, businessDay);
                    if (!businessDay)
                    {
                        result.Success = false;
                        result.Message += $"Not generating output file because execution date ({currentDate:MM/dd/yyyy}) is not a business day of the month ({currentDate:MM/dd/yyyy})";

                        SendEmailNotification(result, $"{currentDate:MMM dd, yyyy HH:mm:ss}", outputFileProperties);
                        _logger.Information(result.Message);
                        return result;
                    }
                    else if (businessDay)
                    {
                        result.Success = true;
                    }

                }

                var validationResult = Validate(outputFileProperties, currentDate);

                if (validationResult.Success == true)
                {
                    inputRecords = textFileService.ReadTextFile<EDIRecord>(_inputFileWithPath);
                    _logger.Information("Found {count} records in {file}.", inputRecords, _inputFileWithPath);

                    if (inputRecords.Any())
                    {
                        // Replace YYYY and MM in input file path with year and month of previous month            
                        var previousMonthDate = currentDate.AddMonths(-1);

                        var outputFilePath = outputFileProperties.OutputFileBasePath;
                        foreach (var record in inputRecords)
                        {
                            if (record.StartsWith("ISA"))
                            {
                                ediRecord = textFileService.MapSegmentValues(record);
                                result = ValidateEdiFile(ediRecord);

                                if (!result.Success)
                                {
                                    result.Success = false;
                                    var NotProcessedFilePath = outputFileProperties.NotProcessedBasePath;
                                    if (!fileSystemService.DirectoryExists(NotProcessedFilePath))
                                        fileSystemService.CreateDirectory(NotProcessedFilePath);

                                    rejectedFilePath = _inputFileWithPath.Replace(outputFileProperties.InputFileBasePath, outputFileProperties.NotProcessedBasePath);
                                    File.Move(_inputFileWithPath, rejectedFilePath);

                                    isaHeaders.Add(ediRecord.ISAHeader);
                                    SendEmailNotification(result, $"{currentDate:MMM dd, yyyy HH:mm:ss}", isaHeaders);
                                    return result;
                                }

                                if (!fileSystemService.DirectoryExists(outputFilePath))
                                    fileSystemService.CreateDirectory(outputFilePath);

                                var outputFileName = outputFileProperties.OutputFileName
                                                    .Replace("ReceiverID", ediRecord.ISAHeader.ReceiverId.Trim().ToLower())
                                                    .Replace("yyMMddHHmmss", currentDate.ToString("yyMMddHHmmss"));


                                outputFileWithPath = fileSystemService.CombinePath(outputFilePath, outputFileName);

                                if (!File.Exists(outputFileWithPath))
                                {
                                    File.Create(outputFileWithPath).Close();
                                }
                                else
                                {
                                    outputFileWithPath = string.Format(outputFileWithPath + currentDate.ToString("ffff"));
                                    File.Create(outputFileWithPath).Close();
                                }

                                textFileService.WriteTextFile(outputFileWithPath, record);
                                fileSizeInBytes = fileSystemService.GetFileLength(outputFileWithPath);
                                isaHeader.OutputFilePath = outputFileWithPath;
                                isaHeader.FileSizeInBytes = fileSizeInBytes;
                                isaHeader.OutPutRecords = record.Split("~").Length;
                                ediRecord.ISAHeader = isaHeader;
                            }
                        }

                        var ArchivedFileWithPath = _inputFileWithPath.Replace(outputFileProperties.InputFileBasePath, outputFileProperties.ArchiveFileBasePath);
                        File.Move(_inputFileWithPath, ArchivedFileWithPath);

                        result.Success = true;
                        result.Message += $"Successfully created billing file ({inputRecords.Count} records) - {fileSizeInBytes} bytes. {outputFileWithPath}";
                    }
                    else
                    {
                        rejectedFilePath = _inputFileWithPath.Replace(outputFileProperties.InputFileBasePath, outputFileProperties.NotProcessedBasePath);
                        File.Move(_inputFileWithPath, rejectedFilePath);
                        result.Success = false;
                        result.Message += "Invalid data in the document/either ISA,IEA segments are missing.";
                        SendEmailNotification(result, $"{currentDate:MMM dd, yyyy HH:mm:ss}", outputFileProperties);
                    }
                }
                else
                {
                    result = validationResult;
                }
                isaHeaders.Add(isaHeader);
                SendEmailNotification(result, $"{currentDate:MMM dd, yyyy HH:mm:ss} PT", isaHeaders);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message += $"Exception: {ex.Message}";
                SendEmailNotification(result, $"{currentDate:MMM dd, yyyy HH:mm:ss} PT", isaHeaders);
            }

            return result;
        }

        public Result Validate(OutputFileProperties outputFileProperties, DateTime currentDate)
        {
            var result = new Result();

            if (string.IsNullOrWhiteSpace(outputFileProperties.InputFileBasePath) || string.IsNullOrWhiteSpace(outputFileProperties.InputFileName))
            {
                result.Success = false;
                result.Message += "InputFileBasePath or InputFileName not provided.";
                return result;
            }

            if (string.IsNullOrWhiteSpace(outputFileProperties.OutputFileBasePath) || string.IsNullOrWhiteSpace(outputFileProperties.OutputFileName))
            {
                result.Success = false;
                result.Message += "OutputFileBasePath or OutputFileName not provided.";
                return result;
            }

            if (!fileSystemService.DirectoryExists(outputFileProperties.OutputFileBasePath))
            {
                result.Success = false;
                result.Message += $"OutputFileBasePath:{outputFileProperties.OutputFileBasePath} not found or cannot be accessed.";
                return result;
            }

            //// Replace YYYY and MM in input file path with year and month of previous month            
            //var previousMonthDate = currentDate.AddMonths(-1);

            var inputFilePath = fileSystemService.CombinePath(outputFileProperties.InputFileBasePath);
            if (!fileSystemService.DirectoryExists(inputFilePath))
            {
                result.Success = false;
                result.Message += $"InputFilePath:{inputFilePath} not found or cannot be accessed";
                return result;
            }

            var inputFileNamePattern = outputFileProperties.InputFileName.ToUpper()
                                .Replace("YY", currentDate.ToString("yy"))
                                .Replace("MM", currentDate.ToString("MM"))
                                .Replace("DD", "??"); // YY and MM for the file name is current date's. Ignore the DD part

            // Get a list of files that match the pattern in the folder
            string[] matchingFiles = fileSystemService.GetFilesFromDirectory(inputFilePath, inputFileNamePattern);

            // TODO : Handle if there are multiple files matching the pattern if required. Per BU, its not a possibility today. So pick up the first one
            if (matchingFiles.Length > 0)
            {
                _inputFileWithPath = matchingFiles[0];
                _logger.Information("{InputFileNamePattern}, {MatchingFileCount}, {InputFileWithPath}", inputFileNamePattern, matchingFiles.Length, _inputFileWithPath);
            }
            else
            {
                result.Success = false;
                result.Message += $"Input file not found. Pattern: {inputFileNamePattern}, Path: {inputFilePath}";
                return result;
            }

            if (!IsFileReady(_inputFileWithPath))
            {
                result.Success = false;
                result.Message += $"Input file:{_inputFileWithPath} not ready for reading even after configured wait time.";
                return result;
            }

            result.Success = true;
            result.Message = "Validations were successful. ";
            return result;
        }

        #endregion

        public Result ValidateEdiFile(EDIRecord ediRecord)
        {
            var result = new Result();

            if (ediRecord.ISAHeader == null)
            {
                result.Success = false;
                result.Message += "ISA segment is not valid";
                return result;
            }
            else if (ediRecord.ISAHeader.SegmentLength < 17)
            {
                result.Success = false;
                result.Message += "Insufficient elements in ISA segment";
                return result;
            }
            else if (ediRecord.ISAHeader.InterchangeControlStandardsId != "U")
            {
                result.Success = false;
                result.Message += "Invalid Interchange Control Standards Id\n";
                return result;
            }
            else if (ediRecord.ISAHeader.AcknowledgmentRequested != "0" && ediRecord.ISAHeader.AcknowledgmentRequested != "1")
            {
                result.Success = false;
                result.Message += "Invalid Acknowledgment Requested value\n";
                return result;
            }
            else if (!IsNumeric(ediRecord.ISAHeader.InterchangeControlNumber) || ediRecord.ISAHeader.InterchangeControlNumber.Length != 9)
            {
                result.Success = false;
                result.Message += "Invalid Interchange Control Number\n";
                return result;
            }
            else if (ediRecord.ISAHeader.InterchangeControlNumber != ediRecord.IeaTrailer.InterchangeControlNumber)
            {
                result.Success = false;
                result.Message = "ISA Interchange control number and IEA Interchange control number are not matching ";
                return result;
            }


            result.Success = true;
            result.Message = "Edi file header validations were successful\n";
            return result;

        }


        //public Result  ValidateEdiFile(string filePath)
        //{
        //    Result result = new Result();
        //    // Read the EDI file
        //    string ediContent = File.ReadAllText(filePath);

        //    // Split the EDI content into blocks
        //    string[] blocks = ediContent.Split(new[] { "ISA" }, StringSplitOptions.RemoveEmptyEntries);

        //    // Validate each block
        //    foreach (string block in blocks) // Skip the first element as it might be empty or incomplete
        //    {
        //        ValidateBlock(block);
        //    }

        //    result.Success = true;
        //    result.Message += "Validation complete.";
        //    return result;
        //}
        //private Result  ValidateBlock(string block)
        //{    
        //    Result result = new Result();
        //    // Split the block into segments
        //    string[] segments = block.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        //    // Validate each segment in the block
        //    foreach (string segment in segments)
        //    {
        //         result = ValidateSegment(segment);
        //    }

        //    return result;
        //}
        //private Result ValidateSegment(string segment)
        //{ Result result = new Result();
        //    // Define regular expressions for segment and element validation
        //    Regex segmentRegex = new Regex(@"^[A-Z]{2,3}$");  // Assuming segments are two or three uppercase letters
        //    Regex elementRegex = new Regex(@"^[A-Za-z0-9]*$");  // Adjust as per your specifications

        //    // Check segment format
        //    if (!segmentRegex.IsMatch(segment))
        //    {
        //        result.Success = false;
        //        result.Message ="Invalid segment format: {segment}";

        //    }

        //    // Split the segment into elements
        //    string[] elements = segment.Split('*');

        //    // Validate element count
        //    if (elements.Length != ExpectedElementCount(segment))
        //    {
        //        Console.WriteLine($"Invalid element count for segment {segment}: {elements.Length}");
        //    }

        //    // Validate each element
        //    for (int i = 0; i < elements.Length; i++)
        //    {
        //        if (!elementRegex.IsMatch(elements[i]))
        //        {
        //            Console.WriteLine($"Invalid element format at position {i + 1} in segment {segment}: {elements[i]}");
        //        }
        //    }

        //    // Additional segment-specific validations can be added based on the EDI 822 specification
        //    // For example, checking specific data values, code sets, etc.
        //    ValidateSegmentSpecificRules(segment, elements);

        //    return result;
        //}
        //private static void ValidateSegmentSpecificRules(string segment, string[] elements)
        //{
        //    // Example: Validate specific rules for the BIA segment
        //    if (segment == "BIA")
        //    {
        //        // Implement BIA segment-specific validations here
        //    }
        //}
        //private static int ExpectedElementCount(string segment)
        //{
        //    // This is just an example, you should replace it with actual counts based on your EDI 822 specification
        //    switch (segment)
        //    {
        //        case "BIA":
        //            return 5;
        //        case "CUR":
        //            return 3;
        //        // Add more cases for other segments
        //        default:
        //            return 0; // Unknown segment, handle accordingly
        //    }
        //}

        private bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }

        #region Protected Abstract Method(s)
        protected abstract IEnumerable<EDIRecord> FilterRecords(List<string> inputRecords, OutputFileProperties outputFileProperties, DateTime currentDate);
        #endregion


        static DateTime ConvertToPacificTime(DateTime utcDateTime)
        {
            // Specify the time zone identifier for Pacific Time (US and Canada)
            string timeZoneId = "Pacific Standard Time";

            // Get the TimeZoneInfo object for Pacific Time
            TimeZoneInfo pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            // Convert the UTC time to Pacific Time
            DateTime pacificDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, pacificTimeZone);

            return pacificDateTime;
        }

        #region Private Method(s)
        private bool IsFileReady(string fileName)
        {

            var result = false;
            var maxAttempt = _configurationService.AppSettings.FileReadyWaitMaxAttempt;
            var waitSecs = _configurationService.AppSettings.FileReadyWaitMaxAttempt;
            // Check if file is ready. Wait for some (as specified in config) if its the process of being written into the folder
            for (int i = 0; i < maxAttempt; i++)
            {
                try
                {
                    if (fileSystemService.FileExists(fileName))
                    {
                        long length = fileSystemService.GetFileLength(fileName);
                        result = length > 0;
                        if (result)
                        {
                            break; // Exit the loop if the file is ready
                        }
                    }
                }
                catch //(Exception ex)
                {
                    Thread.Sleep(waitSecs * 1000);
                }
            }

            return result;
        }

        private void SendEmailNotification(Result result, string dateTime, OutputFileProperties outputFileProperties)
        {
            var infoRows = new List<InfoRow>();

            var emailSubject = $"WARNING!!  (STG) Monthly {OutputFileType} File – Failed Generation";
            infoRows.Add(new InfoRow("Input File path", _inputFileWithPath));
            infoRows.Add(new InfoRow("Failed Reason", result.Message));

            emailService.SendEmail(emailSubject, infoRows, dateTime, result.Success);
        }


        private void SendEmailNotification(Result result, string dateTime, List<ISAHeader> headers)
        {
            var infoRows = new List<InfoRow>();
            Result? sendEmailResponse = new();
            string emailSubject = string.Empty;
            if (headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    if (result.Success) // Compose success email
                    {
                        emailSubject = $"Monthly {OutputFileType}  File – Successful File Generation";
                        infoRows.Add(new InfoRow("Input File Name", _inputFileWithPath));
                        infoRows.Add(new InfoRow("Company ID", header.ReceiverId));
                        infoRows.Add(new InfoRow("Interchange Control Number", header.InterchangeControlNumber));
                        infoRows.Add(new InfoRow("File Name", header.OutputFilePath));
                        infoRows.Add(new InfoRow("Output File Type", OutputFileType));
                        infoRows.Add(new InfoRow("Output File Size", header.FileSizeInBytes.ToString()));
                        infoRows.Add(new InfoRow("Upload Method", "s/FTP "));
                        infoRows.Add(new InfoRow("Total Number of output Records", header.OutPutRecords.ToString()));

                    }
                    else // Compose failure email
                    {
                        emailSubject = $"WARNING!!  (STG) Monthly {OutputFileType} File – Failed Generation";
                        infoRows.Add(new InfoRow("Company ID", header.ReceiverId));
                        infoRows.Add(new InfoRow("Interchange Control Number", header.InterchangeControlNumber));
                        infoRows.Add(new InfoRow("Input File Name", _inputFileWithPath));
                        infoRows.Add(new InfoRow("Rejected File path", header.RejectedFilePath));
                        infoRows.Add(new InfoRow("Output File Name", "N/A"));
                        infoRows.Add(new InfoRow("Output File Type", OutputFileType));
                        infoRows.Add(new InfoRow("Failed Reason", result.Message));
                    }

                    if (sendEmailResponse.Success)
                    {
                        sendEmailResponse.Message = result.Message;
                        _logger.Information("SendEmail successful {@message}", sendEmailResponse.Message);
                    }
                    else
                    {
                        sendEmailResponse.Message = result.Message;
                        _logger.Error("SendEmail failed {@message}", sendEmailResponse.Message);
                    }
                }
                emailService.SendEmail(emailSubject, infoRows, dateTime, result.Success);
            }

            #endregion
        }
    }
}

