using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckPointUserResults.Dto
{
    public class EditMainDto
    {
        public long CheckPointUserResultId { get; set; }
        public string FinalNote { get; set; }
        public UserLevel Now { get; set; }
        public List<long> TagIds { get; set; }
    }
}
