using System.Linq;
using System.Web.Mvc;
using NLog;
using RefImpl.Filters;
using RefImpl.Helpers;
using RefImplBibl.Interfaces;
using RefImplBibl.Models;

namespace RefImpl.Controllers
{
    [CkgAuthorize]
    [CkgHandleError]
    public class EquiController : CkgController
    {
        private readonly ISap _sap;

        public EquiController(ISap sap, IAnwenderInfoProvider anwenderInfoProvider) : base(anwenderInfoProvider)
        {
            _sap = sap;
            var logger = LogManager.GetLogger("EquiController");
            logger.Trace("Equi Controller");
        }

        public ActionResult Index()
        {
            _sap.Z_DPM_EQUI_GET();
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var equi = new EQUI();
            return View(equi);
        }

        [HttpPost]
        public ActionResult Create(EQUI equi)
        {
            if (ModelState.IsValid)
            {
                var id = _sap.Z_DPM_INSERT_EQUI(equi);
                return RedirectToAction("Edit", new {id});
            }

            return View(equi);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var equi = _sap.Z_DPM_EQUI_GET().First();

            return View(equi);
        }

        [HttpPost]
        public ActionResult Edit(EQUI equi)
        {
            if (ModelState.IsValid)
            {
                var id = _sap.Z_DPM_UPDATE_EQUI(equi);
                return RedirectToAction("Edit", new {id});
            }

            return View(equi);
        }

        [HttpGet]
        public ActionResult ThrowEx()
        {
            int i = 9;
            int l = 0;

            var result = i/l;

            return new ContentResult();
        }
    }
}
