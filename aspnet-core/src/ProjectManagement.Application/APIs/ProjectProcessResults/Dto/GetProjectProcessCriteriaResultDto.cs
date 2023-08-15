namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    public class GetProjectProcessCriteriaResultDto
    {
        public object Id { get; set; }
        public object ProjectId { get; set; }
        public object ProcessCriteriaId { get; set; }
        public object ProjectProcessResultId { get; set; }
        public object Note { get; set; }
        public object Status { get; set; }
        public object Score { get; set; }
    }
}