using System.IO;
using System.Net;
// ReSharper disable UnusedMethodReturnValue.Local

namespace Tear
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;

            GetWebRequestHtml(args[0]);
        }

        private static string GetWebRequestHtml(string url)
        {
            var request = (HttpWebRequest) WebRequest.Create(url);
            var html = "";

            using (var response = (HttpWebResponse) request.GetResponse())
            using (var stream = response.GetResponseStream())
                if (stream != null)
                    using (var reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }

            return html;
        }
    }
}
