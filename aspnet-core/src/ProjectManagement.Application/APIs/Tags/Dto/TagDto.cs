using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Tags.Dto
{
    [AutoMapTo(typeof(Tag))]
    public class TagDto:EntityDto<long>
    {
        public string Name { get; set; }
    }
}
