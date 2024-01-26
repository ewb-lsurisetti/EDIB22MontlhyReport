using EdiMonthlyReportGenerator.Services.Interfaces;
using EdiMonthlyReportGenerator.Strategies;
using EDIMonthlyReportGenerator.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EdiMonthlyReportGenerator.Services.Implements
{
    public class DependencyInjectionService
    {
        private readonly IServiceCollection _services;
        private IServiceProvider _serviceProvider;

        public DependencyInjectionService()
        {
            _services = new ServiceCollection();
            ConfigureServices();
            BuildServiceProvider();
        }

        private void ConfigureServices()
        {
            _services.AddScoped<EdiMonthlyReportGeneratorService>();
            _services.AddScoped<IConfigurationService, ConfigurationService>();
            _services.AddScoped<IDataWarehouseService, DataWarehouseService>();
            _services.AddScoped<IEmailService, EmailService>();
            _services.AddScoped<FileExtractSettings>();
            _services.AddScoped<ITextFileService, TextFileService>();
            _services.AddScoped<IFileSystemService, FileSystemService>();
            _services.AddScoped<IGenerator, MonthlyReportEdi822>();
        }

        private IServiceProvider BuildServiceProvider()
        {
            // Build the service provider once all services are registered
            _serviceProvider = _services.BuildServiceProvider();
            return _serviceProvider;
        }

        public T GetService<T>() => _serviceProvider.GetRequiredService<T>();
    }
}
