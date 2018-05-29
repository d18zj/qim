using System;
using Newtonsoft.Json;

namespace Qim.Runtime.Serialization
{
    public class StringTypeConverter : JsonConverter
    {
        /// <summary>
        ///     是否允许Null值
        /// </summary>
        public bool AllowNull { get; set; } = false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null )
            {
                if (AllowNull)
                {
                    writer.WriteNull();
                    return;
                }
                throw new JsonSerializationException($"Cannot serialization null value.");
            }
            var type = (Type) value;
            writer.WriteValue(type.AssemblyQualifiedName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                if (AllowNull)
                {
                    return null;
                }
                throw new JsonSerializationException($"Cannot convert null value to {objectType}");
            }
            if (reader.TokenType == JsonToken.String)
            {
                var text = reader.Value.ToString();
                var type = Type.GetType(text);
                if (type == null)
                {
                    throw new JsonSerializationException($"Cannot convert '{text}' value to {objectType}");
                }
            }
            throw new JsonSerializationException($"Unexpected token {reader.TokenType} when convert to {objectType}");
        }


        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Type);
        }
    }
}