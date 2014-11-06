using System.Collections.Generic;

namespace GeneralTools.Log.Models
{
    public class LogConstants
    {
        public static IDictionary<int, string> PortalTypes = new Dictionary<int, string>
            {
                { 1, "Portal" },
                { 2, "Services" },
                { 3, "MVC" },
            };
    }
}
