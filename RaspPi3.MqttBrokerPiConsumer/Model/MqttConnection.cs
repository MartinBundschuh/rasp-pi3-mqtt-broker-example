using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    [Table("MqttConnections")]
    public class MqttConnection
    {
        public enum CloudMqttBroker { Default = 11599, Ssl = 21599, Tsl = 31599 }

        public string BrokerName { get; set; }
        public CloudMqttBroker BrokerPort { get; set; }
        public MqttSslProtocols MqttSslProtocols { get; set; }
        public bool IsSecureConnection { get; set; }
    }
}
