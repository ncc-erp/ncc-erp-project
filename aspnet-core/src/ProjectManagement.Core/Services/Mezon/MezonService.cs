using Abp.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NccCore.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Services.Mezon.Dtos;

namespace ProjectManagement.Services.Mezon
{
    public class MezonService : BaseWebService
    {
        private const string serviceName = "MezonService";
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _baseAddress;

        public MezonService(HttpClient httpClient,
            IConfiguration configuration, 
            ILogger<MezonService> logger, 
            IAbpSession abpSession) : base(httpClient, configuration, logger, abpSession, serviceName)
        {
            _clientId = configuration.GetValue<string>($"{serviceName}:ClientId");
            _clientSecret = configuration.GetValue<string>($"{serviceName}:ClientSecret");
            _redirectUri = configuration.GetValue<string>($"{serviceName}:RedirectUri");
            _baseAddress = configuration.GetValue<string>($"{serviceName}:BaseAddress");
        }
        
        public async Task<OAuth2TokenResponse> GetTokenAsync(string token)
        {
            return await PostAsyncV2<OAuth2TokenResponse>("oauth2/token", new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", token },
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "redirect_uri", _redirectUri }
            });
        }

        public async Task<UserInfoResponse> GetUserInfoAsync(string accessToken)
        {
            return await PostAsyncV2<UserInfoResponse>($"{_baseAddress}/userinfo", null, accessToken);
        }

        public string GenerateOAuthUrl()
        {
            var state = Guid.NewGuid().ToString("N").Truncate(11);

            return $"{_baseAddress}/oauth2/auth?" +
                   $"client_id={_clientId}&" +
                   $"redirect_uri={Uri.EscapeDataString(_redirectUri)}&" +
                   $"response_type=code&" +
                   $"scope=openid+offline&" +
                   $"state={state}";
        }

        public MezonServiceConfig GetConfig()
        {
            return new MezonServiceConfig
            {
                ServiceName = serviceName,
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                RedirectUri = _redirectUri
            };
        }
    }
}
