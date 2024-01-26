using EdiMonthlyReportGenerator.Models;

namespace EdiMonthlyReportGenerator.Services.Interfaces
{
    public interface ITextFileService
    {
        List<string> ReadTextFile<T>(string filePath);
        void WriteTextFile(string outputFileWithPath, string outputrecord);
        public EDIRecord MapSegmentValues(string block);

    }
}
