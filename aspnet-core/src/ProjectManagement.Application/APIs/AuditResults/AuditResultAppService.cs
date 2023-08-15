
using Abp.Authorization;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.AuditResults.Dto;
using ProjectManagement.APIs.PMReportProjects.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.AuditResults
{
    [AbpAuthorize]
    public class AuditResultAppService:ProjectManagementAppServiceBase
    {
        public async Task<AuditResultDto> Create(AuditResultDto input)
        {
            var isExist = await WorkScope.GetAll<AuditResult>()
                            .AnyAsync(x=>x.AuditSessionId == input.AuditSessionId && x.ProjectId == input.ProjectId);
            if (isExist)
            {
                throw new UserFriendlyException("Audit Result already exists.");
            }
            input.PMId = WorkScope.GetAsync<Project>(input.ProjectId).Result.PMId;
            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<AuditResult>(input));
            return input;
        }
        public async Task<AuditResultDto> Update(AuditResultDto input)
        {
            var isExist = await WorkScope.GetAll<AuditResult>()
                            .AnyAsync(x => x.AuditSessionId == input.AuditSessionId && x.ProjectId == input.ProjectId && x.Id != input.Id);
            var auditResult = await WorkScope.GetAsync<AuditResult>(input.Id);
            auditResult.Status = input.status;
            if (isExist)
            {
                throw new UserFriendlyException("Audit Result already exists.");
            }
            await WorkScope.UpdateAsync(auditResult);
            return input;
        }
        public async Task Delete(long id)
        {
            await WorkScope.DeleteAsync<AuditResult>(id);
        }
        public async Task<string> GetNote(long id)
        {
            var isExist = await WorkScope.GetAsync<AuditResult>(id);
            return isExist.Note;
        }
        public async Task<UpdateNoteDto> UpdateNote(UpdateNoteDto input)
        {
            var isExist = await WorkScope.GetAsync<AuditResult>(input.Id);
            isExist.Note = input.Note;
            await WorkScope.UpdateAsync(isExist);
            return input;
        }
    }
}
