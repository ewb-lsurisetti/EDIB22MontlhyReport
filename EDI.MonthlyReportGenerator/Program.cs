using EdiMonthlyReportGenerator.Services.Implements;

namespace EDI.MonthlyReportGenerator
{
    public static class Program
    {
        static void Main()
        {
            // Initializations - includes configuring services and building service provider
            var dependencyInjectionService = new DependencyInjectionService();
            var app = dependencyInjectionService.GetService<EdiMonthlyReportGeneratorService>();

            // Run the application
            app.Run();          
        }
    }
}