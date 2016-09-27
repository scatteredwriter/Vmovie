using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.System.Profile;

namespace V电影.Model
{
    public class DeviceInfo
    {
        private DeviceNetworkType _network_type;
        private DeviceType _device_type;

        public DeviceNetworkType Network_Type
        {
            get
            {
                return _network_type;
            }
            set
            {
                _network_type = value;
            }
        }

        public DeviceType Device_type
        {
            get
            {
                return _device_type;
            }
            set
            {
                _device_type = value;
            }
        }

        public DeviceInfo()
        {
            bool isConnected = false;

            Network_Type = DeviceNetworkType.Unkonwn;
            Device_type = DeviceType.Unknown;
            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile == null)
            {
                Network_Type = DeviceNetworkType.None;
            }
            else
            {
                NetworkConnectivityLevel cl = profile.GetNetworkConnectivityLevel();
                isConnected = (cl != NetworkConnectivityLevel.None);
            }
            if (!isConnected)
            {
                Network_Type = DeviceNetworkType.None;
            }
            if (profile.IsWwanConnectionProfile)
            {
                if (profile.WwanConnectionProfileDetails == null)
                {
                    Network_Type = DeviceNetworkType.Unkonwn;
                }
                WwanDataClass connectionClass = profile.WwanConnectionProfileDetails.GetCurrentDataClass();
                switch (connectionClass)
                {
                    //2G
                    case WwanDataClass.Edge:
                    case WwanDataClass.Gprs:
                        Network_Type = DeviceNetworkType.IIG;
                        break;
                    //3G
                    case WwanDataClass.Cdma1xEvdo:
                    case WwanDataClass.Cdma1xEvdoRevA:
                    case WwanDataClass.Cdma1xEvdoRevB:
                    case WwanDataClass.Cdma1xEvdv:
                    case WwanDataClass.Cdma1xRtt:
                    case WwanDataClass.Cdma3xRtt:
                    case WwanDataClass.CdmaUmb:
                    case WwanDataClass.Umts:
                    case WwanDataClass.Hsdpa:
                    case WwanDataClass.Hsupa:
                        Network_Type = DeviceNetworkType.IIIG;
                        break;
                    //4G
                    case WwanDataClass.LteAdvanced:
                        Network_Type = DeviceNetworkType.IVG;
                        break;
                    //无网
                    case WwanDataClass.None:
                        Network_Type = DeviceNetworkType.None;
                        break;
                    case WwanDataClass.Custom:
                    default:
                        Network_Type = DeviceNetworkType.Unkonwn;
                        break;
                }
            }
            else if (profile.IsWlanConnectionProfile)
            {
                Network_Type = DeviceNetworkType.WIFI;
            }
            else
            {
                //不是Wifi也不是蜂窝数据判断为Lan
                Network_Type = DeviceNetworkType.LAN;
            }

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
            {
                Device_type = DeviceType.Mobile;
            }
            else if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
            {
                Device_type = DeviceType.PC;
            }
            else
            {
                Device_type = DeviceType.Other;
            }
        }
    }

    public enum DeviceNetworkType
    {
        WIFI = 0,
        IIG = 1,
        IIIG = 2,
        IVG = 3,
        LAN = 4,
        None = 5,
        Unkonwn = 6
    };

    public enum DeviceType
    {
        PC = 0,
        Mobile = 1,
        Other = 2,
        Unknown = 3
    };
}
