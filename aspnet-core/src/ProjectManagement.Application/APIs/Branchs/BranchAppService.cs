using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Branchs.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Branchs
{
    [AbpAuthorize]
    public class BranchAppService : ProjectManagementAppServiceBase
    {
       

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Branchs)]
        public async Task<GridResult<BranchDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Branch>()
                .Select(s => new BranchDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    DisplayName = s.DisplayName,
                    Code = s.Code,
                    Color = s.Color
                });
            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<BranchDto>> GetAllNotPagging()
        {
            return await WorkScope.GetAll<Branch>()
                 .Select(s => new BranchDto
                 {
                     Id = s.Id,
                     Name = s.Name,
                     DisplayName = s.DisplayName,
                 }).ToListAsync();

        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Branchs_Create)]
        public async Task<BranchDto> Create(BranchDto input)
        {
            var isExist = await WorkScope.GetAll<Branch>().AnyAsync(x => x.Name == input.Name || x.Code == input.Code);

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or Code already exist !"));

            await WorkScope.InsertAsync(ObjectMapper.Map<Branch>(input));

            return input;
        }
        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Branchs_Edit)]
        public async Task<BranchDto> Update(BranchDto input)
        {
            var Branch = await WorkScope.GetAsync<Branch>(input.Id);

            var isExist = await WorkScope.GetAll<Branch>().AnyAsync(x => x.Id != input.Id && (x.Name == input.Name || x.Code == input.Code));

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or Code already exist !"));

            await WorkScope.UpdateAsync(ObjectMapper.Map<BranchDto, Branch>(input, Branch));
            return input;
        }


        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_Branchs_Delete)]
        public async Task Delete(long BranchId)
        {
            var hasUser = await WorkScope.GetAll<User>().AnyAsync(s => s.BranchId == BranchId);
            if (hasUser)
                throw new UserFriendlyException(String.Format("Branch Id {0} has user", BranchId));
            await WorkScope.GetRepo<Branch>().DeleteAsync(BranchId);
        }


        [HttpGet]
        public async Task<List<BranchDto>> GetAllBranchFilter(bool isAll = false)
        {
            var branchId = await WorkScope.GetAll<Branch>().Select(s => s.Id).FirstOrDefaultAsync();
            var query = await WorkScope.GetAll<Branch>()
                 .Select(s => new BranchDto
                 {
                     Id = s.Id,
                     Name = s.Name,
                     DisplayName = s.DisplayName
                 }).ToListAsync();
            if (isAll)
            {
                query.Add(new BranchDto
                {
                    Name = "All",
                    DisplayName = "All",
                    Id = 0,
                });
            }
            return query.OrderBy(s => s.Id).ToList();
        }
    }
}

