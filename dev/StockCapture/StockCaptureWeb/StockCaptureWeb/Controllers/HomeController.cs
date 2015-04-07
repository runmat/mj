using System.Web.Mvc;
using StockCapture;

namespace StockCaptureWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var repository = new Repository())
            {
                var stocks = repository.LoadStockQuotes();
                var latestStocks = repository.GetLatestStockQuotes(2);
            }

            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
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
    }
}
