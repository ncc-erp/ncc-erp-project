using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.PmReports.Dto
{
    public class PMUnsentWeeklyReportDto
    {

        public int Index { get; set; }
        public string ProjectName { get; set; }
        public string WeeklyName { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public long? KomuId { get; set; }
        public long? PmId { get; set; }
    }
}
