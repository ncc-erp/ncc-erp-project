using Abp.Authorization;
using Abp.Configuration;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using NccCore.Uitls;
using OfficeOpenXml;
using ProjectManagement.APIs.ProjectUsers.Dto;
using ProjectManagement.APIs.Punishments.Dto;
using ProjectManagement.APIs.Timesheets.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.UploadFilesService;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Punishments
{
    [AbpAuthorize]
    public class PunishmentAppService : ProjectManagementAppServiceBase, IPunishmentAppService
    {
        private readonly UploadFileService _uploadFileService;

        public PunishmentAppService(
            UploadFileService uploadFileService) : base()
        {
            _uploadFileService = uploadFileService;
        }

        [HttpPost]
        [AbpAuthorize()]
        [AbpAuthorize(PermissionNames.Timesheets_Punishment)]
        public async Task<long> Create([FromForm] PunishmentDto input)
        {
            if (input.File == null || input.File.Length == 0)
                throw new UserFriendlyException("Please select a valid file to upload.");

            ValidatePunishmentTemplate(input.File);

            var punishmentExists = await WorkScope.GetAll<Punishment>()
                .AnyAsync(x => x.Year == input.Year && x.Month == input.Month);

            if (punishmentExists)
                throw new UserFriendlyException($"A punishment record already exists for {input.Month}/{input.Year}.");

            var cleanFileName = input.File.FileName.Replace(" ", "_");
            var uniqueFileName = $"{DateTimeUtils.NowToyyyyMMddHHmmssfff()}_{cleanFileName}";

            var filePath = await _uploadFileService.UploadPunishmentFileAsync(input.File, uniqueFileName);

            if (string.IsNullOrEmpty(filePath))
                throw new UserFriendlyException("File upload failed. Please try again.");

            var punishment = ObjectMapper.Map<Punishment>(input);

            punishment.FileName = uniqueFileName;
            punishment.FilePath = filePath;

            return await WorkScope.InsertAndGetIdAsync(punishment);
        }

        [HttpPut]
        [AbpAuthorize()]
        [AbpAuthorize(PermissionNames.Timesheets_Punishment)]
        public async Task<Punishment> Update([FromForm] PunishmentDto input)
        {
            var punishment = await WorkScope.GetAsync<Punishment>(input.Id);
            if (punishment == default)
                throw new UserFriendlyException($"Punishment with Id {input.Id} does not exist.");

            if (punishment.Month != input.Month || punishment.Year != input.Year)
            {
                var isDuplicate = await WorkScope.GetAll<Punishment>()
                    .AnyAsync(x => x.Id != input.Id && x.Year == input.Year && x.Month == input.Month);

                if (isDuplicate)
                    throw new UserFriendlyException($"A punishment already exists for {input.Month}/{input.Year}.");
            }

            punishment.Month = input.Month;
            punishment.Year = input.Year;
            punishment.Note = input.Note;

            if (input.File != null)
            {
                ValidatePunishmentTemplate(input.File);
                var cleanFileName = input.File.FileName.Replace(" ", "_");
                var filename = $"{DateTimeUtils.NowToyyyyMMddHHmmssfff()}_{cleanFileName}";

                var filePath = await _uploadFileService.UploadPunishmentFileAsync(input.File, filename);

                if (string.IsNullOrEmpty(filePath))
                    throw new UserFriendlyException("File upload failed.");

                punishment.FileName = filename;
                punishment.FilePath = filePath;
            }
            return await WorkScope.UpdateAsync(punishment);
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Timesheets_Punishment)]
        public async Task Delete(long id)
        {
            var punishment =  await WorkScope.GetAsync<Punishment>(id);
            await WorkScope.SoftDeleteAsync(punishment);
        }

        [HttpPost]
        [AbpAuthorize()]
        [AbpAuthorize(PermissionNames.Timesheets_Punishment)]
        public async Task<GridResult<GetPunishmentDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Punishment>().OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .Select(x => new GetPunishmentDto
                {
                    Id = x.Id,
                    Month = x.Month,
                    Year = x.Year,
                    FileName = x.FileName,
                    FilePath = x.FilePath,
                    Note = x.Note,
                });

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        [AbpAuthorize()]
        [AbpAuthorize(PermissionNames.Timesheets_Punishment)]
        public async Task<PunishmentDto> GetPunishment(long id)
        {
            var query = WorkScope.GetAll<Punishment>().Where(x => x.Id == id)
                                .Select(x => new PunishmentDto
                                {
                                    Id = x.Id,
                                    Month = x.Month,
                                    Year = x.Year,
                                    Note = x.Note,
                                });
            return await query.FirstOrDefaultAsync();
        }

        [HttpGet]
        [AbpAuthorize()]
        [AbpAuthorize(PermissionNames.Timesheets_Punishment)]
        public async Task<object> DownloadPunishmentFileAsync(long id)
        {
            var filePath = WorkScope.GetAll<Punishment>()
                .Where(s => s.Id == id)
                .Select(s => s.FilePath)
                .FirstOrDefault();

            if (filePath == null)
                throw new UserFriendlyException(String.Format("File path not found"));

            var data = await _uploadFileService.DownloadPunishmentFileAsync(filePath);

            var fileName = FileUtils.GetFileName(filePath);

            return new
            {
                FileName = fileName,
                Data = data
            };
        }

        private void ValidatePunishmentTemplate(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new UserFriendlyException("Uploaded file cannot be empty.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".xlsx" && extension != ".xls")
                throw new UserFriendlyException("Please upload a valid Excel file (.xlsx or .xls).");

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    if (worksheet == null || worksheet.Dimension == null)
                        throw new UserFriendlyException("The Excel file contains no data.");

                    var expectedHeaders = new[] { "STT", "PM", "EMAIL", "Date of punishment", "Reason", "Amount of fine" };

                    for (int i = 0; i < expectedHeaders.Length; i++)
                    {
                        var cellValue = worksheet.Cells[1, i + 1].Value?.ToString()?.Trim();
                        if (!string.Equals(cellValue, expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                            throw new UserFriendlyException($"Invalid file structure. Column {i + 1} must be '{expectedHeaders[i]}'.");
                    }
                }
            }
        }
    }
}
