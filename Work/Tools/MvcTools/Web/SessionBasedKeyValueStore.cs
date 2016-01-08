using GeneralTools.Contracts;

namespace MvcTools.Web
{
    public class SessionBasedKeyValueStore : IKeyValueStore<string>
    {
        public string GetValue(string key)
        {
            return SessionHelper.GetSessionString(key);
        }

        public void SetValue(string key, string value)
        {
            SessionHelper.SetSessionValue(key, value);
        }
    }
}