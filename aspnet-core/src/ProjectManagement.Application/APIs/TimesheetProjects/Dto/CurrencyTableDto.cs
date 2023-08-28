using System.Collections.Generic;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class TimesheetInfoDto
    {
        public long TimesheetId { get; set; }
        public string TimesheetName { get; set; }
        public string Date { get; set; }

        public List<CurrencyDto> Currencies { get; set; }
    }

    public class CurrencyDto
    {
        public string CurrencyName { get; set; }

        public double ExchangeRate { get; set; }
    }
}