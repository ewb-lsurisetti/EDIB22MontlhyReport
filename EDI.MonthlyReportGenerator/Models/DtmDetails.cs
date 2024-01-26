namespace EDIMonthlyReportGenerator.Models
{
    /*
     * The DTM (Date/Time Reference) segment is used in EDI X12 documents to provide date and time information related to various aspects of the transaction. It is a versatile segment that can be used to convey dates and times for different purposes within the document. 
     * Here's a breakdown of the key elements within the DTM segment:
     */
    public class DtmDetails
    {
        /// <summary>
        /// '011': Shipment Pickup Date
        //'037': Payment Due Date
        //'090': Estimated Arrival Time
        //'370': Production Start Date
        //'175': Effective Date
        //'007': Invoice Date
        /// </summary>
        public string DateTimeQualifier { get; set; }

        /// <summary>
        /// 'D8': Date in CCYYMMDD format (e.g., '20231118' for November 18, 2023).
         //'RD': Relative Date(e.g., '+30' for 30 days from the current date).
        /// </summary>
        public string DateTimePeriodFormatQualifier { get; set; }

        public string Datetime { get; set; }


    }
}
