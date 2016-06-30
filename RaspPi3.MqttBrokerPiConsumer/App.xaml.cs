using Okra;
using RaspPi3.MqttBrokerPiConsumer.Model;
using System;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt.Messages;
using Windows.Devices.WiFi;

[assembly: CLSCompliant(false)]
namespace RaspPi3.MqttBrokerPiConsumer
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : OkraApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
            : base(new AppBootstrapper())
        {
            InitializeComponent();
            Task.Run(() => SetupSqLiteAsync(false));
            Task.Run(() => WifiConnector.ConnectToWifiIfNeededAsync());
        }

        private async static Task SetupSqLiteAsync(bool runInitialDataSave)
        {
            using (var sqLiteHandler = new SqLiteHandler())
                sqLiteHandler.SyncDataTables();

            if (runInitialDataSave)
                await InsertFirstSetupDataAsync();
        }

        private static async Task InsertFirstSetupDataAsync()
        {
            // TODO: Set up your WiFi connection info.
            var wifi = new WifiConnection
            {
                Ssid = "ti8m-IoT",
                Password = "****",
                RecconectionKind = WiFiReconnectionKind.Automatic
            };

            var newConnection = new MqttConnection
            {
                BrokerName = "m21.cloudmqtt.com",
                BrokerPort = CloudMqttBrokerPort.Ssl,
                SslProtocol = MqttProtocol.SslLevel3,
                IsSecureConnection = true
            };

            // TODO: Set up your MQTT Broker login data.
            var newMqttUser = new MqttUser
            {
                Name = "ti8mRaspPi3",
                ClientId = Guid.NewGuid().ToString(),
                BrokerName = newConnection.BrokerName,
                Password = "****"
            };

            // TODO: Set up yout WebApi login info.
            var newWebApiUser = new WebApiUser
            {
                Name = newMqttUser.Name,
                Email = "userMail@provider.ch",
                Password = "****",
                BaseUrl = "https://rasppi3webapi20160622020016.azurewebsites.net/"
            };

            // TODO: Register for some Topics
            var newMqttTopic = new MqttTopic
            {
                UserName = newMqttUser.Name,
                Name = "TestChannel",
                AccessMode = ChannelAccessMode.ReadWrite,
                QualityOfService = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE
            };

            using (var db = new SqLiteHandler())
            {
                db.WifiConnections.Add(wifi);
                db.MqttConnections.Add(newConnection);
                db.MqttUsers.Add(newMqttUser);
                db.MqttTopics.Add(newMqttTopic);
                db.WebApiUsers.Add(newWebApiUser);
                await db.SaveChangesAsync();
            }
        }
    }
}
