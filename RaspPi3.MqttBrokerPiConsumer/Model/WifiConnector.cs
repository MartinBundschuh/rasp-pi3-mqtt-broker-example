using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Devices.WiFi;
using Windows.Networking.Connectivity;
using Windows.Security.Credentials;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    internal static class WifiConnector
    {
        private static WiFiAdapter wifiAdapter;
        private static WiFiAvailableNetwork network;
        private static WifiConnection connection;

        internal async static Task ConnectToWifiIfNeededAsync()
        {
            if (IsWifiConnectionNeeded() && await IsAllowedToAccessAsync())
                await ConnectToWifiIfPossibleAsync();
        }

        private static bool IsWifiConnectionNeeded()
        {
            return Debugger.IsAttached ? true : !NetworkInterface.GetIsNetworkAvailable();
        }

        private static async Task ConnectToWifiIfPossibleAsync()
        {
            try
            {
                await InitializeWifiAdapterAsync();
                InitializeConnection();
                await InitializeAvailableNetworkAsync();

                var passwordCredential = new PasswordCredential
                {
                    Password = connection.password
                };

                await wifiAdapter.ConnectAsync(network, connection.RecconectionKind, passwordCredential);
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached)
                    throw new Exception("WifiError", e);
            }
        }

        private static async Task<bool> IsAllowedToAccessAsync()
        {
            return await WiFiAdapter.RequestAccessAsync() == WiFiAccessStatus.Allowed;
        }

        private static void InitializeConnection()
        {
            using (var db = new SqLiteHandler())
            {
                connection = db.Select<WifiConnection>()
                    .FirstOrDefault(w => w.Ssid == "ti8m-IoT");
            }
        }

        private static async Task InitializeWifiAdapterAsync()
        {
            var wifiAdapters = await WiFiAdapter.FindAllAdaptersAsync();
            wifiAdapter = wifiAdapters.FirstOrDefault();
            if (wifiAdapter == null)
                throw new Exception("No WiFi Adapter could be found.");
        }

        private static async Task InitializeAvailableNetworkAsync()
        {
            if (wifiAdapter != null)
            {
                await wifiAdapter.ScanAsync();
                network = wifiAdapter.NetworkReport.AvailableNetworks
                    .FirstOrDefault(n => n.Ssid == connection.Ssid);
                if (network == null)
                    throw new Exception("No Network could be found.");
            }
        }

        internal static void DisconnectIfConnected()
        {
            if (IsConnected())
                wifiAdapter.Disconnect();
        }

        internal static bool IsConnected()
        {
            var con = NetworkInformation.GetInternetConnectionProfile();
            return con != null && con.IsWlanConnectionProfile && wifiAdapter != null;
        }
    }
}
