using EdiMonthlyReportGenerator.Models;
using EdiMonthlyReportGenerator.Services.Interfaces;
using EDIMonthlyReportGenerator.Models;
using EDIMonthlyReportGenerator.Record;
using System.Text;

namespace EdiMonthlyReportGenerator.Services
{
    public class TextFileService : ITextFileService
    {
        public void WriteTextFile(string filePath, string data)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(data);
            }
        }

        public List<string> ReadTextFile<T>(string filePath)
        {
            string ediContent = File.ReadAllText(filePath);
            return ExtractIsaToIeaBlocks(filePath);
        }
        public static List<string> ExtractIsaToIeaBlocks(string filePath)
        {
            List<string> isaToIeaBlocks = new();
            bool isRecording = false;
            StringBuilder blockBuilder = new();

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("ISA"))
                {
                    isRecording = true; // Start recording when ISA is found
                    blockBuilder.Clear(); // Clear previous content
                }

                if (isRecording)
                {
                    blockBuilder.AppendLine(line); // Add line to the current block

                    if (line.StartsWith("IEA"))
                    {
                        isaToIeaBlocks.Add(blockBuilder.ToString()); // Add the block to the list
                        isRecording = false; // Reset for the next block
                    }
                }
            }

            return isaToIeaBlocks;
        }

        public EDIRecord MapSegmentValues(string block)
        {
            EDIRecord result = new EDIRecord();
            if (!string.IsNullOrEmpty(block))
            {
                var elements = block.Split('~');
                foreach (var element in elements)
                {
                    var segments = element.Split("*");
                    if (element.StartsWith("ISA"))
                    {
                        ISAHeader iSAHeader = new ISAHeader()
                        {
                            SegmentIdentifier = segments[0],
                            AuthorizationInformationQualifier = segments[1],
                            AuthorizationInformation = segments[2],
                            SecurityInformationQualifier = segments[3],
                            SecurityInformation = segments[4],
                            InterchangeSenderIDQualifier = segments[5],
                            InterchangeSenderId = segments[6],
                            InterchangeReceiverIDQualifier = segments[7],
                            ReceiverId = segments[8],
                            InterchangeDate = segments[9],
                            InterchangeTime = segments[10],
                            InterchangeControlStandardsId = segments[11],
                            InterchangeControlVersionNumber = segments[12],
                            InterchangeControlNumber = segments[13],
                            AcknowledgmentRequested = segments[14],
                            UsageIndicator = segments[15],
                            SegmentLength = segments.Length
                        };
                        result.ISAHeader = iSAHeader;
                    }
                    else if (element.Trim().StartsWith("GS"))
                    {
                        var gsSegments = element.Trim().Split("*");
                        GSHeader gSHeader = new()
                        {
                            SegmentIdentifier = gsSegments[0],
                            FunctionalIdentifierCode = gsSegments[1],
                            ApplicationSendersCode = gsSegments[2],
                            ApplicationReceiversCode = gsSegments[3],
                            Date = gsSegments[4],
                            Time = gsSegments[5],
                            GroupControlNumber = gsSegments[6],
                            ResponsibleAgencyCode = gsSegments[7],
                            VersionIdentifierCode = gsSegments[8]
                        };
                        result.GSHeader = gSHeader;
                    }
                    else if (element.Trim().StartsWith("ST"))
                    {
                        var stSegments = element.Trim().Split("*");

                        STSegment stSegment = new()
                        {
                            SegmentIdentifier = stSegments[0],
                            TransactionSetIdentifierCode = stSegments[1],
                            transactionSetControlNumber = stSegments[2],
                        };
                        result.StSegment = stSegment;
                    }

                    else if (element.Trim().StartsWith("IEA"))
                    {
                        var ieaElement = element.Trim().Split('*');
                        IEATrailer iEATrailer = new IEATrailer()
                        {
                            NumberOfIncludedFunctionalGroups = Convert.ToInt16(ieaElement[1]),
                            InterchangeControlNumber = ieaElement[2]
                        };
                        result.IeaTrailer = iEATrailer;
                    }
                }
            }
            return result;
        }
    }
}