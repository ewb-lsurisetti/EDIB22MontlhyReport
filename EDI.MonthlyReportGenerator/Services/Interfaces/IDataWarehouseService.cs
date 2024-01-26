namespace EdiMonthlyReportGenerator.Services.Implements
{
    public interface IDataWarehouseService
    {
        public bool IsBusinessDay(DateTime date);
        public DateTime? GetNextBusinessDay(DateTime? date);
        public (DateTime?, DateTime?) GetUSBusinessDates(DateTime? date);
    }
}
