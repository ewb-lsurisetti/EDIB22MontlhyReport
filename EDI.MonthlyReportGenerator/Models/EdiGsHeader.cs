namespace EDIMonthlyReportGenerator.Models
{/*
  * The GS segment is used to group related transactions or messages within the interchange. It helps identify the sender, receiver, date, and control 
  * information specific to that group of transactions. 
  * Like the ISA segment, the meanings of the elements may vary depending on the specific EDI standard and the agreements between trading partners.
  */
    public class EdiGsHeader
    {
        //GS01: (e.g., 'AA') - This code specifies the type or purpose of the functional group.In this case, 'AA' is being used, which typically
        //represents a general purpose functional group.
        public string FunctionalIdentifierCode { get; set; }


        ///GS02: (e.g., '123000013') - This is the identifier of the entity or party that is sending the data within the functional group.
        public string ApplicationSendersCode { get; set; }


        /// <summary>
        ///GS03  - This is the identifier of the entity or party that is receiving the data within the functional group.
        /// </summary>
        /// <param name="(e.g., '098C')"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public string ApplicationReceiversCode { get; set; }

        /// <summary>
        /// GS04:(e.g., '19910718') - This is the date in YYMMDD format when the functional group was created or sent.
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public string Date { get; set; }

        //GS05: - This is the time in HHMM format when the functional group was created or sent (e.g., '1131')
        public string Time { get; set; }

        ///GS06:  <summary>
        /// GS06: (e.g., '1') - This is a unique control number assigned to the functional group, typically starting from '1' and incrementing with 
        /// each new functional group in the interchange.
        /// </summary>
        public string GroupControlNumber { get; set; }

        // GS07: (e.g., '01') - This code identifies the responsible agency that assigns the code values. '01' typically represents the ANSI X12 standard.
        public string ResponsibleAgencyCode { get; set; }


        /// <summary>
        /// GS08: Version / Release /
        /// (e.g., '004010') - This code indicates the version or release of the X12 standard being used for this functional group. 
        /// </summary>
        public string IndustryIdentifierCode { get; set; }

    }
}
