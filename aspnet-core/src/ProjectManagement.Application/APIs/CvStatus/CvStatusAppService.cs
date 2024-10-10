using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.CvStatus.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.CvStatus
{
    [AbpAuthorize]
    public class CvStatusAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_CVStatus_Create)]
        public async Task<CvStatusCreateEditDto> Create(CvStatusCreateEditDto input)
        {
            var checkName = await WorkScope.GetAll<Entities.CvStatus>().AnyAsync(cv => cv.Name == input.Name);
            if (checkName)
            {
                throw new UserFriendlyException("Name already exists !");
            }
            await WorkScope.InsertAsync(ObjectMapper.Map<Entities.CvStatus>(input));
            return input;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_CVStatus)]
        public async Task<GridResult<CvStatusDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Entities.CvStatus>()
                .Select(cv => new CvStatusDto
                {
                    Id = cv.Id,
                    Name = cv.Name,
                    Color = cv.Color,
                });
            return await query.GetGridResult(query, input);
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_CVStatus_Edit)]
        public async Task<CvStatusCreateEditDto> Update(CvStatusCreateEditDto input)
        {
            var cvStatus = await WorkScope.GetAsync<Entities.CvStatus>(input.Id);
            var checkName = await WorkScope.GetAll<Entities.CvStatus>().AnyAsync(cv => cv.Name == input.Name && cv.Id != input.Id);
            if (checkName)
            {
                throw new UserFriendlyException("Name already exists !");
            }
            await WorkScope.UpdateAsync(ObjectMapper.Map<CvStatusCreateEditDto, Entities.CvStatus>(input, cvStatus));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_CVStatus_Delete)]
        public async Task Delete(long id)
        {
            var cvStatus = await WorkScope.GetAsync<Entities.CvStatus>(id);
            var isUsed = await WorkScope.GetAll<ResourceRequestCV>().AnyAsync(cv => cv.CvStatusId == id);
            if (isUsed)
            {
                throw new UserFriendlyException($"CVStatus ID {id} is being used");
            }
            await WorkScope.GetRepo<Entities.CvStatus>().DeleteAsync(cvStatus);
        }

        [HttpGet]
        public async Task<List<CvStatusDto>> GetAll()
        {
            return await WorkScope.GetAll<Entities.CvStatus>()
                .Select(s => new CvStatusDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Color = s.Color,
                }).ToListAsync();

        }
    }
}
