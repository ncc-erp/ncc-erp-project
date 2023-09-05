using Abp.Application.Services;
using Microsoft.Extensions.Configuration;
using NccCore.IoC;
using ProjectManagement.Entities;
using ProjectManagement.Services.PmReports.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.PmReports
{
    public class PmReportManager : IApplicationService
    {
        private readonly IConfiguration _configuration;
        private readonly IWorkScope workScope;

        public PmReportManager(IConfiguration configuration, IWorkScope workScopee)
        {
            _configuration = configuration;
            workScope = workScopee;
        }

        public async Task<List<PMUnsentWeeklyReportDto>> GetPMsUnsentWeeklyReport()
        {
            int index = 1;
            var list = workScope.GetAll<PMReportProject>()
                .Where(x => x.PMReport.IsActive
                    && x.Status == PMReportProjectStatus.Draft
                    && x.Project.IsRequiredWeeklyReport)
                .Select(x => new PMUnsentWeeklyReportDto
                {
                    Index = 0,
                    ProjectName = x.Project.Name,
                    WeeklyName = x.PMReport.Name,
                    EmailAddress = x.Project.PM.EmailAddress,
                    Name = x.PM.Name,
                    Surname = x.PM.Surname,
                    FullName = x.PM.FullName,
                    UserName = x.PM.UserName,
                    KomuId = x.PM.KomuUserId,
                    PmId = x.PM.Id

                }).ToList();
            list.ForEach(i => i.Index = index++);
            return list;
        }

    }
}
