using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.UI;
using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using ProjectManagement.APIs.PMReportProjects.Dto;
using ProjectManagement.APIs.ProcessCriterias;
using ProjectManagement.APIs.ProcessCriterias.Dto;
using ProjectManagement.APIs.ProjectProcessCriterias;
using ProjectManagement.APIs.ProjectProcessCriterias.Dto;
using ProjectManagement.APIs.ProjectProcessResults.Dto;
using ProjectManagement.APIs.TimesheetProjects.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Configuration;
using ProjectManagement.Configuration.Dto;
using ProjectManagement.Entities;
using ProjectManagement.GeneralModels;
using ProjectManagement.Net.MimeTypes;
using ProjectManagement.Services.Common;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ProjectManagement.APIs.ProjectProcessResults.Dto.GetProjectProcessResultDto;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using Range = Aspose.Cells.Range;

namespace ProjectManagement.APIs.ProjectProcessResults
{
    public class ProjectProcessResultAppService : ProjectManagementAppServiceBase
    {
        private readonly CommonManager _commonManager;
        protected readonly ProjectProcessCriteriaAppService projectProcessCriteriaAppService;
        protected readonly ProcessCriteriaAppService processCriteriaAppService;

        public ProjectProcessResultAppService(ProjectProcessCriteriaAppService projectProcessCriteriaAppService, ProcessCriteriaAppService processCriteriaAppService, CommonManager commonManager)
        {
            this.projectProcessCriteriaAppService = projectProcessCriteriaAppService;
            this.processCriteriaAppService = processCriteriaAppService;
            _commonManager = commonManager;
        }

        [AbpAuthorize()]
        [HttpPost]
        public async Task<CreateProjectProcessResultDto> Create(CreateProjectProcessResultDto input)
        {
            var existedProjectProcessresult = WorkScope.GetAll<ProjectProcessResult>()
                .Where(x => x.ProjectId == input.ProjectId && x.AuditDate.Date == input.AuditDate.Date)
                .FirstOrDefault();

            if (existedProjectProcessresult != null)
            {
                throw new UserFriendlyException($"Duplicated AuditDate and ProjectId with existed Project Process result!");
            }

            var projectProcessResult = new ProjectProcessResult
            {
                AuditDate = input.AuditDate,
                ProjectId = input.ProjectId,
            };

            input.Id = await WorkScope.InsertAndGetIdAsync(projectProcessResult);

            //Auto add all ProcessCriteria already tailored from ProjectProcessCriteria into ProjectProcessCriteriaResult
            var projectProcessCriterias = await WorkScope.GetAll<ProjectProcessCriteria>()
                .Include(x => x.ProcessCriteria)
                .Where(x => x.ProjectId == input.ProjectId)
                .Where(x => x.ProcessCriteria.IsLeaf == true)
                .ToListAsync();

            //Them phan Score lay tu Config Score trong DB
            var projectProcessCriteriaResults = projectProcessCriterias.Select(x => new ProjectProcessCriteriaResult
            {
                ProjectProcessResultId = projectProcessResult.Id,
                ProjectId = input.ProjectId,
                ProcessCriteriaId = x.ProcessCriteriaId
            }).ToList();

            await WorkScope.InsertRangeAsync(projectProcessCriteriaResults);

            return input;
        }

        /*   public async Task<UpdateProjectProcessCriteriaDto> AddProcessCriteriasToProjectProcessCriteriaResult(UpdateProjectProcessCriteriaDto input)
           {
               ValidExistProject(input.ProjectId, null);
               var listPCIds = WorkScope.GetAll<ProjectProcessCriteria>()
                   .Where(x => input.ProcessCriteriaIds.Contains(x.ProcessCriteriaId))
                   .Select(x => x.ProcessCriteriaId)
                   .ToList();
               if (listPCIds == null || listPCIds.Count() < 1)
               {
                   throw new UserFriendlyException("Can not found any ProcessCriteriaId in ProjectProcessCriteria");
               }

               var listPPCIds = WorkScope.GetAll<ProjectProcessCriteriaResult>()
                   .Where(x => x.ProjectId == input.ProjectId)
                   .Where(x => listPCIds.Contains(x.ProcessCriteriaId))
                   .Select(x => x.ProcessCriteriaId)
                   .Distinct()
                   .ToList();

               var listToAdd = listPCIds.Where(x => !listPPCIds.Contains(x)).ToList().Select(x => new ProjectProcessCriteriaResult
               {
                   ProjectId = input.ProjectId,
                   ProcessCriteriaId = x
               }).ToList();

               if (listToAdd == null || listToAdd.Count() < 1)
               {
                   throw new UserFriendlyException("Project process criteria already exist");
               }

               await WorkScope.InsertRangeAsync(listToAdd);
               return input;
           }
   */

        public async Task<UpdateProjectProcessCriteriaDto> AddProcessCriteriasToProjectProcessCriteriaResult(UpdateProjectProcessCriteriaDto input)
        {
            ValidExistProject(input.ProjectId, null);

            var projectProcessCriterias = await WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == input.ProjectId)
                .ToListAsync();

            var existingProcessCriteriaIds = await WorkScope.GetAll<ProjectProcessCriteriaResult>()
                .Where(x => x.ProjectId == input.ProjectId)
                .Select(x => x.ProcessCriteriaId)
                .ToListAsync();

            var processCriteriaIdsToAdd = projectProcessCriterias
                .Where(x => !existingProcessCriteriaIds.Contains(x.ProcessCriteriaId))
                .Select(x => x.ProcessCriteriaId)
                .ToList();

            if (processCriteriaIdsToAdd.Count == 0)
            {
                throw new UserFriendlyException("Project process criteria already exist");
            }

            var projectProcessCriteriaResultsToAdd = processCriteriaIdsToAdd
                .Select(x => new ProjectProcessCriteriaResult
                {
                    ProjectId = input.ProjectId,
                    ProcessCriteriaId = x
                })
                .ToList();

            await WorkScope.InsertRangeAsync(projectProcessCriteriaResultsToAdd);

            return input;
        }

        private void ValidExistProject(long? projectId, List<long>? listProjectIds)
        {
            var project = WorkScope.GetAll<Project>()
                .Where(x => (projectId.HasValue && x.Id == projectId) || (listProjectIds != null && listProjectIds.Count() > 0 && listProjectIds.Contains(x.Id)))
                .FirstOrDefault();
            if (project == default)
            {
                throw new UserFriendlyException($"Can not found any project");
            }
        }

        public async Task<List<GetAllProjectProcessResultDto>> GetAll(InputToGetAll input)
        {
            var mapPMIdToFullName = WorkScope.GetAll<User>()
                .Select(x => new { x.Id, x.FullName })
                .ToDictionary(x => x.Id, x => x.FullName);

            var listPPCs = WorkScope.GetAll<ProjectProcessResult>()
            .Select(x => new
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Project = x.Project,
                PM = x.Project.PM,
                Client = x.Project.Client,
                Note = x.Note,
                AuditDate = x.AuditDate,
                Score = x.Score,
                Status = x.Status,
                PMId = x.PMId,
            })
            .ToList();

            if (input.Status.HasValue)
            {
                listPPCs.Where(x => x.Status == input.Status);
            }
            if (input.ProjectId.HasValue)
            {
                listPPCs.Where(x => x.ProjectId == input.ProjectId);
            }
            if (!string.IsNullOrEmpty(input.SearchText))
            {
                var searchTextLower = input.SearchText.ToLower();
                listPPCs.Where(x => x.Project.Name.ToLower().Trim().Contains(searchTextLower) || x.Project.Code.ToLower().Trim().Contains(searchTextLower));
            }

            var finalRes = listPPCs.GroupBy(x => new { x.ProjectId })
            .Select(x => new GetAllProjectProcessResultDto
            {
                ProjectId = x.Key.ProjectId,
                ProjectCode = x.FirstOrDefault().Project.Code,
                ProjectName = x.FirstOrDefault().Project.Name,
                ProjectType = x.FirstOrDefault().Project.ProjectType,
                PMName = x.FirstOrDefault().Project.PM.FullName,
                ClientName = x.FirstOrDefault().Project.Client.Name,
                AuditInfos = x.Select(a => new AuditInfo
                {
                    Id = a.Id,
                    AuditDate = a.AuditDate,
                    Score = a.Score,
                    Note = a.Note,
                    Status = a.Status,
                    PMName = a.PMId != default && mapPMIdToFullName.ContainsKey(a.PMId) ? mapPMIdToFullName[a.PMId] : "",
                }).ToList()
            }).ToList();

            return finalRes;
        }

        [HttpPut]
        public async Task<UpDateProjectProcessResultDto> Udpate(UpDateProjectProcessResultDto input)
        {
            var projectResult = WorkScope.GetAll<ProjectProcessResult>()
                .Where(x => x.Id == input.Id)
                .FirstOrDefault();
            if (projectResult == default)
            {
                throw new UserFriendlyException($"Can not found any project process result with Id = {input.Id}");
            }
            projectResult.AuditDate = input.AuditDate;
            projectResult.Note = input.Note;
            await WorkScope.UpdateAsync(projectResult);
            return input;
        }

        public async Task<UpDateProjectProcessCriteriaResultDto> UpdateProjectCriteriaResult(UpDateProjectProcessCriteriaResultDto input)
        {
            var projectCriteriaResult = await WorkScope.GetAsync<ProjectProcessCriteriaResult>(input.Id);
            if (projectCriteriaResult == default)
            {
                throw new UserFriendlyException($"Can not found any project process criteria result with Id = {input.Id}");
            }
            projectCriteriaResult.Note = input.Note;
            projectCriteriaResult.Score = await GetScoreByProjectScoreKPIStatus(input.Status);
            await WorkScope.UpdateAsync(projectCriteriaResult);
            var listAfterUpdate = WorkScope.GetAll<ProjectProcessCriteriaResult>()
                .Where(x => x.ProjectId == projectCriteriaResult.ProjectId && x.ProjectProcessResultId == projectCriteriaResult.ProjectProcessResultId)
                .Select(x => x.Status).ToList();
            var PPR = await WorkScope.GetAsync<ProjectProcessResult>(projectCriteriaResult.ProjectProcessResultId);
            PPR.Score = await GetScoreForProject(listAfterUpdate);
            PPR.Status = await GetStatusOfProjectProcessResult(PPR.Score);
            CurrentUnitOfWork.SaveChanges();
            return input;
        }

        [AbpAuthorize(PermissionNames.Audits_Results_Delete)]
        public async Task Delete(long Id)
        {
            var projectCriteriaResult = WorkScope.GetAll<ProjectProcessCriteriaResult>()
             .Where(x => x.ProjectProcessResultId == Id)
             .ToList();
            if (projectCriteriaResult == default)
            {
                throw new UserFriendlyException($"Can not found any project process result with Id = {Id}");
            }
            foreach (var item in projectCriteriaResult)
            {
                await WorkScope.DeleteAsync(item);
            }

            var projectResult = WorkScope.GetAll<ProjectProcessResult>()
                .Where(x => x.Id == Id)
                .FirstOrDefault();

            if (projectResult == default)
            {
                throw new UserFriendlyException($"Can not found any project process result with Id = {Id}");
            }
            await WorkScope.DeleteAsync(projectResult);
        }

        [AbpAuthorize(PermissionNames.Audits_Results_Import_Result)]
        [HttpPost]
        public async Task<ImportProcessCriteriaResultDto> ImportToProjectProcessResult([FromForm] ImportFileDto input)
        {
            var valid = await ValidImport(input);
            if (valid.FailedList.Count > 0)
            {
                return new ImportProcessCriteriaResultDto { FailedList = valid.FailedList };
            }

            var PMId = WorkScope.GetAll<Project>()
                .Where(x => x.Id == input.ProjectId)
                .Select(x => x.PMId)
                .FirstOrDefault();

            if (PMId == default)
            {
                throw new UserFriendlyException($"Can not found any project with Id = {input.ProjectId}");
            }

            var PPRScore = await GetScoreForProject(valid.CreateList.Select(x => x.Status).ToList());
            var PPRStatus = await GetStatusOfProjectProcessResult(PPRScore);

            var PPR = new ProjectProcessResult
            {
                ProjectId = input.ProjectId,
                PMId = PMId,
                AuditDate = input.AuditDate,
                Score = PPRScore,
                Status = PPRStatus,
                Note = input.Note
            };
            PPR.Id = await WorkScope.InsertAndGetIdAsync(PPR);
            valid.CreateList.ForEach(async x =>
            {
                x.ProjectProcessResultId = PPR.Id;
                x.Score = await GetScoreForCriteria(x.Status);
            });
            var listInsert = await WorkScope.InsertRangeAsync(valid.CreateList);
            await CurrentUnitOfWork.SaveChangesAsync();
            return new ImportProcessCriteriaResultDto
            {
                AuditInfo = new AuditInfo
                {
                    Id = PPR.Id,
                    AuditDate = PPR.AuditDate,
                    Note = PPR.Note,
                    Score = PPR.Score,
                    Status = PPR.Status
                },
                CriteriaResult = listInsert.Select(x => new GetProjectProcessCriteriaResultDto
                {
                    Id = x.Id,
                    ProjectId = x.ProjectId,
                    ProcessCriteriaId = x.ProcessCriteriaId,
                    ProjectProcessResultId = x.ProjectProcessResultId,
                    Note = x.Note,
                    Status = x.Status,
                    Score = x.Score,
                }).ToList()
            };
        }

        private async Task<ValidImportDto> ValidImport(ImportFileDto input)
        {
            if (input.File == null || !Path.GetExtension(input.File.FileName).Equals(".xlsx"))
            {
                throw new UserFriendlyException("File null or is not .xlsx file");
            }

            using (var stream = new MemoryStream())
            {
                input.File.CopyTo(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var listLeafs = WorkScope.GetAll<ProjectProcessCriteria>()
                       .Where(x => x.ProjectId == input.ProjectId)
                       .Select(x => x.ProcessCriteriaId).ToList();
                    var listCriteria = GetListProcessCriteriaForExport(listLeafs, input.ProjectId).Result.Select(x => x.Code).ToList();
                    var listCodeER = await GetProcessCriteriaCode(input.ProjectId);
                    var lisCodeERString = listCodeER.Select(x => x.Item1.ToLower().Trim());
                    var rowCount = worksheet.Dimension.End.Row;
                    var PPCR = new ProjectProcessCriteriaResult();
                    if (rowCount < 1)
                    {
                        throw new UserFriendlyException("Can not found data on this file");
                    }
                    if (rowCount - 1 < listCodeER.Count)
                    {
                        throw new UserFriendlyException("Data column is missed");
                    }

                    var listLeaf = listCodeER.Where(x => x.Item2 == true).Select(x => x.Item1.ToLower().Trim());
                    var mapCodeToId = listCodeER.Select(s => new { Code = s.Item1.ToLower().Trim(), Id = s.Item3 })
                                                .ToDictionary(s => s.Code, k => k.Id);

                    var listCriteriaIds = new List<long>();
                    var listInputToCreatePPCRs = new List<ProjectProcessCriteriaResult>();

                    var mapNameToEnum = ((NCStatus[])Enum.GetValues(typeof(NCStatus)))
                        .Select(c => new { Value = (int)c, Name = c.ToString() })
                        .ToDictionary(s => s.Name.ToLower().Trim(), k => k.Value);

                    var failedList = new List<ResponseFailDto>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var criteriaCode = worksheet.Cells[row, 1].GetValue<string>();
                        var statusName = worksheet.Cells[row, 3].GetValue<string>();
                        var note = worksheet.Cells[row, 4].GetValue<string>();
                        if (string.IsNullOrEmpty(criteriaCode))
                        {
                            failedList.Add(new ResponseFailDto { Row = row, ReasonFail = $"Criteria code is null or empty" });
                            continue;
                        }
                        var criteriaCodeToLower = criteriaCode.ToLower().Trim();
                        if (!listCriteria.Contains(criteriaCodeToLower))
                        {
                            failedList.Add(new ResponseFailDto { Row = row, ReasonFail = $"Criteria with code = {criteriaCode} does not exist" });
                            continue;
                        }

                        if (listLeaf.Contains(criteriaCode))
                        {
                            var criteriaId = mapCodeToId[criteriaCode];

                            if (listCriteriaIds.Contains(criteriaId))
                            {
                                failedList.Add(new ResponseFailDto { Row = row, ReasonFail = $"Criteria with code = {criteriaCode} already exist in list to create" });
                                continue;
                            }
                            listCriteriaIds.Add(criteriaId);

                            if (string.IsNullOrEmpty(statusName))
                            {
                                failedList.Add(new ResponseFailDto { Row = row, ReasonFail = $"Status name is null or empty" });
                                continue;
                            }
                            var statusNameLower = statusName.ToLower().Trim();

                            if (!mapNameToEnum.ContainsKey(statusNameLower))
                            {
                                failedList.Add(new ResponseFailDto { Row = row, ReasonFail = $"Status with name = {statusName} does not exist" });
                                continue;
                            }

                            var statusValue = mapNameToEnum[statusNameLower];

                            PPCR = new ProjectProcessCriteriaResult
                            {
                                ProcessCriteriaId = criteriaId,
                                Status = (NCStatus)statusValue,
                                ProjectId = input.ProjectId,
                                Note = note
                            };

                            listInputToCreatePPCRs.Add(PPCR);
                            listCriteriaIds.Add(criteriaId);
                        }
                    }

                    return new ValidImportDto
                    {
                        CreateList = listInputToCreatePPCRs,
                        FailedList = failedList
                    };
                }
            }
        }

        private async Task<List<Tuple<string, bool, long>>> GetProcessCriteriaCode(long projectID)
        {
            var listLeaf = WorkScope.GetAll<ProjectProcessCriteria>()
               .Where(x => x.ProjectId == projectID)
               .Select(x => x.ProcessCriteriaId).ToList();
            var listCriteria = await GetListProcessCriteriaForExport(listLeaf, projectID);
            return listCriteria.Select(x => new Tuple<string, bool, long>(x.Code, x.IsLeaf, x.Id)).ToList();
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

        private async Task<int> GetScoreForCriteria(NCStatus input)
        {
            AuditScoreDto auditScoreDto = await GetAuditScore();
            switch (input)
            {
                case NCStatus.NC:
                    return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_NC;

                case NCStatus.OB:
                    return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_OB;

                case NCStatus.RE:
                    return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_RE;

                case NCStatus.EX:
                    return auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_EX;
            }
            return 0;
        }

        private async Task<int> GetScoreForProject(List<NCStatus> input)
        {
            AuditScoreDto auditScoreDto = await GetAuditScore();
            var finalScore = auditScoreDto.GIVEN_SCORE;
            input.ForEach(x =>
            {
                if (x == NCStatus.NC)
                {
                    finalScore += auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_NC;
                }
                if (x == NCStatus.OB)
                {
                    finalScore += auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_OB;
                }
                if (x == NCStatus.RE)
                {
                    finalScore += auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_RE;
                }
                if (x == NCStatus.EX)
                {
                    finalScore += auditScoreDto.PROJECT_PROCESS_CRITERIA_RESULT_STATUS_EX;
                }
            });
            return finalScore;
        }

        private async Task<ProjectScoreKPIStatus> GetStatusOfProjectProcessResult(int score)
        {
            AuditScoreDto auditScoreDto = await GetAuditScore();

            if (score >= auditScoreDto.PROJECT_SCORE_WHEN_STATUS_GREEN)
            {
                return ProjectScoreKPIStatus.Green;
            }
            if (score >= auditScoreDto.PROJECT_SCORE_WHEN_STATUS_AMBER && score < auditScoreDto.PROJECT_SCORE_WHEN_STATUS_GREEN)
            {
                return ProjectScoreKPIStatus.Amber;
            }
            return ProjectScoreKPIStatus.Red;
        }

        [AbpAuthorize(PermissionNames.Audits_Results_DownLoad_Template)]
        [HttpGet]
        public async Task<FileBase64Dto> ExportProjectProcessResultTemplate(long projectID)
        {
            var listLeaf = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == projectID)
                .Select(x => x.ProcessCriteriaId)
                .ToList();
            var projectInfor = WorkScope.Get<Project>(projectID);
            var listCriteria = await GetListProcessCriteriaForExport(listLeaf, projectID);
            return await ExportProjectProcessCriteriaToExcel(listCriteria, new GetProjectInfoDto { ProjectCode = projectInfor.Code, ProjectName = projectInfor.Name });
        }

        #region export excel(Hoang)
        //private async Task<FileBase64Dto> ExportProjectProcessCriteriaToExcel(List<GetProcessCriteriaTemplateDto> input, GetProjectInfoDto project)
        //{
        //    using (var wb = new ExcelPackage())
        //    {
        //        var NCStatus = Enum.GetValues(typeof(NCStatus))
        //            .Cast<NCStatus>()
        //            .Select(v => v.ToString())
        //            .ToList();
        //        var sheetAudit = wb.Workbook.Worksheets.Add("Audit");
        //        sheetAudit.Cells.Style.Font.Name = "Arial";
        //        sheetAudit.Cells.Style.Font.Size = 10;
        //        sheetAudit.Cells["A1:G1"].Style.Font.Bold = true;
        //        sheetAudit.Cells["A1:G1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        sheetAudit.Cells["A1:G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        sheetAudit.Cells["A1:G1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //        sheetAudit.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 173, 71));
        //        sheetAudit.Cells["A1:G1"].Style.Font.Color.SetColor(Color.White);
        //        sheetAudit.Cells["A1"].Value = "Code";
        //        sheetAudit.Cells["B1"].Value = "Criteria";
        //        sheetAudit.Cells["C1"].Value = "NC classification";
        //        sheetAudit.Cells["D1"].Value = "Comment";
        //        sheetAudit.Cells["E1"].Value = "Tailoring Note";
        //        sheetAudit.Cells["F1"].Value = "Guideline";
        //        sheetAudit.Cells["G1"].Value = "Q&A Examples";

        //        // Freeze the first row
        //        sheetAudit.View.FreezePanes(2, 1);

        //        var startAudit = sheetAudit.Cells["A2"].Start.Row;
        //        foreach (var item in input)
        //        {
        //            if (!item.IsLeaf)
        //            {
        //                sheetAudit.Cells[$"A{startAudit}:G{startAudit}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                sheetAudit.Cells[$"A{startAudit}:G{startAudit}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(218, 241, 243));
        //            }
        //            if (!item.ParentId.HasValue && !item.IsLeaf)
        //            {
        //                sheetAudit.Cells[$"A{startAudit}:G{startAudit}"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                sheetAudit.Cells[$"A{startAudit}:G{startAudit}"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(155, 194, 230));
        //            }
        //            sheetAudit.Cells[$"A{startAudit}"].Value = item.Code;
        //            sheetAudit.Cells[$"B{startAudit}"].Value = item.Name;
        //            if (item.IsLeaf)
        //            {
        //                var unitmeasure = sheetAudit.DataValidations.AddListValidation($"C{startAudit}");
        //                foreach (var itemNC in NCStatus)
        //                {
        //                    unitmeasure.Formula.Values.Add(itemNC);
        //                }
        //            }
        //            sheetAudit.Cells[$"D{startAudit}"].Value = "";
        //            sheetAudit.Cells[$"E{startAudit}"].Value = CommonUtil.ConvertHtmlToPlainText(item.PmNote);
        //            sheetAudit.Cells[$"F{startAudit}"].Value = CommonUtil.ConvertHtmlToPlainText(item.GuidLine);
        //            sheetAudit.Cells[$"G{startAudit}"].Value = CommonUtil.ConvertHtmlToPlainText(item.QAExample);
        //            startAudit++;
        //        }
        //        sheetAudit.Cells[$"A2:B{startAudit - 1}"].Style.Font.Bold = true;
        //        sheetAudit.Cells[$"A2:B{startAudit - 1}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        sheetAudit.Cells[$"A2:B{startAudit - 1}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        //        sheetAudit.Cells[$"C2:C{startAudit - 1}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        sheetAudit.Cells[$"C2:C{startAudit - 1}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //        sheetAudit.Cells[$"D2:G{startAudit - 1}"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //        sheetAudit.Cells[$"D2:G{startAudit - 1}"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
        //        sheetAudit.Cells.AutoFitColumns();
        //        sheetAudit.Column(2).Width = 30;
        //        sheetAudit.Column(4).Width = 40;
        //        sheetAudit.Column(5).Width = 50;
        //        sheetAudit.Column(6).Width = 50;
        //        sheetAudit.Column(7).Width = 50;
        //        sheetAudit.Cells.Style.WrapText = true;
        //        return new FileBase64Dto
        //        {
        //            FileName = $"Audit [{project.ProjectCode}] {project.ProjectName}.xlsx",
        //            FileType = MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet,
        //            Base64 = Convert.ToBase64String(wb.GetAsByteArray())
        //        };
        //    }
        //}
        #endregion

        // export excel file, using Aspose library(duy)
        private async Task<FileBase64Dto> ExportProjectProcessCriteriaToExcel(List<GetProcessCriteriaTemplateDto> input, GetProjectInfoDto project)
        {
            using (var wb = new Workbook())
            {
                var NCStatus = Enum.GetValues(typeof(NCStatus))
                    .Cast<NCStatus>()
                    .Select(v => v.ToString())
                    .ToList();
                var sheetAudit = wb.Worksheets[0];
                sheetAudit.Name = "Audit";
                var sheetAudit2 = wb.Worksheets[wb.Worksheets.Add()];
                sheetAudit2.Name = "Audit2";

                Style defaultStyle = sheetAudit.Cells.Style;
                defaultStyle.VerticalAlignment = TextAlignmentType.Center;
                defaultStyle.Font.Name = "Arial";
                defaultStyle.Font.Size = 10;
                sheetAudit.Cells.Style = defaultStyle;

                Range range = sheetAudit.Cells.CreateRange("A1", "G1");
                Style style = wb.CreateStyle();
                style.Font.IsBold = true;
                style.VerticalAlignment = TextAlignmentType.Center;
                style.HorizontalAlignment = TextAlignmentType.Center;
                style.Pattern = BackgroundType.Solid;
                style.ForegroundColor = Color.FromArgb(112, 173, 71);
                style.Font.Color = Color.White;
                //Create a StyleFlag object.
                StyleFlag flg = new StyleFlag();
                flg.All = true;
                range.ApplyStyle(style, flg);
                sheetAudit.Cells["A1"].Value = "Code";
                sheetAudit.Cells["B1"].Value = "Criteria";
                sheetAudit.Cells["C1"].Value = "NC classification";
                sheetAudit.Cells["D1"].Value = "Comment";
                sheetAudit.Cells["E1"].Value = "Tailoring Note";
                sheetAudit.Cells["F1"].Value = "Guideline";
                sheetAudit.Cells["G1"].Value = "Q&A Examples";

                var startAudit = sheetAudit.Cells["A2"].Row + 1;

                // validation setup
                // Create a range in the second worksheet.
                range = sheetAudit2.Cells.CreateRange($"A1", $"A{NCStatus.Count}");

                // Name the range.
                range.Name = "MyRange";
                // Freeze the first row and first two columns
                sheetAudit.FreezePanes(1, 2, 1, 2);

                // Fill different cells with data in the range.
                int start = 0;
                foreach (var item in NCStatus)
                {
                    range[start++, 0].PutValue(item);
                }

                foreach (var item in input)
                {
                    if (!item.IsLeaf)
                    {
                        range = sheetAudit.Cells.CreateRange($"A{startAudit}", $"G{startAudit}");
                        style = defaultStyle;
                        style.Pattern = BackgroundType.Solid;
                        style.ForegroundColor = Color.FromArgb(218, 241, 243);
                        range.ApplyStyle(style, flg);
                    }
                    if (!item.ParentId.HasValue && !item.IsLeaf)
                    {
                        range = sheetAudit.Cells.CreateRange($"A{startAudit}", $"G{startAudit}");
                        style = defaultStyle;
                        style.Pattern = BackgroundType.Solid;
                        style.ForegroundColor = Color.FromArgb(155, 194, 230);
                        range.ApplyStyle(style, flg);
                    }
                    sheetAudit.Cells[$"A{startAudit}"].Value = item.Code;
                    sheetAudit.Cells[$"B{startAudit}"].Value = item.Name;
                    style = sheetAudit.Cells[$"A{startAudit}"].GetStyle();
                    style.Font.IsBold = true;
                    sheetAudit.Cells[$"A{startAudit}"].SetStyle(style);
                    sheetAudit.Cells[$"B{startAudit}"].SetStyle(style);
                    if (item.IsLeaf)
                    {
                        style = sheetAudit.Cells[$"A{startAudit}"].GetStyle();
                        style.Font.IsBold = false;
                        sheetAudit.Cells[$"A{startAudit}"].SetStyle(style);
                        sheetAudit.Cells[$"B{startAudit}"].SetStyle(style);

                        // Get the validations collection.
                        ValidationCollection validations = sheetAudit.Validations;

                        // Specify the validation area.
                        CellArea area = new CellArea();
                        area.StartRow = startAudit - 1;
                        area.EndRow = startAudit - 1;
                        area.StartColumn = 2;
                        area.EndColumn = 2;

                        // Create a new validation to the validations list.
                        validations.Add(area);

                        // Set the validation type.
                        validations[0].Type = ValidationType.List;
                        // Set the operator.
                        validations[0].Operator = OperatorType.None;

                        // Set the in cell drop down.
                        validations[0].InCellDropDown = true;

                        // Set the formula1.
                        validations[0].Formula1 = "=MyRange";

                        // Add the validation area.
                        validations[0].AddArea(area);
                    }
                    sheetAudit.Cells[$"D{startAudit}"].HtmlString = "";
                    sheetAudit.Cells[$"E{startAudit}"].HtmlString = item.PmNote.HasValue() ? " " + item.PmNote : " ";
                    sheetAudit.Cells[$"F{startAudit}"].HtmlString = item.GuidLine.HasValue() ? " " + item.GuidLine : " ";
                    sheetAudit.Cells[$"G{startAudit}"].HtmlString = item.QAExample.HasValue() ? " " + item.QAExample : " ";

                    // Set Cell's Style
                    Style styleWrap = sheetAudit.Cells[$"A{startAudit}"].GetStyle();
                    Style styleHorizonCenter = sheetAudit.Cells[$"C{startAudit}"].GetStyle();
                    Style styleBold = sheetAudit.Cells[$"E{startAudit}"].GetStyle();
                    Range rangeFull = sheetAudit.Cells.CreateRange($"A{startAudit}", $"G{startAudit}");
                    Range rangeAB = sheetAudit.Cells.CreateRange($"A{startAudit}", $"B{startAudit}");

                    styleWrap.IsTextWrapped = true;
                    styleHorizonCenter.HorizontalAlignment = TextAlignmentType.Center;
                    styleBold.Font.IsBold = true;

                    rangeAB.SetStyle(styleBold);
                    rangeFull.SetStyle(styleWrap);
                    sheetAudit.Cells[$"C{startAudit}"].SetStyle(styleHorizonCenter);

                    startAudit++;
                }
                sheetAudit.Cells.SetColumnWidth(1, 30);
                sheetAudit.Cells.SetColumnWidth(2, 20);
                sheetAudit.Cells.SetColumnWidth(3, 30);
                sheetAudit.Cells.SetColumnWidth(4, 40);
                sheetAudit.Cells.SetColumnWidth(5, 40);
                sheetAudit.Cells.SetColumnWidth(6, 40);

                // hide and protect the second worksheet
                sheetAudit2.Protect(ProtectionType.All);
                sheetAudit2.Protection.Password = "123qwe";
                sheetAudit2.IsVisible = false;

                wb.Settings.CalcMode = CalcModeType.Manual;
                wb.Settings.ReCalculateOnOpen = false;

                MemoryStream ms = new MemoryStream();
                wb.Save(ms, SaveFormat.Xlsx);
                ms.Seek(0, SeekOrigin.Begin);
                var buffer = FileUtils.DeleteXlsxSheet("Evaluation Warning", sheetAudit2.Name, ms);
                return new FileBase64Dto
                {
                    FileName = $"Audit [{project.ProjectCode}] {project.ProjectName}.xlsx",
                    FileType = MimeTypeNames.ApplicationVndMsExcel,
                    Base64 = Convert.ToBase64String(buffer)
                };
            }
        }

        private async Task<List<GetProcessCriteriaTemplateDto>> GetListProcessCriteriaForExport(List<long> listLeaf, long projectId)
        {
            var listID = new List<long>();
            var listAllCriteria = WorkScope.GetAll<ProcessCriteria>().ToList();
            var dicIdPmNote = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == projectId)
                .ToDictionary(x => x.ProcessCriteriaId, y => y.Note);

            foreach (var item in listLeaf)
            {
                listID.AddRange(_commonManager.GetAllParentId(item, listAllCriteria));
            }
            listID = listID.Distinct().ToList();
            return WorkScope.GetAll<ProcessCriteria>()
                .Where(x => listID.Contains(x.Id))
                .Select(x => new GetProcessCriteriaTemplateDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    GuidLine = x.GuidLine,
                    IsActive = x.IsActive,
                    IsApplicable = x.IsApplicable,
                    Name = x.Name,
                    IsLeaf = x.IsLeaf,
                    Level = x.Level,
                    PmNote = dicIdPmNote.ContainsKey(x.Id) ? dicIdPmNote[x.Id] : "",
                    ParentId = x.ParentId,
                    QAExample = x.QAExample,
                })
                .ToList()
                .OrderBy(x => CommonUtil.GetNaturalSortKey(x.Code)).ToList();
        }

        private List<long> ConvertTree(IEnumerable<TreeItem<GetProcessCriteriaDto>> tree)
        {
            var criteriaList = new List<long>();
            if (tree.Count() > 0)
            {
                foreach (var child in tree)
                {
                    criteriaList.Add(child.Item.Id);
                    if (child.Children.Count() > 0)
                    {
                        criteriaList.AddRange(ConvertTree(child.Children));
                    }
                }
            }
            return criteriaList;
        }

        [AbpAuthorize(PermissionNames.Audits_Results)]
        [HttpPost]
        public async Task<GridResult<GetAllPagingProjectProcessResultDto>> GetAllPaging(GridParam input)
        {
            var filterStatus = input.FilterItems.Where(x => x.PropertyName == "status").FirstOrDefault();
            var dicIdName = new Dictionary<long, string>();
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                dicIdName = WorkScope.GetAll<User>().ToDictionary(x => x.Id, y => y.FullName);
            }
            var listPPRs = WorkScope.GetAll<ProjectProcessResult>()
                .Select(x => new
                {
                    ProjectInfor = new
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.Project.Name,
                        ProjectCode = x.Project.Code,
                        ProjectType = x.Project.ProjectType,
                        ClientName = x.Project.Client.Name,
                        ClientCode = x.Project.Client.Code,
                        ProjectStatus = x.Project.Status,
                        PMName = x.Project.PM.FullName,
                        PMId = x.Project.PMId,
                        ClientId = x.Project.Client.Id,
                    },
                    AuditDate = x.AuditDate,
                    Status = x.Status,
                    Score = x.Score,
                    Note = x.Note,
                    LastModifyTime = x.LastModificationTime.HasValue ? x.LastModificationTime : x.CreationTime,
                    LastModifyUser = x.LastModifierUserId.HasValue ? x.LastModifierUserId : x.CreatorUserId,
                    PMId = x.PMId,
                    Id = x.Id
                }).ToList()
                .GroupBy(x => x.ProjectInfor)
                .Select(
                x => new GetAllPagingProjectProcessResultDto
                {
                    ProjectId = x.Key.ProjectId,
                    ProjectName = x.Key.ProjectName,
                    ProjectCode = x.Key.ProjectCode,
                    ProjectType = x.Key.ProjectType,
                    ClientName = x.Key.ClientName ?? "",
                    ClientCode = x.Key.ClientCode ?? "",
                    ProjectStatus = x.Key.ProjectStatus,
                    PMName = x.Key.PMName,
                    PmId = x.Key.PMId,
                    ClientId = x.Key.ClientId,
                    AuditResultInfor = x.Select(y => new AuditResultInforDto
                    {
                        Id = y.Id,
                        AuditDate = y.AuditDate.ToString("dd/MM/yyyy"),
                        Status = y.Status,
                        Score = y.Score,
                        Note = y.Note,
                        LastModifyTime = y.LastModifyTime.Value.ToString("HH:mm dd/MM/yyyy"),
                        LastModifyUser = y.LastModifyUser,
                        PmId = y.PMId,
                        PMName = dicIdName.ContainsKey(y.PMId) ? dicIdName[y.PMId] : null,
                        LastModifyUserName = dicIdName.ContainsKey(y.LastModifyUser.Value) ? dicIdName[y.LastModifyUser.Value] : null,
                    }).OrderByDescending(x => x.Id)
                    .WhereIf(filterStatus != default, x => Convert.ToInt64(x.Status) == Convert.ToInt64(filterStatus.Value))
                    .ToList()
                }).ToList();
            listPPRs.RemoveAll(x => x.AuditResultInfor.Count == 0);
            var result = listPPRs.AsQueryable();
            if (filterStatus != default)
            {
                input.FilterItems.Remove(filterStatus);
            }
            return result.GetGridResultSync(result, input);
        }

        [HttpGet]
        public async Task<List<GetAllPagingProjectProcessCriteriaDto>> GetProjectToImportResult()
        {
            var listExsit = WorkScope.GetAll<ProjectProcessCriteria>()
                .GroupBy(x => x.ProjectId).Select(x => x.Key).ToList();
            return await WorkScope.GetAll<Project>()
                .Where(x => listExsit.Contains(x.Id))
                .Select(x => new GetAllPagingProjectProcessCriteriaDto
                {
                    ProjectId = x.Id,
                    ProjectName = x.Name,
                    ProjectCode = x.Code,
                    ProjectType = x.ProjectType,
                    ClientName = x.Client.Name,
                    PMName = x.PM.FullName,
                }).ToListAsync();
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

        [AbpAuthorize(PermissionNames.Audits_Results_Edit)]
        [HttpPost]
        public async Task UpdateNote(UpdateNoteDto input)
        {
            var item = await WorkScope.GetAsync<ProjectProcessResult>(input.Id);
            item.Note = input.Note;
            await WorkScope.UpdateAsync(item);
        }

        [HttpGet]
        public async Task<List<GetPMDto>> GetPMProcessResultInfors()
        {
            var dicIdName = WorkScope.GetAll<User>().ToDictionary(x => x.Id, y => new { y.EmailAddress, y.FullName });
            var listPM = WorkScope.GetAll<ProjectProcessResult>()
                .Select(x => new GetPMDto
                {
                    Id = x.Project.PMId,
                    EmailAddress = dicIdName[x.Project.PMId].EmailAddress,
                    FullName = dicIdName[x.Project.PMId].FullName,
                }).Distinct()
                .ToList();
            return listPM;
        }

        [HttpGet]
        public async Task<List<GetClientDto>> GetClientProcessResultInfors()
        {
            var dicIdName = WorkScope.GetAll<Client>().ToDictionary(x => x.Id, y => new { y.Code, y.Name });
            var listPM = WorkScope.GetAll<ProjectProcessResult>()
                .Where(x => x.Project.ClientId.HasValue)
                .Select(x => new GetClientDto
                {
                    Id = x.Project.ClientId.Value,
                    Code = dicIdName[x.Project.ClientId.Value].Code,
                    Name = dicIdName[x.Project.ClientId.Value].Name,
                }).Distinct()
                .ToList();
            return listPM;
        }
    }
}