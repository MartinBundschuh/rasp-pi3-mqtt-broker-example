using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    public class WebApiMessage : IJsonConvertAble
    {
        [DataMember]
        public string JsonString { get; set; }

        [DataMember]
        public MqttTopic Topic { get; set; }

        [DataMember]
        public MqttUser User { get; set; }

        [DataMember]
    }
}
