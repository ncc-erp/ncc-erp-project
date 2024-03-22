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
using ProjectManagement.Services.ProjectUserBill.Dto;
using ProjectManagement.Services.ProjectUserBills;
using ProjectManagement.Services.ResourceService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using UpdateNoteDto = ProjectManagement.APIs.ProjectUserBills.Dto.UpdateNoteDto;

namespace ProjectManagement.APIs.ProjectUserBills
{
    [AbpAuthorize]
    public class ProjectUserBillAppService : ProjectManagementAppServiceBase
    {
        private ProjectTimesheetManager timesheetManager;
        private ProjectUserBillManager projectUserBillManager;

        public ProjectUserBillAppService(ProjectTimesheetManager timesheetManager
            , ProjectUserBillManager projectUserBillManager)
        {
            this.timesheetManager = timesheetManager;
            this.projectUserBillManager = projectUserBillManager;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo,
          PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo,
          PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo)]
        public async Task<List<Services.ProjectUserBill.Dto.GetProjectUserBillDto>> GetAllByProject(GetAllProjectUserBillDto input)
        {
            return await projectUserBillManager.GetAllByProject(input);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo,
          PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo,
          PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo)]
        public async Task<List<LinkedResourceInfoDto>> GetAllLinkedResourcesByProject(long projectId)
        {
            return await projectUserBillManager.GetAllLinkedResourcesByProject(projectId);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo,
          PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo,
          PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo)]
        public async Task<List<string>> GetAllChargeRoleByProject(long projectId)
        {
            return await projectUserBillManager.GetAllChargeRoleByProject(projectId);
        }

        [HttpPost]
        [AbpAuthorize(
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount
        )]
        public async Task<bool> LinkUserToBillAccount(LinkedResourcesDto input)
        {
            var linkedResources = await projectUserBillManager.AddLinkedResources(input);
            return linkedResources.Any();
        }

        [HttpPost]
        [AbpAuthorize(
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabBillInfo_UpdateUserToBillAccount
        )]
        public async Task<bool> RemoveLinkedResource(LinkedResourcesDto input)
        {
            await projectUserBillManager.RemoveLinkedResource(input);
            return true;
        }

        [HttpPost]
        public async Task LinkOneProjectUserBillAccount(LinkedResourceDto input)
        {
            await projectUserBillManager.LinkOneLinkedResource(input);
        }


        [HttpGet]
        public async Task<List<UserDto>> GetAllUser(bool onlyStaff, long projectId, long? currentUserId, bool isIncludedUserInPUB)
        {
            return await projectUserBillManager.GetAllUser(onlyStaff, projectId, currentUserId, isIncludedUserInPUB);
        }

        [HttpPost]
        public async Task<List<SubProjectBillDto>> GetSubProjectBills(InputSubProjectBillDto input)
        {
            var query = projectUserBillManager.GetSubProjectBills(input.ParentProjectId)
                .Select(x => new SubProjectBillDto
                {
                    ProjectBillId = x.Id,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.Name,
                    ProjectCode = x.Project.Code,
                    AccountName = x.AccountName,
                    BillRole = x.BillRole,
                    BillRate = x.BillRate,
                    StartTime = x.StartTime.Date,
                    EndTime = x.EndTime.Value.Date,
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

        [HttpGet]
        public async Task<List<ProjectClientPlanningDto>> GetAllProjectClientBill()
        {
            var query = await WorkScope.GetAll<ProjectUserBill>()
                .Select(x => new ProjectClientPlanningDto
                {
                    Id = x.Project.ClientId,
                    Name = x.Project.Client.Name
                })
                .Distinct()
                .Where(x => x.Id != null)
                .ToListAsync();
            return query;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Resource_TabAllBillAccount)]
        //[AbpAllowAnonymous]
        public GridResult<BillInfoDto> GetAllBillInfo(InputGetBillInfoDto input)
        {
            var result = WorkScope.All<ProjectUserBill>()
                .Select(x => new
                {
                    UserInfor = new GetUserBillDto
                    {
                        UserId = x.UserId,
                        AvatarPath = x.User.AvatarPath,
                        FullName = x.User.FullName,
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
                        ProjectStatus = x.Project.Status,
                        ProjectId = x.ProjectId,
                        ProjectName = x.Project.Name,
                        AccountName = x.AccountName,
                        BillRate = x.BillRate,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        Note = x.Note,
                        isActive = x.isActive,
                        ChargeType = x.ChargeType,
                        CurrencyCode = x.Project.Currency.Code,
                        ClientId = x.Project.ClientId,
                        ClientCode = x.Project.Client.Code,
                        ClientName = x.Project.Client.Name
                    },
                    IsCharge = x.isActive
                })
                .WhereIf(!string.IsNullOrEmpty(input.SearchText), s => s.UserInfor.EmailAddress.Contains(input.SearchText) ||
                    s.Project != null && (
                    (s.Project.AccountName != null && s.Project.AccountName.ToLower().Contains(input.SearchText.ToLower())) ||
                    (s.Project.ProjectCode != null && s.Project.ProjectCode.ToLower().Contains(input.SearchText.ToLower())) ||
                    (s.Project.ProjectName != null && s.Project.ProjectName.ToLower().Contains(input.SearchText.ToLower())) ||
                    (s.Project.ClientCode != null && s.Project.ClientCode.ToLower().Contains(input.SearchText.ToLower())) ||
                    (s.Project.ClientName != null && s.Project.ClientName.ToLower().Contains(input.SearchText.ToLower())) ||
                    (s.Project.Note != null && s.Project.Note.ToLower().Contains(input.SearchText.ToLower()))
                ))
                .WhereIf(input.ProjectId.HasValue, s => s.Project.ProjectId == input.ProjectId.Value)
                .WhereIf(input.ClientId.HasValue, s => s.Project.ClientId == input.ClientId.Value)
                .WhereIf(input.ProjectStatus.HasValue, s => s.Project.ProjectStatus == input.ProjectStatus.Value)
                .WhereIf(input.IsCharge.HasValue, s => s.IsCharge == input.IsCharge.Value)
                .GroupBy(s => s.UserInfor)
                .Select(s => new BillInfoDto
                {
                    UserInfor = s.Key,
                    Projects = s.Select(x => x.Project).OrderBy(x => x.ClientCode).ToList()
                }).OrderBy(s => s.UserInfor.EmailAddress)
                .AsQueryable();

            return result.GetGridResultSync(result, input);
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

        [HttpGet]
        [AbpAuthorize()]
        public async Task<List<GetAllResourceDto>> GetAllResource()
        {
            return await projectUserBillManager.QueryAllResource(false);
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
        public async Task<UpdateProjectUserBillDto> Create(UpdateProjectUserBillDto input)
        {
            if (input.EndTime.HasValue && input.StartTime.Date > input.EndTime.Value.Date)
                throw new UserFriendlyException($"Start date cannot be greater than end date !");

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
        public async Task<UpdateProjectUserBillDto> Update(UpdateProjectUserBillDto input)
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
            var projectUserBill = await WorkScope.GetAll<ProjectUserBill>()
                .Include(s => s.LinkedResources)
                .Where(s => s.Id == projectUserBillId)
                .FirstOrDefaultAsync();
            var linkedResources = projectUserBill.LinkedResources?.ToList();
            if (linkedResources != null)
            {
                foreach (var linkedResource in linkedResources)
                {
                    await WorkScope.DeleteAsync(linkedResource);
                }
            }
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
        public List<SubInvoiceDto> GetAllProjectCanUsing(long projectId)
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
        public ParentInvoiceDto GetParentInvoiceByProject(long projectId)
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

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<BillAccountDto>> GetAllBillAccount()
        {
            return await projectUserBillManager.GetAllBillAccount();
        }
        [HttpGet]
        public async Task<object> Get(long userId, long projectId)
        {
            return await WorkScope.GetAll<ProjectUserBill>().Select(x =>
             new
             {
                 UserInfor = new GetUserBillDto
                 {
                     UserId = x.UserId,
                     //UserName = x.User.Name,
                     AvatarPath = x.User.AvatarPath,
                     FullName = x.User.FullName,
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
                     ProjectStatus = x.Project.Status,
                     ProjectId = x.ProjectId,
                     ProjectName = x.Project.Name,
                     AccountName = x.User.FullName,
                     //BillRole = x.BillRole,
                     BillRate = x.BillRate,
                     StartTime = x.StartTime,
                     EndTime = x.EndTime,
                     Note = x.Note,
                     isActive = x.isActive,
                     ChargeType = x.ChargeType,
                     CurrencyCode = x.Project.Currency.Code,
                     ClientId = x.Project.ClientId,
                     ClientCode = x.Project.Client.Code,
                     ClientName = x.Project.Client.Name
                 }
             }).FirstOrDefaultAsync(x => x.UserInfor.UserId == userId && x.Project.ProjectId == projectId);
        }

        [HttpPut]
        public async Task UpdateBillNote(UpdateProjectUserBillNoteDto input)
        {
            var item = await WorkScope.GetAll<ProjectUserBill>()
                .FirstOrDefaultAsync(x => x.UserId == input.UserId && x.ProjectId == input.ProjectId);
            item.Note = input.Note;
            await WorkScope.UpdateAsync(item);
        }
    }
}