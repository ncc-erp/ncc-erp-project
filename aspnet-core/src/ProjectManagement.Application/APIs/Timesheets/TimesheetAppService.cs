using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Timesheets.Dto;
using ProjectManagement.APIs.TS.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Manager.TimesheetManagers;
using ProjectManagement.Manager.TimesheetProjectManager;
using ProjectManagement.Services.ProjectTimesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.TimeSheets
{
    /*  [AbpAuthorize]*/

    public class TimeSheetAppService : ProjectManagementAppServiceBase
    {
        private ProjectTimesheetManager _timesheetManager;
        private readonly CloseTimesheet _closeTimesheet;
        private readonly ReactiveTimesheetProject _reActiveTimeSheetproject;

        public TimeSheetAppService(ProjectTimesheetManager timesheetManager, CloseTimesheet closeTimesheet,
            ReactiveTimesheetProject reactiveTimesheetProject)
        {
            _timesheetManager = timesheetManager;
            _closeTimesheet = closeTimesheet;
            _reActiveTimeSheetproject = reactiveTimesheetProject;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets)]
        public async Task<GridResult<GetTimesheetDto>> GetAllPaging(GridParam input)
        {
            var qtimesheetProject = WorkScope.GetAll<TimesheetProject>();
            var qtimesheetProjectBill = WorkScope.GetAll<TimesheetProjectBill>()
                .Where(s => s.IsActive);
            var query = WorkScope.GetAll<Timesheet>().OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .Select(x => new GetTimesheetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Month = x.Month,
                    Year = x.Year,
                    IsActive = x.IsActive,
                    CreatedInvoice = x.CreatedInvoice,
                    TotalWorkingDay = x.TotalWorkingDay,

                    TotalProject = qtimesheetProject.Where(y => y.TimesheetId == x.Id).Count(),

                    TotalHasFile = qtimesheetProject.Where(y => y.TimesheetId == x.Id &&
                                                            y.FilePath != null &&
                                                            y.Project.RequireTimesheetFile).Count(),

                    WorkingDayOfUser = qtimesheetProjectBill.Where(s => s.TimesheetId == x.Id).Sum(s => s.WorkingTime),

                    TotalIsRequiredFile = qtimesheetProject.Where(y => y.TimesheetId == x.Id)
                    .Where(s => s.Project.RequireTimesheetFile).Count(),
                });

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<TimesheetDto> Get(long timesheetId)
        {
            var query = WorkScope.GetAll<Timesheet>().Where(x => x.Id == timesheetId)
                                .Select(x => new TimesheetDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Month = x.Month,
                                    Year = x.Year,
                                    IsActive = x.IsActive,
                                    CreatedInvoice = x.CreatedInvoice,
                                    TotalWorkingDay = x.TotalWorkingDay
                                });
            return await query.FirstOrDefaultAsync();
        }

        private async Task CheckValidCreate(TimesheetDto input)
        {
            if (input.TotalWorkingDay == null || input.TotalWorkingDay <= 0)
            {
                throw new UserFriendlyException("Total Of Working Day field is required and greater than 0 !");
            }

            var nameExist = await WorkScope.GetAll<Timesheet>().AnyAsync(x => x.Name == input.Name);
            if (nameExist)
            {
                throw new UserFriendlyException("Name is already exist !");
            }

            var alreadyCreated = await WorkScope.GetAll<Timesheet>().AnyAsync(x => x.Year == input.Year && x.Month == input.Month);
            if (alreadyCreated)
            {
                throw new UserFriendlyException($"Timesheet {input.Month}-{input.Year} already exist !");
            }
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets_Create)]
        public async Task<object> Create(TimesheetDto input)
        {
            await CheckValidCreate(input);
            var failList = new List<string>();
            var timesheet = new Timesheet
            {
                Name = input.Name,
                IsActive = true,
                Year = input.Year,
                Month = input.Month,
                TotalWorkingDay = input.TotalWorkingDay
            };

            input.Id = await WorkScope.InsertAndGetIdAsync(timesheet);
            timesheet.Id = input.Id;

            var listPUB = await _timesheetManager.GetListProjectUserBillDto(timesheet.Year, timesheet.Month, null);

            var projectIds = listPUB.Select(s => s.ProjectId);

            var mapProject = await WorkScope.GetAll<Project>().Where(s => projectIds.Contains(s.Id)).ToDictionaryAsync(s => s.Id);

            var listProjectIdToBillInfo = listPUB.GroupBy(s => new { s.ProjectId, s.Discount, s.TransferFee, s.LastInvoiceNumber, s.ParentInvoiceId })
                .Select(s => new { s.Key.ProjectId, s.Key.Discount, s.Key.TransferFee, s.Key.LastInvoiceNumber, s.Key.ParentInvoiceId, ListBillInfo = s.ToList() });

            var listTimesheetProjectBill = new List<TimesheetProjectBill>();
            var listTimesheetProject = new List<TimesheetProject>();

            foreach (var item in listProjectIdToBillInfo)
            {
                var projectId = item.ProjectId;
                var lastInvoiceNumber = item.LastInvoiceNumber + 1;
                var listBillInfo = item.ListBillInfo;
                var timesheetProject = new TimesheetProject
                {
                    ProjectId = projectId,
                    TimesheetId = timesheet.Id,
                    Discount = item.Discount,
                    TransferFee = item.TransferFee,
                    WorkingDay = input.TotalWorkingDay.Value,
                    InvoiceNumber = lastInvoiceNumber,
                    ParentInvoiceId = item.ParentInvoiceId
                };

                listTimesheetProject.Add(timesheetProject);

                //Update project
                if (mapProject.ContainsKey(projectId))
                {
                    var project = mapProject[projectId];
                    project.LastInvoiceNumber = lastInvoiceNumber;
                }
                else
                {
                    failList.Add("projectId:" + projectId);
                }

                foreach (var pub in listBillInfo)
                {
                    var timesheetProjectBill = new TimesheetProjectBill
                    {
                        ProjectId = projectId,
                        TimesheetId = timesheet.Id,
                        UserId = pub.UserId,
                        BillRole = pub.BillRole,
                        BillRate = pub.BillRate,
                        StartTime = pub.StartTime,
                        EndTime = pub.EndTime,
                        AccountName = pub.AccountName,
                        IsActive = true,
                        CurrencyId = pub.CurrencyId,
                        ChargeType = pub.ChargeType,
                    };
                    listTimesheetProjectBill.Add(timesheetProjectBill);
                }
            }
            await WorkScope.InsertRangeAsync(listTimesheetProject);
            await WorkScope.InsertRangeAsync(listTimesheetProjectBill);
            await CurrentUnitOfWork.SaveChangesAsync();
            //_closeTimesheet.CreateReqCloseTimesheetBGJ(timesheet);
            return new { failList, input };
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Timesheets_Edit)]
        public async Task<TimesheetDto> Update(TimesheetDto input)
        {
            if (input.TotalWorkingDay == null || input.TotalWorkingDay <= 0)
            {
                throw new UserFriendlyException("Total Of Working Day field is required and greater than 0 !");
            }
            var timesheet = await WorkScope.GetAsync<Timesheet>(input.Id);

            if (!timesheet.IsActive)
            {
                throw new UserFriendlyException("Timesheet not active !");
            }

            var nameExist = await WorkScope.GetAll<Timesheet>().AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if (nameExist)
            {
                throw new UserFriendlyException("Name is already exist !");
            }

            var alreadyCreated = await WorkScope.GetAll<Timesheet>().AnyAsync(x => x.Id != input.Id && x.Year == input.Year && x.Month == input.Month);
            if (alreadyCreated)
            {
                throw new UserFriendlyException($"Timesheet {input.Month}-{input.Year} already exist !");
            }

            await WorkScope.UpdateAsync(ObjectMapper.Map<TimesheetDto, Timesheet>(input, timesheet));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Timesheets_Delete)]
        public async Task Delete(long timesheetId)
        {
            var ts = await WorkScope.GetAll<TimesheetProject>()
                .Where(s => s.TimesheetId == timesheetId)
                .Select(s => new
                {
                    s.Timesheet.IsActive,
                    s.FilePath
                }).FirstOrDefaultAsync();

            if (!ts.IsActive)
            {
                throw new UserFriendlyException("Timesheet not active !");
            }

            if (ts.FilePath != null)
                throw new UserFriendlyException("Timesheet has attached file !");

            await ForceDelete(timesheetId);
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Timesheets_ForceDelete)]
        public async Task ForceDelete(long timesheetId)
        {
            var timesheet = await WorkScope.GetAsync<Timesheet>(timesheetId);

            var timesheetProject = await WorkScope.GetAll<TimesheetProject>().Where(x => x.TimesheetId == timesheetId).ToListAsync();
            foreach (var item in timesheetProject)
            {
                item.IsDeleted = true;
            }

            timesheet.IsDeleted = true;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(PermissionNames.Timesheets_CloseAndActive)]
        public async Task ReverseActive(long id)
        {
            var timesheet = await WorkScope.GetAsync<Timesheet>(id);
            timesheet.IsActive = !timesheet.IsActive;
            await WorkScope.UpdateAsync(timesheet);
            // delete all re-close background job of timesheetproject in this timesheet
            if (timesheet.IsActive)
            {
                WorkScope.GetAll<TimesheetProject>().Where(tp => tp.TimesheetId == id)
                    .ToList().ForEach(tsp => {
                        _reActiveTimeSheetproject.DeleteOldRequestInBackgroundJob(tsp.Id);
                        tsp.IsActive = false;
                        });
            }
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task UpdateAvatarFromTimesheet(UpdateAvatarDto input)
        {
            if (string.IsNullOrEmpty(input.AvatarPath))
            {
                Logger.Error($"user with {input.AvatarPath} is null or empty");
                return;
            }
            var user = await GetUserByEmailAsync(input.EmailAddress);

            if (user == null)
            {
                Logger.Error($"not found user with email {input.EmailAddress}");
                return;
            }

            user.AvatarPath = input.AvatarPath;
            await WorkScope.UpdateAsync(user);
        }

        [HttpGet]
        public List<GetProjectPMNameDto> GetListPMByProjectCode()
        {
            return WorkScope.GetAll<Project>()
                .Select(x => new GetProjectPMNameDto
                {
                    ProjectCode = x.Code,
                    PMEmail = x.PM.EmailAddress
                })
                .ToList();
        }
    }
}