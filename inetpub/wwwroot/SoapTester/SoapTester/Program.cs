using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Xml.Serialization;
using SoapTester.AGIS;
using SoapTester.Ruecklaeuferschnittstelle;

namespace SoapTester
{
    class Program
    {
        static void Main(string[] args)
        {
            //WebRequest.DefaultWebProxy = new WebProxy("127.0.0.1", 8888);

            //DateTime now = DateTime.Now;

            //AGIS.CarDocuWebServicePortClient client = new CarDocuWebServicePortClient();

            //client.Endpoint.Address = new EndpointAddress("https://tui-agis-services.audi.de/agis/adapter/ws");

            //var response = client.GetDriverInformation(new GetDriverInformation());

            PostSoapClient client = new PostSoapClient();
            client.Endpoint.Address = new EndpointAddress("http://sgwt.kroschke.de/hla/Ruecklaeuferschnittstelle.asmx");

            var model = (Ruecklaeuferschnittstelle.Ruecklaeuferschnittstelle) XmlDeserializeFromString(Resource.XMLTest, typeof (Ruecklaeuferschnittstelle.Ruecklaeuferschnittstelle));

            var result = client.Put("lease_motion", "58Rt%!dR", model);

            var listResult = client.List("lease_motion", "58Rt%!dR", new long[]
            {
                270260798
            });
        }

        public static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }
    }
}

/*
var xml = @"<car/>";
var serializer = new XmlSerializer(typeof(Car));
using (var reader = new StringReader(xml))
{
    var car = (Car)serializer.Deserialize(reader);
}
*/