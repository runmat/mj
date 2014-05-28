using System.Web.Mvc;
using MvcAppZulassungsdienst.ViewModels;
using MvcTools.Web;

namespace MvcAppZulassungsdienst.Controllers
{
    public class BeauftragungController : Controller
    {
        public BeauftragungViewModel Model { get { return (BeauftragungViewModel)SessionStore.GetModel("vmBeauftragung", () => new BeauftragungViewModel()); } }
        

        public ActionResult Beauftragung()
        {
            return View(Model);
        }

    }
}
