using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Mezon.Dtos
{
    public class MezonServiceConfig
    {
        public string ServiceName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RedirectUri { get; set; }
    }
}
