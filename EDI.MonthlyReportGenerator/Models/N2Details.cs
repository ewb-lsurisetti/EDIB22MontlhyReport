namespace EDIMonthlyReportGenerator.Models
{
    /// <summary>
    /// N2: The N2 segment is used to provide additional address information.
    /// </summary>
    public class N2Details
    {
        /// <summary>
        /// 'ATTN: CONTROLLER')
        /// </summary>
        public string AddressInformation { get; set; }

        //(e.g., 'THIS IS LINE 3') - This field may contain additional address details, such as a second address line.
        public string AddressInformation2 { get; set; }
    }
}
