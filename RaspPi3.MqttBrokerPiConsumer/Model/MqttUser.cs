using System;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    // TODO: Store in AppData or WebSpace or somewhere else
    internal static class MqttUser
    {
        internal static string ClientId = Guid.NewGuid().ToString();
        internal static string UserName = "ti8mPhone";
        internal static string Password = "ti8m";
    }
}
