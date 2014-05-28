using System.Web.Mvc;
using Telerik.Web.Mvc;
using MvcAppZulassungsdienst.ViewModels;
using MvcTools.Web;

namespace MvcAppZulassungsdienst.Controllers
{
    public class NachGekaufteKennzeichenController : Controller
    {
        //public NachGekaufteKennzeichenViewModel Model { get { return RepositoryBuffer<NachGekaufteKennzeichenViewModel>.Model; } }
        public NachGekaufteKennzeichenViewModel Model { get { return (NachGekaufteKennzeichenViewModel)SessionStore.GetModel("vmNachGekaufteKennzeichen",
                                                                        () => new NachGekaufteKennzeichenViewModel()); } }

        public string ID { get { return SessionHelper.GetSessionValue("ID", ""); } set { SessionHelper.SetSessionValue("ID", value); } }

        public ActionResult Index(string id)
        {
            ID = id;
            //Model.Init(ID);

            return View(Model);
        }

        [GridAction]
        public ActionResult KennzeichenKopfListAjaxSelect()
        {
            Model.Init(ID);
            
            if (Model.KennzeichenKopfList == null)
                return new EmptyResult();

            return View(new GridModel(Model.KennzeichenKopfList));
        }
    
        [GridAction]
        public ActionResult KennzeichenPosListAjaxSelect(string bstnr)
        {
            return View(new GridModel(Model.GetKennzeichenPosList(bstnr)));
        }

        public JsonResult GetKopfData()
        {
            return Json(new { kopfCount = Model.KopfCount, lieferant = Model.Lieferant });
        }
    }
}
