using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class UploadCvBillAccountDto : EntityDto<long>
    {
        [Required]
        public string NameCv { get; set; }
        [Required]
        public IFormFile SelectedFile { get; set; }
    }

    public class GetCvBillAccountDto : EntityDto<long>
    {
        public string NameCv { get; set; }
        public string LinkCV { get; set; }
    }
}