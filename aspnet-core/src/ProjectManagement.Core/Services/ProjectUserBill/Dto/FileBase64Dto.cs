using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class FileBase64Dto
    {
        public string FileName { get; set; }
        public string Base64 { get; set; }
        public string FileType { get; set; }
    }
}