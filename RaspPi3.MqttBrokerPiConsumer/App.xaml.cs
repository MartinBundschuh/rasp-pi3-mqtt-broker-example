using Okra;
using RaspPi3.MqttBrokerPiConsumer.Model;
using System;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
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

            if (true)
                FirstSetup();
        }

        private static void FirstSetup()
        {
            using (var sqLiteHandler = new SqLiteHandler())
            {
                sqLiteHandler.DropAllTables();
                sqLiteHandler.CreateAllTables();
            }

            InsertFirstSetupData();
        }

        private static void InsertFirstSetupData()
        {
            var newConnection = new MqttConnection
            {
                BrokerName = "m21.cloudmqtt.com",
                BrokerPort = CloudMqttBrokerPort.Ssl,
                SslProtocol = MqttSslProtocols.SSLv3,
                IsSecureConnection = true
            };

            var newMqttUser = new MqttUser
            {
                Name = "ti8mRaspPi3",
                ClientId = Guid.NewGuid().ToString(),
                BrokerName = newConnection.BrokerName,
                Password = "ti8m"
            };

            var newMqttTopic = new MqttTopic
            {
                UserName = newMqttUser.Name,
                Name = "TestChannel",
                AcessMode = ChannelAccesMode.ReadWrite,
                QualityOfService = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE
            };

            using (var db = new SqLiteHandler())
            {
                db.MqttConnections.Add(newConnection);
                db.MqttUsers.Add(newMqttUser);
                db.MqttTopics.Add(newMqttTopic);
                Task.Run(() => db.SaveChangesAsync());
            }
        }
    }
}
