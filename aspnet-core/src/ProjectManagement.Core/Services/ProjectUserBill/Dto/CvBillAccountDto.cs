using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class UploadCvBillAccountDto
    {
        public long Id { get; set; }
        public IFormFile SelectedFile { get; set; }
    }

    public class GetCvBillAccountDto
    {
        public long Id { get; set; }
        public string LinkCV { get; set; }
    }
}