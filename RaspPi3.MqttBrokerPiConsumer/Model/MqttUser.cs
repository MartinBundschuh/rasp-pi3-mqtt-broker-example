using System.Collections.Generic;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    // TODO: Store and load data in azure
    public class MqttUser
    {
        public string UserName { get; set; }
        public string ClientId { get; set; }
        public string Password { get; set; }
        public virtual MqttConnection Connection { get; set; }
        public virtual List<MqttTopic> TopicsToSubscribe { get; set; }

        internal List<MqttTopic> GetTopicsToSubscribe()
        {
            if (TopicsToSubscribe == null)
            {
                // TODO: Select all topics where UserName = this.Name and Acess = Read or ReadWrite and iterate overit
                TopicsToSubscribe = new List<MqttTopic>();
                TopicsToSubscribe.Add(new MqttTopic
                {
                    User = this,
                    Name = "TestConnection",
                    Acess = MqttTopic.AccessMode.ReadWrite,
                    QualityOfService = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE
                });
            }

            return TopicsToSubscribe;
        }
    }
}
