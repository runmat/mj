using System.Web.Mvc;
using CkgDomainLogic.AutohausFahrzeugdaten.Contracts;
using CkgDomainLogic.AutohausFahrzeugdaten.ViewModels;
using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    /// <summary>
    /// AutohausFahrzeugdaten-Controller (aufgegliedert in partielle Klassen, je nach Funktionsgruppe)
    /// </summary>
    public partial class AutohausFahrzeugdatenController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<UploadFahrzeugdatenViewModel>(); } }

        public AutohausFahrzeugdatenController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IUploadFahrzeugdatenDataService uploadFahrzeugdatenDataService,
            IDataConverterDataService dataConverterDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(UploadFahrzeugdatenViewModel, appSettings, logonContext, uploadFahrzeugdatenDataService, dataConverterDataService);

            InitModelStatics();
        }

        void InitModelStatics()
        {
            MappedUploadMappingSelectionModel.GetViewModel = GetViewModel<UploadFahrzeugdatenViewModel>;
        }

        public ActionResult Index()
        {
            return RedirectToAction("UploadFahrzeugdaten");
        }
    }
}
