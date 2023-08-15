using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using ProjectManagement.APIs.ProcessCriterias.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Helper;
using ProjectManagement.Services.Common;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProcessCriterias
{
    [AbpAuthorize]
    public class ProcessCriteriaAppService : ProjectManagementAppServiceBase
    {
        private readonly CommonManager _commonManager;

        public ProcessCriteriaAppService(CommonManager commonManager)
        {
            _commonManager = commonManager;
        }

        [AbpAuthorize(PermissionNames.Audits_Criteria)]
        [HttpPost]
        public async Task<TreeCriteriaDto> GetAll(InputToGetProcessCriteriaDto input)
        {
            var listLCs = WorkScope.GetAll<ProcessCriteria>().ToList();
            var listData = listLCs
                .Select(x => new GetProcessCriteriaDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    GuidLine = x.GuidLine,
                    IsActive = x.IsActive,
                    IsApplicable = x.IsApplicable,
                    Name = x.Name,
                    IsLeaf = x.IsLeaf,
                    Level = x.Level,
                    ParentId = x.ParentId,
                    QAExample = x.QAExample,
                })
                .OrderBy(x => CommonUtil.GetNaturalSortKey(x.Code))
                .ToList();

            if (input.IsGetAll())
            {
                return new TreeCriteriaDto
                {
                    Childrens = listData.GenerateTree(c => c.Id, c => c.ParentId)
                };
            }

            var listCriteriaIds = listData
               .WhereIf(input.IsLeaf.HasValue, x => x.IsLeaf == input.IsLeaf)
               .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
               .WhereIf(input.IsApplicable.HasValue, x => x.IsApplicable == input.IsApplicable)
               .WhereIf(!string.IsNullOrEmpty(input.SearchText),
               (x => x.Name.ToLower().Contains(input.SearchText.Trim().ToLower()) || (x.Code.Trim().ToLower().Contains(input.SearchText.Trim().ToLower()))))
               .Select(x => x.Id)
               .ToList();

            var resultIds = new List<long>();
            foreach (var id in listCriteriaIds)
            {
                resultIds.AddRange(_commonManager.GetAllNodeAndLeafIdById(id, listLCs, true));
            }
            resultIds = resultIds.Distinct().ToList();

            return new TreeCriteriaDto
            {
                Childrens = listData.Where(x => resultIds.Contains(x.Id)).ToList().GenerateTree(c => c.Id, c => c.ParentId)
            };
        }


        [AbpAuthorize(PermissionNames.Audits_Criteria_Create)]
        [HttpPost]
        public async Task<CreateProcessCriteriaDto> Create(CreateProcessCriteriaDto input)
        {
            var exist = await WorkScope.GetAll<ProcessCriteria>().AnyAsync(x => x.Name == input.Name || x.Code == input.Code);
            if (exist)
            { throw new UserFriendlyException(String.Format("Name or Code already exist in ProcessCriteria!")); }
            if (string.IsNullOrEmpty(input.Name.Trim()))
            {
                throw new UserFriendlyException(String.Format("Name of ProcessCriteria can't be Null or Empty!"));
            }
            var entity = ObjectMapper.Map<ProcessCriteria>(input);
            entity.IsActive = true;
            entity.IsLeaf = true;
            entity.IsApplicable = true;

            if (input.ParentId.HasValue)
            {
                var parent = await WorkScope.GetAsync<ProcessCriteria>(input.ParentId.Value);
                parent.IsLeaf = false;
                entity.Level = parent.Level + 1;
                //remove parent from Tailoring
                var parentTailor = WorkScope.GetAll<ProjectProcessCriteria>().Where(x => x.ProcessCriteriaId == input.ParentId.Value).ToList();
                if (parentTailor.Count > 0)
                {
                    parentTailor.ForEach(x =>
                    {
                        x.IsDeleted = true;
                    });
                }

                CurrentUnitOfWork.SaveChanges();
            }
            else
            {
                entity.Level = 1;
            }

            await WorkScope.InsertAsync(entity);
            return input;
        }

        [AbpAuthorize(PermissionNames.Audits_Criteria_Edit)]
        [HttpPut]
        public async Task<UpdateProcessCriteriaDto> Update(UpdateProcessCriteriaDto input)
        {
            //Valid
            var processCriteria = await WorkScope.GetAsync<ProcessCriteria>(input.Id);
            if (processCriteria == default)
                throw new UserFriendlyException($"Can not found process criteria with Id = {input.Id}");

            var exist = await WorkScope.GetAll<ProcessCriteria>().AnyAsync(x => (x.Name == input.Name || x.Code == input.Code) && x.Id != input.Id);
            if (exist)
            { throw new UserFriendlyException(String.Format("Name or Code already exist in ProcessCriteria!")); }
            if (string.IsNullOrEmpty(input.Name.Trim()))
            {
                throw new UserFriendlyException(String.Format("Name of ProcessCriteria can't be Null or Empty!"));
            }
            if (input.Code != processCriteria.Code && !processCriteria.IsLeaf)
            {
                await RenameChildCode(input);
            }
            await WorkScope.UpdateAsync(ObjectMapper.Map<UpdateProcessCriteriaDto, ProcessCriteria>(input, processCriteria));
            return input;
        }

        private async Task RenameChildCode(UpdateProcessCriteriaDto input)
        {
            var listPCs = WorkScope.GetAll<ProcessCriteria>().ToList();
            var listChildIds = _commonManager.GetAllChildId(input.Id, listPCs).Distinct();
            var listChilds = listPCs.Where(x => listChildIds.Contains(x.Id) && x.Id != input.Id).ToList();
            /* var split = input.Code.Split('.');
             var countDot = input.Code.Count(x => x == '.');
             var alterCode = split[countDot];
             listChilds.ForEach(x =>
             {
                 var splitChild =x.Code.Split(".");
                 splitChild[countDot] = alterCode;
                 x.Code = string.Join(".", splitChild);
             });*/
            var split = input.Code.Split('.');
            var countDot = split.Length - 1;
            for (int i = 0; i < listChilds.Count; i++)
            {
                var splitChild = listChilds[i].Code.Split(".");
                splitChild[countDot] = split[countDot];
                listChilds[i].Code = string.Join(".", splitChild);
            }

            await WorkScope.UpdateRangeAsync(listChilds);
        }

        [AbpAuthorize(PermissionNames.Audits_Criteria_DeActive)]
        public async Task Deactive(long Id)
        {
            var processCriteria = await WorkScope.GetAsync<ProcessCriteria>(Id);
            if (processCriteria == default)
                throw new UserFriendlyException($"Can not found process criteria with Id = {Id}");

            if (!processCriteria.IsLeaf)
            {
                var result = new List<long>();
                var listPCs = WorkScope.GetAll<ProcessCriteria>().ToList();
                var listIds = _commonManager.GetAllNodeAndLeafIdById(Id, listPCs).Distinct().ToList();
                var listChildPCs = listPCs
                     .Where(x => listIds.Contains(x.Id))
                     .ToList();
                listChildPCs.ForEach(x =>
                {
                    x.IsActive = false;
                });
                CurrentUnitOfWork.SaveChanges();
            }
            processCriteria.IsActive = false;
            await WorkScope.UpdateAsync(processCriteria);
        }

        [AbpAuthorize(PermissionNames.Audits_Criteria_Active)]
        public async Task Active(long Id)
        {
            var processCriteria = await WorkScope.GetAsync<ProcessCriteria>(Id);
            if (processCriteria == default)
                throw new UserFriendlyException($"Can not found process criteria with Id = {Id}");
            processCriteria.IsActive = true;
            if (processCriteria.ParentId.HasValue)
            {
                var parent = await WorkScope.GetAsync<ProcessCriteria>(processCriteria.ParentId.Value);
                if (!parent.IsActive)
                {
                    throw new UserFriendlyException($"You must activate its parent to activate this criteria!");
                }
            }
            await WorkScope.UpdateAsync(processCriteria);
        }

        [AbpAuthorize(PermissionNames.Audits_Criteria_Delete)]
        [HttpDelete]
        public async Task Delete(long id)
        {
            var processCriteria = await WorkScope.GetAsync<ProcessCriteria>(id);
            if (processCriteria == default)
                throw new UserFriendlyException($"Can not found process criteria with Id = {id}");
            if (!processCriteria.IsLeaf)
            {
                var listPCs = WorkScope.GetAll<ProcessCriteria>().ToList();
                var listIds = _commonManager.GetAllNodeAndLeafIdById(id, listPCs).Distinct().ToList();
                await ValidToDeleteListCriteria(id, listIds);
                foreach (var Id in listIds)
                {
                    await WorkScope.DeleteAsync<ProcessCriteria>(Id);
                }
            }
            else
            {
                await ValidToDeleteLeafCriteria(id);
                await WorkScope.DeleteAsync<ProcessCriteria>(id);
            }
            CurrentUnitOfWork.SaveChanges();
            if (processCriteria.ParentId.HasValue)
            {
                var parentID = processCriteria.ParentId.Value;
                var parent = await WorkScope.GetAsync<ProcessCriteria>(parentID);
                var countRemainChild = WorkScope.GetAll<ProcessCriteria>().Any(x => x.ParentId == parentID);
                if (!countRemainChild)
                {
                    parent.IsLeaf = true;
                    await WorkScope.UpdateAsync(parent);
                }
            }
        }

        [HttpGet]
        public string GetGuildLine(long Id)
        {
            var PC = WorkScope.GetAll<ProcessCriteria>()
                .Where(x => x.Id == Id)
                .FirstOrDefault();
            if (PC == default)
            {
                throw new UserFriendlyException($"Can not process criteria with Id = {Id}");
            }
            return PC.GuidLine;
        }

        [HttpGet]
        public string GetQAExample(long Id)
        {
            var PC = WorkScope.GetAll<ProcessCriteria>()
                .Where(x => x.Id == Id)
                .FirstOrDefault();
            if (PC == default)
            {
                throw new UserFriendlyException($"Can not process criteria with Id = {Id}");
            }
            return PC.QAExample;
        }

        public async Task ValidToDeleteLeafCriteria(long id)
        {
            var project = WorkScope.GetAll<ProjectProcessCriteriaResult>()
                .Where(x => x.ProcessCriteriaId == id)
                .FirstOrDefault();
            if (project != default)
            {
                throw new UserFriendlyException($"Can not delete criteria because it had result");
            }
        }

        public async Task ValidToDeleteListCriteria(long id, List<long> listCriteriaIds = default)
        {
            var listPCs = new List<ProcessCriteria>();
            var listPCIds = new List<long>();
            if (listCriteriaIds == default)
            {
                listPCs = WorkScope.GetAll<ProcessCriteria>().ToList();
                listPCIds = _commonManager.GetAllNodeAndLeafIdById(id, listPCs).Distinct().ToList();
            }
            var listIds = WorkScope.GetAll<ProjectProcessCriteriaResult>()
                .WhereIf(listCriteriaIds != default, x => listCriteriaIds.Contains(x.ProcessCriteriaId))
                .WhereIf(listCriteriaIds == default, x => listPCIds.Contains(x.ProcessCriteriaId))
                .Select(x => x.ProcessCriteriaId)
                .ToList();
            if (listIds != null && listIds.Count() > 0)
            {
                throw new UserFriendlyException($"Can not delete criteria because its leaf had result");
            }
        }

        [HttpGet]
        public async Task<GetProcessCriteriaDto> Get(long id)
        {
            var item = await WorkScope.GetAsync<ProcessCriteria>(id);
            return new GetProcessCriteriaDto
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                IsApplicable = item.IsApplicable,
                IsActive = item.IsActive,
                GuidLine = item.GuidLine,
                QAExample = item.QAExample,
                ParentId = item.ParentId,
                Level = item.Level,
                IsLeaf = item.IsLeaf
            };
        }

        [HttpGet]
        public async Task<List<GetProcessCriteriaDto>> GetFlatedSideList()
        {
            return await WorkScope.GetAll<ProcessCriteria>()
                .OrderBy(x => x.Code)
                .ThenBy(x => x.Level)
                .Select(x => new GetProcessCriteriaDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsApplicable = x.IsApplicable,
                    IsActive = x.IsActive,
                    GuidLine = x.GuidLine,
                    QAExample = x.QAExample,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    IsLeaf = x.IsLeaf
                })
                .ToListAsync();
        }

        [HttpGet]
        public async Task<List<GetForDropDownDto>> GetForDropDown()
        {
            var allProcessCriteria = await WorkScope.GetAll<ProcessCriteria>().ToListAsync();

            return allProcessCriteria
                .Select(x => new GetForDropDownDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsApplicable = x.IsApplicable,
                    IsActive = x.IsActive,
                    GuidLine = x.GuidLine,
                    QAExample = x.QAExample,
                    ParentId = x.ParentId,
                    Level = x.Level,
                    IsLeaf = x.IsLeaf,
                    MaxValueOfListCode = allProcessCriteria
                        .Where(y => y.ParentId == x.Id && y.Level >= 1).Count() > 0 ? allProcessCriteria
                        .Where(y => y.ParentId == x.Id && y.Level >= 1)
                        .Max(x => GetNumberAfterLastDot(x.Code)) : 0
                })
                .OrderBy(x => CommonUtil.GetNaturalSortKey(x.Code))
                .ToList();
        }

        private int GetNumberAfterLastDot(string input)
        {
            int dotIndex = input.LastIndexOf('.');
            string lastNumber = dotIndex != -1 ? input.Substring(dotIndex + 1) : input;
            int result = Convert.ToInt32(lastNumber);
            return result;
        }

        [HttpGet]
        public async Task<List<GetProcessCriteriaDto>> GetAllProcessTailoringContain(long id)
        {
            var listProcessTailoring = await GetPCIdsTailoringContain(id);

            var listLeafContain = WorkScope.GetAll<ProcessCriteria>()
                .Where(x => listProcessTailoring.Contains(x.Id))
                .Select(x => new GetProcessCriteriaDto
                {
                    Id = x.Id,
                    IsLeaf = x.IsLeaf,
                    Code = x.Code,
                    IsActive = x.IsActive,
                    IsApplicable = x.IsApplicable,
                    Level = x.Level,
                    Name = x.Name,
                    ParentId = x.ParentId,
                }).ToList();
            return listLeafContain;
        }

        private async Task<List<long>> GetPCIdsTailoringContain(long id)
        {
            var listPCs = WorkScope.GetAll<ProcessCriteria>().ToList();
            var nodeAndLeaf = _commonManager.GetAllNodeAndLeafIdById(id, listPCs);
            return WorkScope.GetAll<ProjectProcessCriteria>()
                        .Where(x => nodeAndLeaf.Contains(x.ProcessCriteriaId))
                        .Select(x => x.ProcessCriteriaId)
                        .ToList();
        }

        [AbpAuthorize(PermissionNames.Audits_Criteria_Delete)]
        [HttpDelete]
        public async Task RemoveCriteriaFromTailoring(long Id)
        {
            var items = await GetPCIdsTailoringContain(Id);
            var criteria = WorkScope.GetAll<ProjectProcessCriteria>().
                Where(x => items.Contains(x.ProcessCriteriaId)).ToList();
            criteria.ForEach(x =>
            {
                x.IsDeleted = true;
            });
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAuthorize(PermissionNames.Audits_Criteria_ChangeApplicable)]
        [HttpPut]
        public async Task ChangeApplicable(long Id)
        {
            var item = await WorkScope.GetAsync<ProcessCriteria>(Id);
            item.IsApplicable = !item.IsApplicable;
            await WorkScope.UpdateAsync(item);
        }

        public async Task<bool> ValidTailoringContain(long id)
        {
            return WorkScope.GetAll<ProjectProcessCriteria>()
                .Any(x => x.ProcessCriteriaId == id);
        }
    }
}