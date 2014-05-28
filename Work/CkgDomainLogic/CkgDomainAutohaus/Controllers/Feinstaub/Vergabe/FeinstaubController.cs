using System.Web.Mvc;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.Feinstaub.ViewModels;
using CkgDomainLogic.General.Controllers;

namespace AutohausPortalMvc.Controllers
{
    /// <summary>
    /// Feinstaub-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class FeinstaubController  
    {
        public AutohausFeinstaubVergabeViewModel FeinstaubVergabeViewModel { get { return GetViewModel<AutohausFeinstaubVergabeViewModel>(); } }

        [CkgApplication]
        public ActionResult Feinstaubplaketten()
        {
            FeinstaubVergabeViewModel.LoadStammdaten(ModelState);

            ViewBag.Kraftstoffcodes = FeinstaubVergabeViewModel.Kraftstoffcodes;

            return View(FeinstaubVergabeViewModel);
        }

        [HttpPost]
        public ActionResult CheckFeinstaubVergabe(FeinstaubCheckUI model)
        {
            if (ModelState.IsValid)
            {
                FeinstaubVergabeViewModel.CheckPlakettenvergabe(model, ModelState);
            }

            ViewBag.Kraftstoffcodes = FeinstaubVergabeViewModel.Kraftstoffcodes;

            return PartialView("Partial/Check", model);
        }

        [HttpPost]
        public ActionResult ShowFeinstaubVergabe()
        {
            return PartialView("Partial/Vergabe", FeinstaubVergabeViewModel.GetVergabeModel());
        }

        [HttpPost]
        public ActionResult SaveFeinstaubVergabe(FeinstaubVergabeUI model)
        {
            if (ModelState.IsValid)
            {
                FeinstaubVergabeViewModel.SavePlakettenvergabe(model, ModelState);
            }

            return PartialView("Partial/Vergabe", model);
        }
    }
}
