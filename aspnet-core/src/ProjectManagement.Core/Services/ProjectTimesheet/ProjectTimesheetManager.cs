using Abp.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NccCore.IoC;
using ProjectManagement.Entities;
using ProjectManagement.Services.ProjectTimesheet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Services.ProjectTimesheet
{
    public class ProjectTimesheetManager : ApplicationService
    {
        private readonly IWorkScope _workScope;
        private readonly ILogger<ProjectTimesheetManager> Logger;

        public ProjectTimesheetManager(IWorkScope workScope, ILogger<ProjectTimesheetManager> logger)
        {
            _workScope = workScope;
            this.Logger = logger;
        }

        public async Task<Entities.Timesheet> GetLastTimesheet()
        {
            return await _workScope.GetAll<Entities.Timesheet>()
               .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
               .Select(x => x)
               .FirstOrDefaultAsync();
        }

        public async Task<TimesheetDto> GetActiveTimesheet()
        {
            return await _workScope.GetAll<Entities.Timesheet>()
                .Where(s => s.IsActive)
               .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
               .Select(x => new TimesheetDto
               {
                   Id = x.Id,
                   Month = x.Month,
                   Year = x.Year,
                   IsActive = x.IsActive
               })
               .FirstOrDefaultAsync();
        }

        public async Task<TimesheetDto> GetTimesheetById(long id)
        {
            return await _workScope.GetAll<Entities.Timesheet>()
                .Where(s => s.Id == id)
               .Select(x => new TimesheetDto
               {
                   Id = x.Id,
                   Month = x.Month,
                   Year = x.Year,
                   IsActive = x.IsActive,
                   TotalWorkingDay = x.TotalWorkingDay
               })
               .FirstOrDefaultAsync();
        }

        public async Task CreateTimesheetProjectBill(ProjectUserBill pub, Project project)
        {
            var activeTimesheet = await GetLastTimesheet();
            if (!activeTimesheet.IsActive)
            {
                Logger.LogInformation("CreateTimesheetProjectBill() no activeTimesheetId");
                return;
            }
            var timesheetproject = await _workScope.GetAll<TimesheetProject>()
                .Where(x => x.ProjectId == pub.ProjectId && x.TimesheetId == activeTimesheet.Id)
                .FirstOrDefaultAsync();
            if (timesheetproject == default)
            {
                Logger.LogInformation($"UpdateTimesheetProjectBill() not found TimesheetProject ProjectId={pub.ProjectId}, TimesheetId={activeTimesheet}");
                return;
            }
            if (timesheetproject.IsComplete.HasValue && timesheetproject.IsComplete.Value == true)
            {
                return;
            }
            var tpb = new TimesheetProjectBill
            {
                TimesheetId = activeTimesheet.Id,
                IsActive = true,
                BillRate = pub.BillRate,
                BillRole = pub.BillRole,
                //CurrencyId = pub.CurrencyId,
                ProjectId = pub.ProjectId,
                UserId = pub.UserId,
                WorkingTime = 0,
                ChargeType = pub.ChargeType.HasValue ? pub.ChargeType : project.ChargeType,
                CurrencyId = project.CurrencyId,
                AccountName = pub.AccountName,
            };

            await _workScope.InsertAsync(tpb);
        }

        public async Task UpdateTimesheetProjectBill(ProjectUserBill pub)
        {
            var activeTimesheet = await GetLastTimesheet();
            if (!activeTimesheet.IsActive)
            {
                Logger.LogInformation("UpdateTimesheetProjectBill() no activeTimesheetId");
                return;
            }

            var tpb = await _workScope.GetAll<TimesheetProjectBill>()
                .Where(s => s.ProjectId == pub.ProjectId)
                .Where(s => s.TimesheetId == activeTimesheet.Id)
                .Where(s => s.UserId == pub.UserId)
                .FirstOrDefaultAsync();

            if (tpb == default)
            {
                Logger.LogInformation($"UpdateTimesheetProjectBill() not found TimesheetProjectBill ProjectId={pub.ProjectId}, TimesheetId={activeTimesheet}, UserId={pub.UserId}");
                return;
            }
            var timesheetproject = await _workScope.GetAll<TimesheetProject>()
                .Where(x => x.ProjectId == pub.ProjectId && x.TimesheetId == activeTimesheet.Id)
                .FirstOrDefaultAsync();
            if (timesheetproject == default)
            {
                Logger.LogInformation($"UpdateTimesheetProjectBill() not found TimesheetProject ProjectId={pub.ProjectId}, TimesheetId={activeTimesheet}");
                return;
            }
            if (timesheetproject.IsComplete.HasValue && timesheetproject.IsComplete.Value == true)
            {
                return;
            }
            tpb.BillRate = pub.BillRate;
            tpb.BillRole = pub.BillRole;
            tpb.IsActive = pub.isActive;
            tpb.AccountName = pub.AccountName;

            await _workScope.UpdateAsync(tpb);
        }

        public async Task DeleteTimesheetProjectBill(long projectId, long timesheetId)
        {
            var tspbList = await _workScope.GetAll<TimesheetProjectBill>()
                .Where(x => x.ProjectId == projectId && x.TimesheetId == timesheetId)
                .ToListAsync();

            if (tspbList == null || tspbList.Count == 0)
            {
                return;
            }

            foreach (var item in tspbList)
            {
                item.IsDeleted = true;
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<List<ProjectUserBillDto>> GetListProjectUserBillDto(int year, int month, long? projectId)
        {
            var firstDayOfMonth = new DateTime(year, month, 1).Date;
            var q = _workScope.GetAll<ProjectUserBill>()
               .Where(s => s.Project.IsCharge == true)
               .Where(s => s.isActive)
               .Where(s => !s.EndTime.HasValue || s.EndTime.Value.Date >= firstDayOfMonth);

            if (projectId.HasValue)
            {
                q = q.Where(s => s.ProjectId == projectId.Value);
            }
            else
            {
                q = q.Where(s => s.Project.Status == Constants.Enum.ProjectEnum.ProjectStatus.InProgress);
            }

            return await q.Select(s => new ProjectUserBillDto
            {
                ProjectId = s.ProjectId,
                UserId = s.UserId,
                BillRate = s.BillRate,
                BillRole = s.BillRole,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                ChargeType = s.ChargeType.HasValue ? s.ChargeType : s.Project.ChargeType,
                CurrencyId = s.Project.CurrencyId,
                Discount = s.Project.Discount,
                TransferFee = s.Project.Client.TransferFee,
                LastInvoiceNumber = s.Project.LastInvoiceNumber,
                AccountName = s.AccountName,
                ParentInvoiceId = s.Project.ParentInvoiceId
            }).ToListAsync();
        }
    }
}