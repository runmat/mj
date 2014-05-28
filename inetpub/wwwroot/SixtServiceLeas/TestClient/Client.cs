using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using TestClient.SixtServiceLeas;

namespace TestClient
{
    internal class Client
    {
        private readonly string username;
        private readonly string password;

        internal Client()
        {
            this.username = System.Configuration.ConfigurationManager.AppSettings["Username"];
            this.password = System.Configuration.ConfigurationManager.AppSettings["Password"];
        }

        internal void TestWMGetFreisetzung_Status()
        {
            using (var channel = new ServiceDataSoapClient())
            {
                var res = channel.WMGetFreisetzung_Status(this.username, this.password);
                res = res.Substring(1);
                var doc = new XmlDocument();
                doc.LoadXml(res);
            }
        }
    }
}
