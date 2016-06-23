﻿using SQLite.Net.Attributes;
using System.Runtime.Serialization;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [DataContract]
    [Type(typeof(MqttTopic))]
    [Table(nameof(MqttTopic))]
    public class MqttTopic : SQLiteSaveAbleObject
    {
        public enum ChannelAccessMode { None = 0, Read = 1, Write = 2, ReadWrite = 3 }

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
