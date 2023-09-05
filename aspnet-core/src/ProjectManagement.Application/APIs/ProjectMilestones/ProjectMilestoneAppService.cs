
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.ProjectMilestones.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectMilestones
{
    [AbpAuthorize]
    public class ProjectMilestoneAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize()]
        public async Task<GridResult<GetProjectMilestoneDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<ProjectMilestone>()
                        .Select(x => new GetProjectMilestoneDto
                        {
                            Id = x.Id,
                            Description = x.Description,
                            Flag = x.Flag.ToString(),
                            Name = x.Name,
                            Note = x.Note,
                            ProjectId = x.ProjectId,
                            Status = x.Status.ToString(),
                            UATTimeEnd = x.UATTimeEnd,
                            UATTimeStart = x.UATTimeStart
                        });
            return await query.GetGridResult(query, input);
        }

        [AbpAuthorize()]
        public async Task<ProjectMilestoneDto> Create(ProjectMilestoneDto input)
        {
            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<ProjectMilestone>(input));
            return input;
        }

        [AbpAuthorize()]
        public async Task<ProjectMilestoneDto> Update(ProjectMilestoneDto input)
        {
            var isExist = await WorkScope.GetAsync<ProjectMilestone>(input.Id);
            ObjectMapper.Map(input, isExist);
            await WorkScope.UpdateAsync(isExist);
            return input;
        }

        [AbpAuthorize()]
        public async Task Delete(long id)
        {
            await WorkScope.DeleteAsync<ProjectMilestone>(id);
        }
    }
}
