using EdiMonthlyReportGenerator.Models;

namespace EdiMonthlyReportGenerator.Services.Interfaces
{
    public interface IEmailService
    {
        Result SendEmail(string emailSubject, IEnumerable<InfoRow> infoRows, string dateTime, bool isSuccessful = false);
    }
}
