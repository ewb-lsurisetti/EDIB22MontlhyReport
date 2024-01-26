namespace EDIMonthlyReportGenerator.Record
{
    /// <summary>
    /// IEA*1*000000003~
    /// IEA: This is the IEA (Interchange Control Trailer) segment, which marks the end of the entire interchange, which can contain one or more functional groups.
    /// </summary>
    public class IEATrailer
    {
        // '1') 
        public int NumberOfIncludedFunctionalGroups { get; set; }

        //(e.g., '000000003')
        public string InterchangeControlNumber { get; set; }
    }
}
