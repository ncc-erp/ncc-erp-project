using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.OnboardingTemplate.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.OnboardingTemplate
{
    [AbpAuthorize(PermissionNames.Admin)]
    public class OnboardingTemplateAppService : ProjectManagementAppServiceBase
    {
        public async Task<List<OnboardingChecklistTemplateDto>> GetAll()
        {
            var templates = await WorkScope.GetAll<OnboardingChecklist>()
                .OrderBy(x => x.Order)
                .ToListAsync();

            return templates.Select(x => new OnboardingChecklistTemplateDto
            {
                Id = x.Id,
                Label = x.Label,
                Order = x.Order,
                Details = x.DetailsJson
            }).ToList();
        }

        [HttpPost]
        public async Task<OnboardingChecklistTemplateDto> Save(OnboardingChecklistTemplateDto input)
        {
            var entity = await WorkScope.GetAll<OnboardingChecklist>()
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            if (entity == null)
            {
                entity = new OnboardingChecklist();
            }

            entity.Label = input.Label;
            entity.Order = input.Order;
            entity.DetailsJson = input.Details;
            var id = await WorkScope.InsertOrUpdateAndGetIdAsync(entity);
            input.Id = id;
            return input;
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            var entity = await WorkScope.GetAsync<OnboardingChecklist>(id);
            if (entity != null)
            {
                await WorkScope.DeleteAsync(entity);
            }
        }

        [HttpPost]
        public async Task<GridResult<OnboardingChecklistTemplateDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<OnboardingChecklist>()
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new OnboardingChecklistTemplateDto
                {
                    Id = x.Id,
                    Details  = x.DetailsJson,
                    Label = x.Label,
                    Order = x.Order,
                });
           
            return await query.GetGridResult(query, input);
        }
    }
}
