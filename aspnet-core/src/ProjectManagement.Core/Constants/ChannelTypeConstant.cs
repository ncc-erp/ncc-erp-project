using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Constants
{
    public static class ChannelTypeConstant
    {
        public const string PM_CHANNEL = "sendMessageToThongBaoPM";
        public const string GENERAL_CHANNEL = "sendMessageToThongBao";
        public const string USER_ONLY = "sendMessageToUser";
        public const string KOMU_USER = "getUserIdByUsername";
        public const string KOMU_CHANNELID = "sendMessageToChannel";
    }
}
