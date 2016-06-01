using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using static RaspPi3.MqttBrokerPiConsumer.Model.MqttConnection;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    internal class MqttConnector
    {
        internal string LatestPublishedTopic = string.Empty;
        internal string LatestPublishedMessage = string.Empty;
        internal string LatestReceivedTopic = string.Empty;
        internal string LatestReceivedMessage = string.Empty;

        private MqttClient mqttClient;
        internal MqttUser mqttUser;
        private readonly MqttConnection mqttConnection;

        internal MqttConnector()
        {
            //mqttUser = new MqttUser("ti8mRaspPi3");
            var newConnection = new MqttConnection
            {
                BrokerName = "m21.cloudmqtt.com",
                BrokerPort = CloudMqttBroker.Ssl,
                MqttSslProtocols = MqttSslProtocols.SSLv3,
                IsSecureConnection = true
            };

            mqttUser = new MqttUser
            {
                UserName = "ti8mRaspPi3",
                ClientId = Guid.NewGuid().ToString(),
                Password = "ti8m",
                Connection = newConnection
            };

            mqttUser.TopicsToSubscribe = mqttUser.GetTopicsToSubscribe();
            mqttConnection = newConnection;
        }

        internal void Connect()
        {
            int port;
            if (!int.TryParse(mqttConnection.BrokerPort.ToString("d"), out port))
                throw new InvalidCastException("No valid Port given.");

            mqttClient = new MqttClient(mqttConnection.BrokerName, port, mqttConnection.IsSecureConnection, mqttConnection.MqttSslProtocols);
            AddEvents();
            SubscribeToTopics();
            mqttClient.Connect(mqttUser.ClientId, mqttUser.UserName, mqttUser.Password);
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
            // TODO: Get all relevant Topics
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

            // Seems to be an asynchronous function inside.
            while (mqttClient != null && mqttClient.IsConnected) { }
        }

        private void ClearMessageFields()
        {
            LatestPublishedMessage = string.Empty;
            LatestPublishedTopic = string.Empty;
            LatestReceivedMessage = string.Empty;
            LatestReceivedTopic = string.Empty;
        }

        public bool IsConnected { get { return mqttClient != null && mqttClient.IsConnected; } }
    }
}
