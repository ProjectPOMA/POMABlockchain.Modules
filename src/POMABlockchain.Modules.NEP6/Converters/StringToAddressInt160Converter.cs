using System;
using POMABlockchain.Modules.Core;
using POMABlockchain.Modules.KeyPairs;
using Newtonsoft.Json;

namespace POMABlockchain.Modules.NEP6.Converters
{
    public class StringToAddressInt160Converter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            UInt160 data = (UInt160)value;

            var stringData = data.ToAddress();

            writer.WriteValue(stringData);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = (string)reader.Value;
            return result.ToScriptHash();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
    }
}
