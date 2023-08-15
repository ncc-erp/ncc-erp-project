namespace ProjectManagement.Manager.TimesheetProjectManager.Dto
{
    public class ReactiveTimesheetBGJDto
    {
        public long TimesheetProjectId { get; set; }
        public long? CurrentUserLoginId { get; set; }
        public int? TenantId { get; set; }
    }
}