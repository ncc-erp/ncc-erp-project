using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class OtTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Hours { get; set; }
    }
}
