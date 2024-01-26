namespace EDIMonthlyReportGenerator.Record
{
    /// <summary>
    /// GE: This is the GE (Functional Group Trailer) segment, which marks the end of a functional group of related transaction sets within the EDI document.
    /// </summary>
    /// e.g.GE*1*3~
    public class GeTrailer
    {
        public string NumberOfTransactionSetsIncluded { get; set; }

        public string GroupControlNumber { get; set; }
    }
}
