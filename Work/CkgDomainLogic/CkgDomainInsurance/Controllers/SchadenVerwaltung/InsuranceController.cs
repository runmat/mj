using System.Web.Mvc;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// Insurance-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class InsuranceController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Konfigurator");
        }
    }
}
