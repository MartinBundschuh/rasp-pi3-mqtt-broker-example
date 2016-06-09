using Okra;
using RaspPi3.MqttBrokerPiConsumer.Model;
using System;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Windows.Devices.WiFi;
using static RaspPi3.MqttBrokerPiConsumer.Model.MqttConnection;
using static RaspPi3.MqttBrokerPiConsumer.Model.MqttTopic;

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

            // TODO: Run this method for first device setup.
            SetupSqLite(false);
            WifiConnector.ConnectToWifiIfNeededAsync();
        }

        private static void SetupSqLite(bool runInitialDataSave)
        {
            using (var sqLiteHandler = new SqLiteHandler())
                sqLiteHandler.SyncDataTables();

            if (runInitialDataSave)
                InsertFirstSetupData();
        }

        private static void InsertFirstSetupData()
        {
            // TODO: Set up your WiFI connection info.
            var wifi = new WifiConnection
            {
                Ssid = "ti8m-IoT",
                password = "****",
                RecconectionKind = WiFiReconnectionKind.Automatic
            };

            var newConnection = new MqttConnection
            {
                BrokerName = "m21.cloudmqtt.com",
                BrokerPort = CloudMqttBrokerPort.Ssl,
                SslProtocol = MqttSslProtocols.SSLv3,
                IsSecureConnection = true
            };

            // TODO: Set up your Mqttbroker login data.
            var newMqttUser = new MqttUser
            {
                Name = "ti8mRaspPi3",
                ClientId = Guid.NewGuid().ToString(),
                BrokerName = newConnection.BrokerName,
                Password = "****"
            };

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
                Task.Run(() => db.SaveChangesAsync());
            }
        }
    }
}
