using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.CheckListItems;
using ProjectManagement.APIs.ChecklistTitles.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ChecklistTitles
{
    [AbpAuthorize]
    public class CheckListCategoryAppService : ProjectManagementAppServiceBase
    {
        private readonly CheckListItemAppService _checkListItem;

        public CheckListCategoryAppService(CheckListItemAppService checkListItem)
        {
            _checkListItem = checkListItem;
        }

        [HttpPost]
        public async Task<GridResult<CheckListCategoryDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<CheckListCategory>()
                        .Select(x => new CheckListCategoryDto
                        {
                            Id = x.Id,
                            Name = x.Name
                        });
            return await query.GetGridResult(query, input);
        }

        public async Task<List<CheckListCategoryDto>> GetAll()
        {
            return await WorkScope.GetAll<CheckListCategory>()
                        .Select(x => new CheckListCategoryDto
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).ToListAsync();
        }
        public async Task<CheckListCategoryDto> Create(CheckListCategoryDto input)
        {
            var isExist = await WorkScope.GetAll<CheckListCategory>().AnyAsync(x => x.Name.ToLower() == input.Name.ToLower());
            if (isExist)
            {
                throw new UserFriendlyException("Name '" + input.Name + "' of Checklist Title already existed.");
            }
            var item = ObjectMapper.Map<CheckListCategory>(input);
            input.Id = await WorkScope.InsertAndGetIdAsync(item);
            return input;
        }
        public async Task<CheckListCategoryDto> Update(CheckListCategoryDto input)
        {
            var isExist = await WorkScope.GetAll<CheckListCategory>()
                                .AnyAsync(x => x.Name.ToLower() == input.Name.ToLower() && x.Id != input.Id);
            if (isExist)
            {
                throw new UserFriendlyException("Name '" + input.Name + "' of Checklist Title already existed.");
            }
            var item = await WorkScope.GetAsync<CheckListCategory>(input.Id);
            ObjectMapper.Map(input, item);
            await WorkScope.UpdateAsync(item);
            return input;
        }
        public async Task Delete(long id)
        {
            var isExistItem = await WorkScope.GetAll<CheckListItem>().AnyAsync(x => x.CategoryId == id);
            if (isExistItem)
            {
                throw new UserFriendlyException("Check List Category with '" + id + "' has Check List Item. Please delete them before deleting.");
            }
            await WorkScope.DeleteAsync<CheckListCategory>(id);
        }

    }
}
