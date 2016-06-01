using SQLite.Net.Attributes;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [Type(typeof(MqttTopic))]
    [Table("MqttTopics")]
    public class MqttTopic
    {
        public enum AccessMode { None = 0, Read = 1, Write = 2, ReadWrite = 3 }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Indexed]
        public string MqttUserUsername { get; set; }
        [Ignore]
        public MqttUser User { get; set; }
        public string Name { get; set; }
        public AccessMode Acess { get; set; }
        public byte QualityOfService { get; set; }
    }
}
