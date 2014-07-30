using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class DatenOhneDokumenteFilter
    {
        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Selektion { get; set; }

        public string Selektionstypen { get { return string.Format("A,{0};K,{1}", Localize.AllVehicleBasicData, Localize.DocumentsInCustomerContinuance); } }
    }
}
