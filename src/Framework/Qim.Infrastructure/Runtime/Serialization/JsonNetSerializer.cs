using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Qim.Runtime.Serialization
{
    public class JsonNetSerializer : IObjectSerializer
    {
        public byte[] SerializeBinary<TObject>(TObject obj)
        {
            Ensure.NotNull(obj, nameof(obj));
            MemoryStream ms = new MemoryStream();
            using (JsonWriter writer = new JsonTextWriter(new StreamWriter(ms)))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, obj);
            }
            return ms.ToArray();
        }

        public TObject Deserialize<TObject>(byte[] bytes)
        {
            if (bytes == null) return default(TObject);

            MemoryStream ms = new MemoryStream(bytes);
            using (JsonReader reader = new JsonTextReader(new StreamReader(ms,Encoding.UTF8)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<TObject>(reader);
            }
        }

        public string Serialize<TObject>(TObject obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public TObject Deserialize<TObject>(string serializedString)
        {
            Ensure.NotNull(serializedString, nameof(serializedString));
            return JsonConvert.DeserializeObject<TObject>(serializedString);
        }
    }
}