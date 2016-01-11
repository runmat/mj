using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using CkgDomainLogic.General.Services;
using DocumentTools.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using SapORM.Services;
using ServicesMvc;
// ReSharper disable All

namespace ServicesMvcTest
{
    class Program
    {
        static void Main()
        {
            Test();
        }

        static bool WeatherDateMatches(string firstDateTxt, string dtTxt)
        {
            var firstDate = DateTime.Parse(firstDateTxt);
            var dt = DateTime.Parse(dtTxt);

            var hour = dt.Date == DateTime.Now.Date ? firstDate.Hour : 12;

            return dt.Hour == hour;
        }

        static void Test()
        {
            string sData = File.ReadAllText(@"C:\Users\JenzenM\Downloads\data.json");
            var jsonData = new JavaScriptSerializer().Deserialize<WeatherData>(sData);

            var firstDateTxt = jsonData.list.FirstOrDefault().dt_txt;
            var list = jsonData.list.Where(d => WeatherDateMatches(firstDateTxt, d.dt_txt)).Select(d => d).ToArray();

            jsonData.list = list;

            sData = new JavaScriptSerializer().Serialize(jsonData);
        }
    }

    public class WeatherData
    {
        public WdCity city { get; set; }

        public WdItem[] list  { get; set; }
    }

    public class WdCity
    {
        public long id { get; set; }
        public string name  { get; set; }
        public string country { get; set; }
        public int cnt { get; set; }
    }

    public class WdItem
    {
        public long dt { get; set; }
        public WdItemMain main { get; set; }
        public string dt_txt { get; set; }
    }

    public class WdItemMain
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double temp_kf { get; set; }
    }
}
