namespace EDIMonthlyReportGenerator.Record
{
    public class N1Details
    {
        /// <summary>
        /// N1*BK*GREAT LAKES NATIONAL BANK~
        /// (e.g., 'BK') - This code identifies the type of entity. In this case, 'BK' may represent a bank.
        /// </summary>
        public string EntityIdentifierCode { get; set; }

        /// <summary>
        /// (e.g., 'GREAT LAKES NATIONAL BANK') - This field contains the name of the entity being identified.
        /// </summary>
        public string EntityName { get; set; }
    }
}
