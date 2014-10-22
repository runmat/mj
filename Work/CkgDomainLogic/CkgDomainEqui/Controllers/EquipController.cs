using System.Collections;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Controllers;
using GeneralTools.Contracts;

namespace ServicesMvc.Controllers
{
    public partial class EquipController : CkgDomainController
    {
        public override string DataContextKey { get { return "EquiViewModel"; } }

        public EquipController(IAppSettings appSettings, ILogonContextDataService logonContext, 
            IBriefbestandDataService briefbestandDataService, IAbweichungenDataService abweichungenDataService)
            : base(appSettings, logonContext)
        {
            InitViewModel(VersandAbweichungenViewModel, appSettings, logonContext, abweichungenDataService);
            InitViewModel(VersandBeauftragungenViewModel, appSettings, logonContext, briefbestandDataService);
            InitViewModel(EinAusgaengeViewModel, appSettings, logonContext, briefbestandDataService);
        }


        #region Export

        protected override IEnumerable GetGridExportData()
        {
            if (MainViewModel is VersandBeauftragungenViewModel)
                return VersandBeauftragungenViewModel.VersandBeauftragungenFiltered;

            if (MainViewModel is EinAusgaengeViewModel)
                return EinAusgaengeViewModel.EinAusgaengeFiltered;

            if (MainViewModel is VersandAbweichungenViewModel)
                return VersandAbweichungenViewModel.VersandAbweichungenFiltered;

            return null;
        }

        #endregion   
    }
}
