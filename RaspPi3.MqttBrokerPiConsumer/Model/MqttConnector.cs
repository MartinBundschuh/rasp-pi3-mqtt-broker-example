using System;
using System.Linq;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    internal class MqttConnector
    {
        internal string LatestPublishedTopic = string.Empty;
        internal string LatestPublishedMessage = string.Empty;
        internal string LatestReceivedTopic = string.Empty;
        internal string LatestReceivedMessage = string.Empty;
        public bool IsConnected { get { return mqttClient != null && mqttClient.IsConnected; } }

        private MqttClient mqttClient;
        internal MqttUser mqttUser;

        internal MqttConnector()
        {
            using (var db = new SqLiteHandler())
            {
                var connectionQuery = db.Select<MqttConnection>();
                var mqttConnection = connectionQuery.FirstOrDefault(c => c.BrokerName == "m21.cloudmqtt.com");

                var userQuery = db.Select<MqttUser>();
                mqttUser = userQuery.FirstOrDefault(u => u.BrokerName == mqttConnection.BrokerName);
                mqttUser.Connection = mqttConnection;
            }
        }

        internal void Connect()
        {
            int port;
            if (!int.TryParse(mqttUser.Connection.BrokerPort.ToString("d"), out port))
                throw new InvalidCastException("No valid Port given.");

            mqttClient = new MqttClient(mqttUser.Connection.BrokerName, port,
                mqttUser.Connection.IsSecureConnection, mqttUser.Connection.SslProtocol);
            AddEvents();
            SubscribeToTopics();
            mqttClient.Connect(mqttUser.ClientId, mqttUser.Name, mqttUser.Password);
        }

        private void AddEvents()
        {
            mqttClient.MqttMsgPublishReceived += (s, e) =>
            {
                LatestReceivedTopic = e.Topic;
                LatestReceivedMessage = Encoding.UTF8.GetString(e.Message);
            };
        }

        private void SubscribeToTopics()
        {
            foreach (var topic in mqttUser.TopicsToSubscribe)
                Subscribe(topic);
        }

        private void Subscribe(MqttTopic topic)
        {
            mqttClient.Subscribe(new string[] { topic.Name }, new byte[] { topic.QualityOfService });
        }

        internal void Publish(MqttTopic topic, string messageToPublish)
        {
            mqttClient.Publish(topic.Name, Encoding.UTF8.GetBytes(messageToPublish), topic.QualityOfService, true);
            LatestPublishedMessage = messageToPublish;
            LatestPublishedTopic = topic.Name;
        }

        internal void DisConnect()
        {
            // Oterhwise ReadOnly is not set properly.
            if (mqttClient != null)
                mqttClient.Disconnect();
            ClearMessageFields();

            // Seems to be an async function inside.
            while (mqttClient != null && mqttClient.IsConnected) { }
        }

        private void ClearMessageFields()
        {
            LatestPublishedMessage = string.Empty;
            LatestPublishedTopic = string.Empty;
            LatestReceivedMessage = string.Empty;
            LatestReceivedTopic = string.Empty;
        }
    }
}
