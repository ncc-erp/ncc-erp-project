using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectFiles.Dto
{
    public class ReadProjectFileDto
    {
        public byte[] Bytes { get; set; }
        public string FileName { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
