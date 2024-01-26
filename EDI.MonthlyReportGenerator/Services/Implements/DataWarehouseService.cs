using EdiMonthlyReportGenerator.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace EdiMonthlyReportGenerator.Services.Implements
{
    public class DataWarehouseService : IDataWarehouseService
    {
        private readonly IConfigurationService _configurationService;
        private const string EwbDwMartSharedKey = "EwbDwMartShared";
        public DataWarehouseService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public DateTime? GetNextBusinessDay(DateTime? date)
        {
            DateTime? result = date;

            var businessDates = GetUSBusinessDates(date);
            var latestBusDay = businessDates.Item1;
            var nextBusDay = businessDates.Item2;

            if (latestBusDay != null && nextBusDay != null && latestBusDay.GetValueOrDefault().Date != date?.Date)
            {
                result = nextBusDay;
            }

            // TODO : Handle other countries when logic is available

            return result;
        }

        public (DateTime?, DateTime?) GetUSBusinessDates(DateTime? date)
        {
            DateTime? latestBusDay = null;
            DateTime? nextBusDay = null;
            var connectionString = _configurationService.GetConnectionString(EwbDwMartSharedKey);
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandTimeout = 300; // 5 mins or 300 seconds
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM [dbo].[fn_Reporting_Dates] (@InputDate)";
                cmd.Parameters.Add("@InputDate", SqlDbType.Date);
                cmd.Parameters["@InputDate"].Value = date?.Date;
                bool justOpenConn = false;

                if (cmd.Connection.State != ConnectionState.Open)
                {
                    justOpenConn = true;
                    cmd.Connection.Open();
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        latestBusDay = reader.GetDateTime("Latest_Bus_Day");
                        nextBusDay = reader.GetDateTime("Next_Bus_Day");
                    }
                }

                if (justOpenConn && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                }
            }
            return (latestBusDay, nextBusDay);
        }

        public bool IsBusinessDay(DateTime date)
        {
            var businessDates = GetUSBusinessDates(date);
            return businessDates.Item1 != null && businessDates.Item1.GetValueOrDefault().Date == date.Date;
        }
    }
}