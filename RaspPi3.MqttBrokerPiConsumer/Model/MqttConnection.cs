using SQLite.Net.Attributes;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [Type(typeof(MqttConnection))]
    [Table("MqttConnections")]
    public class MqttConnection : SqLiteSaveableObject
    {
        public enum CloudMqttBroker { Default = 11599, Ssl = 21599, Tsl = 31599 }

        [PrimaryKey]
        public string BrokerName { get; set; }
        public CloudMqttBroker BrokerPort { get; set; }
        public MqttSslProtocols MqttSslProtocols { get; set; }
        public bool IsSecureConnection { get; set; }
    }
}
