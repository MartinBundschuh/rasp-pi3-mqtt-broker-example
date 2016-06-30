using System;
using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    public class MqttMessage : IJsonConvertAble
    {
        [DataMember]
        public MqttUser SendFrom { get; set; }

        [DataMember]
        public DateTime TimeStampSend { get; set; }

        [DataMember]
        public string ObjectSendJson { get; set; }
    }
}
