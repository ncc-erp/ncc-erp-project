namespace ProjectManagement.Manager.TimesheetManagers
{
    public class TimesheetBGJDto
    {
        public long TimesheetId { get; set; }
        public long? CurrentUserLoginId { get; set; }
        public int? TenantId { get; set; }
    }
}