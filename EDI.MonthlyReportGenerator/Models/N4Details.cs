namespace EDIMonthlyReportGenerator.Models
{
    /// <summary>
    /// N4: The N4 segment is used to specify the city, state, postal code, and country code of the address.
    /// </summary>
    public class N4Details
    {
        // N401: (e.g., 'MILWAUKEE') - This field contains the city name.
        public string CityName { get; set; }
        ///(e.g., 'WI') - This field contains the state or province code.
        //N402: 
        public string StateProvinceCode { get; set; }
        //N403: (e.g., '53224') - This field contains the postal code.
        public string PostalCode { get; set; }
    }
}
