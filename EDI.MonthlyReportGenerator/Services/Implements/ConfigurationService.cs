// Ignore Spelling: App

using EdiMonthlyReportGenerator.Models;
using EdiMonthlyReportGenerator.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace EdiMonthlyReportGenerator.Services.Implements
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;
        public AppSettings AppSettings { get; }

        public ConfigurationService()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"))
                .Build();

            // Load app settings from config
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();

            // Configure logger
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(_configuration)
               .Enrich.WithProperty("CorrelationId", Guid.NewGuid().ToString())
               .CreateLogger();
        }

        public IEnumerable<OutputFileProperties> GetOutputFileProperties()
        {
            if (AppSettings.OutputFileProperties == null || AppSettings.OutputFileProperties.Count == 0)
            {
                throw new InvalidOperationException("No output file types specified in the configuration."); // TODO: Log as error
            }

            return AppSettings.OutputFileProperties;
        }

        public string? GetConnectionString(string key)
        {           
             return _configuration.GetConnectionString(key);           

        }
    }
}