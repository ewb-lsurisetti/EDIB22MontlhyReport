namespace EDIMonthlyReportGenerator.Models
{
    /// <summary>
    /// CUR*BK*USD~ 
    /// The CUR segment is essential for ensuring that all parties involved in the EDI exchange understand the currency in which financial values are expressed.
    /// It helps prevent misunderstandings and facilitates accurate financial transactions.

    /// Please note that the currency codes are standardized, and 'USD' is the ISO 4217 currency code for United States Dollars.
    ///In EDI, other currency codes may also be used as needed to represent different currencies.
    /// </summary>
    public class CurDetails
    {

        /// <summary>
        /// CUR01:(e.g., 'BK') - This element specifies the entity or organization that is providing the currency information.In this case, 
        /// 'BK' might represent a bank or financial institution.
        /// </summary>
        public string Currency { get; set; }

        public string Bank { get; set; }
        /// <summary>
        /// CUR02:(e.g., 'USD') - This element contains the three-letter currency code that represents the currency being used for financial amounts. 
        /// 'USD' typically stands for United States Dollars.
        /// </summary>


        public string CurrencyCode { get; set; }



    }
}
