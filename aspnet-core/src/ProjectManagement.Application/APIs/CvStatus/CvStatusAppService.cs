using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.CvStatus.Dto;
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
    }
}
