namespace EdiMonthlyReportGenerator.Models
{
    /// <summary>
    /// Standard response class
    /// </summary>
    public class Result
    {
        public Result() { }
        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
