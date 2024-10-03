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
    }
}
