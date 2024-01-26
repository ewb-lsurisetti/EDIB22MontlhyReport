namespace EDIMonthlyReportGenerator.Models
{
    /// <summary>
    /// t's important to note that the structure and meaning of the "ENT" segment can vary widely depending on the specific business context and agreements between trading partners. 
    /// This segment appears to be a custom extension or non-standard segment used for specific purposes within your organization's EDI implementation. The values and interpretation of 
    /// the elements within the "ENT" segment should be defined and documented as part of your EDI trading partner agreements.
    /// </summary>
    public class EntDetails
    {
        //ENT01:
        //(e.g., '1') - This element typically represents an identifier for the entity being referenced.In this case, '1' may indicate a specific entity or organization.
        public string EntityIdentifierCode { get; set; }


        ///ENT02:  <summary>
        /// ENT02: (e.g., 'BK') - This element specifies the type or category of the entity being referenced. 'BK' could represent "Bank" or another entity type in your specific context.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public string EntityTypeCode { get; set; }

        //ENT03: (e.g., '13') - This element contains an identifier or code associated with the organization or entity.
        public string OrganizationIdentifier { get; set; }


        /// <summary>
        /// /ENT04: (e.g., '000430300') - This element typically contains a reference number or identifier specific to the entity or organization.
        /// </summary>
        public string ReferenceIdentifier { get; set; }

        /// <summary>
        /// ENT05: (e.g., 'AO') - This element may provide additional information or context about the entity or organization.In this case, 'AO' is used.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public string AdditionalOrganizationIdentifier { get; set; }

        // ENT06: (e.g., 'ZZ') - Similar to ENT04, this is another reference identifier, which might be specific to the entity or organization.
        public string ReferenceIdentifier2 { get; set; }

        //ENT07:(e.g., 'PEAS & CARROTS') - This element contains the name or description of the entity being referenced.
        public string EntityName { get; set; }
    }
}
