using System.Collections.Generic;
using AutoAct.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace AutoAct.Rest
{
    public class CustomConverter : ISerializer, IDeserializer
    {
        private static readonly JsonSerializerSettings SerializerSettings;

        static CustomConverter()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                Converters = new List<JsonConverter>
                    {
                        new IsoDateTimeConverter { DateTimeFormat = ApplicationStrings.DateTimeFormatStringIso8601 },
                        new StringEnumConverter()
                    }, 
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, SerializerSettings);
        }

        public T Deserialize<T>(IRestResponse response)
        {
            var type = typeof(T);

            return (T)JsonConvert.DeserializeObject(response.Content, type, SerializerSettings);
        }

        string IDeserializer.RootElement { get; set; }
        string IDeserializer.Namespace { get; set; }
        string IDeserializer.DateFormat { get; set; }
        string ISerializer.RootElement { get; set; }
        string ISerializer.Namespace { get; set; }
        string ISerializer.DateFormat { get; set; }
        public string ContentType { get; set; }
    }
}
