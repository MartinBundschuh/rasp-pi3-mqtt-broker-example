using uPLibrary.Networking.M2Mqtt;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    public enum ChannelAccessMode {
        None = 0,
        Read = 1,
        Write = 2,
        ReadWrite = 3
    }

    public enum CloudMqttBrokerPort {
        None = 0,
        Default = 11599,
        Ssl = 21599,
        Tls = 31599
    }

    public enum MqttProtocol
    {
        None = MqttSslProtocols.None,
        SslLevel3 = MqttSslProtocols.SSLv3,
        TslLevel1_0 = MqttSslProtocols.TLSv1_0,
        TslLevel1_1 = MqttSslProtocols.TLSv1_1,
        TslLevel1_2 = MqttSslProtocols.TLSv1_2
    }
}
