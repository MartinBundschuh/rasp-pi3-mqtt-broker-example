namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    public class MqttTopic
    {
        public enum AccessMode { None = 0, Read = 1, Write = 2, ReadWrite = 3 }

        public int Id { get; set; }
        public MqttUser User { get; set; }
        public string Name { get; set; }
        public AccessMode Acess { get; set; }
        public byte QualityOfService { get; set; }
    }
}
