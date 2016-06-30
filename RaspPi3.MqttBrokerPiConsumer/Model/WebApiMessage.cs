using System;
using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    public class WebApiMessage
    {
        // Autoincrement
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public DateTime TimeStampSend { get; set; }

        [DataMember]
        public string ObjectSendJson { get; set; }

        [DataMember]
        public string UserFrom { get; set; }

        [DataMember]
        public string Topic { get; set; }

        [DataMember]
        public string BrokerAndPort { get; set; }
    }
}
