using System.Web.Mvc;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Contracts;
using CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.ViewModels;
using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class AutohausPartnerUndFahrzeugdatenController : CkgDomainController 
    {
        public override string DataContextKey { get { return GetDataContextKey<UploadPartnerUndFahrzeugdatenViewModel>(); } }

        public AutohausPartnerUndFahrzeugdatenController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IUploadPartnerUndFahrzeugdatenDataService uploadPartnerUndFahrzeugdatenDataService,
            IDataConverterDataService dataConverterDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(ViewModel, appSettings, logonContext, uploadPartnerUndFahrzeugdatenDataService, dataConverterDataService);

            InitModelStatics();
        }

        void InitModelStatics()
        {
            MappedUploadMappingSelectionModel.GetViewModel = GetViewModel<UploadPartnerUndFahrzeugdatenViewModel>;
        }

        public ActionResult Index()
        {
            return RedirectToAction("UploadFahrzeugdaten");
        }
    }
}
