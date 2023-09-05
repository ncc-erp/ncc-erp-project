using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services
{
    public class AbpResponseResult<T> where T : class
    {
        [JsonProperty("result")]
        public T Result { get; set; }
        [JsonProperty("targetUrl")]
        public string TargetUrl { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("unAuthorizedRequest")]
        public bool UnAuthorizeRequest { get; set; }
        [JsonProperty("__abp")]
        public bool Abp { get; set; }
    }
}
