using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Positions.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Positions
{
    [AbpAuthorize]
    public class PositionAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Positions)]
        public async Task<GridResult<PositionDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Position>()
                .Select(s => new PositionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    ShortName = s.ShortName,
                    Code = s.Code,
                    Color = s.Color,
                });
            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<PositionDto>> GetAll()
        {
            var query = WorkScope.GetAll<Position>()
               .Select(s => new PositionDto
               {
                   Id = s.Id,
                   Name = s.Name,
                   ShortName = s.ShortName,
                   Code = s.Code,
                   Color = s.Color
               })
               .OrderBy(s => s.ShortName);
            return await query.ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Positions_Create)]
        public async Task<PositionDto> Create(PositionDto input)
        {
            var isExist = await WorkScope.GetAll<Position>().AnyAsync(x => x.Name == input.Name || x.Code == input.Code || x.ShortName == input.ShortName);

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or ShortName or Code already exist or Name input not coincide Old Name !"));

            await WorkScope.InsertAsync(ObjectMapper.Map<Position>(input));

            return input;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Positions_Edit)]
        public async Task<PositionDto> Update(PositionDto input)
        {
            var Position = await WorkScope.GetAsync<Position>(input.Id);

            var isExist = await WorkScope.GetAll<Position>().AnyAsync(x => x.Id != input.Id && (x.Name == input.Name || x.Code == input.Code || x.ShortName == input.ShortName));

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or ShortName or Code already exist or Name input not coincide Old Name !"));

            await WorkScope.UpdateAsync(ObjectMapper.Map<PositionDto, Position>(input, Position));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_Positions_Delete)]
        public async Task Delete(long positionId)
        {
            var hasUser = await WorkScope.GetAll<User>().AnyAsync(s => s.PositionId == positionId);
            if (hasUser)
                throw new UserFriendlyException(String.Format("Position Id {0} has user", positionId));
            await WorkScope.GetRepo<Position>().DeleteAsync(positionId);
        }

        [HttpGet]
        public async Task<List<PositionDto>> GetAllPositionFilter(bool isAll = false)
        {
            var positionId = await WorkScope.GetAll<Position>().Select(s => s.Id).FirstOrDefaultAsync();
            var query = await WorkScope.GetAll<Position>()
                 .Select(s => new PositionDto
                 {
                     Id = s.Id,
                     Name = s.Name,
                     ShortName = s.ShortName,
                     Code = s.Code,
                     Color = s.Color,
                 }).ToListAsync();
            if (isAll)
            {
                query.Add(new PositionDto
                {
                    Name = "All",
                    ShortName = "All",
                    Id = 0,
                });
            }
            return query.OrderBy(s => s.Id).ToList();
        }

        [HttpGet]
        public List<PositionDto> GetAllNotPagging()
        {
            return WorkScope.GetAll<ProjectManagement.Entities.Position>()
                 .Select(s => new PositionDto
                 {
                     Id = s.Id,
                     Name = s.Name,
                     ShortName = s.ShortName,
                     Code = s.Code,
                     Color = s.Color,
                 }).ToList();
        }
    }
}