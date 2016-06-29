using SQLite.Net.Attributes;
using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    [Type(typeof(MqttConnection))]
    [Table(nameof(MqttConnection))]
    public class MqttConnection : SQLiteSaveAbleObject
    {

        [DataMember]
        [PrimaryKey]
        public string BrokerName { get; set; }

        [DataMember]
        public CloudMqttBrokerPort BrokerPort { get; set; }

        [DataMember]
        public MqttProtocol SslProtocol { get; set; }

        [DataMember]
        public bool IsSecureConnection { get; set; }
    }
}
