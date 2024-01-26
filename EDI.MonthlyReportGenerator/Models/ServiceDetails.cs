namespace EDIMonthlyReportGenerator.Record
{
    /// <summary>
    /// SER*TB*99999901713*912500.1*45**0*LOW BAL MAINT FEE*FCH*B~
    /// This segment is often used to convey information related to services, promotions, allowances, or charges within an EDI document
    /// </summary>
    public class ServiceDetails
    {
        //SER01: Service, Promotion, Allowance, or Charge Code(e.g., 'TB')
        public string ServiceCode { get; set; }

        // SER02: numerical value related to the service, promotion, allowance, or charge : '99999901713
        public string Number { get; set; }

        //SER03: (e.g., '912500.1') 
        public string MonetaryAmount { get; set; }
        
        // '45'
        public string Quantity { get; set; }

        public string  UnitforMeasurementCode { get; set; }

        public string ServiceDescription { get; set; }
        
        //(e.g., 'FCH') 
        public string ConditionResponseCode { get; set; }

        //(e.g., 'B')
        public string Percentage { get; set; }
    }
}
