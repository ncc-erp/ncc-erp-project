using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using NccCore.Uitls;
using ProjectManagement.APIs.ProjectUserBills.Dto;
using ProjectManagement.APIs.Timesheets.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Services.ProjectTimesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserBills
{
    [AbpAuthorize]
    public class ProjectUserBillAppService : ProjectManagementAppServiceBase
    {
        private ProjectTimesheetManager timesheetManager;


        public ProjectUserBillAppService(ProjectTimesheetManager timesheetManager)
        {
            this.timesheetManager = timesheetManager;
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo)]
        public async Task<List<GetProjectUserBillDto>> GetAllByProject(long projectId)
        {
            var isViewRate = await IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View);
            var query = WorkScope.GetAll<ProjectUserBill>()
                .Where(x => x.ProjectId == projectId)
                .OrderByDescending(x => x.CreationTime)
                        .Select(x => new GetProjectUserBillDto
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            UserName = x.User.Name,
                            ProjectId = x.ProjectId,
                            ProjectName = x.Project.Name,
                            AccountName = x.AccountName,
                            BillRole = x.BillRole,
                            BillRate = isViewRate ? x.BillRate : 0,
                            StartTime = x.StartTime.Date,
                            EndTime = x.EndTime.Value.Date,
                            //CurrencyName = x.Project.Currency.Name,
                            Note = x.Note,
                            shadowNote = x.shadowNote,
                            isActive = x.isActive,
                            AvatarPath = x.User.AvatarPath,
                            FullName = x.User.FullName,
                            Branch = x.User.BranchOld,
                            BranchColor = x.User.Branch.Color,
                            BranchDisplayName = x.User.Branch.DisplayName,
                            PositionId = x.User.PositionId,
                            PositionName = x.User.Position.ShortName,
                            PositionColor = x.User.Position.Color,
                            EmailAddress = x.User.EmailAddress,
                            UserType = x.User.UserType,
                            UserLevel = x.User.UserLevel,
                            ChargeType = x.ChargeType.HasValue ? x.ChargeType : x.Project.ChargeType,
                        });
            return await query.ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<long> GetLastInvoiceNumber(long projectId)
        {
            return await WorkScope.GetAll<Project>()
             .Where(s => s.Id == projectId)
             .Select(s => s.LastInvoiceNumber)
             .FirstOrDefaultAsync();
        }


        private async Task<Project> GetProjectById(long projectId)
        {
            return await WorkScope.GetAll<Project>().FirstOrDefaultAsync(x => x.Id == projectId);
        }

        [HttpGet]
        public async Task<List<ProjectUserAccountPlanningDto>> GetAllProjectUserBill()
        {
            var query = await WorkScope.GetAll<ProjectUserBill>()
                .Include(x => x.Project)
                .Select(x => new ProjectUserAccountPlanningDto
                {
                    Id = x.ProjectId,
                    Name = x.Project.Name,
                })
                .Distinct()
                .ToListAsync();

            return query;
        }

        // duy
        [HttpPost]
        [AbpAuthorize(PermissionNames.Resource_TabPlanningBillAcccount)]
        public async Task<GridResult<BillInfoDto>> GetAllPlanningBillInfo(InputGetBillInfoDto input)
        {
            // paging user id 
            var projectUserBills = WorkScope.All<ProjectUserBill>()
                .Include(p => p.User).Include(p => p.Project)
                .Select(x => new
                {
                    UserInfor = new GetUserBillDto
                    {
                        UserId = x.UserId,
                        UserName = x.User.Name,
                        AvatarPath = x.User.AvatarPath,
                        FullName = x.User.FullName,
                        Branch = x.User.BranchOld,
                        BranchColor = x.User.Branch.Color,
                        BranchDisplayName = x.User.Branch.DisplayName,
                        PositionId = x.User.PositionId,
                        PositionName = x.User.Position.ShortName,
                        PositionColor = x.User.Position.Color,
                        EmailAddress = x.User.EmailAddress,
                        UserType = x.User.UserType,
                        UserLevel = x.User.UserLevel,
                    },
                    Project = new GetProjectBillDto
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.Project.Name,
                        AccountName = x.AccountName,
                        //BillRole = x.BillRole,
                        BillRate = float.NaN,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        Note = x.Note,
                        shadowNote = x.shadowNote,
                        isActive = x.isActive,
                        ChargeType = x.ChargeType.HasValue ? x.ChargeType.Value : x.Project.ChargeType,
                    }
                })
                    .WhereIf(input.ProjectId != null, x => x.Project.ProjectId == input.ProjectId)
                    .WhereIf(!string.IsNullOrEmpty(input.SearchText), x =>
                        (x.UserInfor.UserName.Trim().ToLower().Contains(input.SearchText.Trim().ToLower())) ||
                        (x.UserInfor.FullName.Trim().ToLower().Contains(input.SearchText.Trim().ToLower())) ||
                        (x.UserInfor.EmailAddress.Trim().ToLower().Contains(input.SearchText.Trim().ToLower())) ||
                        (x.Project.ProjectName.Trim().ToLower().Contains(input.SearchText.Trim().ToLower())))
                    .WhereIf(input.ChargeStatus == ChargeStatus.IsCharge, x => x.Project.isActive == true)
                    .WhereIf(input.ChargeStatus == ChargeStatus.IsNotCharge, x => x.Project.isActive == false)
                    .AsEnumerable().GroupBy(p => p.UserInfor.UserId)
                    .Select(group => new BillInfoDto
                    {
                        UserInfor = group.First().UserInfor,
                        Projects = group.Select(g => g.Project).ToList()
                    })
                    .Where(x => x.Projects.Any(x => (
                        input.StartDate <= x.EndTime && x.EndTime <= input.EndDate
                        || input.StartDate <= x.StartTime && x.StartTime <= input.EndDate)))
                    .WhereIf(input.JoinOutStatus == JoinOutStatus.IsJoin,
                        x => x.Projects.Any(x => (input.StartDate <= x.StartTime && x.StartTime <= input.EndDate)))
                    .WhereIf(input.JoinOutStatus == JoinOutStatus.IsOut,
                        x => x.Projects.Any(x => (input.StartDate <= x.EndTime && x.EndTime <= input.EndDate))).AsQueryable();
            return projectUserBills.GetGridResultSync(projectUserBills, input.GirdParam);
        }

        /// <summary>
        /// return list projects that have the same client with input projectId
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<SubInvoiceDto> GetAvailableProjectsForSettingInvoice(long projectId)
        {

            var clientId = WorkScope.GetAll<Project>().Where(s => s.Id == projectId).Select(s => s.ClientId).FirstOrDefault();
            if (clientId == default)
            {
                return null;
            }

            return WorkScope.GetAll<Project>()
                .Where(s => s.ClientId == clientId)
                .Where(s => s.Id != projectId)
                .Select(s => new SubInvoiceDto { ProjectId = s.Id, ProjectName = s.Name })
                .ToList();

        }


        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit)]
        public async Task<long> UpdateLastInvoiceNumber(UpdateLastInvoiceNumberDto input)
        {
            var project = await GetProjectById(input.ProjectId);
            if (project != null)
            {
                project.LastInvoiceNumber = input.LastInvoiceNumber;
                var projectUpdate = await WorkScope.UpdateAsync<Project>(project);
                return projectUpdate.LastInvoiceNumber;
            }

            return default;
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<float> GetDiscount(long projectId)
        {
            return await WorkScope.GetAll<Project>()
             .Where(s => s.Id == projectId)
             .Select(s => s.Discount)
             .FirstOrDefaultAsync();
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit)]
        public async Task<float> UpdateDiscount(UpdateDiscountDto input)
        {
            var project = await GetProjectById(input.ProjectId);
            if (project != null)
            {
                project.Discount = input.Discount;
                var projectUpdate = await WorkScope.UpdateAsync<Project>(project);
                return projectUpdate.Discount;
            }

            return default;

        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<ProjectRateDto> GetRate(long projectId)
        {
            var query = WorkScope.GetAll<Project>().Include(x => x.Currency).Where(x => x.Id == projectId)
                                .Select(x => new ProjectRateDto
                                {
                                    IsCharge = x.IsCharge,
                                    ChargeType = x.ChargeType,
                                    CurrencyName = x.Currency.Name

                                });
            return await query.FirstOrDefaultAsync();
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<ProjectInvoiceSettingDto> GetBillInfo(long projectId)
        {
            var subProjects = WorkScope.GetAll<Project>()
              .Where(s => s.ParentInvoiceId == projectId)
              .Select(s => new IdNameDto { Id = s.Id, Name = s.Name })
              .ToList();

            var dto = await WorkScope.GetAll<Project>().Where(x => x.Id == projectId)
                                .Select(x => new ProjectInvoiceSettingDto
                                {
                                    CurrencyName = x.Currency.Name,
                                    Discount = x.Discount,
                                    InvoiceNumber = x.LastInvoiceNumber,
                                    MainProjectId = x.ParentInvoiceId,
                                    IsMainProjectInvoice = !x.ParentInvoiceId.HasValue,
                                    SubProjects = subProjects,
                                }).FirstOrDefaultAsync();

            if (dto != default && !dto.IsMainProjectInvoice)
            {
                dto.MainProjectName = WorkScope.GetAll<Project>()
                    .Where(s => s.Id == dto.MainProjectId.Value)
                    .Select(s => s.Name)
                    .FirstOrDefault();
            }

            return dto;
        }


        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Create,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Create,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Create)]
        public async Task<ProjectUserBillDto> Create(ProjectUserBillDto input)
        {
            if (input.EndTime.HasValue && input.StartTime.Date > input.EndTime.Value.Date)
                throw new UserFriendlyException($"Start date cannot be greater than end date !");

            var duplicatedPUB = await WorkScope.GetAll<ProjectUserBill>()
                .Where(s => s.UserId == input.UserId)
                .Where(s => s.ProjectId == input.ProjectId)
                .Select(s => new { s.User.FullName, s.BillRole, s.isActive, s.BillRate })
                .FirstOrDefaultAsync();

            if (duplicatedPUB != default)
            {
                throw new UserFriendlyException($"Already exist: {duplicatedPUB.FullName} - {duplicatedPUB.BillRole} - {duplicatedPUB.BillRate} - Active: {duplicatedPUB.isActive}");
            }
            var project = await WorkScope.GetAll<Project>()
                .Where(s => s.Id == input.ProjectId)
                .Select(s => new Project
                {
                    CurrencyId = s.CurrencyId,
                    ChargeType = s.ChargeType
                })
                .FirstOrDefaultAsync();

            var entity = ObjectMapper.Map<ProjectUserBill>(input);

            input.Id = await WorkScope.InsertAndGetIdAsync(entity);

            await this.timesheetManager.CreateTimesheetProjectBill(entity, project);

            return input;
        }


        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Edit,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Edit,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Edit)]
        public async Task<ProjectUserBillDto> Update(ProjectUserBillDto input)
        {
            var projectUserBill = await WorkScope.GetAsync<ProjectUserBill>(input.Id);

            if (input.EndTime.HasValue && input.StartTime.Date > input.EndTime.Value.Date)
                throw new UserFriendlyException($"Start date cannot be greater than end date !");

            var isEditNote = await IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit);
            if (!isEditNote)
            {
                input.Note = projectUserBill.Note;
            }
            var entity = ObjectMapper.Map(input, projectUserBill);

            await WorkScope.UpdateAsync(entity);

            await this.timesheetManager.UpdateTimesheetProjectBill(entity);
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Delete,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_Delete,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_Delete)]
        public async Task Delete(long projectUserBillId)
        {
            var projectUserBill = await WorkScope.GetAsync<ProjectUserBill>(projectUserBillId);

            await WorkScope.DeleteAsync(projectUserBill);
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Note_Edit)]
        public async Task<UpdateNoteDto> UpdateNote(UpdateNoteDto input)
        {
            var projectUserBill = await WorkScope.GetAsync<ProjectUserBill>(input.Id);

            var entity = ObjectMapper.Map(input, projectUserBill);

            await WorkScope.UpdateAsync(entity);

            return input;
        }
        #region Integrate Finfast

        private IQueryable<ProjectInvoiceDto> GetAllProject()
        {
            return WorkScope.GetAll<Project>()
                 .Select(x => new ProjectInvoiceDto
                 {
                     Name = x.Name,
                     ProjectId = x.Id,
                     ParentInvoiceId = x.ParentInvoiceId,
                     ClientId = x.ClientId,
                 });
        }

        [HttpGet]
        public async Task<List<SubInvoiceDto>> GetAllProjectCanUsing(long projectId)
        {
            var clientId = WorkScope.Get<Project>(projectId).ClientId;
            var listProjectAll = GetAllProject();
            return listProjectAll
                .Where(pa => pa.ClientId == clientId)
                .Where(pa => pa.ProjectId != projectId)
                .Select(x => new SubInvoiceDto
                {
                    ProjectId = x.ProjectId,
                    ProjectName = x.Name,
                    ParentId = x.ParentInvoiceId,
                    ParentName = x.ParentInvoiceId.HasValue ?
                        listProjectAll.FirstOrDefault(pa => pa.ProjectId == x.ParentInvoiceId.Value).Name : null,
                }).ToList();
        }
        [HttpGet]
        public async Task<ParentInvoiceDto> GetParentInvoiceByProject(long projectId)
        {
            var listProjectAll = GetAllProject();
            return listProjectAll
                .Where(s => s.ProjectId == projectId)
                .Select(s => new ParentInvoiceDto
                {
                    ProjectId = s.ProjectId,
                    ParentId = s.ParentInvoiceId,
                    ParentName = s.ParentInvoiceId.HasValue ? listProjectAll
                                .FirstOrDefault(pa => pa.ProjectId == s.ParentInvoiceId.Value).Name : null,
                    SubInvoices = listProjectAll.Where(s => s.ParentInvoiceId == projectId)
                                                .Select(x => new SubInvoiceDto
                                                {
                                                    ProjectId = x.ProjectId,
                                                    ProjectName = x.Name
                                                }).ToList()
                }).FirstOrDefault();
        }
        [HttpPost]
        public async Task<string> AddSubInvoice(AddSubInvoiceDto input)
        {
            var subInvoice = await WorkScope.GetAsync<Project>(input.SubInvoiceId);
            subInvoice.ParentInvoiceId = input.ParentInvoiceId;
            await CurrentUnitOfWork.SaveChangesAsync();
            return "Added Successfullly";
        }
        [HttpPost]
        public async Task<string> AddSubInvoices(AddSubInvoicesDto input)
        {
            var listProject = new List<Project>();
            var subProjects = await WorkScope.GetAll<Project>().Where(x => x.ParentInvoiceId == input.ParentInvoiceId).Where(x => !input.SubInvoiceIds.Any(s => s == x.Id)).ToListAsync();
            var parentProject = await WorkScope.GetAsync<Project>(input.ParentInvoiceId);
            parentProject.ParentInvoiceId = null;
            listProject.Add(parentProject);
            foreach (var subProject in subProjects)
            {
                subProject.ParentInvoiceId = null;
                listProject.Add(subProject);
            }
            foreach (var projectId in input.SubInvoiceIds)
            {
                var subInvoice = await WorkScope.GetAsync<Project>(projectId);
                subInvoice.ParentInvoiceId = input.ParentInvoiceId;
                listProject.Add(subInvoice);
            }
            await WorkScope.UpdateRangeAsync(listProject);
            return $"Added {input.SubInvoiceIds.Count} sub to main project";
        }


        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_InvoiceSetting_Edit)]
        public void UpdateInvoiceSetting(UpdateInvoiceDto input)
        {

            var project = WorkScope.Get<Project>(input.ProjectId);

            project.LastInvoiceNumber = input.InvoiceNumber;
            project.Discount = input.Discount;


            var projectsChangeToMain = WorkScope.GetAll<Project>()
                        .Where(s => s.ParentInvoiceId == project.Id)
                        .ToList();

            projectsChangeToMain.ForEach(p =>
            {
                p.ParentInvoiceId = null;
                p.LastModificationTime = DateTimeUtils.GetNow();
                p.LastModifierUserId = AbpSession.UserId;
            });

            if (input.IsMainProjectInvoice)
            {
                project.ParentInvoiceId = null;

                if (input.SubProjectIds != null && !input.SubProjectIds.IsEmpty())
                {

                    var subProjects = WorkScope.GetAll<Project>().Where(s => input.SubProjectIds.Contains(s.Id)).ToList();
                    subProjects.ForEach(p =>
                    {
                        p.ParentInvoiceId = input.ProjectId;
                        p.LastModificationTime = DateTimeUtils.GetNow();
                        p.LastModifierUserId = AbpSession.UserId;
                    });
                }

            }
            else
            {
                if (!input.MainProjectId.HasValue)
                {
                    throw new UserFriendlyException("You have to select Main Project!");
                }
                project.ParentInvoiceId = input.MainProjectId.Value;

            }

            CurrentUnitOfWork.SaveChanges();

        }

        [HttpGet]
        [AbpAuthorize]
        public string CheckInvoiceSetting()
        {
            var listProject = WorkScope.GetAll<Project>()
             .Where(s => s.ProjectType == ProjectType.ODC || s.ProjectType == ProjectType.TimeAndMaterials || s.ProjectType == ProjectType.FIXPRICE)
             .Select(s => new { s.Id, s.Name, s.ParentInvoiceId })
             .ToList();

            var sb = new StringBuilder();
            foreach (var project in listProject)
            {
                if (project.ParentInvoiceId.HasValue)
                {
                    var parrentProject = listProject.Where(s => s.Id == project.ParentInvoiceId.Value).FirstOrDefault();
                    if (parrentProject == default)
                    {
                        sb.AppendLine($"{project.Name} is Sub but not found main project");
                    }
                    else
                    {
                        if (parrentProject.ParentInvoiceId.HasValue)
                        {
                            sb.AppendLine($"{project.Name} is Sub of {parrentProject.Name} but {parrentProject.Name} is SUB too");
                        }
                    }

                }

            }
            return sb.ToString();

        }



        [HttpGet]
        public async Task OutParentInvoice(long subInvoiceId)
        {
            var subInvoice = await WorkScope.GetAsync<Project>(subInvoiceId);
            subInvoice.ParentInvoiceId = null;
            await CurrentUnitOfWork.SaveChangesAsync();
        }
        #endregion
    }
}
