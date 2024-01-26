using EdiMonthlyReportGenerator.Models;

namespace EdiMonthlyReportGenerator.Services.Interfaces
{
    public interface IConfigurationService
    {
        IEnumerable<OutputFileProperties> GetOutputFileProperties();
        string? GetConnectionString(string key);
        AppSettings AppSettings { get; }
    }
}
