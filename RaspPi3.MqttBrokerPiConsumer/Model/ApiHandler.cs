using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    class ApiHandler
    {
        private readonly Uri apiBaseUrl;
        private static HttpClient httpClient;

        public ApiHandler()
        {
            apiBaseUrl = new Uri("https://rasppi3webapi20160622020016.azurewebsites.net/api/");
            httpClient = new HttpClient { BaseAddress = apiBaseUrl };
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "CC0091")]
        internal async void SaveMessage (MqttMessage mqttMessage, string topic)
        {
            var apiMessage = new WebApiMessage
            {
                ObjectSendJson = mqttMessage.ObjectSendJson,
                UserFrom = mqttMessage.SendFrom.Name,
                Topic = topic,
                BrokerAndPort = mqttMessage.SendFrom.BrokerName + mqttMessage.SendFrom.Connection.BrokerPort,
                TimeStampSend = mqttMessage.TimeStampSend
            };

            var responseMessage = await httpClient.PostAsJsonAsync(nameof(MqttMessage), apiMessage);
        }
    }
}
