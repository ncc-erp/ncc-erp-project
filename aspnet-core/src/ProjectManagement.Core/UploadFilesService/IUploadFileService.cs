using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.UploadFilesService
{
    public interface IUploadFileService
    {
        Task<string> UploadAvatarAsync(IFormFile file, string tenantName);
        Task<string> UploadTimsheetAsync(IFormFile file, string tenantName, int year, int month, string fileName);

        Task<string> UploadFileAsync(IFormFile file, string[] allowFileTypes, string filePath);

        Task<byte[]> DownloadFileAsync(string filePath);
    }
}
