using Newtonsoft.Json;

namespace MvcTools.Web
{
    public class JSon
    {
        public static T Deserialize<T>(string model) where T : class
        {
            return JsonConvert.DeserializeObject<T>(model, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local });
        }

        public static string Serialize<T>(T model) where T : class
        {
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local });
        }
    }
}
