using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProjectManagement.Services.Komu.KomuDto
{
    public class KomuMessage
    {
        [JsonProperty("pathImage")]
        public string PathImage { get; set; }
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
