using SQLite.Net.Attributes;
using Windows.Devices.WiFi;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    [Type(typeof(WifiConnection))]
    [Table(nameof(WifiConnection))]
    class WifiConnection : SQLiteSaveAbleObject
    {
        [PrimaryKey]
        public string Ssid { get; set; }

        public string Password { get; set; }

        public WiFiReconnectionKind RecconectionKind { get; set; }
    }
}
