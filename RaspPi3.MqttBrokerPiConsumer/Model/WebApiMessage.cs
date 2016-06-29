using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    public class WebApiMessage
    {
        [DataMember]
        public string ObjectSendJson { get; set; }

        [DataMember]
        public MqttTopic Topic { get; set; }
    }
}
