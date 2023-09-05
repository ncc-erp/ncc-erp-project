using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.CheckListItems.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckListItems
{
    [AbpAuthorize]
    public class CheckListItemAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        public async Task<GridResult<CheckListItemDetailDto>> GetAllPaging(GridParam input)
        {

            var checkListMandatories = WorkScope.GetAll<CheckListItemMandatory>();
            var query = from i in WorkScope.GetAll<CheckListItem>()
                        select new CheckListItemDetailDto
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Code = i.Code,
                            CategoryId = i.CategoryId,
                            CategoryName = i.CheckListCategory.Name,
                            Description = i.Description,
                            AuditTarget = i.AuditTarget,
                            PersonInCharge = i.PersonInCharge,
                            Note = i.Note,
                            mandatorys = checkListMandatories
                                .Where(x => x.CheckListItemId == i.Id)
                                .Select(x=>x.ProjectType)
                                .ToList()
                        };

            var filterMandatory = input.FilterItems != null ? input.FilterItems.Where(x => x.PropertyName == "mandatory").ToList() : null;
            if (filterMandatory != null && filterMandatory.Count > 0)
            {
                foreach (var item in filterMandatory)
                {
                    string searchByMandatory = item.Value.ToString();
                    input.FilterItems.Remove(item);
                    Enum.TryParse(searchByMandatory, out ProjectType Mandatory);
                    var listContainsProjectName = checkListMandatories.Where(x => x.ProjectType == Mandatory);
                    query = query.Where(x => listContainsProjectName.Any(y => y.CheckListItemId == x.Id));
                }
                
            }
            return await query.GetGridResult(query, input);
        }

        public async Task<List<CheckListItemDetailDto>> GetAll()
        {
            var listMan = WorkScope.GetAll<CheckListItemMandatory>();
            return await WorkScope.GetAll<CheckListItem>().Select(i => new CheckListItemDetailDto
            {
                Id = i.Id,
                Name = i.Name,
                Code = i.Code,
                CategoryId = i.CategoryId,
                CategoryName = i.CheckListCategory.Name,
                Description = i.Description,
                AuditTarget = i.AuditTarget,
                PersonInCharge = i.PersonInCharge,
                Note = i.Note,
                mandatorys = listMan.Where(x => x.CheckListItemId == i.Id).Select(x => x.ProjectType).ToList()
            }).ToListAsync();
        }

        public async Task<CheckListItemDetailDto> Create(CheckListItemDetailDto input)
        {
            var isExist = await WorkScope.GetAll<CheckListItem>().AnyAsync(x => x.Code.ToLower() == input.Code.ToLower());
            if (isExist)
            {
                throw new UserFriendlyException("Code '" + input.Code + "' of Checklist Item already existed.");
            }
            var item = ObjectMapper.Map<CheckListItem>(input);
            input.Id = await WorkScope.InsertAndGetIdAsync(item);
 
            foreach (var i in input.mandatorys)
            {// insert mandatory
                var itemMan = new CheckListItemMandatory
                {
                    CheckListItemId = input.Id,
                    ProjectType = i,
                };
                await WorkScope.InsertAndGetIdAsync(itemMan);
            }
            return input;
        }

        public async Task<CheckListItemDetailDto> Update(CheckListItemDetailDto input)
        {
            var checkExist = await WorkScope.GetAll<CheckListItem>()
                                .AnyAsync(x => x.Code.ToLower() == input.Code.ToLower() && x.Id != input.Id);
            if (checkExist)
            {
                throw new UserFriendlyException(string.Format("Code '{0}' of Checklist Item already existed.", input.Code));
            }

            var checkCategory = await WorkScope.GetAsync<CheckListCategory>(input.CategoryId);

            var item = await WorkScope.GetAsync<CheckListItem>(input.Id);
            ObjectMapper.Map(input, item);
            await WorkScope.UpdateAsync(item);

            // delete mandatory not exist in input
            var deleteMan = await WorkScope.GetAll<CheckListItemMandatory>()
                              .Where(x => x.CheckListItemId == input.Id && !input.mandatorys.Contains(x.ProjectType))
                              .ToListAsync();
            foreach (var i in deleteMan)
            {
                await WorkScope.DeleteAsync(i);
            }
            // insert if not exist mandatory
            foreach (var i in input.mandatorys)
            {
                var isExist = await WorkScope.GetAll<CheckListItemMandatory>()
                                .AnyAsync(x => x.CheckListItemId == input.Id && x.ProjectType == i);
                if (isExist) continue;
                var itemMan = new CheckListItemMandatory
                {
                    CheckListItemId = input.Id,
                    ProjectType = i,
                };
                itemMan.Id = await WorkScope.InsertAndGetIdAsync(itemMan);
            }
            return input;
        }

        public async Task Delete(long id)
        {
            // delete mandatory
            var mandatorys = await WorkScope.GetAll<CheckListItemMandatory>().Where(x => x.CheckListItemId == id).ToListAsync();
            foreach (var i in mandatorys)
            {
                await WorkScope.DeleteAsync(i);
            }
            await WorkScope.DeleteAsync<CheckListItem>(id);
        }
    }
}
