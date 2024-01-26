namespace EDIMonthlyReportGenerator.Models
{
    /// <summary>
    /// The "BLN" segment is not a standard segment in the EDI X12 format. EDI X12 segments typically consist of three uppercase letters followed by numerical elements with specific meanings within the context of the transaction set. 
    /// The "BLN" segment is not a recognized standard segment within the EDI X12 format.
    /// </summary>
    public class BlnDetails
    {
        ///(e.g., 'TE')
        public string BalanceTypeCode { get; set; }


        public string MonetaryAmount { get; set; }
        //(e.g., '0') -
        public string Quantity { get; set; }
    }
}
