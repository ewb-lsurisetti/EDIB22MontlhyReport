namespace EDIMonthlyReportGenerator.Models
{
    /// <summary>
    /// e.g.RTE*2A*1.8*20277.77*1*365~
    /// The RTE segment is used to communicate pricing and rate information for products or services, and the meaning of the elements can vary depending
    /// on the specific industry and trading partner agreements.
    /// </summary>
    public class RteDetails
    {
        public string SegmentIdentifier { get; set; }
        // RTE01: (e.g., '2A')
        public string RateQualifier { get; set; }

        //(e.g., '1.8')
        public string RateAmount { get; set; }

        //RTE03:(e.g., '20277.77')
        public string RateQualifier2 { get; set; }

        //RTE04: (e.g., '1')
        public string RateAmount2 { get; set; }
        // RTE05:   (e.g., '365')
        public string RateQualifier3 { get; set; }
  


    }
}
