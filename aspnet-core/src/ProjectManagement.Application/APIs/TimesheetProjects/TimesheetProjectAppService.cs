﻿using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Office.Interop.Excel;
using NccCore.Extension;
using NccCore.Paging;
using NccCore.Uitls;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ProjectManagement.APIs.Projects.Dto;
using ProjectManagement.APIs.TimeSheetProjectBills.Dto;
using ProjectManagement.APIs.TimesheetProjects.Dto;
using ProjectManagement.APIs.Timesheets.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.BackgroundJobs;
using ProjectManagement.Configuration;
using ProjectManagement.Constants;
using ProjectManagement.Entities;
using ProjectManagement.Helper;
using ProjectManagement.Manager.TimesheetProjectManager;
using ProjectManagement.Manager.TimesheetProjectManager.Dto;
using ProjectManagement.NccCore.Helper;
using ProjectManagement.Net.MimeTypes;
using ProjectManagement.Services.ExchangeRate;
using ProjectManagement.Services.Finance;
using ProjectManagement.Services.Finance.Dto;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Services.ProjectTimesheet;
using ProjectManagement.Services.Timesheet;
using ProjectManagement.Services.Timesheet.Dto;
using ProjectManagement.UploadFilesService;
using ProjectManagement.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TimesheetProjects
{
    [AbpAuthorize]
    public class TimesheetProjectAppService : ProjectManagementAppServiceBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly FinfastService _financeService;
        private ISettingManager _settingManager;
        private KomuService _komuService;
        private readonly string templateFolder = Path.Combine("wwwroot", "template");
        private readonly ProjectTimesheetManager _timesheetManager;
        private readonly TimesheetService _timesheetService;
        private readonly UploadFileService _uploadFileService;
        private readonly ExchangeRateService _exchangeRateService = new ExchangeRateService();
        private readonly ReactiveTimesheetProject _reactiveTimesheetProject;
        private readonly IRepository<BackgroundJobInfo, long> _storeJob;

        public TimesheetProjectAppService(
            IWebHostEnvironment environment,
            FinfastService financeService,
            KomuService komuService,
            ISettingManager settingManager,
            ProjectTimesheetManager timesheetManager,
            TimesheetService timesheetService,
            UploadFileService uploadFileService,
            ReactiveTimesheetProject activeTimesheetProject,
            IRepository<BackgroundJobInfo, long> storeJob)
        {
            _hostingEnvironment = environment;
            _financeService = financeService;
            _komuService = komuService;
            _settingManager = settingManager;
            _timesheetManager = timesheetManager;
            _timesheetService = timesheetService;
            _uploadFileService = uploadFileService;
            _reactiveTimesheetProject = activeTimesheetProject;
            _storeJob = storeJob;
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<GetTimesheetProjectDto>> GetAllByProject(long projectId)
        {
            var query = WorkScope.GetAll<TimesheetProject>()
                        .Where(x => x.ProjectId == projectId)
                        .OrderByDescending(x => x.CreationTime)
                        .Select(tsp => new GetTimesheetProjectDto
                        {
                            Id = tsp.Id,
                            TimeSheetName = $"T{tsp.Timesheet.Month}/{tsp.Timesheet.Year}",
                            ProjectId = tsp.ProjectId,
                            TimesheetFile = tsp.FilePath,
                            Note = tsp.Note,
                            HistoryFile = tsp.HistoryFile
                        });
            return await query.ToListAsync();
        }

        [HttpGet]
        public async Task<List<GetDetailInvoiceDto>> ViewInvoice(long timesheetId)
        {
            var query = from c in WorkScope.GetAll<Client>()
                        join tsp in WorkScope.GetAll<TimesheetProject>().Where(x => x.TimesheetId == timesheetId && x.Timesheet.IsActive)
                        on c.Id equals tsp.Project.ClientId
                        group tsp by new { c.Id, c.Name } into pp
                        select new GetDetailInvoiceDto
                        {
                            ClientId = pp.Key.Id,
                            ClientName = pp.Key.Name,
                            TotalProject = pp.Count()
                        };
            return await query.ToListAsync();
        }

        [HttpGet]
        public async Task<List<GetProjectDto>> GetAllProjectForDropDown(long timesheetId)
        {
            var timesheetProject = await WorkScope.GetAll<TimesheetProject>().Where(x => x.TimesheetId == timesheetId).Select(x => x.ProjectId).ToListAsync();
            var query = WorkScope.GetAll<Project>().Where(x => !timesheetProject.Contains(x.Id))
                .Where(x => x.Status != ProjectStatus.Potential && x.Status != ProjectStatus.Closed)
                .Select(x => new GetProjectDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ProjectType = x.ProjectType,
                    StartTime = x.StartTime.Date,
                    EndTime = x.EndTime.Value.Date,
                    Status = x.Status,
                    ClientId = x.ClientId,
                    ClientName = x.Client.Name,
                    IsCharge = x.IsCharge,
                    PmId = x.PMId,
                    PmName = x.PM.Name,
                });
            return await query.ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail)]
        public async Task<ResultTimesheetDetail> GetAllProjectTimesheetByTimesheet(GridParam input, long timesheetId)
        {
            var allowViewBillRate = PermissionChecker.IsGranted(PermissionNames.Timesheets_TimesheetDetail_ViewBillRate);
            var allowViewAllTSProject = PermissionChecker.IsGranted(PermissionNames.Timesheets_TimesheetDetail_ViewAll);

            var timeSheetDeTails = GetTimesheetDetail(timesheetId, allowViewBillRate, allowViewAllTSProject);
            var queryFilter = timeSheetDeTails.ApplySearchAndFilter(input);
            var totalProject = await queryFilter.CountAsync();

            var paggingProjects = await queryFilter.TakePage(input).ToListAsync();

            var allProjects = await timeSheetDeTails.ToListAsync();
            allProjects = GetClosedTimesheetProject(allProjects);

            var dicProjectIdToInvoiceInfo = GetDicMainProjectIdToInvoiceInfo(allProjects);
            UpdateInvoiceSetting(paggingProjects, dicProjectIdToInvoiceInfo);

            var listTotalAmountByCurrency = allProjects.GroupBy(x => x.Currency)
                                    .Select(x => new TotalMoneyByCurrencyDto
                                    {
                                        CurrencyName = x.Key,
                                        Amount = x.Sum(x => x.TotalAmountProjectBillInfomation)
                                    }).ToList();

            var gridResult = new GridResult<GetTimesheetDetailDto>(paggingProjects, totalProject);

            var result = new ResultTimesheetDetail
            {
                ListTimesheetDetail = gridResult,
                ListTotalAmountByCurrency = listTotalAmountByCurrency,
            };

            return result;
        }

        private Dictionary<long, InvoiceSettingDto> GetDicMainProjectIdToInvoiceInfo(List<GetTimesheetDetailDto> listProject)
        {
            var listMainProjectInfo = listProject.Where(s => s.IsMainProjectInvoice)
                .Select(s => new { s.ProjectId, s.ProjectName, s.Discount, s.TransferFee, s.ClientCode, s.Currency, s.InvoiceNumber, s.PaymentDueBy })
                .ToList();

            var listSubProject = listProject.Where(s => s.MainProjectId.HasValue)
                .Select(s => new { s.ProjectId, MainProjectId = s.MainProjectId.Value, s.ProjectName })
                .ToList();

            var dicProjectIdToInvoiceInfo = (from m in listMainProjectInfo
                                             join s in listSubProject on m.ProjectId equals s.MainProjectId into ss
                                             select new
                                             {
                                                 m.ProjectId,
                                                 InvoiceInfo = new InvoiceSettingDto
                                                 {
                                                     ProjectName = m.ProjectName,
                                                     TransferFee = m.TransferFee,
                                                     Discount = m.Discount,
                                                     ClientCode = m.ClientCode,
                                                     CurrencyCode = m.Currency,
                                                     InvoiceNumber = m.InvoiceNumber,
                                                     PaymentDueBy = m.PaymentDueBy,
                                                     SubProjects = ss.Select(x => new IdNameDto { Id = x.ProjectId, Name = x.ProjectName }).ToList()
                                                 }
                                             }).ToDictionary(s => s.ProjectId, s => s.InvoiceInfo);

            return dicProjectIdToInvoiceInfo;
        }

        private void UpdateInvoiceSetting(List<GetTimesheetDetailDto> listToUpdate, Dictionary<long, InvoiceSettingDto> dicProjectIdToInvoiceInfo)
        {
            listToUpdate.ForEach(dto =>
            {
                if (dto.IsMainProjectInvoice)
                {
                    dto.SubProjects = dicProjectIdToInvoiceInfo.ContainsKey(dto.ProjectId) ? dicProjectIdToInvoiceInfo[dto.ProjectId].SubProjects : null;
                }
                else
                {
                    if (dto.MainProjectId.HasValue)
                    {
                        dto.MainProjectName = dicProjectIdToInvoiceInfo.ContainsKey(dto.MainProjectId.Value) ? dicProjectIdToInvoiceInfo[dto.MainProjectId.Value].ProjectName : null;

                        dto.Discount = dicProjectIdToInvoiceInfo.ContainsKey(dto.MainProjectId.Value) ? dicProjectIdToInvoiceInfo[dto.MainProjectId.Value].Discount : 0;
                    }
                    dto.TransferFee = 0;
                }
            });
        }

        private IOrderedQueryable<GetTimesheetDetailDto> GetTimesheetDetail(long timesheetId, bool allowViewBillRate = false, bool allowViewAllTSProject = false, bool isShortInfo = false)
        {
            var query = WorkScope.GetAll<TimesheetProject>()
                .Where(x => x.TimesheetId == timesheetId)
                .Where(x => allowViewAllTSProject || x.Project.PMId == AbpSession.UserId);

            if (isShortInfo)
            {
                return GetShortTimesheetProjectInfo(query);
            }
            else
            {
                return GetFullTimesheetProjectInfo(query, allowViewBillRate);
            }
        }

        private IOrderedQueryable<GetTimesheetDetailDto> GetShortTimesheetProjectInfo(IQueryable<TimesheetProject> timesheetProjectsInfo)
        {
            var defaultWorkingHours = Convert.ToInt32(SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.DefaultWorkingHours).Result);

            return timesheetProjectsInfo.Select(tsp => new GetTimesheetDetailDto
            {
                Id = tsp.Id,
                ProjectId = tsp.ProjectId,
                TimesheetId = tsp.TimesheetId,
                ProjectName = tsp.Project.Name,
                ClientId = tsp.Project.ClientId,
                ClientCode = tsp.Project.Client != null ? tsp.Project.Client.Code : default,
                ProjectBillInfomation = WorkScope.GetAll<TimesheetProjectBill>()
                                                    .Where(x => x.TimesheetId == tsp.TimesheetId && x.ProjectId == tsp.ProjectId && x.IsActive)
                                                    .OrderBy(x => x.User.EmailAddress)
                                                    .Select(x => new TimesheetProjectBillInfoDto
                                                    {
                                                        BillRate = x.BillRate,
                                                        WorkingTime = x.WorkingTime,
                                                        StartTime = x.StartTime,
                                                        EndTime = x.EndTime,
                                                        Currency = x.Currency.Code,
                                                        ChargeType = x.ChargeType.HasValue ? x.ChargeType : x.Project.ChargeType,
                                                        DefaultWorkingHours = defaultWorkingHours,
                                                        TimeSheetWorkingDay = tsp.WorkingDay,
                                                    }).ToList(),
                ProjectCurrency = tsp.Project.Currency.Code,
                ProjectChargeType = tsp.Project.ChargeType,
                InvoiceNumber = tsp.InvoiceNumber,
                WorkingDay = tsp.WorkingDay,
                Discount = tsp.Discount,
                TransferFee = tsp.TransferFee,
                MainProjectId = tsp.ParentInvoiceId,
                PaymentDueBy = tsp.Project.Client.PaymentDueBy,
                StartDate = tsp.Project.StartTime,
                EndDate = tsp.Project.EndTime,
                IsActive = tsp.IsActive,
                ProjectType = tsp.Project.ProjectType,
                ProjectStatus = tsp.Project.Status
            }).OrderByDescending(x => x.ClientId);
        }

        private IOrderedQueryable<GetTimesheetDetailDto> GetFullTimesheetProjectInfo(IQueryable<TimesheetProject> timesheetProjectsInfo, bool allowViewBillRate = false)
        {
            var defaultWorkingHours = Convert.ToInt32(SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.DefaultWorkingHours).Result);

            return timesheetProjectsInfo.Select(tsp => new GetTimesheetDetailDto
            {
                Id = tsp.Id,
                ProjectId = tsp.ProjectId,
                TimesheetId = tsp.TimesheetId,
                ProjectName = tsp.Project.Name,
                ProjectCode = tsp.Project.Code,
                PmId = tsp.Project.PMId,
                PmUserType = tsp.Project.PM.UserType,
                PmAvatarPath = tsp.Project.PM.AvatarPath,
                PmBranch = tsp.Project.PM.BranchOld,
                PmBranchColor = tsp.Project.PM.Branch.Color,
                PmBranchDisplayName = tsp.Project.PM.Branch.DisplayName,
                PmEmailAddress = tsp.Project.PM.EmailAddress,
                PmFullName = tsp.Project.PM.Name + " " + tsp.Project.PM.Surname,
                ClientId = tsp.Project.ClientId,
                ClientName = tsp.Project.Client != null ? tsp.Project.Client.Name : default,
                ClientCode = tsp.Project.Client != null ? tsp.Project.Client.Code : default,
                FilePath = tsp.FilePath,
                ProjectBillInfomation = WorkScope.GetAll<TimesheetProjectBill>()
                                                    .Where(x => x.TimesheetId == tsp.TimesheetId && x.ProjectId == tsp.ProjectId && x.IsActive)
                                                    .OrderBy(x => x.User.EmailAddress)
                                                    .Select(x => new TimesheetProjectBillInfoDto
                                                    {
                                                        UserFullName = x.User.FullName,
                                                        AccountName = x.AccountName,
                                                        BillRate = allowViewBillRate ? x.BillRate : 0,
                                                        BillRole = x.BillRole,
                                                        WorkingTime = x.WorkingTime,
                                                        StartTime = x.StartTime,
                                                        EndTime = x.EndTime,
                                                        Description = x.Note,
                                                        Currency = x.Currency.Name,
                                                        ChargeType = x.ChargeType.HasValue ? x.ChargeType : x.Project.ChargeType,
                                                        DefaultWorkingHours = defaultWorkingHours,
                                                        TimeSheetWorkingDay = tsp.WorkingDay,
                                                    }).ToList(),
                Note = tsp.Note,
                HistoryFile = tsp.HistoryFile,
                HasFile = !string.IsNullOrEmpty(tsp.FilePath),
                IsComplete = tsp.IsComplete.Value == true ? true : false,
                RequireTimesheetFile = tsp.Project.RequireTimesheetFile,
                ProjectCurrency = tsp.Project.Currency.Name,
                ProjectChargeType = tsp.Project.ChargeType,
                InvoiceNumber = tsp.InvoiceNumber,
                WorkingDay = tsp.WorkingDay,
                Discount = tsp.Discount,
                TransferFee = tsp.TransferFee,
                MainProjectId = tsp.ParentInvoiceId,
                StartDate = tsp.Project.StartTime,
                EndDate = tsp.Project.EndTime,
                IsActive = tsp.IsActive,
                ProjectType = tsp.Project.ProjectType,
                ProjectStatus = tsp.Project.Status,
                ListProjectCodes = tsp.Project.ListProjectCodes
            }).OrderByDescending(x => x.ClientId);
        }

        private List<GetTimesheetDetailDto> GetClosedTimesheetProject(List<GetTimesheetDetailDto> listTimesheetDetail)
        {
            var tsProjectBackgroudJobName = typeof(ReactivetimesheetProjectBackgroudJob).FullName;
            var tsProjectId = "TimesheetProjectId";
            // get all time sheet project close time in background job
            var closeTimeJobs = _storeJob.GetAll()
                 .Where(s => s.JobType.Contains(tsProjectBackgroudJobName))
                 .Where(s => s.JobArgs.Contains(tsProjectId))
                 .Select(s => new
                 {
                     ID = JsonConvert.DeserializeObject<ReactiveTimesheetBGJDto>(s.JobArgs).TimesheetProjectId,
                     Time = s.NextTryTime
                 })
                 .ToDictionary(key => key.ID);
            foreach (var item in listTimesheetDetail)
            {
                if (closeTimeJobs.TryGetValue(item.Id, out var backgroundJobInfo))
                {
                    item.CloseTime = backgroundJobInfo.Time.ToString("dd-MM-yyyy HH:mm");
                }
            }
            return listTimesheetDetail;
        }

        public async Task<TimesheetProject> CreateTimesheetProject(TimesheetProjectDto input, long invoiceNumber, float transferFee, float discount, float workingDay, long? parentInvoiceId)
        {
            var timesheetProject = new TimesheetProject
            {
                ProjectId = input.ProjectId,
                TimesheetId = input.TimesheetId,
                IsComplete = false,
                InvoiceNumber = invoiceNumber,
                TransferFee = transferFee,
                Discount = discount,
                WorkingDay = workingDay,
                Note = input.Note,
                ParentInvoiceId = parentInvoiceId
            };

            return await WorkScope.InsertAsync(timesheetProject);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_AddProjectToTimesheet)]
        public async Task<Object> Create(TimesheetProjectDto input)
        {
            var isExist = await WorkScope.GetAll<TimesheetProject>()
               .AnyAsync(x => x.ProjectId == input.ProjectId && x.TimesheetId == input.TimesheetId);
            if (isExist)
                throw new UserFriendlyException($"ProjectId {input.ProjectId} already exist in this Timesheet.");

            var project = await WorkScope.GetAll<Project>()
                .Where(s => s.Id == input.ProjectId)
                .Select(s => new
                {
                    Project = s,
                    s.Client.TransferFee
                }).FirstOrDefaultAsync();

            if (project.Project.IsCharge != true || project.Project.Status != ProjectStatus.InProgress)
            {
                throw new UserFriendlyException($"Project Id {input.ProjectId} is No-charged or Not Inprogress. So, you can't add it to the timesheet");
            }

            var timesheet = await _timesheetManager.GetTimesheetById(input.TimesheetId);
            if (timesheet == default || !timesheet.IsActive)
            {
                throw new UserFriendlyException($"The timesheet Id {input.TimesheetId} is not exist or not active");
            }

            await _timesheetManager.DeleteTimesheetProjectBill(input.ProjectId, timesheet.Id);

            var listPUB = await _timesheetManager.GetListProjectUserBillDto(timesheet.Year, timesheet.Month, input.ProjectId);

            if (listPUB == null || listPUB.IsEmpty())
            {
                throw new UserFriendlyException($"Project has no Bill Account!");
            }

            var invoiceNumber = project.Project.LastInvoiceNumber + 1;
            float discount = project.Project.Discount;
            float transferFee = project.TransferFee;

            var timesheetProject = await CreateTimesheetProject(input, invoiceNumber, transferFee, discount, (float)timesheet.TotalWorkingDay, project.Project.ParentInvoiceId);

            project.Project.LastInvoiceNumber = invoiceNumber;

            var listTimesheetProjectBill = new List<TimesheetProjectBill>();

            foreach (var pub in listPUB)
            {
                var timesheetProjectBill = new TimesheetProjectBill
                {
                    ProjectId = pub.ProjectId,
                    AccountName = pub.AccountName,
                    TimesheetId = timesheet.Id,
                    UserId = pub.UserId,
                    BillRole = pub.BillRole,
                    BillRate = pub.BillRate,
                    StartTime = pub.StartTime,
                    EndTime = pub.EndTime,
                    IsActive = true,
                    ChargeType = pub.ChargeType,
                    CurrencyId = project.Project.CurrencyId,
                };
                listTimesheetProjectBill.Add(timesheetProjectBill);
            }

            await WorkScope.InsertRangeAsync(listTimesheetProjectBill);
            await CurrentUnitOfWork.SaveChangesAsync();
            return new
            {
                TimesheetProject = timesheetProject,
                ListTimesheetProjectBill = listTimesheetProjectBill
            };
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail)]
        public async Task<List<ProjectDto>> GetAllRemainProjectInTimesheet(long timesheetId)
        {
            var timesheetProjects = WorkScope.GetAll<TimesheetProject>().Where(x => x.TimesheetId == timesheetId).Select(x => x.ProjectId);
            var query = WorkScope.GetAll<Project>().Where(x => !timesheetProjects.Contains(x.Id) && x.IsCharge == true && x.Status != ProjectStatus.Potential && x.Status != ProjectStatus.Closed)
                                .Select(x => new ProjectDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Code = x.Code,
                                    ProjectType = x.ProjectType,
                                    StartTime = x.StartTime.Date,
                                    EndTime = x.EndTime.Value.Date,
                                    Status = x.Status
                                });

            return await query.ToListAsync();
        }

        [HttpPut]
        public async Task<SetTimesheetProjectCompleteDto> SetComplete(SetTimesheetProjectCompleteDto input)
        {
            var timesheetProject = await WorkScope.GetAsync<TimesheetProject>(input.Id);
            timesheetProject.IsComplete = input.IsComplete;
            await CurrentUnitOfWork.SaveChangesAsync();
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_Delete)]
        public async Task Delete(long timesheetProjectId)
        {
            var timeSheetProject = await WorkScope.GetAll<TimesheetProject>()
                .Include(x => x.Timesheet)
                .FirstOrDefaultAsync(x => x.Id == timesheetProjectId);

            if (!timeSheetProject.Timesheet.IsActive)
            {
                throw new UserFriendlyException("Timesheet not active !");
            }

            if (timeSheetProject.FilePath != null)
            {
                throw new UserFriendlyException("Timesheet already has attachments, cannot be deleted !");
            }
            var timesheetProjectBills = await WorkScope.GetAll<TimesheetProjectBill>()
                .Where(x => x.TimesheetId == timeSheetProject.TimesheetId && x.ProjectId == timeSheetProject.ProjectId)
                .ToListAsync();

            foreach (var tsProjectBill in timesheetProjectBills)
            {
                tsProjectBill.IsDeleted = true;
            }
            timeSheetProject.IsDeleted = true;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_UploadTimesheetFile)]
        public async Task UpdateFileTimeSheetProject([FromForm] FileInputDto input)
        {
            var timesheetProjectId = input.TimesheetProjectId;
            var timesheet = WorkScope.GetAll<TimesheetProject>()
              .Where(x => x.Id == timesheetProjectId)
              .Select(s => new
              {
                  s.Timesheet.Year,
                  s.Timesheet.Month,
                  s.Project.Code,
                  IsTimesheetActive = s.Timesheet.IsActive,
                  IsTimesheetProjectActive = s.IsActive,
                  s.Project.Name,
                  s.ProjectId,
                  TimsheetProject = s
              }).FirstOrDefault();

            if (timesheet == default)
            {
                throw new UserFriendlyException($"TimesheetProjectId {timesheetProjectId} is NOT exist");
            }

            if (!timesheet.IsTimesheetActive && !timesheet.IsTimesheetProjectActive)
            {
                throw new UserFriendlyException($"TimesheetProjectId{timesheetProjectId} is NOT active -> contact sale to active");
            }

            if (input.File == null)
            {
                timesheet.TimsheetProject.FilePath = null;
            }
            else
            {
                var filename = DateTimeUtils.yyyyMM(timesheet.Year, timesheet.Month) + "_" + timesheet.Code + "_" + input.File.FileName;
                var filePath = await _uploadFileService.UploadTimesheetFileAsync(input.File, timesheet.Year, timesheet.Month, filename);

                timesheet.TimsheetProject.FilePath = filePath;
            }

            CurrentUnitOfWork.SaveChanges();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_UploadTimesheetFile)]
        public async Task UpdateFileTimeSheetProject1([FromForm] FileInputDto input)
        {
            String path = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "timesheets");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var timesheetProject = await WorkScope.GetAll<TimesheetProject>()
                .Include(x => x.Project)
                .Include(x => x.Timesheet)
                .Where(x => x.Id == input.TimesheetProjectId)
                .FirstOrDefaultAsync();

            if (!timesheetProject.Timesheet.IsActive)
            {
                throw new UserFriendlyException("Timesheet not active !");
            }

            var now = DateTimeUtils.GetNow();
            var user = await WorkScope.GetAsync<User>(AbpSession.UserId.Value);
            var userName = UserHelper.GetUserName(user.EmailAddress);
            if (user != null && !user.KomuUserId.HasValue)
            {
                user.KomuUserId = await _komuService.GetKomuUserId(new KomuUserDto { Username = userName ?? user.UserName });
                await WorkScope.UpdateAsync(user);
            }
            var historyFile = new StringBuilder();
            var message = new StringBuilder();
            var projectUri = await _settingManager.GetSettingValueForApplicationAsync(AppSettingNames.ProjectUri);
            var titlelink = $"{projectUri}/app/list-project-detail/timesheet-tab?id={timesheetProject.ProjectId}";

            if (input != null && input.File != null && input.File.Length > 0)
            {
                string fileName = input.File.FileName;
                string FileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
                if (FileExtension == "xlsx" || FileExtension == "xltx" || FileExtension == "docx")
                {
                    var filePath = timesheetProject.Timesheet.Year + "-" + timesheetProject.Timesheet.Month + "_" + timesheetProject.Project.Code + "_" + fileName;
                    if (timesheetProject.FilePath != null && timesheetProject.FilePath != fileName)
                    {
                        File.Delete(Path.Combine(path, timesheetProject.FilePath));

                        timesheetProject.FilePath = null;
                        await WorkScope.UpdateAsync(timesheetProject);
                    }

                    using (var stream = System.IO.File.Create(Path.Combine(path, filePath)))
                    {
                        await input.File.CopyToAsync(stream);
                        timesheetProject.FilePath = filePath;
                    }
                }
                else
                {
                    throw new UserFriendlyException(String.Format("Only accept files xlsx, xltx, docx !"));
                }

                // thêm history upload file
                historyFile.Append($"{now.ToString("yyyy/MM/dd HH:mm")} {user.UserName} upload {timesheetProject.FilePath}<br>");
                message.AppendLine($"Chào bạn lúc **{now.ToString("yyyy/MM/dd HH:mm")}** có **{userName ?? user.UserName}** upload **{timesheetProject.FilePath}** vào project " +
                            $"\"**{timesheetProject.Project.Name}**\" trong đợt timesheet \"**{timesheetProject.Timesheet.Name}**\".");
            }
            else
            {
                historyFile.Append($"{now.ToString("yyyy/MM/dd HH:mm")} {user.UserName} delete {timesheetProject.FilePath}<br>");
                message.AppendLine($"Chào bạn lúc **{now.ToString("yyyy/MM/dd HH:mm")}** có **{userName ?? user.UserName}** delete **{timesheetProject.FilePath}** vào project " +
                           $"\"**{timesheetProject.Project.Name}**\" trong đợt timesheet \"**{timesheetProject.Timesheet.Name}**\".");

                File.Delete(Path.Combine(path, timesheetProject.FilePath));
                timesheetProject.FilePath = null;
            }

            var komuUserNameSetting = await _settingManager.GetSettingValueForApplicationAsync(AppSettingNames.KomuUserNames);
            var komuUserNames = komuUserNameSetting.Split(";").ToList();
            komuUserNames.RemoveAt(komuUserNames.Count - 1);
            message.AppendLine(titlelink);
            foreach (var username in komuUserNames)
            {
                _komuService.NotifyToChannel(new KomuMessage
                {
                    UserName = username,
                    Message = message.ToString(),
                }, ChannelTypeConstant.USER_ONLY);
            }
            timesheetProject.HistoryFile += historyFile;
            await WorkScope.UpdateAsync(timesheetProject);
        }

        [HttpGet]
        public async Task<object> DownloadFileTimesheetProject(long timesheetProjectId)
        {
            var filePath = WorkScope.GetAll<TimesheetProject>()
                .Where(s => s.Id == timesheetProjectId)
                .Select(s => s.FilePath)
                .FirstOrDefault();

            var data = await _uploadFileService.DownloadTimesheetFileAsync(filePath);
            var fileName = FileUtils.GetFileName(filePath);
            return new
            {
                FileName = fileName,
                Data = data
            };
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_UpdateNote)]
        public async Task<IActionResult> UpdateNote(UpdateTsProjectNoteDto input)
        {
            var projectTimesheet = await WorkScope.GetAll<TimesheetProject>().FirstOrDefaultAsync(x => x.Id == input.Id);
            if (projectTimesheet != null)
            {
                projectTimesheet.Note = input.Note;
                await WorkScope.UpdateAsync<TimesheetProject>(projectTimesheet);
                return new OkObjectResult("Update Note Success!");
            }
            return new BadRequestObjectResult("Not Found Timesheet Project!");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPM()
        {
            var pms = await WorkScope.GetAll<Project>()
                .Select(u => new
                {
                    Id = u.PMId,
                    UserName = u.PM.UserName,
                    Name = u.PM.Name,
                    Surname = u.PM.Surname,
                    EmailAddress = u.PM.EmailAddress,
                    FullName = u.PM.FullName,
                    AvatarPath = u.PM.AvatarPath,
                    UserType = u.PM.UserType,
                    UserLevel = u.PM.UserLevel
                })
                .Distinct()
                .ToListAsync();
            return new OkObjectResult(pms);
        }

        public async Task<TimesheetChartDto> GetBillInfoChart(long projectId, DateTime? fromDate, DateTime? toDate)
        {
            DateTime endDate = toDate.HasValue ? toDate.Value : DateTimeUtils.GetNow().AddMonths(-1);
            DateTime startDate = fromDate.HasValue ? fromDate.Value : endDate.AddMonths(-6);
            var toYear = endDate.Year;
            var toMonth = endDate.Month;
            var fromYear = startDate.Year;
            var fromMonth = startDate.Month;

            var mapTimesheet = await WorkScope.GetAll<TimesheetProjectBill>()
                .Where(s => s.ProjectId == projectId)
                .Where(s => s.TimeSheet.Year > fromYear || (s.TimeSheet.Year == fromYear && s.TimeSheet.Month >= fromMonth))
                .Where(s => s.TimeSheet.Year < toYear || (s.TimeSheet.Year == toYear && s.TimeSheet.Month <= toMonth))
                .GroupBy(s => new { s.TimesheetId, s.TimeSheet.Year, s.TimeSheet.Month, s.TimeSheet.TotalWorkingDay })
                .Select(s => new
                {
                    s.Key.Year,
                    s.Key.Month,
                    TotalWorkingDay = s.Key.TotalWorkingDay.HasValue ? s.Key.TotalWorkingDay : 22,
                    ManDay = s.Sum(x => x.WorkingTime),
                }).ToDictionaryAsync(s => s.Year + "-" + s.Month);

            var listMonth = DateTimeUtils.GetListMonth(startDate, endDate);
            var listManDay = new List<double>();
            var listManMonth = new List<double>();
            foreach (var date in listMonth)
            {
                var key = date.Year + "-" + date.Month;
                var manDay = mapTimesheet.ContainsKey(key) ? mapTimesheet[key].ManDay : 0;
                listManDay.Add(manDay);

                var manMonth = mapTimesheet.ContainsKey(key) ? Math.Round((double)(manDay / mapTimesheet[key].TotalWorkingDay), 2) : 0;
                listManMonth.Add(manMonth);
            }

            var result = new TimesheetChartDto()
            {
                Labels = listMonth.Select(s => DateTimeUtils.GetMonthName(s)).ToList(),
                ManDays = listManDay,
                ManMonths = listManMonth
            };

            return result;
        }

        private async Task<TimesheetTaxDto> GetTimesheetDetailForTaxInTimesheetTool(TimesheetDetailForTaxDto input)
        {
            return await _timesheetService.GetTimesheetDetailForTax(input);
        }

        private ExcelWorksheet CopySheet(ExcelWorkbook workbook, string existingWorksheetName, string newWorksheetName)
        {
            ExcelWorksheet worksheet = workbook.Worksheets.Copy(existingWorksheetName, newWorksheetName);
            return worksheet;
        }

        private async Task<InvoiceData> GetInvoiceData(InputExportInvoiceDto input, bool throwExceptionIfEmpty = true)
        {
            var defaultWorkingHours = Convert.ToInt32(await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.DefaultWorkingHours));
            var result = new InvoiceData();
            var listTSProjectCode = WorkScope.All<Project>()
                .Where(s => input.ProjectIds.Contains(s.Id)).Select(x => x.ListProjectCodes).ToList()
                .Where(x => !string.IsNullOrEmpty(x))
                .SelectMany(x => x.Split(',').Select(code => code.Trim()));

            var qtimesheetProject = WorkScope.All<TimesheetProject>()
                .Include(x => x.Project)
                .ThenInclude(p => p.Client)
                .Include(x => x.Project.Currency)
                .Include(x => x.Timesheet)
                .Where(s => s.TimesheetId == input.TimesheetId)
                .Where(s => input.ProjectIds.Contains(s.ProjectId));

            var qtimesheetProjectBill = WorkScope.All<TimesheetProjectBill>()
                .Where(s => s.TimesheetId == input.TimesheetId)
                .Where(s => s.IsActive)
                .Where(s => s.WorkingTime > 0)
                .Where(s => input.ProjectIds.Contains(s.ProjectId));

            result.Info = qtimesheetProject
                .WhereIf(throwExceptionIfEmpty, s => !s.ParentInvoiceId.HasValue)
                .Select(s => new InvoiceGeneralInfo
                {
                    ClientAddress = s.Project.Client.Address,
                    ClientName = s.Project.Client.Name,
                    Discount = s.Discount,
                    InvoiceNumber = s.InvoiceNumber,
                    PaymentDueBy = s.Project.Client.PaymentDueBy,
                    PaymentInfo = s.Project.Currency.InvoicePaymentInfo,
                    TransferFee = s.TransferFee,
                    Year = s.Timesheet.Year,
                    Month = s.Timesheet.Month,
                    InvoiceDateSetting = s.Project.Client.InvoiceDateSetting
                }).FirstOrDefault();

            if (throwExceptionIfEmpty && result.Info == default)
            {
                throw new UserFriendlyException("You have to select at least 1 project is MAIN in Invoice Setting");
            }

            result.TimesheetUsers = await (from tpb in qtimesheetProjectBill
                                           from tp in qtimesheetProject.Select(s => new { s.ProjectId, s.WorkingDay })
                                           where tpb.ProjectId == tp.ProjectId
                                           select new TimesheetUser
                                           {
                                               BillRate = tpb.BillRate,
                                               ChargeType = tpb.ChargeType.HasValue ? tpb.ChargeType.Value : tpb.Project.ChargeType.Value,
                                               CurrencyName = tpb.Currency.Name,
                                               UserFullName = tpb.User.FullName,
                                               AccountName = tpb.AccountName,
                                               Mode = input.Mode,
                                               DefaultWorkingHours = defaultWorkingHours,
                                               ProjectName = tpb.Project.Name,
                                               TimesheetWorkingDay = tp.WorkingDay,
                                               WorkingDay = tpb.WorkingTime,
                                               UserId = tpb.UserId,
                                               EmailAddress = tpb.User.EmailAddress,
                                               ProjectCode = tpb.Project.Code,
                                               EndTime = tpb.EndTime,
                                               StartTime = tpb.StartTime,
                                           }).ToListAsync();
            result.ProjectCodes = result.TimesheetUsers.Select(x => x.ProjectCode).Union(listTSProjectCode).ToList();
            return result;
        }

        private void FillDataToExcelFileInvoiceSheet(ExcelPackage excelPackageIn, InvoiceData data)
        {
            var invoiceSheet = excelPackageIn.Workbook.Worksheets[0];

            var tsTable = invoiceSheet.Tables.First();
            var tsTableStart = tsTable.Address.Start;
            invoiceSheet.InsertRow(tsTableStart.Row + 1, data.TimesheetUsers.Count - 1, tsTableStart.Row + data.TimesheetUsers.Count);
            int rowIndex = tsTableStart.Row + 1;

            double sumLineTotal = 0;
            foreach (var tsUser in data.TimesheetUsers)
            {
                //Fill data sheet invoice
                invoiceSheet.Cells[rowIndex, 2].Value = tsUser.FullName;
                invoiceSheet.Cells[rowIndex, 3].Value = tsUser.ProjectName;
                invoiceSheet.Cells[rowIndex, 4].Value = tsUser.BillRateDisplay;
                invoiceSheet.Cells[rowIndex, 5].Value = tsUser.CurrencyName + "/" + tsUser.ChargeTypeDisplay;
                invoiceSheet.Cells[rowIndex, 6].Value = tsUser.WorkingDayDisplay;
                invoiceSheet.Cells[rowIndex, 7].Value = tsUser.LineTotal;
                sumLineTotal += tsUser.LineTotal;
                rowIndex++;
            }

            var netTotal = sumLineTotal;
            var invoiceTotal = netTotal + data.Info.TransferFee - (data.Info.Discount * netTotal) / 100;
            invoiceSheet.Cells["ClientName"].Value = data.Info.ClientName;
            invoiceSheet.Cells["ClientAddress"].Value = data.Info.ClientAddress;
            invoiceSheet.Cells["InvoiceNumber"].Value = data.Info.InvoiceNumber;
            invoiceSheet.Cells["InvoiceDate"].Value = data.Info.InvoiceDateStr();
            invoiceSheet.Cells["PaymentDueBy"].Value = $"PAYMENT DUE BY: {data.Info.PaymentDueByStr()}";
            invoiceSheet.Names["TransferFee"].Value = data.Info.TransferFee;
            invoiceSheet.Names["InvoiceNetTotal"].Value = netTotal;
            invoiceSheet.Names["DiscountLabel"].Value = $"Discount ({data.Info.Discount}%)";
            invoiceSheet.Names["Discount"].Value = (data.Info.Discount * netTotal) / 100;
            invoiceSheet.Names["InvoiceTotal"].Value = invoiceTotal;
            invoiceSheet.Names["InvoiceTotalTop"].Value = invoiceTotal;
            invoiceSheet.Cells["Currency"].Value = data.CurrencyName();

            //Fill data payment
            string[] arrPaymentInfo = data.Info.PaymentInfoArr();
            var arrPaymentInfoCount = arrPaymentInfo.Length;

            var indexPayment = invoiceSheet.Names["PaymentDetails"].Start.Row;
            invoiceSheet.InsertRow(indexPayment, arrPaymentInfoCount, indexPayment + arrPaymentInfoCount);

            invoiceSheet.Names["CurrencyTotal"].Value = $"{data.CurrencyName()} TOTAL";

            for (int i = 0; i < arrPaymentInfoCount; i++)
            {
                invoiceSheet.Cells["B" + indexPayment].Value = arrPaymentInfo[i];
                indexPayment++;
            }

            invoiceSheet.Cells["B" + indexPayment].Value = $"Payment Reference: {data.Info.InvoiceNumber}";
        }

        private void FillDataToExcelFileTSDetail(ExcelPackage excelPackageIn, InvoiceData data)
        {

            var invoiceSheet = excelPackageIn.Workbook.Worksheets[0];
            var tsTable = invoiceSheet.Tables.First();
            var tsTableStart = tsTable.Address.Start;
            invoiceSheet.InsertRow(tsTableStart.Row + 1, data.TimesheetUsers.Count - 1, tsTableStart.Row + data.TimesheetUsers.Count);

            int rowIndex = tsTableStart.Row + 1;
            double sumLineTotal = 0;
            int sequenceNumber = 1;

            foreach (var tsUser in data.TimesheetUsers)
            {
                // Fill data sheet invoice
                invoiceSheet.Cells[rowIndex, 2].Value = sequenceNumber;
                invoiceSheet.Cells[rowIndex, 3].Value = tsUser.FullName;
                invoiceSheet.Cells[rowIndex, 4].Value = tsUser.ProjectName;
                invoiceSheet.Cells[rowIndex, 5].Value = tsUser.WorkingDayDisplay;

                sumLineTotal += tsUser.LineTotal;
                rowIndex++;
                sequenceNumber++;
            }
        }

        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_ExportInvoice)]
        public async Task<FileBase64Dto> ExportInvoice(InputExportInvoiceDto input)
        {
            var data = await GetInvoiceData(input);

            var templateFilePath = Path.Combine(templateFolder, "Invoice.xlsx");

            using (var memoryStream = new MemoryStream(File.ReadAllBytes(templateFilePath)))
            {
                using (var excelPackageIn = new ExcelPackage(memoryStream))
                {
                    FillDataToExcelFileInvoiceSheet(excelPackageIn, data);
                    string fileBase64 = Convert.ToBase64String(excelPackageIn.GetAsByteArray());

                    return new FileBase64Dto
                    {
                        FileName = data.ExportFileName(),
                        FileType = MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet,
                        Base64 = fileBase64
                    };
                }
            }
        }

        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_ExportInvoice)]
        public async Task<FileExportInvoiceDto> ExportInvoiceAsPDF(InputExportInvoiceDto input)
        {
            var result = await GetInvoiceData(input);
            try
            {
                ExportInvoicePDFHelper export = new ExportInvoicePDFHelper(this._hostingEnvironment);
                FileExportInvoiceDto fileExport = export.ExportInvoiceDataPdf(result);
                return fileExport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceForTax)]
        public async Task<FileBase64Dto> ExportInvoiceForTax(InputExportInvoiceDto input)
        {
            var dataInvoice = await GetInvoiceData(input);
            List<TimesheetDetailUser> dataTimesheetDetail = await GetTimesheetDetailData(dataInvoice);

            var templateFilePath = Path.Combine(templateFolder, "Invoice.xlsx");

            using (var memoryStream = new MemoryStream(File.ReadAllBytes(templateFilePath)))
            {
                using (var excelPackageIn = new ExcelPackage(memoryStream))
                {
                    FillDataToExcelFileInvoiceSheet(excelPackageIn, dataInvoice);
                    FillDataToSheetTimesheetDetail(excelPackageIn, dataTimesheetDetail);
                    string fileBase64 = Convert.ToBase64String(excelPackageIn.GetAsByteArray());

                    return new FileBase64Dto
                    {
                        FileName = dataInvoice.ExportFileName(),
                        FileType = MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet,
                        Base64 = fileBase64
                    };
                }
            }
        }

        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_ExportTSdetail)]
        public async Task<FileBase64Dto> ExportTSdetail(InputExportInvoiceDto input)
        {
            var dataInvoice = await GetInvoiceData(input, throwExceptionIfEmpty: false);
            List<TimesheetDetailUser> dataTimesheetDetail = await GetTimesheetDetailData(dataInvoice, false);

            var templateFilePath = Path.Combine(templateFolder, "TSDetail.xlsx");

            using (var memoryStream = new MemoryStream(File.ReadAllBytes(templateFilePath)))
            {
                using (var excelPackageIn = new ExcelPackage(memoryStream))
                {
                    FillDataToExcelFileTSDetail(excelPackageIn, dataInvoice);
                    FillDataToSheetTimesheetDetail(excelPackageIn, dataTimesheetDetail);
                    string fileBase64 = Convert.ToBase64String(excelPackageIn.GetAsByteArray());

                    return new FileBase64Dto
                    {
                        FileName = dataInvoice.ExportTimesheetFileName(),
                        FileType = MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet,
                        Base64 = fileBase64
                    };
                }
            }
        }

        private async Task<List<TimesheetDetailUser>> GetTimesheetDetailData(InvoiceData invoiceData, bool isFilterProjectCode = true)
        {

            int month = invoiceData.Info.Month;
            int year = invoiceData.Info.Year;

            var timesheetDetailForTaxDto = new TimesheetDetailForTaxDto
            {
                Year = invoiceData.Info.Year,
                Month = invoiceData.Info.Month,
                ProjectCodes = invoiceData.ProjectCodes
            };
            var timesheetDetailForTax = await GetTimesheetDetailForTaxInTimesheetTool(timesheetDetailForTaxDto);

            var listTimesheet = timesheetDetailForTax.ListTimesheet;
            var listWorkingDay = timesheetDetailForTax.ListWorkingDay;

            var resultList = new List<TimesheetDetailUser>();
            List<DateTime> listBillDate = new List<DateTime> { };

            int countProject = invoiceData.ProjectCodes.Count();
            foreach (var tsProject in invoiceData.TimesheetUsers)
            {
                var timesheetDetailUser = new TimesheetDetailUser { };
                var tsDetails = listTimesheet
                                                        .Where(x => x.EmailAddress == tsProject.EmailAddress)
                                                        .WhereIf(isFilterProjectCode, x => x.ProjectCode == tsProject.ProjectCode)
                                                        .OrderBy(x => x.DateAt).ToList();

                var firstDayOfMonth = DateTimeUtils.FirstDayOfMonth(new DateTime(year, month, 1));
                var lastDayOfMonth = DateTimeUtils.LastDayOfMonth(firstDayOfMonth);

                var startBillDate = new DateTime(Math.Max(firstDayOfMonth.Ticks, tsProject.StartTime.Ticks));
                var endTime = tsProject.EndTime.HasValue ? tsProject.EndTime.Value : lastDayOfMonth;
                var endBillDate = new DateTime(Math.Min(lastDayOfMonth.Ticks, endTime.Ticks));

                listBillDate = listWorkingDay
                                                .Where(x => x.Date >= startBillDate.Date && x.Date <= endBillDate.Date)
                                                .ToList();

                var timesheetDetails = TimesheetDetailOfUserInProject(listBillDate, tsDetails, tsProject.WorkingDay);
                timesheetDetailUser.FullName = tsProject.FullName;
                timesheetDetailUser.ProjectName = tsProject.ProjectName;
                timesheetDetailUser.ProjectNumber = countProject;
                timesheetDetailUser.TimesheetDetails = timesheetDetails;

                resultList.Add(timesheetDetailUser);
            }

            return resultList;
        }

        private void AddMoreBillDate(List<DateTime> listBillDate, double totalWorkingDay)
        {
            var maxBillDay = listBillDate.Count;
            if (totalWorkingDay <= maxBillDay)
            {
                return;
            }

            var firstBillDate = listBillDate.FirstOrDefault();

            var firstDateOfMonth = DateTimeUtils.FirstDayOfMonth(firstBillDate);
            var date = firstDateOfMonth;
            while (date.Month == firstDateOfMonth.Month || totalWorkingDay > maxBillDay)
            {
                if (date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday)
                {
                    listBillDate.Add(date);
                    maxBillDay += 1;
                }
                date = date.AddDays(1);
            }

            listBillDate = listBillDate.OrderBy(s => s.Ticks).ToList();
        }

        private List<TimesheetDetail> TimesheetDetailOfUserInProject(List<DateTime> listBillDate, List<TimesheetDetail> listTimesheetDetail, double totalWorkingDay)
        {
            AddMoreBillDate(listBillDate, totalWorkingDay);

            var resultList = new List<TimesheetDetail>();

            double remainDay = totalWorkingDay;

            double workingDay = 1;
            DateTime dateAtLast = DateTime.Today;

            foreach (var billDate in listBillDate)
            {
                if (remainDay <= 0) break;

                workingDay = Math.Min(remainDay, 1);
                remainDay -= workingDay;

                var tsDetail = listTimesheetDetail.Where(x => x.DateAt.Date == billDate.Date).FirstOrDefault();

                if (tsDetail == default)
                {
                    tsDetail = listTimesheetDetail.OrderBy(s => DateTimeUtils.DateDiffAbs(s.DateAt, billDate)).FirstOrDefault();
                }
                if (tsDetail != default)
                {
                    var timesheetDetail = new TimesheetDetail
                    {
                        DateAt = billDate,
                        ManDay = workingDay,
                        TaskName = tsDetail.TaskName,
                        Note = tsDetail.Note,
                    };
                    resultList.Add(timesheetDetail);
                }
            }

            return resultList;
        }

        private ExcelPackage FillDataToSheetTimesheetDetail(ExcelPackage excelPackageIn, List<TimesheetDetailUser> listTimesheetDetailOfUser)
        {
            int projectNumber = listTimesheetDetailOfUser.Select(x => x.ProjectNumber).FirstOrDefault();
            foreach (var timesheetDetailOfUser in listTimesheetDetailOfUser)
            {
                var nameSheetDetailTimesheet = timesheetDetailOfUser.FullName.Replace(" ", "") + "_" + timesheetDetailOfUser.ProjectName.Replace(" ", "");
                if (projectNumber == 1)
                {
                    nameSheetDetailTimesheet = timesheetDetailOfUser.FullName.Replace(" ", "");
                }
                var sheetDetailTimesheet = CopySheet(excelPackageIn.Workbook, "Detail", nameSheetDetailTimesheet);
                sheetDetailTimesheet.Cells["B2:E2"].Value = $"TIMESHEET OF {timesheetDetailOfUser.FullName.ToUpper()} IN {timesheetDetailOfUser.ProjectName.ToUpper()}"; ;
                var tsTable = sheetDetailTimesheet.Tables.First();
                if (timesheetDetailOfUser.TimesheetDetails.Count <= 0)
                {
                    continue;
                }
                var tsTableStart = tsTable.Address.Start;
                sheetDetailTimesheet.InsertRow(tsTableStart.Row + 1, timesheetDetailOfUser.TimesheetDetails.Count - 1, tsTableStart.Row + timesheetDetailOfUser.TimesheetDetails.Count);
                int rowIndex = tsTableStart.Row + 1;

                foreach (var timesheetDetail in timesheetDetailOfUser.TimesheetDetails)
                {
                    sheetDetailTimesheet.Cells[rowIndex, 2].Value = timesheetDetail.DateAt;
                    sheetDetailTimesheet.Cells[rowIndex, 3].Value = timesheetDetail.ManDay;
                    sheetDetailTimesheet.Cells[rowIndex, 4].Value = timesheetDetail.TaskName;
                    sheetDetailTimesheet.Cells[rowIndex, 5].Value = timesheetDetail.Note;
                    rowIndex++;
                }
            }

            return excelPackageIn;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_UpdateProjectCodes)]
        public async Task<string> UpdateProjectCodes(long projectId, string ListProjectCodes)
        {
            var project = await WorkScope.GetAll<Project>()
                .FirstOrDefaultAsync(x => x.Id == projectId);

            if (project == null)
            {
                throw new UserFriendlyException("Not found Project with projectId  provided.");
            }

            project.ListProjectCodes = ListProjectCodes;

            await CurrentUnitOfWork.SaveChangesAsync();

            return project.ListProjectCodes;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_EditInvoiceInfo)]
        public async Task<float> UpdateTimesheetProject(UpdateTimesheetProjectDto input)
        {
            var timesheetProject = await WorkScope.GetAll<TimesheetProject>().FirstOrDefaultAsync(x => x.Id == input.Id);
            if (timesheetProject == null)
            {
                return default;
            }

            long timesheetId = timesheetProject.TimesheetId;
            var timesheet = await WorkScope.GetAll<Timesheet>().Where(x => x.Id == timesheetId).Where(x => x.IsActive).FirstOrDefaultAsync();

            if (timesheet == default)
            {
                throw new UserFriendlyException($"Unable to update for closed timesheet Id {timesheet}");
            }

            timesheetProject.WorkingDay = input.WorkingDay;
            timesheetProject.InvoiceNumber = input.InvoiceNumber;
            timesheetProject.TransferFee = input.TransferFee;
            timesheetProject.Discount = input.Discount;

            var listTimesheetProjects = WorkScope.GetAll<TimesheetProject>()
                .Where(x => x.TimesheetId == timesheetProject.TimesheetId)
                .Where(x => x.ParentInvoiceId == timesheetProject.ProjectId)
                .ToList();

            listTimesheetProjects.ForEach(tsp =>
            {
                tsp.ParentInvoiceId = null;
                tsp.LastModificationTime = DateTimeUtils.GetNow();
                tsp.LastModifierUserId = AbpSession.UserId;
            });
            if (!input.IsMainProjectInvoice)
            {
                timesheetProject.ParentInvoiceId = input.MainProjectId;
            }
            else
            {
                timesheetProject.ParentInvoiceId = null;
                var subProjects = WorkScope.GetAll<TimesheetProject>().Where(x => input.SubProjectIds.Contains(x.ProjectId)).ToList();
                subProjects.ForEach(tp =>
                {
                    tp.ParentInvoiceId = timesheetProject.ProjectId;
                    tp.LastModificationTime = DateTimeUtils.GetNow();
                    tp.LastModifierUserId = AbpSession.UserId;
                });
            }

            CurrentUnitOfWork.SaveChanges();

            return timesheetProject.WorkingDay;
        }

        [HttpGet]
        public string CheckTimesheetProjectSetting(long timesheetId)
        {
            var listTimesheetProject = WorkScope.GetAll<TimesheetProject>()
             .Where(t => t.TimesheetId == timesheetId)
             .Where(s => s.Project.ProjectType == ProjectType.ODC || s.Project.ProjectType == ProjectType.TimeAndMaterials || s.Project.ProjectType == ProjectType.FIXPRICE)
             .Select(s => new { Id = s.Id, ProjectName = s.Project.Name, ParentInvoiceId = s.ParentInvoiceId, ProjectId = s.Project.Id })
             .ToList();

            var sb = new StringBuilder();
            foreach (var project in listTimesheetProject)
            {
                if (project.ParentInvoiceId.HasValue)
                {
                    var parrentProject = listTimesheetProject.Where(s => s.ProjectId == project.ParentInvoiceId.Value).FirstOrDefault();
                    if (parrentProject == default)
                    {
                        sb.AppendLine($"{project.ProjectName} is Sub but not found main project");
                    }
                    else
                    {
                        if (parrentProject.ParentInvoiceId.HasValue)
                        {
                            sb.AppendLine($"{project.ProjectName} is Sub of {parrentProject.ProjectName} but {parrentProject.ProjectName} is SUB too");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        #region Integrate Finfast

        [HttpGet]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_SendInvoiceToFinfast)]
        public async Task<ResponseResultProjectDto> SendInvoiceToFinfast(long timesheetId)
        {
            var timesheet = WorkScope.GetAll<Timesheet>()
                .Where(s => s.Id == timesheetId)
                .Select(s => new { s.Month, s.Year })
                .FirstOrDefault();

            var queryAll = GetTimesheetDetail(timesheetId, true, true, true);

            var listAllProject = queryAll.ToList();

            var dicMainProjectIdToInvoiceInfo = GetDicMainProjectIdToInvoiceInfo(listAllProject);

            UpdateInvoiceSetting(listAllProject, dicMainProjectIdToInvoiceInfo);

            var listInvoices = new List<CreateInvoiceDto>();
            foreach (var item in dicMainProjectIdToInvoiceInfo)
            {
                var mainProjectInvoiceMoney = listAllProject.Where(s => s.ProjectId == item.Key).Select(s => s.TotalAmountProjectBillInfomation).FirstOrDefault();

                var subProjectInvoiceMoney = listAllProject.Where(s => s.MainProjectId.HasValue)
                    .Where(s => s.MainProjectId == item.Key).Sum(s => s.TotalAmountProjectBillInfomation);

                listInvoices.Add(new CreateInvoiceDto
                {
                    ClientCode = item.Value.ClientCode,
                    CurrencyCode = item.Value.CurrencyCode,
                    InvoiceNumber = item.Value.InvoiceNumber.ToString(),
                    Month = (short)timesheet.Month,
                    Year = timesheet.Year,
                    NameInvoice = $"Invoice number {item.Value.InvoiceNumber}",
                    TransferFee = item.Value.TransferFee,
                    Deadline = DateTimeUtils.PaymentDueByDate(timesheet.Year, timesheet.Month, item.Value.PaymentDueBy),
                    InvoiceMoney = CommonUtil.Round(mainProjectInvoiceMoney + subProjectInvoiceMoney),
                });
            }

            var listTotalAmountByCurrency = listAllProject.GroupBy(x => x.Currency)
                                    .Select(x => new TotalMoneyByCurrencyDto
                                    {
                                        CurrencyName = x.Key,
                                        Amount = x.Sum(x => x.TotalAmountProjectBillInfomation)
                                    }).ToList();

            var rs = await _financeService.CreateAllInvoices(listInvoices);
            if (rs == default)
                return new ResponseResultProjectDto
                {
                    IsSuccess = false,
                    Message = "Can't Connect to Finfast",
                    SentInvoices = listInvoices,
                    ListTotalMoneyByCurrency = listTotalAmountByCurrency
                };

            rs.SentInvoices = listInvoices;
            rs.ListTotalMoneyByCurrency = listTotalAmountByCurrency;

            return rs;
        }

        #endregion Integrate Finfast

        [HttpPost]
        [AbpAuthorize(PermissionNames.Timesheets_TimesheetDetail_ExportInvoiceAllProject)]
        public async Task<FileBase64Dto> ExportInvoiceAllProject(TimesheetInfoDto input)
        {
            var currentTimeSheet = await GetAllProjectTimesheetByTimesheet(new GridParam { MaxResultCount = int.MaxValue }, input.TimesheetId);
            var invoice = await GetInvoice(currentTimeSheet);
            var accountsNotWorkingFull = await GetAccountsNotWorkingFull(currentTimeSheet);
            var projectsChange = await GetProjectsChange(input.TimesheetId, currentTimeSheet);
            var accountsChange = await GetAccountsChange(input.TimesheetId, currentTimeSheet);

            using (var excelPackage = new ExcelPackage())
            {
                FilLDataInvoice(excelPackage.Workbook.Worksheets.Add("Invoice"), invoice, input);
                FillDataAccountsNotWorkingFullSheet(excelPackage.Workbook.Worksheets.Add("Accounts Not Working Full"), accountsNotWorkingFull);
                FillDataProjectsChangeSheet(excelPackage.Workbook.Worksheets.Add("Projects Change"), projectsChange);
                FillDataAccountsChangeSheet(excelPackage.Workbook.Worksheets.Add("Accounts Change"), accountsChange);

                return new FileBase64Dto
                {
                    FileName = $"INV_{input.TimesheetName}.xlsx",
                    FileType = MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet,
                    Base64 = Convert.ToBase64String(excelPackage.GetAsByteArray())
                };
            }
        }

        private async Task<List<ExportAllProjectInvoiceDto>> GetInvoice(ResultTimesheetDetail currentTimeSheet)
        {
            return currentTimeSheet.ListTimesheetDetail.Items
                .GroupBy(x => x.Currency)
                .Select(x => new ExportAllProjectInvoiceDto
                {
                    Currency = x.Key,
                    Clients = x.GroupBy(x => x.ClientId)
                        .Select(y => new ProjectInvoiceForExportDto
                        {
                            ClientName = y.Select(z => z.ClientName).First(),
                            ProjectInfor = y.Select(z => new ProjectInfoForExportDto
                            {
                                ProjectName = z.ProjectName,
                                ProjectBill = z.ProjectBillInfomation.Sum(x => x.Amount) * (1 - (z.Discount / 100)) + z.TransferFee,
                                Note = $"{z.ProjectBillInfomation.Count} accounts, transfer fee = {z.TransferFee}, discount = {z.Discount}%",
                            }).ToList()
                        }).ToList(),
                }).ToList();
        }

        private async Task<List<ClientAccountsNotWorkingFullDto>> GetAccountsNotWorkingFull(ResultTimesheetDetail currentTimeSheet)
        {
            var query = currentTimeSheet.ListTimesheetDetail.Items
                .GroupBy(x => x.ClientName)
                .Select(x => new ClientAccountsNotWorkingFullDto
                {
                    ClientName = x.Key,
                    ProjectsInfo = x
                        .GroupBy(x => x.ProjectName)
                        .Select(x => new ProjectAccountsNotWorkingFullDto
                        {
                            ProjectName = x.Key,
                            AccountsInfo = x
                                .SelectMany(x => x.ProjectBillInfomation)
                                .Where(billInfo => billInfo.WorkingTime < x.First().WorkingDay)
                                .Select(billInfo => new AccountsNotWorkingFullDto
                                {
                                    FullName = billInfo.FullName,
                                    WorkingTime = billInfo.WorkingTime
                                }).ToList()
                        }).ToList()
                }).ToList();

            return query;
        }

        private async Task<ProjectsChangeDto> GetProjectsChange(long timeSheetId, ResultTimesheetDetail currentTimeSheet)
        {
            var current = WorkScope.Get<Timesheet>(timeSheetId);

            var beforeTimeSheetId = WorkScope.GetAll<Timesheet>()
                .AsEnumerable()
                .Where(x => x.TimeSheetDate <= current.TimeSheetDate && x.Id != current.Id)
                .OrderByDescending(x => x.TimeSheetDate)
                .FirstOrDefault();

            var result = new ProjectsChangeDto();
            if (beforeTimeSheetId != null)
            {
                var beforeTimeSheet = await GetAllProjectTimesheetByTimesheet(new GridParam { MaxResultCount = int.MaxValue }, beforeTimeSheetId.Id);
                result.NewProject = GetNewProjects(beforeTimeSheet, currentTimeSheet);
                result.StopProject = GetStopProjects(beforeTimeSheet, currentTimeSheet);
            }

            return result;
        }

        private List<ClientProjectsChangeInforDto> GetNewProjects(ResultTimesheetDetail beforeTimeSheet, ResultTimesheetDetail currentTimeSheet)
        {
            var listBeforeTimesheetProjectId = beforeTimeSheet.ListTimesheetDetail.Items.Select(y => y.ProjectId).ToList();
            return currentTimeSheet.ListTimesheetDetail.Items
                .Where(x => !listBeforeTimesheetProjectId.Contains(x.ProjectId))
                .Select(x => new
                {
                    ClientName = x.ClientName,
                    ProjectInfor = new
                    {
                        ProjectName = x.ProjectName,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    }
                })
                .GroupBy(x => x.ClientName)
                .Select(x => new ClientProjectsChangeInforDto
                {
                    ClientName = x.Key,
                    ProjectInfors = x.Select(t => new ProjectsChangeInforDto
                    {
                        ProjectName = t.ProjectInfor.ProjectName,
                        StartDate = t.ProjectInfor.StartDate,
                        EndDate = t.ProjectInfor.EndDate
                    }).ToList()
                }).ToList();
        }

        private List<ClientProjectsChangeInforDto> GetStopProjects(ResultTimesheetDetail beforeTimeSheet, ResultTimesheetDetail currentTimeSheet)
        {
            var listCurrentTimesheetId = currentTimeSheet.ListTimesheetDetail.Items.Select(y => y.ProjectId).ToList();
            return beforeTimeSheet.ListTimesheetDetail.Items
                .Where(x => !listCurrentTimesheetId.Contains(x.ProjectId))
                .Select(x => new
                {
                    ClientName = x.ClientName,
                    ProjectInfor = new
                    {
                        ProjectName = x.ProjectName,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    }
                })
                .GroupBy(x => x.ClientName)
                .Select(x => new ClientProjectsChangeInforDto
                {
                    ClientName = x.Key,
                    ProjectInfors = x.Select(t => new ProjectsChangeInforDto
                    {
                        ProjectName = t.ProjectInfor.ProjectName,
                        StartDate = t.ProjectInfor.StartDate,
                        EndDate = t.ProjectInfor.EndDate
                    }).ToList()
                }).ToList();
        }

        private async Task<List<ClientAccountsChangeInforDto>> GetAccountsChange(long timeSheetId, ResultTimesheetDetail currentTimeSheet)
        {
            var current = WorkScope.Get<Timesheet>(timeSheetId);

            return currentTimeSheet.ListTimesheetDetail.Items
                .GroupBy(x => x.ClientName)
                .Select(x => new ClientAccountsChangeInforDto
                {
                    ClientName = x.Key,
                    ProjectsInfor = x
                        .GroupBy(x => x.ProjectName)
                        .Select(x => new ProjectAccountsChangeInforDto
                        {
                            ProjectName = x.Key,
                            NewAccounts = x
                                .SelectMany(x => x.ProjectBillInfomation)
                                .Where(billInfo =>  billInfo.StartTime.Month == current.Month && 
                                                    billInfo.StartTime.Year == current.Year)
                                .Select(billInfo => new AccountsChangeInforDto
                                {
                                    FullName = billInfo.FullName,
                                    StartTime = billInfo.StartTime,
                                    EndTime = billInfo.EndTime
                                }).ToList(),

                            StopAccounts = x
                                .SelectMany(x => x.ProjectBillInfomation)
                                .Where(billInfo =>  billInfo.EndTime.HasValue &&
                                                    billInfo.EndTime.Value.Month == current.Month &&
                                                    billInfo.EndTime.Value.Year == current.Year)
                                .Select(billInfo => new AccountsChangeInforDto
                                {
                                    FullName = billInfo.FullName,
                                    StartTime = billInfo.StartTime,
                                    EndTime = billInfo.EndTime
                                }).ToList()
                        }).ToList()
                }).ToList();
        }

        private void FormatBorder(ExcelWorksheet sheet, string rangeAddress)
        {
            var range = sheet.Cells[rangeAddress];
            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            range.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            range.AutoFitColumns();
        }

        private void FormatFont(ExcelWorksheet sheet, string range, string title, string[] headers)
        {
            sheet.Cells[range].Merge = true;
            sheet.Cells.Style.Font.Name = "Times New Roman";
            sheet.Cells[range.Split(':')[0]].Value = title;
            sheet.Cells[range.Split(':')[0]].Style.Font.Size = 14;
            sheet.Cells[range.Split(':')[0]].Style.Font.Bold = true;
            sheet.Cells[range.Split(':')[0]].Style.Fill.PatternType = ExcelFillStyle.Solid;
            sheet.Cells[range.Split(':')[0]].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

            for (int i = 0; i < headers.Length; i++)
            {
                char column = (char)(range[0] + i);
                int row = int.Parse(range.Substring(1, range.IndexOf(":") - 1)) + 1;
                sheet.Cells[$"{column}{row}"].Value = headers[i];
            }
        }

        private void FilLDataInvoice(ExcelWorksheet sheetInvoice, List<ExportAllProjectInvoiceDto> invoice, TimesheetInfoDto currencyTables)
        {
            FillDataInvoiceTable(sheetInvoice, invoice, currencyTables);
            FilLDataCurrencyTable(sheetInvoice, currencyTables);
        }

        private void FillDataInvoiceTable(ExcelWorksheet sheetInvoice, List<ExportAllProjectInvoiceDto> invoice, TimesheetInfoDto currencyTables)
        {
            sheetInvoice.Cells.Style.Font.Name = "Times New Roman";
            sheetInvoice.Cells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            sheetInvoice.Cells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            sheetInvoice.Cells["A1:F2"].Merge = true;
            sheetInvoice.Cells["A1"].Value = $"Invoice {currencyTables.TimesheetName}";
            sheetInvoice.Cells["A1"].Style.Font.Bold = true;
            sheetInvoice.Cells["A1"].Style.Font.Size = 14;
            sheetInvoice.Cells["A3:F3"].Style.Font.Size = 11;
            sheetInvoice.Cells["A3:F3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheetInvoice.Cells["A3:F3"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
            sheetInvoice.Cells["A3:F3"].Style.Font.Bold = true;
            sheetInvoice.Cells["A3"].Value = "STT";
            sheetInvoice.Cells["B3"].Value = "TÊN CT/KH";
            sheetInvoice.Cells["C3"].Value = "TÊN DỰ ÁN";
            sheetInvoice.Cells["D3"].Value = "INVOICE";
            sheetInvoice.Cells["E3"].Value = "TỔNG INVOICE";
            sheetInvoice.Cells["F3"].Value = "NOTE";
            var indexDataInvoice = sheetInvoice.Cells["A4"].Start.Row;
            int indexInvoice = 1;
            var total = 0.0;
            foreach (var item in invoice)
            {
                var firstInvoice = indexDataInvoice;
                foreach (var client in item.Clients)
                {
                    var indexClientStartRow = indexDataInvoice;
                    var indexClientEndRow = indexDataInvoice + client.ProjectInfor.Count - 1;

                    sheetInvoice.Cells[$"B{indexClientStartRow}:B{indexClientEndRow}"].Merge = true;
                    sheetInvoice.Cells[$"B{indexClientStartRow}"].Value = client.ClientName;                 
                    sheetInvoice.Cells[$"E{indexClientStartRow}:E{indexClientEndRow}"].Merge = true;
                    sheetInvoice.Cells[$"E{indexClientStartRow}"].Value = client.Total;

                    foreach (var project in client.ProjectInfor)
                    {
                        sheetInvoice.Cells[$"A{indexDataInvoice}"].Value = indexInvoice++;
                        sheetInvoice.Cells[$"C{indexDataInvoice}"].Value = project.ProjectName;
                        sheetInvoice.Cells[$"D{indexDataInvoice}"].Value = project.ProjectBill;
                        sheetInvoice.Cells[$"F{indexDataInvoice}"].Value = project.Note;
                        indexDataInvoice++;
                    }
                    sheetInvoice.Cells[$"D{firstInvoice}:E{indexDataInvoice}"].Style.Numberformat.Format = $"_([${item.Currency}] * #,##0.00_);_([${item.Currency}] * (#,##0.00);_([${item.Currency}] * \"-\"??_);_(@_)";
                }
                sheetInvoice.Cells[$"A{indexDataInvoice}:B{indexDataInvoice}"].Merge = true;
                sheetInvoice.Cells[$"A{indexDataInvoice}:F{indexDataInvoice}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                sheetInvoice.Cells[$"A{indexDataInvoice}:F{indexDataInvoice}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));
                sheetInvoice.Cells[$"A{indexDataInvoice}:F{indexDataInvoice}"].Style.Font.Bold = true;
                sheetInvoice.Cells[$"A{indexDataInvoice}"].Value = $"TỔNG {item.Currency}";
                sheetInvoice.Cells[$"E{indexDataInvoice}"].Value = item.Total;
                total += item.Total * currencyTables.Currencies[invoice.IndexOf(item)].ExchangeRate;
                indexDataInvoice++;
            }
            sheetInvoice.Cells[$"A{indexDataInvoice}:B{indexDataInvoice}"].Merge = true;
            sheetInvoice.Cells[$"A{indexDataInvoice}:F{indexDataInvoice}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheetInvoice.Cells[$"A{indexDataInvoice}:F{indexDataInvoice}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(226, 239, 218));
            sheetInvoice.Cells[$"A{indexDataInvoice}:F{indexDataInvoice}"].Style.Font.Bold = true;
            sheetInvoice.Cells[$"A{indexDataInvoice}"].Value = $"TO VND";
            sheetInvoice.Cells[$"E{indexDataInvoice}"].Value = total;
            sheetInvoice.Cells[$"E{indexDataInvoice}"].Style.Numberformat.Format = $"_([$VND] * #,##0.00_);_([$VND] * (#,##0.00);_([$VND] * \"-\"??_);_(@_)";
            sheetInvoice.Column(4).Width = 35;
            sheetInvoice.Column(5).Width = 35;
            sheetInvoice.Column(9).Width = 35;
            sheetInvoice.Cells[$"D4:E{indexDataInvoice}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            sheetInvoice.Cells[$"F4:F{indexDataInvoice}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            FormatBorder(sheetInvoice, $"A1:F{indexDataInvoice}");
        }

        private void FilLDataCurrencyTable(ExcelWorksheet sheetInvoice, TimesheetInfoDto currencyTables)
        {
            var indexCurrencyTable = sheetInvoice.Cells["H4"].Start.Row;
            sheetInvoice.Cells[$"H3:I{indexCurrencyTable}"].Style.Font.Name = "Calibri";
            sheetInvoice.Cells[$"H3:I{indexCurrencyTable}"].Style.Font.Size = 11;
            sheetInvoice.Cells[$"I4:I{indexCurrencyTable}"].Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
            sheetInvoice.Cells["H3:I3"].Merge = true;
            sheetInvoice.Cells["H3:I3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            sheetInvoice.Cells["H3:I3"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 0));
            sheetInvoice.Cells["H3"].Value = $"Tỉ giá ngày: {currencyTables.Date}";
            foreach (var item in currencyTables.Currencies)
            {
                sheetInvoice.Cells[$"H{indexCurrencyTable}"].Value = item.CurrencyName;
                sheetInvoice.Cells[$"I{indexCurrencyTable}"].Value = item.ExchangeRate;
                indexCurrencyTable++;
            }
            
            FormatBorder(sheetInvoice, $"H3:I{indexCurrencyTable - 1}");
        }

        private void FillDataAccountsNotWorkingFullSheet(ExcelWorksheet sheetAccountsNotWorkingFull, List<ClientAccountsNotWorkingFullDto> accountsNotWorkingFull)
        {
            FormatFont(sheetAccountsNotWorkingFull, "A1:E1", "DANH SÁCH ACC KHÔNG FULL", new string[] { "STT", "TÊN CT/KH", "TÊN DỰ ÁN", "ACCOUNT", "SỐ NGÀY THỰC TẾ" });
            var indexDataRow = sheetAccountsNotWorkingFull.Cells["A3"].Start.Row;
            var indexAccountsNotWorkingFull = 1;

            foreach (var client in accountsNotWorkingFull)
            {
                var indexClientRow = indexDataRow;

                foreach (var project in client.ProjectsInfo)
                {
                    sheetAccountsNotWorkingFull.Cells[$"B{indexDataRow}"].Value = client.ClientName;
                    sheetAccountsNotWorkingFull.Cells[$"C{indexDataRow}"].Value = project.ProjectName;

                    var indexAccountRow = indexDataRow;

                    foreach (var account in project.AccountsInfo)
                    {
                        sheetAccountsNotWorkingFull.Cells[$"A{indexAccountRow}"].Value = indexAccountsNotWorkingFull++;
                        sheetAccountsNotWorkingFull.Cells[$"D{indexAccountRow}"].Value = account.FullName;
                        sheetAccountsNotWorkingFull.Cells[$"E{indexAccountRow}"].Value = account.WorkingTime;
                        indexAccountRow++;
                    }

                    if (project.AccountsInfo.Count > 1)
                    {
                        sheetAccountsNotWorkingFull.Cells[$"C{indexDataRow}:C{indexDataRow + project.AccountsInfo.Count - 1}"].Merge = true;
                    }

                    indexDataRow += project.AccountsInfo.Count;
                }

                if (indexClientRow < indexDataRow)
                {
                    sheetAccountsNotWorkingFull.Cells[$"B{indexClientRow}:B{indexDataRow - 1}"].Merge = true;
                }
            }

            FormatBorder(sheetAccountsNotWorkingFull, $"A1:E{indexDataRow - 1}");
        }

        private void FillDataProjectsChangeSheet(ExcelWorksheet sheetProjectsChange, ProjectsChangeDto projectsChange)
        {
            FillDataStopProjectsTable(sheetProjectsChange, projectsChange);
            FillDataNewProjectsTable(sheetProjectsChange, projectsChange);
        }   
        
        private void FillDataStopProjectsTable(ExcelWorksheet sheetProjectsChange, ProjectsChangeDto projectsChange)
        {
            FormatFont(sheetProjectsChange, "A1:E1", "CÁC DỰ ÁN STOP", new string[] { "STT", "CT/KH", "Project", "Ngày Start", "Ngày Stop" });
            var startStopProject = sheetProjectsChange.Cells["A3"].Start.Row;
            var indexStopProject = 1;

            if (projectsChange.StopProject != null)
            {
                foreach (var item in projectsChange.StopProject)
                {
                    var clientMerge = sheetProjectsChange.Cells[$"B{startStopProject}"];
                    clientMerge.Value = item.ClientName;

                    if (item.ProjectInfors.Count > 1)
                    {
                        var endRow = startStopProject + item.ProjectInfors.Count - 1;
                        sheetProjectsChange.Cells[$"B{startStopProject}:B{endRow}"].Merge = true;
                    }

                    foreach (var stopProject in item.ProjectInfors)
                    {
                        sheetProjectsChange.Cells[$"A{startStopProject}"].Value = indexStopProject++;
                        sheetProjectsChange.Cells[$"C{startStopProject}"].Value = stopProject.ProjectName;
                        sheetProjectsChange.Cells[$"D{startStopProject}"].Value = $"{stopProject.StartDate.ToString("dd/MM/yyyy")}";
                        sheetProjectsChange.Cells[$"E{startStopProject}"].Value = $"{(stopProject.EndDate.HasValue ? stopProject.EndDate.Value.ToString("dd/MM/yyyy") : "")}";
                        startStopProject++;
                    }
                }
            }

            FormatBorder(sheetProjectsChange, $"A1:E{startStopProject - 1}");
        }

        private void FillDataNewProjectsTable(ExcelWorksheet sheetProjectsChange, ProjectsChangeDto projectsChange)
        {
            FormatFont(sheetProjectsChange, "G1:K1", "CÁC DỰ ÁN MỚI", new string[] { "STT", "CT/KH", "Project", "Ngày Start", "Ngày Stop" });
            var startNewProject = sheetProjectsChange.Cells["G3"].Start.Row;
            var indexNewProject = 1;

            if (projectsChange.NewProject != null)
            {
                foreach (var item in projectsChange.NewProject)
                {
                    var clientMerge = sheetProjectsChange.Cells[$"H{startNewProject}"];
                    clientMerge.Value = item.ClientName;

                    if (item.ProjectInfors.Count > 1)
                    {
                        var endRow = startNewProject + item.ProjectInfors.Count - 1;
                        sheetProjectsChange.Cells[$"H{startNewProject}:H{endRow}"].Merge = true;
                    }

                    foreach (var newProject in item.ProjectInfors)
                    {
                        sheetProjectsChange.Cells[$"G{startNewProject}"].Value = indexNewProject++;
                        sheetProjectsChange.Cells[$"I{startNewProject}"].Value = newProject.ProjectName;
                        sheetProjectsChange.Cells[$"J{startNewProject}"].Value = $"{newProject.StartDate.ToString("dd/MM/yyyy")}";
                        sheetProjectsChange.Cells[$"K{startNewProject}"].Value = $"{(newProject.EndDate.HasValue ? newProject.EndDate.Value.ToString("dd/MM/yyyy") : "")}";
                        startNewProject++;
                    }
                }
            }

            FormatBorder(sheetProjectsChange, $"G1:K{startNewProject - 1}");
        }

        private void FillDataAccountsChangeSheet(ExcelWorksheet sheetAccountsChange, List<ClientAccountsChangeInforDto> accountsChange)
        {
            FillDataNewAccountsTable(sheetAccountsChange, accountsChange);
            FillDataStopAccountsTable(sheetAccountsChange, accountsChange);
        }

        private void FillDataNewAccountsTable(ExcelWorksheet sheetAccountsChange, List<ClientAccountsChangeInforDto> accountsChange)
        {
            FormatFont(sheetAccountsChange, "A1:F1", "CÁC ACC TĂNG", new string[] { "STT", "CT/KH", "Project", "Account", "Ngày Start", "Ngày Stop" });
            var indexDataNewAccount = sheetAccountsChange.Cells["A3"].Start.Row;
            var indexNewAccount = 1;

            foreach (var client in accountsChange)
            {
                var indexDataNewClient = indexDataNewAccount;

                foreach (var project in client.ProjectsInfor)
                {
                    var indexDataNewProject = indexDataNewAccount;

                    foreach (var account in project.NewAccounts)
                    {
                        sheetAccountsChange.Cells[$"A{indexDataNewAccount}"].Value = indexNewAccount++;
                        sheetAccountsChange.Cells[$"B{indexDataNewAccount}"].Value = client.ClientName;
                        sheetAccountsChange.Cells[$"C{indexDataNewAccount}"].Value = project.ProjectName;
                        sheetAccountsChange.Cells[$"D{indexDataNewAccount}"].Value = account.FullName;
                        sheetAccountsChange.Cells[$"E{indexDataNewAccount}"].Value = $"{account.StartTime:dd/MM/yyyy}";
                        sheetAccountsChange.Cells[$"F{indexDataNewAccount}"].Value = account.EndTime?.ToString("dd/MM/yyyy");
                        indexDataNewAccount++;
                    }

                    if (indexDataNewProject < indexDataNewAccount)
                    {
                        sheetAccountsChange.Cells[$"C{indexDataNewProject}:C{indexDataNewAccount - 1}"].Merge = true;
                    }
                }

                if (indexDataNewClient < indexDataNewAccount)
                {
                    sheetAccountsChange.Cells[$"B{indexDataNewClient}:B{indexDataNewAccount - 1}"].Merge = true;
                }
            }

            FormatBorder(sheetAccountsChange, $"A1:F{indexDataNewAccount - 1}");
        }

        private void FillDataStopAccountsTable(ExcelWorksheet sheetAccountsChange, List<ClientAccountsChangeInforDto> accountsChange)
        {
            FormatFont(sheetAccountsChange, "H1:M1", "CÁC ACC GIẢM", new string[] { "STT", "CT/KH", "Project", "Account", "Ngày Start", "Ngày Stop" });
            var indexDataStopAccount = sheetAccountsChange.Cells["H3"].Start.Row;
            var indexStopAccount = 1;

            foreach (var client in accountsChange)
            {
                var indexDataStopClient = indexDataStopAccount;

                foreach (var project in client.ProjectsInfor)
                {
                    var indexDataStopProject = indexDataStopAccount;

                    foreach (var account in project.StopAccounts)
                    {
                        sheetAccountsChange.Cells[$"H{indexDataStopAccount}"].Value = indexStopAccount++;
                        sheetAccountsChange.Cells[$"I{indexDataStopAccount}"].Value = client.ClientName;
                        sheetAccountsChange.Cells[$"J{indexDataStopAccount}"].Value = project.ProjectName;
                        sheetAccountsChange.Cells[$"K{indexDataStopAccount}"].Value = account.FullName;
                        sheetAccountsChange.Cells[$"L{indexDataStopAccount}"].Value = $"{account.StartTime:dd/MM/yyyy}";
                        sheetAccountsChange.Cells[$"M{indexDataStopAccount}"].Value = $"{account.EndTime.Value:dd/MM/yyyy}";
                        indexDataStopAccount++;
                    }

                    if (indexDataStopProject < indexDataStopAccount)
                    {
                        sheetAccountsChange.Cells[$"J{indexDataStopProject}:J{indexDataStopAccount - 1}"].Merge = true;
                    }
                }

                if (indexDataStopClient < indexDataStopAccount)
                {
                    sheetAccountsChange.Cells[$"I{indexDataStopClient}:I{indexDataStopAccount - 1}"].Merge = true;
                }
            }

            FormatBorder(sheetAccountsChange, $"H1:M{indexDataStopAccount - 1}");
        }

        [HttpGet]
        public async Task<Object> GetExchangeRate(string date, string baseCurrency, string symbols, int places)
        {
            return await _exchangeRateService.GetExchangeRate(date, baseCurrency, symbols, places);
        }

        [HttpPost]
        public async Task ReActiveTimesheetProject(ReactiveTimesheetProjectDto input)
        {
            input.TimesheetProjectIds.ForEach(x =>
            {
                _reactiveTimesheetProject.CreateReqDeActiveTimesheetProjectBGJ(x, input.CloseDate);
            });
            var listItem = WorkScope.GetAll<TimesheetProject>().Include(t => t.Timesheet)
               .Where(x => input.TimesheetProjectIds.Contains(x.Id)).ToList();
            if (listItem.FirstOrDefault() != null && listItem.FirstOrDefault().Timesheet.IsActive)
                throw new UserFriendlyException("This timesheet has been activated!");
            listItem.ForEach(x =>
            {
                x.IsActive = true;
            });
            WorkScope.UpdateRange(listItem);
        }

        [HttpPost]
        public async Task DeActiveTimesheetProject(List<long> ids)
        {
            foreach (var id in ids)
            {
                var item = await WorkScope.GetAsync<TimesheetProject>(id);
                _reactiveTimesheetProject.DeleteOldRequestInBackgroundJob(id);
                item.IsActive = false;
                await WorkScope.UpdateAsync(item);
            }
        }
    }
}