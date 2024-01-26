namespace EDIMonthlyReportGenerator.Models
{
    public class N3Details
    {
        //N3: The N3 segment is used to provide address information, often used for street addresses.
        public string AddressInformation { get; set; }
        /// <summary>
        /// N301: Address Information (e.g., '11000 W LAKE PARK DR') - This field contains the street 
        /// </summary>
        public string AddressInformation2 { get; set; }

        //such as a suite or building number.
        public string AddressInformation3 { get; set; }
    }
}
