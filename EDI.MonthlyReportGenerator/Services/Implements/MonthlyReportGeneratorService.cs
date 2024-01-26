using EdiMonthlyReportGenerator.Models;
using EdiMonthlyReportGenerator.Services.Interfaces;
using EdiMonthlyReportGenerator.Strategies;
using Serilog;

namespace EdiMonthlyReportGenerator.Services.Implements
{
    public class EdiMonthlyReportGeneratorService : IEdiMonthlyReportGeneratorService
    {
        private IConfigurationService _configurationService;
        private readonly Dictionary<string, IGenerator> _generators = new(StringComparer.InvariantCultureIgnoreCase);

        public EdiMonthlyReportGeneratorService(IConfigurationService configurationService, IEnumerable<IGenerator> generators)
        {
            _configurationService = configurationService;

            foreach (var generator in generators)
            {
                _generators.Add(generator.OutputFileType, generator);
            }
        }

        public void GenerateMonthlyReport(OutputFileProperties outputFileProperties)
        {
            if (!_generators.TryGetValue(outputFileProperties.OutputFileType, out var generator)) // Not case-sensitive when looking up outputFileType in the dictionary
            {
                Log.Warning("OutputFileType not supported : {type}", outputFileProperties);
            }
            else
            {
                Result result;
                var logger = Log.ForContext("OutputFileType", outputFileProperties.OutputFileType);
                try
                {
                    logger.Information("[{type}] Starting GenerateEdiMonthlyBillingFile", outputFileProperties.OutputFileType);
                    result = generator.Generate(outputFileProperties);

                    logger.Information("[{type}] Completed EDI GenerateMonthlyBillingFile. Result - Success: {success}, Message: {message}", outputFileProperties.OutputFileType, result.Success, result.Message);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "[{type}] Exception in GenerateEdiMonthlyBillingReport", outputFileProperties.OutputFileType);
                }
            }
        }

        public void Run()
        {
            try
            {
                Log.Information("Edi Job started.");
                foreach (var outputFileProperty in _configurationService.GetOutputFileProperties())
                {
                    GenerateMonthlyReport(outputFileProperty);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception in Run()-EdiMonhtly report");
                throw;
            }
            finally
            {
                Log.Information("EDI Job completed.");
                Log.CloseAndFlush();
            }
        }
    }
}
