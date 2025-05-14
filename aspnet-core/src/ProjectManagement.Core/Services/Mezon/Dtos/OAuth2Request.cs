using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Mezon.Dtos
{
    public class OAuth2Request
    {
        public string Code { get; set; }
        public string Scope { get; set; }
        public string State { get; set; }
    }
}
