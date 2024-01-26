// Ignore Spelling: App

namespace EdiMonthlyReportGenerator.Models
{
    public class AppSettings
    {
        public List<OutputFileProperties> OutputFileProperties { get; set; }
        public int FileReadyWaitMaxAttempt { get; set; }
        public int FileReadyWaitTimeInSecs { get; set; }

        public bool IsManualTrigger { get; set; }
        public bool IsProd { get; set; }       
        public EmailSettings EmailSettings { get; set; }

    }

    public class OutputFileProperties
    {
        public string OutputFileType { get; set; }      
        public string InputFileBasePath { get; set; }
        public string InputFileName { get; set; }
        public string OutputFileBasePath { get; set; }
        public string OutputFileName { get; set; }        
        public string ArchiveFileBasePath { get; set; }
        public string NotProcessedBasePath { get; set; }
    }

    public record EmailSettings(string SmtpServer, int SmtpPort, string FromEmail, string FromEmailName, string Recipients);
}
