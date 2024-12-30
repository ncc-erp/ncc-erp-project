using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.Exceptions;
using NccCore.Uitls;
using ProjectManagement.Constants;
using ProjectManagement.Utils;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjectManagement.UploadFilesService
{
    public class MinioService : IUploadFileService
    {
        private readonly ILogger<MinioService> logger;
        private readonly MinioClient minioClient;

        public MinioService(ILogger<MinioService> logger, MinioClient minioClient)
        {
            this.logger = logger;
            this.minioClient = minioClient;
        }

        public async Task<byte[]> DownloadFileAsync(string filePath)
        {

            using var ms = new MemoryStream();
            var key = $"{ConstantMinio.Prefix?.TrimEnd('/')}/{filePath}";
            await minioClient.GetObjectAsync(ConstantMinio.BucketName, key,
                stream =>
                {
                    stream.CopyTo(ms);
                });

            var fileData = ms.ToArray();

            if (fileData.Length == 0)
                throw new FileNotFoundException($"The file '{filePath}' is not found or empty.");

            return fileData;
        }

        public async Task<string> UploadAvatarAsync(IFormFile file, string tenantName)
        {
            var filePath = $"{ConstantUploadFile.AvatarFolder?.TrimEnd('/')}/{tenantName}/{DateTimeUtils.NowToYYYYMMddHHmmss()}_{Guid.NewGuid()}.{FileUtils.GetFileExtension(file)}";
            return await UploadFileAsync(file, ConstantUploadFile.AllowImageFileTypes, filePath);
        }

        public async Task<string> UploadCvFileAsync(IFormFile file, string tenantName, string fileName)
        {
            var filepath = $"{ConstantUploadFile.Project_Tool}/{tenantName}/{ConstantUploadFile.CV_Folder}/{fileName}";
            return await UploadFileAsync(file, ConstantUploadFile.AllowCVFileTypes, filepath);
        }

        public async Task<string> UploadFileAsync(IFormFile file, string[] allowFileTypes, string filePath)
        {
            var strAllowFileType = string.Join(", ", allowFileTypes);
            logger.LogInformation($"UploadFile() fileName: {file.FileName}, contentType: {file.ContentType}, allowFileTypes: {strAllowFileType}, filePath: {filePath}");
            FileUtils.CheckValidFile(file, allowFileTypes);
            var key = $"{ConstantMinio.Prefix?.TrimEnd('/')}/{filePath}";
            logger.LogInformation($"UploadFile() Key: {key}");
            try
            {
                bool found = await minioClient.BucketExistsAsync(ConstantMinio.BucketName).ConfigureAwait(false);
                if (!found)
                {
                    await minioClient.MakeBucketAsync(ConstantMinio.BucketName).ConfigureAwait(false);
                }
                using (var stream = file.OpenReadStream())
                {
                    await minioClient.PutObjectAsync(ConstantMinio.BucketName, key, stream, stream.Length, file.ContentType).ConfigureAwait(false);
                }
                logger.LogInformation($"Successfully uploaded {key}");
                return key;
            }
            catch (MinioException e)
            {
                logger.LogError(e, "File Upload Error");
                throw new UserFriendlyException("File upload failed. Please try again later.");
            }
        }

        public async Task<string> UploadTimsheetAsync(IFormFile file, string tenantName, int year, int month, string fileName)
        {
            var yyyyMM = DateTimeUtils.yyyyMM(year, month);
            var filePath = $"{AppConsts.APP_NAME}/{tenantName}/{ConstantUploadFile.TIMESHEET_FOLDER}/{yyyyMM}/{fileName}";
            return await UploadFileAsync(file, ConstantUploadFile.AllowTimesheetFileTypes, filePath);
        }
    }
}
