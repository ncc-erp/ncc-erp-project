using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectManagement.APIs.Projects.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.APIPublic
{
    public class APIPublicAppService : ProjectManagementAppServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _secretKey;
        public APIPublicAppService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _secretKey = configuration["Authentication:SecretKey"];
        }
        private bool IsSecretKeyValid()
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("X-Secret-Key", out var secretKeyFromHeader) &&
                string.Equals(secretKeyFromHeader, _secretKey, StringComparison.InvariantCulture))
            {
                return true;
            }

            return false;
        }
        [HttpGet]
        public async Task<ActionResult<List<GetProjectDto>>> GetAllProject()
        {
            if (!IsSecretKeyValid())
            {
                return new UnauthorizedResult();
            }
            var query = WorkScope.GetAll<Project>()
                .Where(p => p.ProjectType != ProjectType.TRAINING && p.ProjectType != ProjectType.PRODUCT)
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.Name,

                });
            var result = await query.ToListAsync();
            return new OkObjectResult(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllClient()
        {
            if (!IsSecretKeyValid())
            {
                return new UnauthorizedResult();
            }
            var clients = await WorkScope.GetAll<Client>().Select(
                x => new
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return new OkObjectResult(clients);
        }
    }
}
