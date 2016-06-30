using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        internal readonly MqttUser mqttUser;
        private MqttClient mqttClient;
        private ApiHandler apiHandler;

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

            apiHandler = new ApiHandler();
        }

        internal void Connect()
        {
            int port;
            if (!int.TryParse(mqttUser.Connection.BrokerPort.ToString("d"), out port))
                throw new InvalidCastException("No valid Port given.");

            mqttClient = new MqttClient(mqttUser.Connection.BrokerName, port,
                mqttUser.Connection.IsSecureConnection, (MqttSslProtocols)mqttUser.Connection.SslProtocol);

            AddEvents();
            SubscribeToTopics(mqttUser.TopicsToSubscribe);
            mqttClient.Connect(mqttUser.ClientId, mqttUser.Name, mqttUser.Password);
        }

        private void AddEvents()
        {
            mqttClient.MqttMsgPublishReceived += (s, e) =>
            {
                var messageString = Encoding.UTF8.GetString(e.Message);
                var mqttMessage = JsonHandler.GetObjectFromJsonString<MqttMessage>(messageString);
                LatestReceivedMessage = string.Concat(GetTimeString(mqttMessage.TimeStampSend), mqttMessage.ObjectSendJson);
                LatestReceivedTopic = e.Topic;

                Task.Run(() => ExecuteProperInstruction(mqttMessage.ObjectSendJson, e.Topic));
                Task.Run(() => apiHandler.SaveMessage(mqttMessage, e.Topic));
            };
        }

        private static void ExecuteProperInstruction(string objectToHandel, string topicName)
        {
            // TODO: Get the right type depending on the topic.
            // Then handle some actions like login or do whatever it takes a.s.o. Call proper web Service.
            switch (topicName)
            {
                case "TestChannel":
                    var mqttUser = JsonHandler.GetObjectFromJsonString<MqttUser>(objectToHandel);
                    break;
            }
        }

        private void SubscribeToTopics(List<MqttTopic> topics)
        {
            mqttClient.Subscribe(topics.Select(t => t.Name).ToArray()
                , topics.Select(t => t.QualityOfService).ToArray());
        }

        internal void Publish(MqttTopic topic, MqttMessage mqttMessageToPublish)
        {
            mqttClient.Publish(topic.Name, JsonHandler.GetJsonBytesFromObject(mqttMessageToPublish), topic.QualityOfService, true);
            LatestPublishedMessage = GetTimeString(DateTime.Now) + JsonHandler.GetJsonStringFromObject(mqttMessageToPublish);
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

        private static string GetTimeString(DateTime dateTime)
        {
            return string.Format(CultureInfo.CurrentCulture, "[{0}] ", dateTime.TimeOfDay);
        }
    }
}
