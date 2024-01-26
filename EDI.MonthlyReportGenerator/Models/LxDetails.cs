namespace EDIMonthlyReportGenerator.Record
{
    /// <summary>
    /// LX*1~ to identify a specific line item or line of data within a transaction set. 
    /// used to provide a sequential line item number or identifier for reference.
    /// </summary>
    public class LxDetails
    {
        
        public string SegmentIdentifier { get; set; }
        //a unique sequential number or identifier 
        public string AssignedNumber  { get; set; }


    }
}
