using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.PmAssistantBot.Dto;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PmAssistantBot
{
    public class PmAssistantBotAppService : ProjectManagementAppServiceBase
    {
        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPmManualWeeklyReport([FromQuery] PmManualWeeklyReportInput input)
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = HttpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            if (secretCode != securityCodeHeader)
            {
                return new BadRequestObjectResult("You do not have permission to retrieve weekly criteria notes.");
            }

            if (input.ProjectId <= 0)
            {
                return new BadRequestObjectResult("ProjectId must be greater than 0.");
            }

            var fromDate = input.WeekStart.Date;
            var toDate = input.WeekEnd.Date.AddDays(1);
            var pmReportProject = await WorkScope.GetAll<PMReportProject>()
                .Where(x => x.ProjectId == input.ProjectId)
                .Where(x => x.PMReport.Type == PMReportType.Weekly)
                .Where(x => x.Status == PMReportProjectStatus.Sent)
                .Where(x => x.TimeSendReport >= fromDate && x.TimeSendReport < toDate)
                .OrderByDescending(x => x.TimeSendReport)
                .Select(x => new
                {
                    x.PMReportId,
                })
                .FirstOrDefaultAsync();

            if (pmReportProject == null)
            {
                return new OkObjectResult(new List<PmManualWeeklyReportDto>());
            }

            var result = await WorkScope.GetAll<ProjectCriteriaResult>()
                .Where(x => x.ProjectId == input.ProjectId)
                .Where(x => x.PMReportId == pmReportProject.PMReportId)
                .Select(x => new PmManualWeeklyReportDto
                {
                    Name = x.ProjectCriteria.Name,
                    Note = x.Note
                })
                .ToListAsync();

            return new OkObjectResult(result);
        }
    }
}
