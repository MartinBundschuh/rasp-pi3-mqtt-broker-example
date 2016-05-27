using System;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    class MqttConnector
    {
        internal string BrokerName = "m21.cloudmqtt.com";
        internal string BrokerPort = "21599"; //11599

        private MqttClient mqttClient;
        private const bool SECURE = true;
        private const MqttSslProtocols MQTTSLPROTOCOLS = MqttSslProtocols.SSLv3;

        internal void Connect()
        {
            int port;
            int.TryParse(BrokerPort, out port);

            try
            {
                mqttClient = new MqttClient(BrokerName, port, SECURE, MQTTSLPROTOCOLS);
                mqttClient.Connect(MqttUser.ClientId, MqttUser.UserName, MqttUser.Password);
            }
            catch (Exception e)
            {
                var debugMessage = e.Message;
                return;
            }
        }

        internal void DisConnect()
        {
            try
            {
                mqttClient.Disconnect();
            }
            catch (Exception e)
            {
                var debugMessage = e.Message;
                return;
            }
        }

        public bool IsConnected { get { return mqttClient.IsConnected; } }
    }
}
