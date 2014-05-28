using System.Web.Mvc;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Controllers;

namespace ServicesMvc.Controllers
{
    public partial class EquiController
    {
        public EquiHistorieViewModel EquipmentHistorieViewModel { get { return GetViewModel<EquiHistorieViewModel>(); } }

        [CkgApplication]
        public ActionResult FahrzeugAkte(string id)
        {
            EquipmentHistorieViewModel.LoadHistorie(id);

            return View(EquipmentHistorieViewModel);
        }

        [HttpPost]
        public ActionResult GetEquiHistorie(string fahrgestellnummer)
        {
            EquipmentHistorieViewModel.LoadHistorie(fahrgestellnummer);

            return PartialView("Historie/Fahrzeughistorie", EquipmentHistorieViewModel);
        }
    }
}
