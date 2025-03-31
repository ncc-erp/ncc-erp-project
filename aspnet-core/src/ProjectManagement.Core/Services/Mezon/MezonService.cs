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
using Amazon.Runtime.Internal;
/*using ProjectManagement.WebServices.ExternalServices.Mezon.Dtos;*/
namespace ProjectManagement.Services.Mezon
{
    public class MezonService : BaseWebService
    {
        private const string serviceName = "MezonService";
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _redirectUri;
        private readonly string _baseAddress;
        private readonly string _appToken;
        private readonly string _appId;

        public MezonService(HttpClient httpClient,
            IConfiguration configuration, 
            ILogger<MezonService> logger, 
            IAbpSession abpSession) : base(httpClient, configuration, logger, abpSession, serviceName)
        {
            _clientId = configuration.GetValue<string>($"{serviceName}:ClientId");
            _clientSecret = configuration.GetValue<string>($"{serviceName}:ClientSecret");
            _redirectUri = configuration.GetValue<string>($"{serviceName}:RedirectUri");
            _baseAddress = configuration.GetValue<string>($"{serviceName}:BaseAddress");
            _appToken = configuration.GetValue<string>($"{serviceName}:AppToken");
            _appId = configuration.GetValue<string>($"{serviceName}:AppId");
        }
        
        public async Task<OAuth2TokenResponse> GetTokenAsync(OAuth2Request request)
        {
            return await PostAsync<OAuth2TokenResponse>("oauth2/token", new Dictionary<string, string>()
            {
                { "grant_type", "authorization_code" },
                { "code", request.Code },
                { "scope", request.Scope },
                { "state", request.State },
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

            return $"{HttpClient.BaseAddress}/oauth2/auth?" +
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
                RedirectUri = _redirectUri,
                AppId = _appId,
                AppToken = _appToken
            };
        }
    }
}
