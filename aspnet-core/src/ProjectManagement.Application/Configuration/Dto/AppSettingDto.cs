using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectManagement.Configuration.Dto
{
    public class AppSettingDto
    {
        public string ClientAppId { get; set; }
        public string ProjectUri { get; set; }
        public string SecurityCode { get; set; }
        public string FinanceUri { get; set; }
        public string FinanceSecretCode { get; set; }
        public string TimesheetUri { get; set; }
        public string TimesheetSecretCode { get; set; }
        public string AutoUpdateProjectInfoToTimesheetTool { get; set; }
        public string HRMUri { get; set; }
        public string HRMSecretCode { get; set; }
        public string CanSendDay { get; set; }
        public string CanSendHour { get; set; }
        public string ExpiredDay { get; set; }
        public string ExpiredHour { get; set; }
        public string KomuUrl { get; set; }
        public string NoticeToKomu { get; set; }
        public string KomuSecretCode { get; set; }
        public string KomuUserNames { get; set; }
        public string UserBot { get; set; }
        public string PasswordBot { get; set; }
        public string KomuRoom { get; set; }
        public string DefaultWorkingHours { get; set; }
        public string TrainingRequestChannel { get; set; }
        public string TalentUriBA { get; set; }
        public string TalentUriFE { get; set; }
        public string TalentSecurityCode { get; set; }
        public string MaxCountHistory { get; set; }
        public string InformPm { get; set; }
        public string ActiveTimesheetProjectPeriod { get; set; }
    }

    public class ProjectSetting
    {
        public string SecurityCode { get; set; }
    }

    public class WeeklyReportSettingDto
    {
        public int TimeCountDown { get; set; } //s
    }

    public class AuditScoreDto
    {
        public int GIVEN_SCORE { get; set; }
        public int PROJECT_SCORE_WHEN_STATUS_GREEN { get; set; }
        public int PROJECT_SCORE_WHEN_STATUS_AMBER { get; set; }
        public int PROJECT_PROCESS_CRITERIA_RESULT_STATUS_NC { get; set; }
        public int PROJECT_PROCESS_CRITERIA_RESULT_STATUS_OB { get; set; }
        public int PROJECT_PROCESS_CRITERIA_RESULT_STATUS_RE { get; set; }
        public int PROJECT_PROCESS_CRITERIA_RESULT_STATUS_EX { get; set; }
    }

    public class GuideLineDto
    {
        [JsonPropertyName("Issue")]
        public string Issue { get; set; }

        [JsonPropertyName("Risk")]
        public string Risk { get; set; }

        [JsonPropertyName("PMNote")]
        public string PMNote { get; set; }

        [JsonPropertyName("CriteriaStatus")]
        public string CriteriaStatus { get; set; }
    }

    // setting time to inform pm send weekly report
    public class InformPmDto
    {
        // channel id
        public string ChannelId { get; set; }
        public List<CheckDateTime> CheckDateTimes { get; set; }
    }

    public class CheckDateTime
    {
        public bool IsCheck { get; set; }
        public string Time { get; set; }
        public int Day { get; set; }
    }
}
