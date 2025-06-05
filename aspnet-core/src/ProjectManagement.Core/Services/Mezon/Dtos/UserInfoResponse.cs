using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Mezon.Dtos
{
    public class MezonUser : UserInfoResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonProperty("mezon_id")]
        public string MezonId { get; set; }
    }

    public class UserInfoResponse
    {
        [JsonProperty("aud")]
        public List<string> Audience { get; set; }

        [JsonProperty("auth_time")]
        public long AuthTime { get; set; }

        [JsonProperty("iat")]
        public long IssuedAt { get; set; }

        [JsonProperty("iss")]
        public string Issuer { get; set; }

        [JsonProperty("rat")]
        public long RequestedAt { get; set; }

        [JsonProperty("sub")]
        public string Subject { get; set; }

        [JsonProperty("user_id")]
        public string MezonId { get; set; }

        public DateTime AuthTimeUtc => DateTimeOffset.FromUnixTimeSeconds(AuthTime).UtcDateTime;
        public DateTime IssuedAtUtc => DateTimeOffset.FromUnixTimeSeconds(IssuedAt).UtcDateTime;
        public DateTime RequestedAtUtc => DateTimeOffset.FromUnixTimeSeconds(RequestedAt).UtcDateTime;
    }
}
