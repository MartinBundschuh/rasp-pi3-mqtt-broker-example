using Okra;
using RaspPi3.MqttBrokerPiConsumer.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using static RaspPi3.MqttBrokerPiConsumer.Model.MqttConnection;

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

            using (var sqLiteHandler = new SqLiteHandler())
                sqLiteHandler.CreateTables(TypeAttribute.GetTypeAttributes());

            var newConnection = new MqttConnection
            {
                BrokerName = "m21.cloudmqtt.com",
                BrokerPort = CloudMqttBroker.Ssl,
                MqttSslProtocols = MqttSslProtocols.SSLv3,
                IsSecureConnection = true
            };

            MqttConnection selectCon;
            using (var db = new SqLiteHandler())
            {
                db.MqttConnections.Add(newConnection);
                Task.Run(() => db.SaveChangesAsync());

                // working, but save not done at this moment since it's async
                var borkerQuery = db.SelectAsync<MqttConnection>();
                selectCon = borkerQuery.Result.FirstOrDefault(c => c.BrokerName == "m21.cloudmqtt.com");
            }
        }
    }
}
