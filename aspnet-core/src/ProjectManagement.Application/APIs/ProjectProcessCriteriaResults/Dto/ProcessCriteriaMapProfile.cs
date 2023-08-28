using AutoMapper;
using ProjectManagement.APIs.ProcessCriterias.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectProcessCriteriaResults.Dto
{
    public class ProcessCriteriaMapProfile:Profile
    {
        public ProcessCriteriaMapProfile()
        {
            CreateMap<ProcessCriteria, GetProcessCriteriaDto>();
        }
    }
}
