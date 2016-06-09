using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using Windows.Security.Credentials;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    internal static class WifiConnector
    {
        private static WiFiAdapter wifiAdapter;
        private static WiFiAvailableNetwork network;
        private static WifiConnection connection;

        internal async static void ConnectToWifiIfNeededAsync()
        {
            if (IsWifiConnectionNeeded() && await IsAllowedToAccessAsync())
                ConnectToWifiIfPossibleAsync();
        }

        private static bool IsWifiConnectionNeeded()
        {
            return !NetworkInterface.GetIsNetworkAvailable();
        }

        private static async void ConnectToWifiIfPossibleAsync()
        {
            InitializeWifiAdapterAsync();
            InitializeConnection();
            InitializeAvailableNetworkAsync();

            var passwordCredential = new PasswordCredential
            {
                Password = connection.password
            };

            await wifiAdapter.ConnectAsync(network, connection.RecconectionKind, passwordCredential);
        }

        private static async Task<bool> IsAllowedToAccessAsync()
        {
            var access = await WiFiAdapter.RequestAccessAsync();
            return access == WiFiAccessStatus.Allowed;
        }

        private static void InitializeConnection()
        {
            using (var db = new SqLiteHandler())
            {
                connection = db.Select<WifiConnection>()
                    .FirstOrDefault(w => w.Ssid == "ti8m-IoT");
            }
        }

        private static async void InitializeWifiAdapterAsync()
        {
            var wifiAdapters = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
            if (wifiAdapters.Count > 0)
                wifiAdapter = await WiFiAdapter.FromIdAsync(wifiAdapters[0].Id);
        }

        private static async void InitializeAvailableNetworkAsync()
        {
            if (wifiAdapter != null)
            {
                await wifiAdapter.ScanAsync();
                network = wifiAdapter.NetworkReport.AvailableNetworks
                    .FirstOrDefault(n => n.Ssid == connection.Ssid);
            }
        }
    }
}
