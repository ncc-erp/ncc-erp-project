using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services
{
    public class BaseResponseDto
    {
        public bool Success { get; set; }
        public string Result { get; set; }
    }
}
