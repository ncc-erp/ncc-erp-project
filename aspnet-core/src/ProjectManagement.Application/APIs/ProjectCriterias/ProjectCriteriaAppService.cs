using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.ProjectCriterias.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectCriterias
{
    [AbpAuthorize]
    public class ProjectCriteriaAppService : ProjectManagementAppServiceBase
    {
        [AbpAuthorize(PermissionNames.Admin_Criteria_Create)]
        [HttpPost]
        public async Task<CreateProjectCriteriaDto> Create(CreateProjectCriteriaDto input)
        {
            var isExist = await WorkScope.GetAll<ProjectCriteria>().AnyAsync(x => x.Name == input.Name);

            if (isExist)
                throw new UserFriendlyException(String.Format("Name already exist !"));

            input.Id = await WorkScope.InsertAndGetIdAsync<ProjectCriteria>(ObjectMapper.Map<ProjectCriteria>(input));
            return input;
        }

        [AbpAuthorize(PermissionNames.Admin_Criteria_Delete)]
        [HttpDelete]
        public async Task Delete(long prjCriteriaId)
        {
            await WorkScope.DeleteAsync<ProjectCriteria>(prjCriteriaId);
        }

        [HttpGet]
        public async Task<GetProjectCriteriaForCheckDto> Get(long id)
        {
            var res = await WorkScope.GetAsync<ProjectCriteria>(id);
            var isExist = WorkScope.GetAll<ProjectCriteriaResult>().Any(x => x.ProjectCriteriaId == id);
            return new GetProjectCriteriaForCheckDto
            {
                Guideline = res.Guideline,
                Id = res.Id,
                Name = res.Name,
                IsExist = isExist,
                IsActive = res.IsActive
            };
        }

        [HttpGet]
        public async Task<List<GetProjectCriteriaDto>> GetAll()
        {
            var listExistCriteria = WorkScope.GetAll<ProjectCriteriaResult>()
                .Select(x => x.ProjectCriteriaId)
                .Distinct().ToList();
            return await WorkScope.GetAll<ProjectCriteria>()
                .Select(x => new GetProjectCriteriaDto
                {
                    Guideline = x.Guideline,
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    IsExist = listExistCriteria.Contains(x.Id)
                }).ToListAsync();
        }

        [AbpAuthorize(PermissionNames.Admin_Criteria_View)]
        [HttpPost]
        public async Task<GridResult<GetProjectCriteriaDto>> GetAllPaging(GridParam input)
        {
            var listExistCriteria = WorkScope.GetAll<ProjectCriteriaResult>()
                .Select(x => x.ProjectCriteriaId)
                .Distinct().ToList();
            var query = WorkScope.GetAll<ProjectCriteria>()
                .Select(c => new GetProjectCriteriaDto
                {
                    Guideline = c.Guideline,
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                    IsExist = listExistCriteria.Contains(c.Id)
                });

            return await query.GetGridResult(query, input);
        }

        [AbpAuthorize(
            PermissionNames.Admin_Criteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Update_Guideline
            )]
        [HttpPut]
        public async Task<CreateProjectCriteriaDto> Update(CreateProjectCriteriaDto input)
        {
            var prjCriteria = await WorkScope.GetAsync<ProjectCriteria>(input.Id);

            var isExist = await WorkScope.GetAll<ProjectCriteria>().AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException(String.Format("Name already exist !"));

            await WorkScope.UpdateAsync(ObjectMapper.Map<CreateProjectCriteriaDto, ProjectCriteria>(input, prjCriteria));
            return input;
        }

        [AbpAuthorize(PermissionNames.Admin_Criteria_Active_DeActive)]
        [HttpDelete]
        public async Task DeActive(long prjCriteriaId)
        {
            var item = await WorkScope.GetAsync<ProjectCriteria>(prjCriteriaId);
            item.IsActive = false;
            await WorkScope.UpdateAsync(item);
        }

        [AbpAuthorize(PermissionNames.Admin_Criteria_Active_DeActive)]
        [HttpDelete]
        public async Task Active(long prjCriteriaId)
        {
            var item = await WorkScope.GetAsync<ProjectCriteria>(prjCriteriaId);
            item.IsActive = true;
            await WorkScope.UpdateAsync(item);
        }
    }
}