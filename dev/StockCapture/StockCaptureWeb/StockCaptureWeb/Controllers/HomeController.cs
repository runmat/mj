using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using StockCapture.ViewModels;

namespace StockCaptureWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly CultureInfo _cultureInfo = new CultureInfo("en-US");

        private IsoDateTimeConverter _isoConvert;
        private IsoDateTimeConverter IsoConvert
        {
            get 
            { 
                return (_isoConvert ?? (_isoConvert = new IsoDateTimeConverter
                                                        {
                                                            DateTimeFormat = "yyyy/MM/dd hh:mm tt",
                                                            Culture = _cultureInfo
                                                        })); 
            }
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View(new StockQuoteViewModel());
        }

        public ActionResult Chart()
        {
            return View(new StockQuoteViewModel());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult GetChartData()
        {
            var stockQuotes = new StockQuoteViewModel().StockQuotes;
            stockQuotes.Reverse(0, stockQuotes.Count);

            var chartData = stockQuotes.Select(sq => new[]
            {
                GetJsonDateTime(sq.Date.GetValueOrDefault()), sq.Val.GetValueOrDefault().ToString("0.0000", _cultureInfo)
            }).ToArray();

            return Json(new
            {
                chartData
            });
        }

        string GetJsonDateTime(DateTime dt)
        {
            return JsonConvert.SerializeObject(dt.AddMinutes(0).AddSeconds(-dt.Second), IsoConvert).Replace("\"","");
        }
    }
}
