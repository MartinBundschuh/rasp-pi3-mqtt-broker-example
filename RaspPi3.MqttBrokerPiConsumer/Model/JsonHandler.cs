using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    public interface IJsonConvertAble
    {
    }

    internal class JsonHandler
    {
        internal static string GetStringFromObject<T>(T objectToSerialize) where T : IJsonConvertAble
        {
            return Encoding.UTF8.GetString(GetBytesFromObject(objectToSerialize));
        }

        internal static byte[] GetBytesFromObject<T>(T objectToSerialize) where T : IJsonConvertAble
        {
            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));

            var memoryStream = new MemoryStream();
            dataContractJsonSerializer.WriteObject(memoryStream, objectToSerialize);
            return memoryStream.ToArray();
        }
    }
}
