using EdiMonthlyReportGenerator.Models;

namespace EdiMonthlyReportGenerator.Strategies
{
    public interface IGenerator
    {
        Result Generate(OutputFileProperties outputFileProperties);
        
        string OutputFileType { get; }
    }
}
