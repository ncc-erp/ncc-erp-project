using System;
using System.Collections.Generic;
using System.Text;
using Abp.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectManagement.Services.PmBot.Dto;
using System.Net.Http;
using System.Threading.Tasks;


namespace ProjectManagement.Services.PmBot
{
    public class PmBotService : BaseWebService
    {
        private const string ServiceName = "PmBotService";
        private const string WeeklyReportByProjectIdEndpoint = "api/weekly-report";
        private const string InactiveWeeklyReportByProjectIdEndpoint = "api/weekly-report/inactive";

        public PmBotService(
            HttpClient httpClient,
            ILogger<PmBotService> logger,
            IConfiguration configuration,
            IAbpSession abpSession
        ) : base(httpClient, configuration, logger, abpSession, ServiceName) { }
        public async Task<SyncMeetingReportResponseDto> SyncProjectMeetingReportAsync(SyncMeetingReportRequestDto input)
        {
            var url = $"{WeeklyReportByProjectIdEndpoint}/{input.ProjectId}";
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                var response = await HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new SyncMeetingReportResponseDto
                    {
                        Success = true
                    };
                }

                return new SyncMeetingReportResponseDto
                {
                    Success = false
                };
            }
            catch (Exception ex)
            {
                logger.LogError($"PmBot sync failed. url={HttpClient.BaseAddress}{url}, projectId={input?.ProjectId}, error={ex.Message}");
                return new SyncMeetingReportResponseDto
                {
                    Success = false
                };
            }
        }

        public async Task InactiveProjectWeeklyReportAsync(long projectId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException("ProjectId must be greater than 0.", nameof(projectId));
            }

            var url = $"{InactiveWeeklyReportByProjectIdEndpoint}/{projectId}";
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, url);
                var response = await HttpClient.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    $"PmBot inactive weekly report trigger failed. url={HttpClient.BaseAddress}{url}, projectId={projectId}, error={ex.Message}");
            }
        }
    }
}


