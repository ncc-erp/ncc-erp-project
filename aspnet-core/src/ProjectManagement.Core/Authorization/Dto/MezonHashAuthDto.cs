using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProjectManagement.Authorization.Dto
{
    public class MezonHashAuthDto
    {
        [JsonProperty("hashData")]
        public string HashData { get; set; }
        [JsonProperty("tenancyName")]
        public string TenancyName { get; set; } = "NCC";
    }

    public class BaseHashData
    {
        public string query_id { get; set; }
        public string user { get; set; }
        public long auth_date { get; set; }
        public string signature { get; set; }
    }
    public class HashData : BaseHashData
    {
        public string hash { get; set; }
    }
}
