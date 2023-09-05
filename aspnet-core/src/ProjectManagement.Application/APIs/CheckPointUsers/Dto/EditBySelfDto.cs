using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckPointUsers.Dto
{
    public class EditBySelfDto 
    {
        public long CheckPointUserId { get; set; }
        public int Score { get; set; }
        public string Note { get; set; }
    }
}
