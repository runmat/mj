using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugSperrenEntsperrenSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Auswahl { get; set; }

        public string AuswahlOptionen { get { return string.Format("GESP,{0};NGESP,{1};ALLE,{2}", Localize.Locked, Localize.NotLocked, Localize.All); } }
    }
}
