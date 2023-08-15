using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProjectManagement.APIs.ProcessCriterias.Dto;
using ProjectManagement.APIs.ProjectProcessCriteriaResults.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Configuration;
using ProjectManagement.Configuration.Dto;
using ProjectManagement.Entities;
using ProjectManagement.Helper;
using ProjectManagement.Services.Common;
using ProjectManagement.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ProjectManagement.APIs.ProjectProcessResults.Dto.GetProjectProcessResultDto;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessCriteriaResults
{
    public class ProjectProcessCriteriaResultAppService : ProjectManagementAppServiceBase
    {
        private readonly IRepository<ProjectProcessCriteriaResult, long> _projectProcessCriteriaResultRepository;
        private readonly IMapper _mapper;
        private readonly CommonManager _commonManager;

        public ProjectProcessCriteriaResultAppService(IRepository<ProjectProcessCriteriaResult,
            long> projectProcessCriteriaResultRepository,
            IMapper mapper,
            CommonManager commonManager
            )
        {
            _projectProcessCriteriaResultRepository = projectProcessCriteriaResultRepository;
            _mapper = mapper;
            _commonManager = commonManager;
        }

        //private async Task CreateProjectProcessCriteriaResult(List<ProjectProcessCriteriaResult> listInput, long PPRId, ImportFileDto input)
        //{
        //    foreach (var item in listInput)
        //    {
        //        item.ProjectId = input.ProjectId;
        //        item.Note = input.Note;
        //        item.ProjectProcessResultId = PPRId;
        //        item.Score = await GetScoreByProjectScoreKPIStatus(item.Status);
        //    }

        //    await WorkScope.InsertRangeAsync(listInput);
        //}

        [AbpAuthorize()]
        [HttpDelete]
        public async Task DeleteProjectProcessCriteriaResult(long Id)
        {
            var projectProcessCriteriaResultExist = WorkScope.GetAll<ProjectProcessCriteriaResult>()
               .Where(x => x.Id == Id)
               .FirstOrDefault();

            if (projectProcessCriteriaResultExist == default)
            {
                throw new UserFriendlyException($"Cannot found any project process criteria result with Id = {Id}");
            }
            await WorkScope.DeleteAsync(projectProcessCriteriaResultExist);
        }

        public async Task<int> GetScoreByProjectScoreKPIStatus(NCStatus status)
        {
            AuditScoreDto auditScoreDto = await GetAuditScore();

            switch (status)
            {
                case NCStatus.NC:
                    {
                        return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_NC;
                    }
                case NCStatus.OB:
                    {
                        return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_OB;
                    }
                case NCStatus.RE:
                    {
                        return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_RE;
                    }
                case NCStatus.EX:
                    {
                        return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_EX;
                    }
            }
            return -1;
        }

        public async Task<AuditScoreDto> GetAuditScore()
        {
            var json = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ScoreAudit);

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            var jObject = JObject.Parse(json);

            return new AuditScoreDto
            {
                GIVEN_SCORE = jObject[nameof(AuditScoreDto.GIVEN_SCORE)].Value<int>(),
                PROJECT_SCORE_WHEN_STATUS_GREEN = jObject[nameof(AuditScoreDto.PROJECT_SCORE_WHEN_STATUS_GREEN)].Value<int>(),
                PROJECT_SCORE_WHEN_STATUS_AMBER = jObject[nameof(AuditScoreDto.PROJECT_SCORE_WHEN_STATUS_AMBER)].Value<int>(),
                PROJECT_PROCESS_CRITERIA_RESULT_STATUS_NC = jObject[nameof(AuditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_NC)].Value<int>(),
                PROJECT_PROCESS_CRITERIA_RESULT_STATUS_OB = jObject[nameof(AuditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_OB)].Value<int>(),
                PROJECT_PROCESS_CRITERIA_RESULT_STATUS_RE = jObject[nameof(AuditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_RE)].Value<int>(),
                PROJECT_PROCESS_CRITERIA_RESULT_STATUS_EX = jObject[nameof(AuditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_EX)].Value<int>()
            };
        }

        [AbpAuthorize()]
        public async Task<List<GetProjectProcessCriteriaResultDto>> GetFlatSideListProjectProcessCriteriaResults(long projectProcessResultId, long projectId)
        {
            var listPPCR = await WorkScope.GetAll<ProjectProcessCriteriaResult>()
                .Include(x => x.ProcessCriteria)
                .Where(x => x.ProjectId == projectId && x.ProjectProcessResultId == projectProcessResultId)
                .ToListAsync();

            var listData = listPPCR.Select(x => new GetProjectProcessCriteriaResultDto
            {
                Id = x.Id,
                ProjectProcessResultId = x.ProjectProcessResultId,
                ProjectId = x.ProjectId,
                Status = x.Status,
                Score = x.Score,
                Note = x.Note,
                ProcessCriteria = ObjectMapper.Map<GetProcessCriteriaDto>(x.ProcessCriteria),
            })
                .ToList();

            return listData;
        }

        [AbpAuthorize(PermissionNames.Audits_Results_Detail_View)]
        [HttpPost]
        public async Task<TreeCriteriaResultDto> GetTreeListProjectProcessCriteriaResults(
            InputToGetProjectProcessCriteriaResultDto input)
        {
            var PPR = WorkScope.Get<ProjectProcessResult>(input.ProjectProcessResultId);
            var listPCs = WorkScope.GetAll<ProcessCriteria>().ToList();
            var dicItem = WorkScope.GetAll<ProjectProcessCriteriaResult>()
                .Where(x => x.ProjectProcessResultId == input.ProjectProcessResultId && x.ProjectId == input.ProjectId)
                .ToDictionary(x => x.ProcessCriteriaId, y => new { y.Status, y.Score, y.Note, y.Id });
            var dicPPCs = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == input.ProjectId)
                .ToDictionary(x => x.ProcessCriteriaId, y => new {y.Applicable, y.Note});
            var idsContain = new List<long>();
            foreach (var x in dicItem.Select(y => y.Key))
            {
                idsContain.AddRange(_commonManager.GetAllParentId(x, listPCs));
            }
            idsContain = idsContain.Distinct().ToList();
            var listResult = listPCs.Where(x => idsContain.Contains(x.Id))
                .Select(x => new GetProjectProcessCriteriaResultDto
                {
                    Id = dicItem.ContainsKey(x.Id) ? dicItem[x.Id].Id : 0,
                    Note = dicItem.ContainsKey(x.Id) ? dicItem[x.Id].Note : "",
                    ProjectId = input.ProjectId,
                    ProjectProcessResultId = input.ProjectProcessResultId,
                    Score = dicItem.ContainsKey(x.Id) ? dicItem[x.Id].Score : 0,
                    Status = dicItem.ContainsKey(x.Id) ? dicItem[x.Id].Status : 0,
                    ProcessCriteria = new GetProcessCriteriaDto
                    {
                        Id = x.Id,
                        Code = x.Code,
                        GuidLine = x.GuidLine,
                        IsActive = x.IsActive,
                        IsApplicable = x.IsApplicable,
                        IsLeaf = x.IsLeaf,
                        Level = x.Level,
                        Name = x.Name,
                        ParentId = x.ParentId,
                        QAExample = x.QAExample
                    },
                    Applicable = dicPPCs.ContainsKey(x.Id) ? dicPPCs[x.Id].Applicable : Applicable.Standard,
                    TailoringNote = dicPPCs.ContainsKey(x.Id) ? dicPPCs[x.Id].Note : "",
                })
                .OrderBy(x => CommonUtil.GetNaturalSortKey(x.ProcessCriteria.Code))
                .ToList();
            if (input.IsGetAll())
            {
                return new TreeCriteriaResultDto
                {
                    TotalScore = PPR.Score,
                    Status = PPR.Status,
                    Childrens = listResult.GenerateTree(c => c.ProcessCriteria.Id, c => c.ProcessCriteria.ParentId)
                };
            }
            var listFilterIds = listResult
                .WhereIf(!string.IsNullOrEmpty(input.SearchText),x => (x.ProcessCriteria.Name.ToLower().Contains(input.SearchText.Trim().ToLower())) || (x.ProcessCriteria.Code.ToLower().Contains(input.SearchText.Trim().ToLower())))
                .WhereIf(input.Status.HasValue, x => x.Status == input.Status)
                .Select(x => x.ProcessCriteria.Id).ToList();
            var idsFilterContains = new List<long>();
            listFilterIds.ForEach(x =>
            {
                idsFilterContains.AddRange(_commonManager.GetAllNodeAndLeafIdById(x, listPCs, true));
            });
            idsFilterContains = idsFilterContains.Distinct().Where(x => idsContain.Contains(x)).ToList();
            return new TreeCriteriaResultDto
            {
                Status = PPR.Status,
                TotalScore = PPR.Score,
                Childrens = listResult.Where(x => idsFilterContains.Contains(x.ProcessCriteria.Id)).ToList().GenerateTree(c => c.ProcessCriteria.Id, c => c.ProcessCriteria.ParentId)
            };
        }

        //private List<Entities.ProcessCriteria> GetParentProcessCriteriaList(List<long> Ids)
        //{
        //    var result = new List<Entities.ProcessCriteria>();
        //    var list = WorkScope.GetAll<Entities.ProcessCriteria>().ToList();

        //    foreach (var id in Ids)
        //    {
        //        var parentIds = GetAllParentId(id, list);
        //        foreach (var parentId in parentIds)
        //        {
        //            var pc = list.FirstOrDefault(x => x.Id == parentId);
        //            if (pc != null && !result.Contains(pc))
        //            {
        //                result.Add(pc);
        //            }
        //        }
        //    }

        //    return result;
        //}

        //private List<long> GetAllParentId(long Id, List<Entities.ProcessCriteria> list)
        //{
        //    var result = new List<long>();
        //    var item = list.Where(x => x.Id == Id).FirstOrDefault();
        //    if (item.ParentId.HasValue)
        //    {
        //        result.AddRange(GetAllParentId((long)item.ParentId, list));
        //    }
        //    result.Add((long)item.Id);
        //    return result;
        //}

        [AbpAuthorize()]
        public async Task<GetProjectProcessCriteriaResultDto> UpdateProjectProcessCriteriaResult(UpdateProjectProcessCriteriaResultDto input)
        {
            var prjPCR = await WorkScope.GetAsync<ProjectProcessCriteriaResult>(input.Id);
            var processCriteria = await WorkScope.GetAsync<ProcessCriteria>(prjPCR.ProcessCriteriaId);
            if (prjPCR == null)
            {
                throw new UserFriendlyException("Project process criteria result is not existed!");
            }
            prjPCR.Status = input.Status;
            prjPCR.Note = input.Note;
            await WorkScope.UpdateAsync(prjPCR);

            return new GetProjectProcessCriteriaResultDto
            {
                Id = prjPCR.Id,
                ProjectProcessResultId = prjPCR.ProjectProcessResultId,
                ProjectId = prjPCR.ProjectId,
                Status = prjPCR.Status,
                Note = prjPCR.Note,
                Score = prjPCR.Score,
                ProcessCriteria = ObjectMapper.Map<GetProcessCriteriaDto>(processCriteria),
            };
        }

        [AbpAuthorize()]
        public async Task<GetProjectProcessCriteriaResultDto> GetProjectProcessCriteriaResultById(long Id)
        {
            var prjPCR = await WorkScope.GetAsync<ProjectProcessCriteriaResult>(Id);
            var processCriteria = await WorkScope.GetAsync<ProcessCriteria>(prjPCR.ProcessCriteriaId);
            return new GetProjectProcessCriteriaResultDto
            {
                Id = prjPCR.Id,
                ProjectProcessResultId = prjPCR.ProjectProcessResultId,
                ProjectId = prjPCR.ProjectId,
                Status = prjPCR.Status,
                Note = prjPCR.Note,
                Score = prjPCR.Score,
                ProcessCriteria = ObjectMapper.Map<GetProcessCriteriaDto>(processCriteria),
            };
        }
    }
}