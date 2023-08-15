using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Office.Interop.Excel;
using NccCore.Extension;
using NccCore.Paging;
using OfficeOpenXml;
using ProjectManagement.APIs.ProjectProcessCriterias.Dto;
using ProjectManagement.APIs.TimesheetProjects.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Constants.Enum;
using ProjectManagement.Entities;
using ProjectManagement.Helper;
using ProjectManagement.Net.MimeTypes;
using ProjectManagement.Services.Common;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using static ProjectManagement.APIs.ProjectProcessResults.Dto.GetProjectProcessResultDto;
using Range = Aspose.Cells.Range;
using Workbook = Aspose.Cells.Workbook;
using Style = Aspose.Cells.Style;

namespace ProjectManagement.APIs.ProjectProcessCriterias
{
    [AbpAuthorize]
    public class ProjectProcessCriteriaAppService : ProjectManagementAppServiceBase
    {
        private readonly CommonManager _commonManager;

        public ProjectProcessCriteriaAppService(CommonManager commonManager)
        {
            _commonManager = commonManager;
        }

        [HttpPost]
        public async Task<List<GetAllProjectProcessCriteriaDto>> GetAll(InputToGetAllDto input)
        {
            var listPPCs = WorkScope.GetAll<ProjectProcessCriteria>()
            .Select(x => new
            {
                Id = x.Id,
                ProjectId = x.ProjectId,
                Project = x.Project,
                PM = x.Project.PM,
                Client = x.Project.Client,
                ProcessCriteriaId = x.ProcessCriteriaId
            })
            .AsEnumerable()
            .GroupBy(x => new { x.ProjectId })
            .Select(x => new GetAllProjectProcessCriteriaDto
            {
                Id = x.FirstOrDefault().Id,
                ProjectId = x.Key.ProjectId,
                ProjectCode = x.FirstOrDefault().Project.Code,
                ProjectName = x.FirstOrDefault().Project.Name,
                ProjectType = CommonUtil.GetProjectTypeString(x.FirstOrDefault().Project.ProjectType),
                PMName = x.FirstOrDefault().Project.PM.FullName,
                ClientName = x.FirstOrDefault().Project.Client.Name ?? "",
                ListProcessCriteriaIds = x.Select(pc => pc.ProcessCriteriaId).ToList()
            }).ToList();

            if (input.GetAll())
            {
                return listPPCs;
            }
            if (input.ProjectId != null && input.ProjectId != 0)
            {
                listPPCs = listPPCs.Where(x => x.ProjectId == input.ProjectId).ToList();
            }
            if (input.ProcessCriteriaId != null && input.ProcessCriteriaId != 0)
            {
                listPPCs = listPPCs.Where(x => x.ListProcessCriteriaIds.Contains((long)input.ProcessCriteriaId)).ToList();
            }

            return listPPCs;
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring)]
        [HttpPost]
        public async Task<GridResult<GetAllPagingProjectProcessCriteriaDto>> GetAllPaging(GridParam input)
        {
            var listPPCs = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.Project.ProjectType != ProjectEnum.ProjectType.TRAINING)
                .Select(x => new
                {
                    ProjectInfor = new
                    {
                        ProjectId = x.ProjectId,
                        ProjectName = x.Project.Name,
                        ProjectCode = x.Project.Code,
                        ProjectType = x.Project.ProjectType,
                        ProjectStatus = x.Project.Status,
                        ClientName = x.Project.Client.Name,
                        ClientCode = x.Project.Client.Code,
                        PMName = x.Project.PM.FullName
                    },
                    ProcessCriteriaId = x.ProcessCriteriaId,
                })
                .ToList()
                .GroupBy(x => x.ProjectInfor).Select(
                x => new GetAllPagingProjectProcessCriteriaDto
                {
                    ProjectId = x.Key.ProjectId,
                    ProjectName = x.Key.ProjectName,
                    ProjectCode = x.Key.ProjectCode,
                    ProjectType = x.Key.ProjectType,
                    ProjectStatus = x.Key.ProjectStatus,
                    ClientName = x.Key.ClientName ?? "",
                    ClientCode = x.Key.ClientCode ?? "",
                    PMName = x.Key.ProjectName,
                    CountCriteria = x.Select(pc => pc.ProcessCriteriaId).Count()
                }).AsQueryable();
            return listPPCs.GetGridResultSync(listPPCs, input);
        }

        public async Task<List<ProcessCriteriaOfProjectDto>> GetAllProjectCriteriaByProjectId(long projectId)
        {
            var listPCIds = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == projectId)
                .Select(x => x.ProcessCriteriaId)
                .ToList();
            var listParentIds = new List<long>();
            var query = WorkScope.GetAll<ProcessCriteria>();
            var listPCs = query.ToList();
            foreach (var id in listPCIds)
            {
                listParentIds.AddRange(_commonManager.GetAllParentId(id, listPCs));
            }
            var listResults = query.Where(x => listParentIds.Contains(x.Id))
                .Select(x => new ProcessCriteriaOfProjectDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    IsApplicable = x.IsApplicable,
                    IsLeaf = x.IsLeaf,
                    Level = x.Level,
                    ParentId = x.ParentId
                }).ToList();
            return listResults;
        }

        public async Task<List<ProcessCriteriaOfProjectDto>> GetAllProcessCriteriaByProjectId(long projectId)
        {
            var listPCIds = await WorkScope.GetAll<ProjectProcessCriteria>()
              .Where(x => x.ProjectId == projectId)
              .Select(x => x.ProcessCriteriaId)
              .ToListAsync();

            var listPCs = await WorkScope.GetAll<ProcessCriteria>()
                .Where(x => listPCIds.Contains(x.Id))
                .Select(x => new ProcessCriteriaOfProjectDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    IsApplicable = x.IsApplicable,
                    IsLeaf = x.IsLeaf,
                    Level = x.Level,
                    ParentId = x.ParentId
                }).ToListAsync();

            return listPCs;
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring_Create)]
        public async Task<CreateProjectProcessCriteriaDto> AddMultiCriteriaToMultiProject(CreateProjectProcessCriteriaDto input)
        {
            ValidExistProject(null, input.ProjectIds);
            //GetAll PC is satify to add ( isLeaf and isApplicable)
            var listPCIds = WorkScope.GetAll<ProcessCriteria>()
                .Where(x => x.IsLeaf && x.IsApplicable && x.IsActive)
                .Select(x => x.Id)
                .ToList();
            if (listPCIds == null || listPCIds.Count < 1)
            {
                throw new UserFriendlyException("Can not found any process criteria (IsLeaf, IsActive)");
            }

            // Gán tất cả PC cho tất cả Project
            var listToAdd = input.ProjectIds.SelectMany(prId => listPCIds
            .Select(pcId => new ProjectProcessCriteria { ProjectId = prId, ProcessCriteriaId = pcId, Applicable = ProjectEnum.Applicable.Standard })).ToList();

            // Lấy ra tất cả PC có trong DB
            var listPPCIds = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => input.ProjectIds.Contains(x.ProjectId))
                .Select(x => new { ProjectId = x.ProjectId, ProcessCriteriaId = x.ProcessCriteriaId })
                .Distinct()
                .ToList();

            if (listPPCIds.Count() > 0)
            {
                // Thực thiện check trùng

                listToAdd.RemoveAll(x => listPPCIds.Any(y => y.ProjectId == x.ProjectId && y.ProcessCriteriaId == x.ProcessCriteriaId));
            }
            if (listToAdd == null || listToAdd.Count() < 1)
            {
                throw new UserFriendlyException("Project process criteria already exist");
            }
            await WorkScope.InsertRangeAsync(listToAdd);

            return input;
        }

        // public async Task<UpdateProjectProcessCriteriaDto> AddMultiCriteriaToOneProject(UpdateProjectProcessCriteriaDto input)
        // {
        //     ValidExistProject(input.ProjectId, null);
        //     var listPCIds = WorkScope.GetAll<ProcessCriteria>()
        //      .Where(x => input.ProcessCriteriaIds.Contains(x.Id))
        //      .Select(x => new { x.Id, x.IsLeaf, x.IsActive })
        //      .ToList();

        //     var invalidPCIds = listPCIds
        //         .Where(x => !x.IsLeaf || !x.IsActive)
        //         .Select(x => x.Id)
        //         .ToList();

        //     if (invalidPCIds.Any())
        //     {
        //         throw new UserFriendlyException($"The following ProcessCriteria are invalid (not IsLeaf or not IsActive): {string.Join(",", invalidPCIds)}");
        //     }

        //     var existingPPCs = WorkScope.GetAll<ProjectProcessCriteria>()
        //         .Where(x => x.ProjectId == input.ProjectId)
        //         .Where(x => listPCIds.Select(y => y.Id).Contains(x.ProcessCriteriaId))
        //         .ToList();

        //     var newPPCs = listPCIds
        //         .Where(x => !existingPPCs.Any(y => y.ProcessCriteriaId == x.Id))
        //         .Select(x => new ProjectProcessCriteria
        //         {
        //             ProjectId = input.ProjectId,
        //             ProcessCriteriaId = x.Id
        //         })
        //         .ToList();

        //     if (newPPCs == null || newPPCs.Count() < 1)
        //     {
        //         throw new UserFriendlyException("Project process criteria already exist");
        //     }

        //     await WorkScope.InsertRangeAsync(newPPCs);

        //     return new UpdateProjectProcessCriteriaDto
        //     {
        //         ProjectId = input.ProjectId,
        //         ProcessCriteriaIds = newPPCs.Select(x => x.ProcessCriteriaId).ToList()
        //     };
        // }
        [AbpAuthorize(PermissionNames.Audits_Tailoring_Update_Project_Tailoring)]
        public async Task<UpdateProjectProcessCriteriaDto> AddMultiCriteriaToOneProject(UpdateProjectProcessCriteriaDto input)
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var listOldPPC = WorkScope.GetAll<ProjectProcessCriteria>()
                    .Where(x => x.ProjectId == input.ProjectId && input.ProcessCriteriaIds
                    .Contains(x.ProcessCriteriaId) && x.IsDeleted == true)
                    .OrderByDescending(x => x.Id)
                    .ToList();

                var checkList = new List<long>();
                var listReverse = new List<ProjectProcessCriteria>();
                listOldPPC.ForEach(x =>
                {
                    if (!checkList.Contains(x.ProcessCriteriaId))
                    {
                        listReverse.Add(x);
                        checkList.Add(x.ProcessCriteriaId);
                    }
                });

                if (listReverse.Count > 0)
                {
                    listReverse.ForEach(x =>
                    {
                        x.IsDeleted = false;
                    });
                    CurrentUnitOfWork.SaveChanges();
                    input.ProcessCriteriaIds.RemoveAll(x => listReverse.Any(y => y.ProcessCriteriaId == x));
                }
            }

            ValidExistProject(input.ProjectId, null);
            var listPCIds = WorkScope.GetAll<ProcessCriteria>()
                .Where(x => input.ProcessCriteriaIds.Contains(x.Id))
                .Where(x => x.IsLeaf && x.IsActive)
                .Select(x => x.Id)
                .ToList();

            if (listPCIds == null || listPCIds.Count() < 1)
            {
                return input;
            }

            var listPPCIds = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == input.ProjectId)
                .Where(x => listPCIds.Contains(x.ProcessCriteriaId))
                .Select(x => x.ProcessCriteriaId)
                .Distinct()
                .ToList();

            var listToAdd = listPCIds.Where(x => !listPPCIds.Contains(x)).ToList().Select(x => new ProjectProcessCriteria
            {
                ProjectId = input.ProjectId,
                ProcessCriteriaId = x,
                Applicable = ProjectEnum.Applicable.Standard
            }).ToList();

            if (listToAdd == null || listToAdd.Count() < 1)
            {
                return input;
            }

            await WorkScope.InsertRangeAsync(listToAdd);
            return input;
        }

        public async Task<OneCriteriaToMutilProjectDto> AddSelectedCriteriaToMultiProject(OneCriteriaToMutilProjectDto input)
        {
            var listProjectIdInPPC = WorkScope.GetAll<ProjectProcessCriteria>().Select(x => x.ProjectId).ToList();
            ValidExistProject(null, listProjectIdInPPC);

            var processCriteria = WorkScope.GetAll<ProcessCriteria>()
                .Where(x => x.Id == input.ProcessCriteriaId && x.IsLeaf && x.IsApplicable && x.IsActive)
                .FirstOrDefault();

            if (processCriteria == null)
            {
                throw new UserFriendlyException("Can not found any process criteria (IsLeaf, IsApplicable, IsActive)");
            }

            var existingPPCs = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProcessCriteriaId == input.ProcessCriteriaId && listProjectIdInPPC.Contains(x.ProjectId))
                .ToList();

            var newPPCs = listProjectIdInPPC
                .Where(projectId => existingPPCs.All(ppc => ppc.ProjectId != projectId))
                .Select(projectId => new ProjectProcessCriteria
                {
                    ProjectId = projectId,
                    ProcessCriteriaId = input.ProcessCriteriaId,
                    Applicable = ProjectEnum.Applicable.Standard
                })
                .ToList();

            if (newPPCs.Count < 1)
            {
                throw new UserFriendlyException("Project process criteria already exist");
            }

            await WorkScope.InsertRangeAsync(newPPCs);

            return input;
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring_Update_Project_Tailoring)]
        [HttpPut]
        public async Task<DeleteCriteriaDto> DeleteCriteria(DeleteCriteriaDto input)
        {
            ValidExistProject(input.ProjectId, null);
            var listIds = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == input.ProjectId)
                .Where(x => input.ProcessCriteriaIds.Contains(x.ProcessCriteriaId))
                .ToList();

            await DeleteProjectProcessCriterias(listIds);
            return input;
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring_Detail_Detele)]
        [HttpDelete]
        public async Task DeleteProjectProcessCriteria(long id)
        {
            await WorkScope.DeleteAsync<ProjectProcessCriteria>(id);
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring_Delete)]
        [HttpDelete]
        public async Task<long> DeleteProject(long projectId)
        {
            ValidExistProject(projectId, null);
            var listIds = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == projectId)
                .ToList();
            await DeleteProjectProcessCriterias(listIds);
            return projectId;
        }

        private async Task DeleteProjectProcessCriterias(List<ProjectProcessCriteria> listPPCs)
        {
            if (listPPCs == null || listPPCs.Count() < 1)
            {
                throw new UserFriendlyException($"Can not found any project process criteria");
            }
            if (listPPCs.Count() == 1)
            {
                await WorkScope.DeleteAsync<ProjectProcessCriteria>(listPPCs[0]);
            }
            else
            {
                listPPCs.ForEach(x =>
                {
                    x.IsDeleted = true;
                });
                CurrentUnitOfWork.SaveChanges();
            }
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

        public List<GetAllProjectToAddDto> GetAllProjectToAddPPC(InputToGetAllProjectToAddDto input)
        {
            var listPPCProjectIds = WorkScope.GetAll<ProjectProcessCriteria>()
                .Select(x => x.ProjectId)
                .ToList();
            var listProjects = WorkScope.GetAll<Project>()
                .Where(x => !listPPCProjectIds.Contains(x.Id))
                .Select(x => new GetAllProjectToAddDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Type = x.ProjectType,
                    PMName = x.PM.FullName,
                    ClientName = x.Client.Name
                }).ToList();

            return listProjects;
        }

        public async Task<List<GetAllPagingProjectProcessCriteriaDto>> GetProjectHaveNotBeenTailor()
        {
            var listExsit = WorkScope.GetAll<ProjectProcessCriteria>()
                .GroupBy(x => x.ProjectId).Select(x => x.Key).ToList();
            return await WorkScope.GetAll<Project>()
                .Where(x => !listExsit.Contains(x.Id)
                    && x.ProjectType != ProjectEnum.ProjectType.TRAINING
                    && x.Status != ProjectEnum.ProjectStatus.Closed)
                .Select(x => new GetAllPagingProjectProcessCriteriaDto
                {
                    ProjectId = x.Id,
                    ProjectName = x.Name,
                    ProjectCode = x.Code,
                    ProjectType = x.ProjectType,
                    ClientName = string.IsNullOrEmpty(x.Client.Name) ? "" : x.Client.Name,
                    ClientCode = string.IsNullOrEmpty(x.Client.Code) ? "" : x.Client.Code,
                    ProjectStatus = x.Status,
                    PMName = x.PM.FullName,
                }).ToListAsync();
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring_Import)]
        [HttpPost]
        public async Task<List<ResponseFailDto>> ImportProjectProcessCriteriaFromExcel([FromForm] ImportProjecProcessCriteriaDto input)
        {
            if (input.File == null || !Path.GetExtension(input.File.FileName).Equals(".xlsx"))
            {
                throw new UserFriendlyException("File null or is not .xlsx file");
            }

            var isExist = await WorkScope.GetAll<ProjectProcessCriteria>().AnyAsync(x => x.ProjectId == input.ProjectId);
            if (isExist)
            {
                throw new UserFriendlyException("Project had aready exist Tailoring!");
            }

            using (var stream = new MemoryStream())
            {
                input.File.CopyTo(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    var mapCodeToId = await WorkScope.GetAll<ProcessCriteria>()
                        .ToDictionaryAsync(x => x.Code.Trim(), y => new { y.Id, y.IsActive, y.IsLeaf });

                    var rowCount = worksheet.Dimension.End.Row;
                    if (rowCount < 2)
                    {
                        throw new UserFriendlyException("Can not found data on this file");
                    }

                    var listToAdd = new List<ProjectProcessCriteria>();
                    var listWarning = new List<ResponseFailDto>();
                    var listCriteriaIds = new List<long>();
                    for (int row = 2; row <= rowCount; row++)
                    {
                        //Code không tồn tại, code null, code sai => trả ra thông báo lỗi Row n: Code number is not exsit or null
                        var code = worksheet.Cells[row, 1].Value.ToString().Trim();

                        if (string.IsNullOrEmpty(code) || !mapCodeToId.ContainsKey(code))
                        {
                            listWarning.Add(new ResponseFailDto { Row = row, ReasonFail = "Code number is not exsit or null" });
                            continue;
                        }
                        var applicable = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString() : "";
                        var pmNote = worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString() : "";

                        if (mapCodeToId.ContainsKey(code) && !string.IsNullOrEmpty(applicable) && (applicable == "Standard" || applicable == "Modify") && mapCodeToId[code].IsLeaf)
                        {
                            if (!mapCodeToId[code].IsActive)
                            {
                                listWarning.Add(new ResponseFailDto { Row = row, ReasonFail = "Criteria can't be imported to tailor because it had been deleted or deactivated" });
                                continue;
                            }
                            var criteriaId = mapCodeToId[code].Id;

                            listToAdd.Add(new ProjectProcessCriteria
                            {
                                Note = pmNote,
                                ProcessCriteriaId = criteriaId,
                                ProjectId = input.ProjectId,
                                Applicable = CommonUtil.GetPPCApplicable(applicable)
                            });
                        }
                    }
                    if (listToAdd.Count > 0)
                    {
                        await WorkScope.InsertRangeAsync(listToAdd);
                    }
                    else
                    {
                        throw new UserFriendlyException("You have to choose at least one field Applicable(Standard/Modify) to import Tailoring");
                    }
                    return listWarning;
                }
            }
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring_Detail)]
        [HttpPost]
        public async Task<TreeProjectProcessCriteriaDto> GetDetail(InputToGetDetail input)
        {
            var listLCs = WorkScope.GetAll<ProcessCriteria>().ToList();
            var dicCriteriaId = WorkScope.GetAll<ProjectProcessCriteria>()
                .Where(x => x.ProjectId == input.ProjectId)
                .Select(x => new { x.ProcessCriteriaId, x.Note, x.Id, x.Applicable })
                .ToDictionary(x => x.ProcessCriteriaId, y => new { y.Note, y.Id, y.Applicable });
            var listCriteria = listLCs
                .Select(x => new GetProjectProcessCriteriaTreeDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    GuidLine = x.GuidLine,
                    IsActive = x.IsActive,
                    IsApplicable = x.IsApplicable,
                    IsLeaf = x.IsLeaf,
                    Level = x.Level,
                    Name = x.Name,
                    Note = dicCriteriaId.ContainsKey(x.Id) ? dicCriteriaId[x.Id].Note : "",
                    ParentId = x.ParentId,
                    ProjectProcessCriteriaId = dicCriteriaId.ContainsKey(x.Id) ? dicCriteriaId[x.Id].Id : 0,
                    Applicable = dicCriteriaId.ContainsKey(x.Id) ? dicCriteriaId[x.Id].Applicable : 0
                })
                .OrderBy(x => CommonUtil.GetNaturalSortKey(x.Code))
                .ToList();
            var resultAll = new List<long>();
            foreach (var id in dicCriteriaId.Select(x => x.Key))
            {
                resultAll.AddRange(_commonManager.GetAllParentId(id, listLCs));
            }
            resultAll = resultAll.Distinct().ToList();
            var listAllContain = listCriteria.Where(x => resultAll.Contains(x.Id)).ToList();
            var listLCsContain = listLCs.Where(x => resultAll.Contains(x.Id)).ToList();
            if (input.GetAll())
            {
                return new TreeProjectProcessCriteriaDto
                {
                    Childrens = listAllContain.GenerateTree(c => c.Id, c => c.ParentId)
                };
            }
            var listIdsFilter = listAllContain
                .Where(x => x.Name.ToLower().Contains(input.SearchText.Trim().ToLower()) || x.Code.Trim().ToLower().Contains(input.SearchText.Trim().ToLower()))
                .WhereIf(input.Applicable >= ProjectEnum.Applicable.Standard, x => x.Applicable == input.Applicable)
                .Select(x => x.Id)
                .ToList();
            var resultFilter = new List<long>();
            listIdsFilter.ForEach(x =>
            {
                resultFilter.AddRange(_commonManager.GetAllNodeAndLeafIdById(x, listLCsContain, true));
            });
            resultFilter = resultFilter.Distinct().ToList();
            return new TreeProjectProcessCriteriaDto
            {
                Childrens = listAllContain.Where(x => resultFilter.Contains(x.Id)).GenerateTree(c => c.Id, c => c.ParentId)
            };
        }

        // export excel file using aspose library(Duy)
        [AbpAuthorize(PermissionNames.Audits_Tailoring_DownLoadTemplate)]
        [HttpPost]
        public async Task<FileBase64Dto> DownloadTemplate()
        {
            var data = WorkScope.GetAll<ProcessCriteria>()
                .Where(x => x.IsActive).AsEnumerable()
                .Select(y => new
                {
                    Code = y.Code,
                    Name = y.Name,
                    IsLeaf = y.IsLeaf,
                    QAExample = y.QAExample,
                    GuildLine = y.GuidLine,
                    Level = y.Level,
                    ParentId = y.ParentId,
                    IsApplicable = y.IsApplicable,
                })
                 .OrderBy(x => CommonUtil.GetNaturalSortKey(x.Code)).ToList();

            using (var wb = new Workbook())
            {
                var applicable = new List<string>() { "Standard", "Modify", "Not Yet" };
                var sheetAudit = wb.Worksheets[0];
                sheetAudit.Name = "Audit";
                var sheetAudit2 = wb.Worksheets[wb.Worksheets.Add()];
                sheetAudit2.Name = "Audit2";

                Style defaultStyle = sheetAudit.Cells.Style;
                defaultStyle.VerticalAlignment = TextAlignmentType.Center;
                defaultStyle.Font.Size = 10;
                defaultStyle.Font.Name = "Aerial";
                sheetAudit.Cells.Style = defaultStyle;

                Range range = sheetAudit.Cells.CreateRange("A1", "F1");
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

                sheetAudit.Cells["A1"].Value = "No";
                sheetAudit.Cells["B1"].Value = "Criteria";
                sheetAudit.Cells["C1"].Value = "Applicable?";
                sheetAudit.Cells["D1"].Value = "Tailoring Note";
                sheetAudit.Cells["E1"].Value = "Guideline";
                sheetAudit.Cells["F1"].Value = "Q&A Examples";

                // Freeze the first row and first two columns
                sheetAudit.FreezePanes(1, 2, 1, 2);

                var startAudit = sheetAudit.Cells["A2"].Row + 1;// first row index == 0
                // validation setup
                // Create a range in the second worksheet.
                range = sheetAudit2.Cells.CreateRange($"A1", $"A{applicable.Count}");

                // Name the range.
                range.Name = "MyRange";

                // Fill different cells with data in the range.
                int start = 0;
                foreach (var item in applicable)
                {
                    range[start++, 0].PutValue(item);
                }

                foreach (var i in data)
                {
                    //set value for cell A,B
                    sheetAudit.Cells[$"A{startAudit}"].Value = i.Code;
                    sheetAudit.Cells[$"B{startAudit}"].Value = " " + (i.Level > 1 ? new string('-', i.Level) + i.Name : i.Name);
                    //set value for cell E,F
                    sheetAudit.Cells[$"E{startAudit}"].HtmlString = " " + i.GuildLine.ToString();
                    sheetAudit.Cells[$"F{startAudit}"].HtmlString = " " + i.QAExample.ToString();

                    // Make Cell's Text wrap
                    Style style1 = sheetAudit.Cells[$"D{startAudit}"].GetStyle();
                    style1.IsTextWrapped = true;
                    sheetAudit.Cells[$"E{startAudit}"].SetStyle(style1);
                    sheetAudit.Cells[$"F{startAudit}"].SetStyle(style1);
                    sheetAudit.Cells[$"B{startAudit}"].SetStyle(style1);
                    sheetAudit.Cells[$"D{startAudit}"].SetStyle(style1);

                    if (i.IsLeaf) // white
                    {
                        sheetAudit.Cells[$"C{startAudit}"].Value = i.IsApplicable == true
                            ? applicable[0] : applicable[2];
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

                    ///////////////////////////////////
                    if (!i.IsLeaf)// dark or light blue
                    {
                        range = sheetAudit.Cells.CreateRange($"A{startAudit}", $"F{startAudit}");
                        //style = defaultStyle;
                        style = sheetAudit.Cells[$"D{startAudit}"].GetStyle(); ;
                        style.Pattern = BackgroundType.Solid;
                        style.ForegroundColor = Color.FromArgb(218, 241, 243);
                        range.ApplyStyle(style, flg);
                    }
                    if (!i.ParentId.HasValue && !i.IsLeaf)// dark blue
                    {
                        range = sheetAudit.Cells.CreateRange($"A{startAudit}", $"F{startAudit}");
                        //style = defaultStyle;
                        style = sheetAudit.Cells[$"D{startAudit}"].GetStyle(); ;
                        style.Pattern = BackgroundType.Solid;
                        style.ForegroundColor = Color.FromArgb(155, 194, 230);
                        range.ApplyStyle(style, flg);
                    }

                    if (!i.IsLeaf) // dark or light blue
                    {
                        Cell cellA = sheetAudit.Cells[$"A{startAudit}"];
                        Cell cellB = sheetAudit.Cells[$"B{startAudit}"];
                        style = sheetAudit.Cells[$"A{startAudit}"].GetStyle();
                        style.Font.IsBold = true;
                        cellA.SetStyle(style);
                        cellB.SetStyle(style);
                    }
                    startAudit++;
                }

                sheetAudit.Cells.SetColumnWidth(1, 40);
                sheetAudit.Cells.SetColumnWidth(2, 10);
                sheetAudit.Cells.SetColumnWidth(3, 50);
                sheetAudit.Cells.SetColumnWidth(4, 50);
                sheetAudit.Cells.SetColumnWidth(5, 50);

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
                    FileName = $"Import Tailoring Template.xlsx",
                    FileType = MimeTypeNames.ApplicationVndMsExcel,
                    Base64 = Convert.ToBase64String(buffer)
                };
            }
        }

        [AbpAuthorize(PermissionNames.Audits_Tailoring_Detail_Update)]
        [HttpPost]
        public async Task UpdateProjectProcessCriteria(UpdateNoteApplicableDto input)
        {
            var item = await WorkScope.GetAsync<ProjectProcessCriteria>(input.Id);
            item.Note = input.Note;
            item.Applicable = input.Applicable;
            await WorkScope.UpdateAsync(item);
        }

    }
}