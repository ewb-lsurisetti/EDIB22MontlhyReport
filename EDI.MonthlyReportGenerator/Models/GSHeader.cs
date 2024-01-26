namespace EDIMonthlyReportGenerator.Models
{
    public class GSHeader
    {
        public string SegmentIdentifier { get; set; }
        public string FunctionalIdentifierCode { get; set; }
        public string ApplicationSendersCode { get; set; }
        public string ApplicationReceiversCode { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string GroupControlNumber { get; set; }
        public string ResponsibleAgencyCode { get; set; }
        public string VersionIdentifierCode { get; set; }
    }
}
