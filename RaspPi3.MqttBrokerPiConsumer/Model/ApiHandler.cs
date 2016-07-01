using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    class ApiHandler
    {
        private readonly WebApiUser apiUser;
        private HttpClient httpClient;
        private const string API_PATH = "api/" + nameof(MqttMessage);

        public ApiHandler(MqttUser mqttUser)
        {
            //using (var db = new SqLiteHandler())
            var db = new SqLiteHandler();
            //{
                apiUser = db.Select<WebApiUser>()
                    .FirstOrDefault(u => u.Name == mqttUser.Name && !u.Password.Contains("*"));

                apiUser.BaseUrl = "https://rasppi3webapi.azurewebsites.net/";
                Task.Run(() => db.SaveChangesAsync());
            //}

            SetUpHttpClient();
            Task.Run(() => PutTokenInHeader());
        }

        private void SetUpHttpClient()
        {
            var apiBaseUrl = new Uri(apiUser.BaseUrl);
            httpClient = new HttpClient { BaseAddress = apiBaseUrl };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<string> GetToken()
        {
            HttpResponseMessage responseMessage;
            using (var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", apiUser.Email),
                new KeyValuePair<string, string>("password", apiUser.Password),
            }))
            {
                responseMessage = await httpClient.PostAsync("Token", formContent);
            }

            var responseJson = responseMessage.Content.ReadAsStringAsync();
            var jObect = JObject.Parse(await responseJson);
            return jObect.GetValue("access_token").ToString();
        }

        private async Task PutTokenInHeader()
        {
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Concat("Bearer ", await token));
        }

        [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "CC0091")]
        internal async void SaveMessage (MqttMessage mqttMessage, string topic)
        {
            var brokerAndPort = string.Format("{0}:{1} ({2})",
                mqttMessage.SendFrom.Connection.BrokerName, Convert.ToInt32(mqttMessage.SendFrom.Connection.BrokerPort), mqttMessage.SendFrom.Connection.BrokerPort);
            var apiMessage = new WebApiMessage
            {
                ObjectSendJson = mqttMessage.ObjectSendJson,
                UserFrom = mqttMessage.SendFrom.Name,
                Topic = topic,
                BrokerAndPort = brokerAndPort,
                TimeStampSend = mqttMessage.TimeStampSend
            };

            var responseMessage = await httpClient.PostAsJsonAsync(API_PATH, apiMessage);
        }
    }
}
