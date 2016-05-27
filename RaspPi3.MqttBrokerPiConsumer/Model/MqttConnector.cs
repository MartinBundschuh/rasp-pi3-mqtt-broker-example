using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    class MqttConnector
    {
        internal string BrokerName = "m21.cloudmqtt.com";
        private enum CloudMqttBroker { Default = 11599, Ssl = 21599, Tsl = 31599 }
        internal string BrokerPort = CloudMqttBroker.Ssl.ToString("d");

        private MqttClient mqttClient;
        private const bool SECURE = true;
        private const MqttSslProtocols MQTTSLPROTOCOLS = MqttSslProtocols.SSLv3;

        internal void Connect()
        {
            int port;
            int.TryParse(BrokerPort, out port);

            mqttClient = new MqttClient(BrokerName, port, SECURE, MQTTSLPROTOCOLS);
            mqttClient.Connect(MqttUser.ClientId, MqttUser.UserName, MqttUser.Password);
        }

        internal void DisConnect()
        {
            // Oterhwise ReadOnly is not set properly.
            mqttClient.Disconnect();

            while (mqttClient != null && mqttClient.IsConnected) { }
        }

        public bool IsConnected { get { return mqttClient != null && mqttClient.IsConnected; } }
    }
}
