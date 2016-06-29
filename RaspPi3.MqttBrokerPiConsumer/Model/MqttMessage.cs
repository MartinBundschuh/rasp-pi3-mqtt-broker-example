using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    public class MqttMessage<T> : IJsonConvertAble where T : IJsonConvertAble
    {
        [DataMember]
        public MqttUser SendFrom { get; set; }

        [DataMember]
        public string ObjectSendJson { get; set; }
    }
}
