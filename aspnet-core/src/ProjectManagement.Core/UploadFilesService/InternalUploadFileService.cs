using Abp.UI;
using Microsoft.AspNetCore.Http;
using NccCore.Uitls;
using ProjectManagement.Constants;
using ProjectManagement.Helper;
using ProjectManagement.Utils;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.UploadFilesService
{
    public class InternalUploadFileService : IUploadFileService
    {
        private readonly string WWWRootFolder = "wwwroot";


        private void CheckValidFile(IFormFile file, string[] allowFileTypes)
        {
            var fileExt = FileUtils.GetFileExtension(file);
            if (!allowFileTypes.Contains(fileExt))
                throw new UserFriendlyException($"Wrong file type {file.ContentType}. Allow file types: {string.Join(", ", allowFileTypes)}");
        }

        public async Task<string> UploadAvatarAsync(IFormFile file,string tenantName)
        {
            CheckValidFile(file, ConstantUploadFile.AllowImageFileTypes);

            var avatarFolder = Path.Combine(WWWRootFolder, ConstantUploadFile.AvatarFolder, tenantName);
            UploadFile.CreateFolderIfNotExists(avatarFolder);

            var fileName = $"{CommonUtil.NowToYYYYMMddHHmmss()}_{Guid.NewGuid()}.{FileUtils.GetFileExtension(file)}";
            var filePath = $"{ConstantUploadFile.AvatarFolder?.TrimEnd('/')}/{tenantName}/{fileName}";

            await UploadFile.UploadFileAsync(avatarFolder, file, fileName);

            return filePath;
        }

        public Task<string> UploadFileAsync(IFormFile file, string[] allowFileTypes, string filePath)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadTimsheetAsync(IFormFile file, string tenantName, int year, int month, string fileName)
        {
            CheckValidFile(file, ConstantUploadFile.AllowTimesheetFileTypes);


            var yyyyMM = DateTimeUtils.yyyyMM(year, month);
            var folder = $"{ConstantUploadFile.UPLOAD_FOLDER}/{tenantName}/{ConstantUploadFile.TIMESHEET_FOLDER}/{yyyyMM}";

            UploadFile.CreateFolderIfNotExists(folder);

            var filePath = $"{folder.TrimEnd('/')}/{fileName}";

            await UploadFile.UploadFileAsync(folder, file, fileName);

            return filePath;
        }

        public async Task<byte[]> DownloadFileAsync(string filePath)
        {
            return await File.ReadAllBytesAsync(filePath);           
        }
    }
}
