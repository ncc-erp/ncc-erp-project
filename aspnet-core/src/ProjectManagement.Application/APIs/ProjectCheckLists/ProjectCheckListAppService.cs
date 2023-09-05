using Abp.Authorization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.AuditResultPeoples.Dto;
using ProjectManagement.APIs.ProjectCheckLists.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectCheckLists
{
    [AbpAuthorize]
    public class ProjectCheckListAppService : ProjectManagementAppServiceBase
    {
        [AbpAuthorize]
        public async Task<ProjectCheckListDto> Create(ProjectCheckListDto input)
        {
            var isExist = await WorkScope.GetAll<ProjectCheckList>()
                .AnyAsync(x => x.ProjectId == input.ProjectId && x.CheckListItemId == input.CheckListItemId);
            if (isExist)
            {
                throw new UserFriendlyException("Project with Id '" + input.ProjectId + "' created with item with id '" + input.CheckListItemId + "'");
            }
            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<ProjectCheckList>(input));
            return input;
        }

        [AbpAuthorize]
        public async Task Delete(long ProjectId, long CheckListItemId)
        {
            var isExist = await WorkScope.GetAll<ProjectCheckList>()
               .Where(x => x.ProjectId == ProjectId && x.CheckListItemId == CheckListItemId).FirstOrDefaultAsync();
            if (isExist == default)
            {
                throw new UserFriendlyException("Project with Id '" + ProjectId + "' does not exist with item with id '" + CheckListItemId + "'");
            }
            await WorkScope.DeleteAsync(isExist);
        }

        [AbpAuthorize]
        public async Task ReverseActive(long ProjectId, long CheckListItemId)
        {
            var isExist = await WorkScope.GetAll<ProjectCheckList>()
               .Where(x => x.ProjectId == ProjectId && x.CheckListItemId == CheckListItemId).FirstOrDefaultAsync();
            if (isExist == default)
            {
                throw new UserFriendlyException("Project with Id '" + ProjectId + "' does not exist with item with id '" + CheckListItemId + "'");
            }
            isExist.IsActive = !isExist.IsActive;
            await WorkScope.UpdateAsync(isExist);
        }

        [AbpAuthorize]
        public async Task<List<ProjectCheckListDto>> AddByProjectType(ProjectType input)
        {
            var projectChecklists = await (from p in WorkScope.GetAll<Project>()
                                             .Where(x => x.ProjectType == input && x.Status != ProjectStatus.Closed && x.Status != ProjectStatus.Potential)
                                           from ci in WorkScope.GetAll<CheckListItem>()
                                           select new ProjectCheckListDto
                                           {
                                               CheckListItemId = ci.Id,
                                               ProjectId = p.Id,
                                               IsActive = true
                                           }).ToListAsync();
            foreach (var i in projectChecklists)
            {
                i.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<ProjectCheckList>(i));
            }
            return projectChecklists;
        }

        [AbpAuthorize]
        public async Task<List<CheckListItemByProjectDto>> GetCheckListItemByProject(long projectId, long? auditSessionId)
        {// lấy về checklist item thuộc project thuộc đợt
            var isExistProject = await WorkScope.GetAsync<Project>(projectId);
            if (auditSessionId.HasValue)
            {
                var isExistAuditSession = await WorkScope.GetAsync<AuditSession>(auditSessionId.Value);
            }
            var auditResultIds = await WorkScope.GetAll<AuditResult>()
                                .Where(x =>(!auditSessionId.HasValue || x.AuditSessionId == auditSessionId) && x.ProjectId == projectId)
                                .Select(x => x.Id).ToListAsync();
            var checkListItemIds = await WorkScope.GetAll<ProjectCheckList>().Where(x => x.ProjectId == projectId)
                .Select(x => x.CheckListItemId).ToListAsync();
            var auditResultPeople = WorkScope.GetAll<AuditResultPeople>()
                                    .Where(x => auditResultIds.Contains(x.AuditResultId) && checkListItemIds.Contains(x.CheckListItemId))
                                    .Select(x => new GetAuditResultPeopleDto
                                    {
                                        Id = x.Id,
                                        CuratorId = x.CuratorId,
                                        CuratorName = !x.CuratorId.HasValue ? default : x.Curator.FullName,
                                        IsPass = x.IsPass,
                                        Note = x.Note,
                                        UserId = x.UserId,
                                        UserName = x.User.FullName,
                                        CheckListItemId = x.CheckListItemId,
                                        AvatarPath = x.User.AvatarPath,
                                        Branch = x.User.BranchOld,
                                        EmailAddress = x.User.EmailAddress,
                                        FullName = x.User.FullName,
                                        UserType = x.User.UserType
                                    });
            var checkListMandatories = WorkScope.GetAll<CheckListItemMandatory>();
            var checkListInProjects = await WorkScope.GetAll<ProjectCheckList>().Include(x => x.CheckListItem)
                    .Where(x => x.ProjectId == projectId)
                    .Select(x => new CheckListItemByProjectDto
                    {
                        Id = x.CheckListItemId,
                        AuditTarget = x.CheckListItem.AuditTarget,
                        CategoryName = x.CheckListItem.CheckListCategory.Name,
                        Code = x.CheckListItem.Code,
                        Description = x.CheckListItem.Description,
                        Name = x.CheckListItem.Name,
                        Note = x.CheckListItem.Note,
                        PersonInCharge = x.CheckListItem.PersonInCharge,
                        RegistrationDate = x.CreationTime,
                        people = auditResultPeople.Where(y => y.CheckListItemId == x.CheckListItemId).ToList(),
                        Mandatories = checkListMandatories.Where(s => s.CheckListItemId == x.CheckListItemId).Select(x => x.ProjectType).ToList(),
                    }).ToListAsync();
            return checkListInProjects;
        }

        [AbpAuthorize]
        public async Task<List<ProjectCheckListDto>> AddCheckListItemByProject(long projectId, List<long> checkListItemIds)
        {
            var result = new List<ProjectCheckListDto>();

            var isExist = await WorkScope.GetAll<ProjectCheckList>()
            .AnyAsync(x => x.ProjectId == projectId && checkListItemIds.Contains(x.CheckListItemId));
            if (isExist)
            {
                throw new UserFriendlyException("Project with Id '" + projectId + "' created with item in list Check List Item");
            }

            foreach (var i in checkListItemIds)
            {
                var temp = new ProjectCheckListDto
                {
                    CheckListItemId = i,
                    IsActive = true,
                    ProjectId = projectId
                };
                temp.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<ProjectCheckList>(temp));
                result.Add(temp);
            }
            return result;
        }
    }
}
