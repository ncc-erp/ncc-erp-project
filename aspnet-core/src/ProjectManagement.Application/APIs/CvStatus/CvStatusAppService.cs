using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.CvStatus.Dto;
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
        public async Task<CvStatusCreateEditDto> Update(long id, CvStatusCreateEditDto input)
        {
            var cvStatus = await WorkScope.GetAsync<Entities.CvStatus>(id);
            var checkName = await WorkScope.GetAll<Entities.CvStatus>().AnyAsync(cv => cv.Name == input.Name && cv.Id != id);
            if (checkName)
            {
                throw new UserFriendlyException("Name already exists !");
            }
            await WorkScope.UpdateAsync(ObjectMapper.Map<CvStatusCreateEditDto, Entities.CvStatus>(input, cvStatus));
            return input;
        }

        [HttpDelete]
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
    }
}
