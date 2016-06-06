using SQLite.Net.Attributes;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [Type(typeof(MqttConnection))]
    [Table(nameof(MqttConnection))]
    public class MqttConnection : SqLiteSaveableObject
    {
        public enum CloudMqttBrokerPort { Default = 11599, Ssl = 21599, Tsl = 31599 }

        [PrimaryKey]
        public string BrokerName { get; set; }
        public CloudMqttBrokerPort BrokerPort { get; set; }
        public MqttSslProtocols SslProtocol { get; set; }
        public bool IsSecureConnection { get; set; }
    }
}
