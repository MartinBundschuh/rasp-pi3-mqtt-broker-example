using SQLite.Net.Attributes;
using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    [Type(typeof(MqttTopic))]
    [Table(nameof(MqttTopic))]
    public class MqttTopic : SQLiteSaveAbleObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string UserName { get; set; }

        [Ignore]
        internal MqttUser User { get; set; }

        [DataMember]
        public string Name { get; set; }

        public ChannelAccessMode AccessMode { get; set; }

        public byte QualityOfService { get; set; }
    }
}
