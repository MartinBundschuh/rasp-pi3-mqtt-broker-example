using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace RaspPi3.MqttBrokerPiConsumer.Model
{
    internal static class JsonHandler
    {
        internal static string GetJsonStringFromObject<T>(T objectToSerialize) where T : IJsonConvertAble
        {
            return Encoding.UTF8.GetString(GetJsonBytesFromObject(objectToSerialize));
        }

        internal static byte[] GetJsonBytesFromObject<T>(T objectToSerialize) where T : IJsonConvertAble
        {
            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));

            using (var memoryStream = new MemoryStream())
            {
                dataContractJsonSerializer.WriteObject(memoryStream, objectToSerialize);
                return memoryStream.ToArray();
            }
        }

        internal static T GetObjectFromJsonString<T>(string jsonString) where T : IJsonConvertAble
        {
            var dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)(dataContractJsonSerializer.ReadObject(memoryStream));
            }
        }
    }
}
