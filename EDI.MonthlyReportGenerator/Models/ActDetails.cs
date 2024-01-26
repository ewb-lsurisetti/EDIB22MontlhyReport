namespace EDIMonthlyReportGenerator.Models
{   /*
     * This segment is typically used in EDI transactions to provide information related to an account or account reconciliation. 
     * Here's a breakdown of the elements within the ACT segment:
     * //ACT*00000000000054017051*BEVERAGE COMPANY*13*000430300*REL*00000000000054017050~
   */
    public class ActDetails
    {
        // ACT01: (e.g., '00000000000054017051') - This element represents the qualifier or type of account number provided in ACT02.The specific meaning of the qualifier depends on the trading partners and their agreements.
        public string AccountNumber { get; set; }

        //ACT02: (e.g., 'BEVERAGE COMPANY') - This element contains the account number or identifier associated with the entity mentioned in the ACT03 element.
        public string CompanyName { get; set; }

        //ACT03: (e.g., '13') - This element specifies the type of entity identified in ACT02.In this case, '13' may represent an organization or company.

        public string EntityIdentifierCode { get; set; }

        /// <summary>
        /// ACT04:(e.g., '000430300') - This element typically contains a reference number or identifier associated with the account or transaction.
        /// </summary>
        /// 
        public string ReferenceIdentifier { get; set; }


        /// <summary>
        /// ACT05: (e.g., 'REL') - This element specifies the type of entity identified in ACT06. 'REL' might represent a related party or entity.
        /// </summary>
        public string EntityIdentifierCode2 { get; set; }

        // ACT06: (e.g., '00000000000054017050') - This element typically contains a reference number or identifier associated with the related entity mentioned in ACT05.
        public string ReferenceIdentifier2 { get; set; }


    }
}
