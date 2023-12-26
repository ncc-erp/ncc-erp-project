namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class UpdateProjectUserBillNoteDto
    {
        public long UserId { get;  set; }
        public long ProjectId { get;  set; }
        public string Note { get;  set; }
    } 
}