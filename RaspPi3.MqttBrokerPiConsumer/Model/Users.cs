using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

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

        [DataMember]
        public string ClientId { get; set; }

        public string Password { get; set; }

        [Indexed]
        public string BrokerName { get; set; }

        [DataMember]
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

    [Type(typeof(WebApiUser))]
    [Table(nameof(WebApiUser))]
    public class WebApiUser : SQLiteSaveAbleObject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Email { get; set; }

        public string BaseUrl { get; set; }

        public string Password { get; set; }

        [Indexed]
        public string Name { get; set; }
    }
}

