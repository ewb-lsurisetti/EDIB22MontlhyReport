namespace EDIMonthlyReportGenerator.Models
{
    public class ISAHeader
    {
        public string SegmentIdentifier { get; set; } //0
        public string AuthorizationInformationQualifier { get; set; } //1
        public string AuthorizationInformation { get; set; } //2
        public string SecurityInformationQualifier { get; set; } //3
        public string SecurityInformation { get; set; } //4
        public string InterchangeSenderIDQualifier { get; set; } //5
        public string InterchangeSenderId { get; set; } //6
        public string InterchangeReceiverIDQualifier { get; set; } //6
        public string ReceiverId { get; set; } //7
        public string InterchangeDate { get; set; } //8
        public string InterchangeTime { get; set; } //9
        public string InterchangeControlStandardsId { get; set; } //10
        public string InterchangeControlVersionNumber { get; set; } //11
        public string InterchangeControlNumber { get; set; } //12
        public string AcknowledgmentRequested { get; set; } //13
        public string UsageIndicator { get; set; } //14
        public string ComponentElementSeparator { get; set; } //15
        public string OutputFilePath { get; set; }
        public string RejectedFilePath { get; set; }
        public long FileSizeInBytes { get; set; }
        public int OutPutRecords { get; set; }
       public int SegmentLength { get; set; }
    }
}
