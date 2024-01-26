namespace EDIMonthlyReportGenerator.Models
{
    public class FileExtractSettings
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public string FilePrefix { get; set; }        
        public string OutputFileString { get; set; }
    }
}
