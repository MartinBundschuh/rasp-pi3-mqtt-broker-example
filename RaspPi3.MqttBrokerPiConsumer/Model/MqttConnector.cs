using System;
using System.Collections.Generic;
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
        internal bool IsConnected { get { return mqttClient != null && mqttClient.IsConnected; } }

        internal MqttUser mqttUser;
        private MqttClient mqttClient;

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
            SubscribeToTopics(mqttUser.TopicsToSubscribe);
            mqttClient.Connect(mqttUser.ClientId, mqttUser.Name, mqttUser.Password);
        }

        private void AddEvents()
        {
            mqttClient.MqttMsgPublishReceived += (s, e) =>
            {
                LatestReceivedTopic = e.Topic;
                LatestReceivedMessage = GetTimeString() + Encoding.UTF8.GetString(e.Message);
            };
        }

        private void SubscribeToTopics(List<MqttTopic> topics)
        {
            mqttClient.Subscribe(topics.Select(t => t.Name).ToArray()
                , topics.Select(t => t.QualityOfService).ToArray());
        }

        internal void Publish<T>(MqttTopic topic, T messageToPublish) where T : IJsonConvertAble
        {
            mqttClient.Publish(topic.Name, JsonHandler.GetBytesFromObject(messageToPublish), topic.QualityOfService, true);
            LatestPublishedMessage = GetTimeString() + JsonHandler.GetStringFromObject(messageToPublish);
            LatestPublishedTopic = topic.Name;
        }

        internal void DisConnect()
        {
            // Oterhwise ReadOnly is not set properly.
            if (mqttClient != null)
                mqttClient.Disconnect();
            ClearMessageFields();

            // Seems to be an async function inside.
            while (mqttClient != null && mqttClient.IsConnected)
            {
            }
        }

        private void ClearMessageFields()
        {
            LatestPublishedMessage = string.Empty;
            LatestPublishedTopic = string.Empty;
            LatestReceivedMessage = string.Empty;
            LatestReceivedTopic = string.Empty;
        }

        private static string GetTimeString()
        {
            return string.Format("[{0}] ", DateTime.Now.TimeOfDay);
        }
    }
}
