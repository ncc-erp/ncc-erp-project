using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Talent.Dtos
{
    public class BranchDto : Entity<long>
    {
        public string Name { get; set; }
    }
}
