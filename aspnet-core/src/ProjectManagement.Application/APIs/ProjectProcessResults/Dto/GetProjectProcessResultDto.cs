using Abp.AutoMapper;
using Abp.Configuration;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using ProjectManagement.Configuration.Dto;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    public class GetProjectProcessResultDto
    {
        public class GetAllProjectProcessResultDto
        {
            public string ProjectName { get; set; }
            public string ProjectCode { get; set; }
            public string PMName { get; set; }
            public long ProjectId { get; set; }

            public ProjectType ProjectType { get; set; }
            public string ClientName { get; set; }
            public List<AuditInfo> AuditInfos { get; set; }
        }

        public class InputToGetAll
        {
            public ProjectScoreKPIStatus? Status { get; set; }
            public long? ProjectId { get; set; }
            public string SearchText { get; set; }
        }

        public class AuditInfo
        {
            public long Id { get; set; }
            public string Note { get; set; }
            public DateTime AuditDate { get; set; }
            public int Score { get; set; }
            public ProjectScoreKPIStatus Status { get; set; }
            public string PMName { get; set; }
        }

        public class ImportFileDto
        {
            public IFormFile File { get; set; }
            public long ProjectId { get; set; }
            public string Note { get; set; }
            public DateTime AuditDate { get; set; }
        }

        public class ResponseFailDto
        {
            public int Row { get; set; }
            public string ReasonFail { get; set; }
        }

        public class UpDateProjectProcessResultDto
        {
            public long Id { get; set; }
            public string Note { get; set; }
            public DateTime AuditDate { get; set; }
        }

        public class UpDateProjectProcessCriteriaResultDto
        {
            public long Id { get; set; }
            public string Note { get; set; }
            public NCStatus Status { get; set; }
            public int Score { get; set; }

        }


    

    }
}