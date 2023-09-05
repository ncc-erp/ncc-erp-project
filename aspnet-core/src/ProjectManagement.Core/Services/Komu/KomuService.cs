using Abp.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectManagement.Constants;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Utils;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Services.Komu
{
    public class KomuService : BaseWebService
    {
        private readonly string _channelIdDevMode;
        private bool _enableSendToKomu = false;
        private const string serviceName = "KomuService";
        private int MAX_LENGTH = 2000;

        public KomuService(
            HttpClient httpClient,
            ILogger<KomuService> logger,
            IConfiguration configuration,
            IAbpSession abpSession
        ) : base(httpClient, configuration, logger, abpSession, serviceName)
        {
            _channelIdDevMode = configuration.GetValue<string>($"{serviceName}:DevModeChannelId");
            var _isNotifyToKomu = configuration.GetValue<string>($"{serviceName}:EnableKomuNotification");
            _enableSendToKomu = _isNotifyToKomu == "true";
        }
        public async Task<long?> GetKomuUserId(KomuUserDto input)
        {
            var komuUser = await PostAsync<KomuUserDto>(ChannelTypeConstant.KOMU_USER, new { username = input.Username });
            if (komuUser != null)
                return komuUser.KomuUserId;

            return default;
        }

        public async Task<ulong?> GetKomuUserId(string userName)
        {
            var komuUser = await PostAsync<KomuUserDto>(KomuUrlConstant.KOMU_USERID, new { username = userName });
            if (komuUser != null)
                return (ulong?)komuUser.KomuUserId;

            return default;

        }

        public void NotifyToChannel(KomuMessage input, string channelType)
        {
            if (!_enableSendToKomu)
            {
                logger.LogInformation("_enableSendToKomu=false => stop");
            }

            if (!string.IsNullOrEmpty(_channelIdDevMode))
            {
                Post(ChannelTypeConstant.KOMU_CHANNELID, new { message = input.Message, channelid = _channelIdDevMode });
            }
            else
            {
                Post(channelType, input);
            }
        }

        public void NotifyToChannelId(KomuMessage input, string channelId)
        {
            if (!_enableSendToKomu)
            {
                logger.LogInformation("_enableSendToKomu=false => stop");
            }
            var channelIdToSend = string.IsNullOrEmpty(_channelIdDevMode) ? channelId : _channelIdDevMode;

            Post(ChannelTypeConstant.KOMU_CHANNELID, new { message = input.Message, channelid = channelIdToSend });

        }

        public async Task NotifyToChannelAwait(string[] arrMessage, string channelId)
        {
            if (_enableSendToKomu != true)
            {
                logger.LogInformation("_isNotifyToKomu=" + _enableSendToKomu + " => stop");
                return;
            }
            var listMessage = CommonUtil.SeparateMessage(arrMessage, MAX_LENGTH, "\n");
            foreach (var message in listMessage)
            {
                await PostAsync<dynamic>(KomuUrlConstant.KOMU_CHANNELID, new { message = message, channelid = channelId });
            }
        }

    }
}