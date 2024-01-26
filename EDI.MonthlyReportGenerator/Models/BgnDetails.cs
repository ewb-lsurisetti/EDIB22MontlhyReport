namespace EDIMonthlyReportGenerator.Models
{
    /// <summary>
    /// The BGN segment helps define the purpose and context of the transaction being exchanged. It provides information about the transaction's purpose, 
    /// reference information, type, and additional description as needed.The specific values and meaning of elements may vary depending on the industry, 
    /// trading partners, and agreements in place for the EDI exchange.
    /// </summary>
    public class BgnDetails
    {
        /// <summary>
        /// This code specifies the purpose or type of the transaction. It indicates why the transaction set is being sent. Common values for BGN01 include:

        //'00': Original '01': Cancel '02': Correct '03': Replace '04': Delete '05': Add '06': In-Reply-To '07': Pending '08': Verify '09': Inquiry
        /// </summary>
        public string TransactionSetPurposeCode1 { get; set; }
        public string ReferenceIdentification { get; set; }

        /// <summary>
        /// 'RP': Remittance Payment 'PS': Payment Order/Remittance Advice 'PY': Payment Order 'RE': Remittance Advice '00': Unspecified
        /// </summary>
        public string TransactionTypeCode { get; set; }

        /// <summary>
        /// This is a free-form description or text that further describes the purpose or nature of the transaction.
        /// </summary>
        public string TransactionSetPurposeCode2 { get; set; }


    }
}
