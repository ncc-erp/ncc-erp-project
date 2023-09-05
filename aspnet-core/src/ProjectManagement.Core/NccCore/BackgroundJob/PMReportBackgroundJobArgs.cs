
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.NccCore.BackgroundJob
{
    public class PMReportBackgroundJobArgs
    {
        public long PMReportId { get; set; }
        public PMReportStatus PMReportStatus { get; set; }
    }
}
