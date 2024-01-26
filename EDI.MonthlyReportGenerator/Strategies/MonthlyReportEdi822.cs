// Ignore Spelling: Fiserv

using EdiMonthlyReportGenerator.Models;
using EdiMonthlyReportGenerator.Services.Implements;
using EdiMonthlyReportGenerator.Services.Interfaces;
using System.Text.RegularExpressions;

namespace EdiMonthlyReportGenerator.Strategies
{
    /// <summary>
    /// Implementation for EDI Monthly Report Generation
    /// Schedule: 6.30 am, 21st day (or next business day) of every month
    /// Ref: Jira - https://ewbank.atlassian.net/browse/AVENGERS-7316
    /// </summary>
    public class MonthlyReportEdi822 : EdiReportBase
    {
        #region Constructor(s)
        public MonthlyReportEdi822(IConfigurationService configurationService, IDataWarehouseService dataWarehouseService, IEmailService emailService, ITextFileService csvFileService, IFileSystemService fileSystemService)
            : base(configurationService, dataWarehouseService, emailService, csvFileService, fileSystemService) { }
        #endregion

        #region Override(s)
        public override string OutputFileType => "EDI822";

        protected override IEnumerable<EDIRecord> FilterRecords(List<string> inputRecords, OutputFileProperties outputFileProperties, DateTime currentDate)
        {

            return (IEnumerable<EDIRecord>)inputRecords
                .Where(record =>
                    record != null
                    && Regex.IsMatch(record, @"^\d+$")
                    )
                .ToList();
        }
        #endregion
    }
}
