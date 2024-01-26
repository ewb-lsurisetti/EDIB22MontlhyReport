using EdiMonthlyReportGenerator.Models;

namespace EdiMonthlyReportGenerator.Services.Interfaces
{
    public interface IEdiMonthlyReportGeneratorService
    {
        void GenerateMonthlyReport(OutputFileProperties outputFileType);
    }
}
