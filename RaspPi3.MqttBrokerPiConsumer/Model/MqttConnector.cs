using System;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    class MqttConnector
    {
        internal string BrokerName = "m21.cloudmqtt.com";
        internal string BrokerPort = "11599";

        private MqttClient mqttClient;
        private const bool SECURE = false;
        private const MqttSslProtocols MQTTSLPROTOCOLS = MqttSslProtocols.None;

        public MqttConnector()
        {

        }

        internal void Connect()
        {
            int port;
            int.TryParse(BrokerPort, out port);

            try
            {
                mqttClient = new MqttClient(BrokerName, port, SECURE, MQTTSLPROTOCOLS);
                mqttClient.Connect(MqttUser.ClientId, MqttUser.UserName, MqttUser.Password);
            }
            catch (Exception)
            {
                return;
            }
        }

        internal void DisConnect()
        {
            try
            {
                mqttClient.Disconnect();
            }
            catch (Exception)
            {
                return;
            }
        }

        public bool IsConnected { get { return mqttClient.IsConnected; } }
    }
}
