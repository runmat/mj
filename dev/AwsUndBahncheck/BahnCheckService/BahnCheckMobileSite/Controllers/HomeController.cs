//#define TEST

using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using BahnCheckDatabase.Models;
using MvcTools.Data;
using MvcTools.Web;

namespace BahnCheckMobileSite.Controllers
{
    public class HomeController : Controller
    {
        public string WantedRbZug { get { return SessionHelper.GetSessionValue("WantedRbZug", ""); } set { SessionHelper.SetSessionValue("WantedRbZug", value); } }

        public int WantedRbOfs { get { return SessionHelper.GetSessionValue("WantedRbOfs", 0); } set { SessionHelper.SetSessionValue("WantedRbOfs", value); } }

        public RbRequest Rb { get { return RepositoryBuffer<RbRequest>.Model; } set { RepositoryBuffer<RbRequest>.Model = value; } }

        private enum PollingStateEnum
        {
            Init = 1,
            Processing = 2,
            Finish = 3
        };

        private PollingStateEnum PollingState { get { return (PollingStateEnum)System.Web.HttpContext.Current.Session["PollingState"]; } set { System.Web.HttpContext.Current.Session["PollingState"] = value; } }

        public ActionResult Index()
        {
            Rb.Bahnhof = "(Bahnhof)";
            Rb.Abfahrt = DateTime.Today;
            Rb.Auskunft = ".. mal sehen ..";
            Rb.Zug = "Zug-Nr";

            return View(Rb);
        }

        [HttpPost]
        public JsonResult RbRequery()
        {
            WantedRbZug = "";
            WantedRbOfs = 0;

            PollingState = PollingStateEnum.Init;
            Rb.ProcessingInfo = "suche zeitlich nächsten Zug";

            return Json(Rb);
        }

        [HttpPost]
        public JsonResult RbRefresh()
        {
            WantedRbOfs = 0;

            PollingState = PollingStateEnum.Init;
            Rb.ProcessingInfo = "diesen Zug erneut abfragen";

            return Json(Rb);
        }

        [HttpPost]
        public JsonResult RbOneTrainEarlierOrLater(int ofs)
        {
            WantedRbOfs = ofs;

            PollingState = PollingStateEnum.Init;
            Rb.ProcessingInfo = "suche einen Zug " + (ofs == 1 ? "später" : "früher") +  "";

            return Json(Rb);
        }

        [HttpPost]
        public JsonResult RbPolling()
        {
            if (PollingState == PollingStateEnum.Init)
            {
                PollingState = PollingStateEnum.Processing;
                Rb.ProcessingInfo += "";

                using (var ct = new RbEntities())
                {
                    var rbDb = ct.GetOpenRbRequest(WantedRbZug, WantedRbOfs);
                    Rb.ID = rbDb.ID;
                }

                return Json(Rb);
            }

            if (PollingState == PollingStateEnum.Processing)
            {
                Rb.ProcessingInfo += ".";

                using (var ct = new RbEntities())
                {
                    var rbDb = ct.GetRbRequestById(Rb.ID);
                    if (rbDb != null && rbDb.UpdDate != null)
                    {
                        PollingState = PollingStateEnum.Finish;

                        WantedRbZug = rbDb.Zug;

                        EfDataService.Copy(rbDb, Rb);

                        Rb.ProcessingInfo = "finished!";
                    }
                }
                Thread.Sleep(400);
            }

            return Json(Rb);
        }
    }
}
