using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class SubProjectInvoiceDto
    {
        public long Id { get; set; } 
        public string Name { get; set; } 
        public string Code { get; set; } 
        public Constants.Enum.ProjectEnum.ProjectType ProjectType { get; set; }
    }
}
