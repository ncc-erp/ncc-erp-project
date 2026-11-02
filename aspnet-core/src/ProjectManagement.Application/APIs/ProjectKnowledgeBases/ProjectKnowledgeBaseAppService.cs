using Abp.Authorization;
using ProjectManagement.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.ProjectKnowledgeBases.Dto;
using ProjectManagement.Entities;
using ProjectManagement.Helper;
using Abp.Dependency;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ProjectManagement.Services.AI;
using ProjectManagement.Services.PmBot;
using System.IO;
using System.Text;
using System.Threading;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectKnowledgeBases
{
    [AbpAuthorize]
    public class ProjectKnowledgeBaseAppService : ProjectManagementAppServiceBase
    {
        private readonly IAiService _aiService;
        private readonly IIocResolver _resolver;
        private readonly KnowledgeBaseProjectHelper _helper;

        public ProjectKnowledgeBaseAppService(IAiService aiService, IIocResolver resolver,
            KnowledgeBaseProjectHelper helper)
        {
            _aiService = aiService;
            _resolver = resolver;
            _helper = helper;
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects)]
        public async Task<KnowledgeBaseDto> GetKnowledgeBase([FromQuery] long projectId)
        {
            var project = await GetProject(projectId);
            var knowledgeBase = await FindKnowledgeBase(project.Id, false);
            return await MapAsync(project, knowledgeBase);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects)]
        public async Task<List<KnowledgeBaseFileOptionDto>> GetSelectableFiles([FromQuery] long projectId)
        {
            var project = await GetProject(projectId);
            var knowledgeBase = await FindKnowledgeBase(project.Id, false);
            var selected = KnowledgeBaseProjectHelper.ReadReferenceFileIds(knowledgeBase?.SourceFileIds);
            var files = await WorkScope.GetAll<ProjectFile>().IgnoreQueryFilters().AsNoTracking()
                .Where(file => file.ProjectId == project.Id && !file.IsDeleted)
                .OrderByDescending(file => file.CreationTime)
                .Select(file => new { file.Id, file.FilePath })
                .ToListAsync();

            var options = files.Where(file => KnowledgeBaseProjectHelper.IsSupportedFile(file.FilePath))
                .Select(file => new KnowledgeBaseFileOptionDto
                {
                    ProjectFileId = file.Id.ToString(CultureInfo.InvariantCulture),
                    Name = KnowledgeBaseProjectHelper.DisplaySourceName(file.FilePath),
                    IsSelected = selected.Contains(file.Id)
                }).ToList();

            return options;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects)]
        public async Task<List<KnowledgeBaseReferenceFileDto>> UpdateReferenceFiles(
            UpdateKnowledgeBaseReferencesInput input)
        {
            var project = await GetProject(input.ProjectId);
            var knowledgeBase = await FindKnowledgeBase(project.Id, true);
            var currentIds = KnowledgeBaseProjectHelper.ReadReferenceFileIds(knowledgeBase?.SourceFileIds);
            var files = await WorkScope.GetAll<ProjectFile>().IgnoreQueryFilters().AsNoTracking()
                .Where(file => file.ProjectId == project.Id && !file.IsDeleted)
                .Select(file => new { file.Id, file.FilePath })
                .ToListAsync();
            var ids = KnowledgeBaseProjectHelper.ValidateReferenceFiles(
                currentIds,
                input.ProjectFileIds,
                files.Where(file => KnowledgeBaseProjectHelper.IsSupportedFile(file.FilePath))
                    .Select(file => file.Id));

            if (knowledgeBase == null)
            {
                knowledgeBase = new ProjectKnowledgeBase
                {
                    TenantId = project.TenantId,
                    ProjectId = project.Id,
                    SourceFileIds = KnowledgeBaseProjectHelper.WriteReferenceFileIds(ids)
                };
                await WorkScope.InsertAsync(knowledgeBase);
            }
            else
            {
                knowledgeBase.SourceFileIds = KnowledgeBaseProjectHelper.WriteReferenceFileIds(ids);
            }

            return await MapReferenceFiles(project, knowledgeBase);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects)]
        public async Task<GeneratedKnowledgeBaseDto> GenerateKnowledgeBase([FromQuery] long projectId)
        {
            await GetProject(projectId);
            var cancellationToken = HttpContextAccessor.HttpContext.RequestAborted;

            var sourceData = await BuildSourceDataAsync(projectId, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            var content = await _aiService.GenerateTextAsync(
                KnowledgeBaseProjectHelper.KnowledgeBasePrompt, sourceData, cancellationToken);
            return new GeneratedKnowledgeBaseDto { Content = content };
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects)]
        public async Task<SavedKnowledgeBaseDto> UpdateKnowledgeBase(UpdateKnowledgeBaseInput input)
        {
            if (input?.Content == null)
                throw new Abp.UI.UserFriendlyException("Knowledge base content cannot be null. Send an empty string to clear the content.");

            var project = await GetProject(input.ProjectId);
            var knowledgeBase = await FindKnowledgeBase(project.Id, true);
            if (knowledgeBase == null)
            {
                knowledgeBase = new ProjectKnowledgeBase
                {
                    TenantId = project.TenantId,
                    ProjectId = project.Id,
                    SourceFileIds = KnowledgeBaseProjectHelper.WriteReferenceFileIds(Array.Empty<long>())
                };
                await WorkScope.InsertAsync(knowledgeBase);
            }

            if (!string.Equals(knowledgeBase.Content, input.Content, StringComparison.Ordinal))
            {
                var updatedAt = DateTime.UtcNow;
                if (!string.IsNullOrWhiteSpace(knowledgeBase.Content))
                {
                    await WorkScope.InsertAsync(new ProjectKnowledgeBaseHistory
                    {
                        KnowledgeBaseId = knowledgeBase.Id,
                        TenantId = knowledgeBase.TenantId,
                        Content = knowledgeBase.Content,
                        CreateAt = updatedAt,
                        UpdateByUserId = AbpSession.UserId
                    });
                }

                knowledgeBase.Content = input.Content;
                knowledgeBase.UpdatedAt = updatedAt;
                knowledgeBase.UpdatedByUserId = AbpSession.UserId;
            }

            return MapSavedContent(project, knowledgeBase);
        }

        private async Task<string> BuildSourceDataAsync(long projectId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (projectId <= 0)
                throw new UserFriendlyException("ProjectId must be a positive integer.");

            var input = new StringBuilder();
            var project = await WorkScope.GetAll<Project>().IgnoreQueryFilters().AsNoTracking()
                .Where(p => p.Id == projectId && !p.IsDeleted && !p.PM.IsDeleted)
                .Select(p => new
                {
                    General = new KnowledgeBaseGeneralDto
                    {
                        ProjectType = p.ProjectType,
                        Name = p.Name,
                        StartTime = p.StartTime,
                        EndTime = p.EndTime,
                        ProjectManager = p.PM.FullName,
                        Customer = p.Client == null || p.Client.IsDeleted ? null : p.Client.Name,
                        Currency = p.Currency == null || p.Currency.IsDeleted ? null : p.Currency.Name,
                        ChargeType = p.ChargeType
                    },
                    Description = new KnowledgeBaseDescriptionDto
                    {
                        BriefDescription = p.BriefDescription,
                        DetailDescription = p.DetailDescription,
                        TechnologyUsed = p.TechnologyUsed,
                        TechnicalProblems = p.TechnicalProblems,
                        OtherProblems = p.OtherProblems,
                        NewKnowledge = p.NewKnowledge
                    }
                }).FirstOrDefaultAsync();
            if (project == null) throw new UserFriendlyException("Project not found.");

            var sourceFileIds = await WorkScope.GetAll<ProjectKnowledgeBase>().IgnoreQueryFilters().AsNoTracking()
                .Where(item => item.ProjectId == projectId)
                .Select(item => item.SourceFileIds)
                .FirstOrDefaultAsync();

            project.Description.Technologies = await WorkScope.GetAll<ProjectTechnology>().IgnoreQueryFilters().AsNoTracking()
                .Where(t => t.ProjectId == projectId && !t.IsDeleted && !t.Technology.IsDeleted)
                .Select(t => t.Technology.Name).ToListAsync();

            AppendSource(input, "General", project.General);
            AppendSource(input, "Project Description", project.Description);

            cancellationToken.ThrowIfCancellationRequested();
            var reports = await WorkScope.GetAll<PMReportProject>().IgnoreQueryFilters().AsNoTracking()
                .Where(p => p.ProjectId == projectId && !p.IsDeleted && !p.PMReport.IsDeleted &&
                    p.PMReport.Type == PMReportType.Weekly &&
                    p.Status == PMReportProjectStatus.Sent)
                .OrderByDescending(p => p.TimeSendReport).ThenByDescending(p => p.Id)
                .Take(5)
                .Select(p => new { p.PMReportId, Name = p.PMReport.Name })
                .ToListAsync();

            if (reports.Count == 0)
            {
                AppendSource(input, "Weekly Report", new List<KnowledgeBaseWeeklyReportDto>());
            }
            else
            {
                var reportIds = reports.Select(report => report.PMReportId).ToList();
                var criteria = await WorkScope.GetAll<ProjectCriteriaResult>().IgnoreQueryFilters().AsNoTracking()
                    .Where(result => result.ProjectId == projectId && !result.IsDeleted &&
                        !result.ProjectCriteria.IsDeleted && reportIds.Contains(result.PMReportId))
                    .Select(result => new
                    {
                        PMReportId = result.PMReportId,
                        Name = result.ProjectCriteria.Name,
                        Status = result.Status,
                        Note = result.Note
                    }).ToListAsync();
                AppendSource(input, "Weekly Report", reports.Select(report => new KnowledgeBaseWeeklyReportDto
                {
                    ReportName = report.Name,
                    Criteria = criteria.Where(result => result.PMReportId == report.PMReportId)
                        .Select(result => new KnowledgeBaseCriterionDto
                        {
                            Name = result.Name,
                            Status = result.Status,
                            Note = result.Note
                        }).ToList()
                }).ToList());
            }
            cancellationToken.ThrowIfCancellationRequested();
            using (var bot = _resolver.ResolveAsDisposable<PmBotService>())
            {
                var sources = await bot.Object.GetKnowledgeBaseSourcesAsync(projectId, cancellationToken);
                AppendSource(input, "Bot - daily notes", sources?.DailyNotesText);
                AppendSource(input, "Bot - meeting transcript", sources?.MeetingTranscript);
                AppendSource(input, "Bot - contribute data", sources?.ContributeData);
            }

            var selectedIds = KnowledgeBaseProjectHelper.ReadReferenceFileIds(sourceFileIds);
            if (selectedIds.Count == 0) return input.ToString();

            var files = await WorkScope.GetAll<ProjectFile>().IgnoreQueryFilters().AsNoTracking()
                .Where(file => selectedIds.Contains(file.Id) && file.ProjectId == projectId && !file.IsDeleted)
                .ToListAsync();

            foreach (var fileId in selectedIds)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var file = files.FirstOrDefault(item => item.Id == fileId);
                if (file == null) continue;

                var name = KnowledgeBaseProjectHelper.DisplaySourceName(file.FilePath);
                try
                {
                    var text = _helper.ReadDocument(file, cancellationToken);
                    AppendSource(input, "Project File: " + name, text);
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    var reason = exception is InvalidDataException
                        ? exception.Message : "Unable to read the document.";
                    throw new UserFriendlyException("Unable to generate the knowledge base. " + name + ": " + reason);
                }
            }

            return input.ToString();
        }

        private static void AppendSource(StringBuilder output, string label, object data)
        {
            output.AppendLine("[" + label + "]");
            output.AppendLine(data == null ? "" : data is string text
                ? text : JsonConvert.SerializeObject(data, new StringEnumConverter()));
            output.AppendLine();
        }

        private async Task<Project> GetProject(long projectId)
        {
            if (projectId <= 0)
                throw new UserFriendlyException("ProjectId must be a positive integer.");

            return await WorkScope.GetAll<Project>().IgnoreQueryFilters().AsNoTracking()
                .Where(item => item.Id == projectId && !item.IsDeleted)
                .FirstOrDefaultAsync()
                ?? throw new UserFriendlyException("Project not found.");
        }

        private Task<ProjectKnowledgeBase> FindKnowledgeBase(long projectId, bool tracked)
        {
            var query = WorkScope.GetAll<ProjectKnowledgeBase>().IgnoreQueryFilters()
                .Where(item => item.ProjectId == projectId);
            return (tracked ? query : query.AsNoTracking()).FirstOrDefaultAsync();
        }

        private async Task<KnowledgeBaseDto> MapAsync(Project project,
            ProjectKnowledgeBase knowledgeBase)
        {
            return new KnowledgeBaseDto
            {
                ProjectId = project.Id.ToString(CultureInfo.InvariantCulture),
                Content = knowledgeBase?.Content,
                UpdatedAt = knowledgeBase?.UpdatedAt.HasValue == true
                    ? DateTime.SpecifyKind(knowledgeBase.UpdatedAt.Value, DateTimeKind.Utc)
                    : (DateTime?)null,
                ReferenceFiles = await MapReferenceFiles(project, knowledgeBase)
            };
        }

        private static SavedKnowledgeBaseDto MapSavedContent(Project project, ProjectKnowledgeBase knowledgeBase)
        {
            return new SavedKnowledgeBaseDto
            {
                ProjectId = project.Id.ToString(CultureInfo.InvariantCulture),
                Content = knowledgeBase.Content,
                UpdatedAt = knowledgeBase.UpdatedAt.HasValue
                    ? DateTime.SpecifyKind(knowledgeBase.UpdatedAt.Value, DateTimeKind.Utc)
                    : (DateTime?)null
            };
        }

        private async Task<List<KnowledgeBaseReferenceFileDto>> MapReferenceFiles(Project project,
            ProjectKnowledgeBase knowledgeBase)
        {
            var ids = KnowledgeBaseProjectHelper.ReadReferenceFileIds(knowledgeBase?.SourceFileIds);
            var files = await WorkScope.GetAll<ProjectFile>().IgnoreQueryFilters().AsNoTracking()
                .Where(file => ids.Contains(file.Id) && file.ProjectId == project.Id && !file.IsDeleted)
                .Select(file => new { file.Id, file.FilePath })
                .ToListAsync();

            return ids.Where(id => files.Any(file => file.Id == id)).Select(id => new KnowledgeBaseReferenceFileDto
            {
                ProjectFileId = id.ToString(CultureInfo.InvariantCulture),
                Name = KnowledgeBaseProjectHelper.DisplaySourceName(files.First(file => file.Id == id).FilePath)
            }).ToList();
        }
    }
}
