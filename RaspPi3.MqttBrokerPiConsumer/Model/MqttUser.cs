using SQLite.Net.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using static RaspPi3.MqttBrokerPiConsumer.Model.MqttTopic;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    [Type(typeof(MqttUser))]
    [Table(nameof(MqttUser))]
    public class MqttUser : SQLiteSaveAbleObject, IJsonConvertAble
    {
        [DataMember]
        [PrimaryKey]
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string Password { get; set; }
        [Indexed]
        public string BrokerName { get; set; }
        [Ignore]
        internal virtual MqttConnection Connection { get; set; }
        [Ignore]
        internal virtual List<MqttTopic> TopicsToSubscribe
        {
            get
            {
                using (var db = new SqLiteHandler())
                {
                    return db.Select<MqttTopic>()
                        .Where(t => t.UserName == Name && (t.AccessMode == ChannelAccessMode.Read || t.AccessMode == ChannelAccessMode.ReadWrite))
                        .ToList();
                }
            }
        }
    }
}
