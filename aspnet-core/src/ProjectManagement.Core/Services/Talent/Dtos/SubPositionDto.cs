using Abp.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Talent.Dtos
{
    public class DropdownPositionDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("items")]
        public List<DropdownSubPositionDto> Items { get; set; }
    }
    public class DropdownSubPositionDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("subPosition")]
        public string SubPosition { get; set; }
    }
}
