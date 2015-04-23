using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using CkgDomainLogic.Fahrzeuge.Contracts;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class FahrzeugeController : CkgDomainController
    {
        private string _dataContextKey = "";
        public override string DataContextKey { get { return _dataContextKey; } }

        public FahrzeugeController(IAppSettings appSettings, ILogonContextDataService logonContext,
            IFahrzeugeDataService fahrzeugeDataService, 
            IFehlteilEtikettenDataService fehlteilEtikettenDataService,
            IUploadFahrzeugeinsteuerungDataService uploadFahrzeugeinsteuerungDataService,
            IFahrzeugzulaeufeDataService fahrzeugzulaeufeDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(AbgemeldeteFahrzeugeViewModel, appSettings, logonContext, fahrzeugeDataService);
            InitViewModel(FehlteilEtikettenViewModel, appSettings, logonContext, fehlteilEtikettenDataService);
            InitViewModel(UploadFahrzeugeinsteuerungViewModel, appSettings, logonContext, uploadFahrzeugeinsteuerungDataService);
            InitViewModel(FahrzeugzulaeufeViewModel, appSettings, logonContext, fahrzeugzulaeufeDataService);
        }
    }
}
