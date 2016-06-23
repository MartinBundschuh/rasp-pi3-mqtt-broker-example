using SQLite.Net.Attributes;
using System.Runtime.Serialization;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    [Type(typeof(MqttConnection))]
    [Table(nameof(MqttConnection))]
    public class MqttConnection : SQLiteSaveAbleObject
    {
        public enum CloudMqttBrokerPort {None = 0, Default = 11599, Ssl = 21599, Tls = 31599 }

        [DataMember]
        [PrimaryKey]
        public string BrokerName { get; set; }

        [DataMember]
        public CloudMqttBrokerPort BrokerPort { get; set; }

        [DataMember]
        public MqttSslProtocols SslProtocol { get; set; }

        [DataMember]
        public bool IsSecureConnection { get; set; }
    }
}
