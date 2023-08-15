using Abp.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.ProjectFiles.Dto;
using ProjectManagement.APIs.Projects.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectFiles
{
    [AbpAuthorize]
    public class ProjectFileAppService : ProjectManagementAppServiceBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProjectFileAppService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile)]
        [HttpPost]
        public async Task<IActionResult> UploadFiles([FromForm] ProjectInputFileDto input)
        {
            try
            {
                var projectFilePaths = await WorkScope.GetAll<ProjectFile>().Where(x => x.ProjectId == input.ProjectId).Select(x => x.FilePath).ToListAsync();

                if (input.Files == null)
                {
                    if (projectFilePaths.Count() > 0)
                    {
                        foreach (string fileName in projectFilePaths)
                        {
                            await DeleteProjectFile(fileName, input.ProjectId);
                        }
                    }
                    return new OkObjectResult("Success");
                }

                List<string> listFileDeletes = projectFilePaths.Except(input.Files.Select(x => x.FileName)).ToList();
                List<string> listFileAddNews = input.Files.Select(x => x.FileName).Except(projectFilePaths).ToList();
                string pathFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "projects");
                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }
                foreach (var fileName in listFileAddNews)
                {
                    await AddProjectFile(fileName, input.ProjectId, input.Files, pathFolder);
                }

                foreach (var fileName in listFileDeletes)
                {
                    await DeleteProjectFile(fileName, input.ProjectId);
                }

                return new OkObjectResult("Success");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_DeleteFile,
          PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_DeleteFile,
          PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_DeleteFile)]
        private async Task DeleteProjectFile(string fileName, long projectId)
        {
            string pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "projects", fileName);
            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
                var item = WorkScope.GetAll<ProjectFile>().Where(x => x.ProjectId == projectId && x.FilePath == fileName).FirstOrDefault();
                await WorkScope.DeleteAsync(item);
            }
        }
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile_UploadFile,
         PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile_UploadFile,
         PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile_UploadFile)]
        private async Task AddProjectFile(string fileName, long projectId, List<IFormFile> files, string pathFolder)
        {
            string pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "projects", fileName);
            var file = files.Where(x => x.FileName.Equals(fileName)).FirstOrDefault();
            string filePath = DateTimeOffset.Now.ToUnixTimeMilliseconds() + "_" + file.FileName;
            var fileCreate = Path.Combine(pathFolder, filePath);
            using (var stream = System.IO.File.Create(fileCreate))
            {
                await file.CopyToAsync(stream);
            }

            var fileId = await WorkScope.InsertAndGetIdAsync(new ProjectFile
            {
                ProjectId = projectId,
                FilePath = filePath
            });
        }
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile,
         PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile,
         PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile)]
        public async Task<IActionResult> GetFiles(long projectId)
        {
            try
            {
                var projectFiles = await WorkScope.GetAll<ProjectFile>().Where(x => x.ProjectId == projectId).ToListAsync();
                List<ReadProjectFileDto> files = new List<ReadProjectFileDto>();
                foreach (var item in projectFiles)
                {
                    String path = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "projects", item.FilePath);
                    if (File.Exists(path))
                    {
                        ReadProjectFileDto data = new ReadProjectFileDto();
                        data.Bytes = File.ReadAllBytes(path);
                        data.FileName = Path.GetFileName(path);
                        data.CreationTime = item.CreationTime;
                        files.Add(data);
                    }
                }
                return new OkObjectResult(files.OrderByDescending(x => x.CreationTime));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
        }
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabProjectFile,
          PermissionNames.Projects_ProductProjects_ProjectDetail_TabProjectFile,
          PermissionNames.Projects_TrainingProjects_ProjectDetail_TabProjectFile)]
        public async Task DeleteFile(string fileName, long projectId)
        {
            String pathFile = Path.Combine(_hostingEnvironment.ContentRootPath, "Uploads", "projects", fileName);
            if (File.Exists(pathFile))
            {
                File.Delete(pathFile);
                var item = WorkScope.GetAll<ProjectFile>().Where(x => x.ProjectId == projectId && x.FilePath == fileName).FirstOrDefault();
                await WorkScope.DeleteAsync(item);
            }
        }
    }
}
