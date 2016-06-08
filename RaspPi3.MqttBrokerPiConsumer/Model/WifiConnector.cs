using System;
using System.Linq;
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

        internal async static void ConnectToWifiIfPossibleAsync()
        {
            try
            {
                // It's sync because of debugging.
                if (await IsAllowedToAccessAsync())
                    InitializeWifiAdapterAsync();

                InitializeConnection();
                InitializeAvailableNetworkAsync();

                var passwordCredential = new PasswordCredential
                {
                    Password = connection.password
                };

                await wifiAdapter.ConnectAsync(network, connection.RecconectionKind, passwordCredential);
            }
            catch (Exception e)
            {
                var debug = e.Message;
            }
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
            await wifiAdapter.ScanAsync();
            network = wifiAdapter.NetworkReport.AvailableNetworks
                .FirstOrDefault(n => n.Ssid == connection.Ssid);
        }
    }
}
