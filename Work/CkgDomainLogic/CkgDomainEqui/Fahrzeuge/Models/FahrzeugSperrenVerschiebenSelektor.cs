using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugSperrenVerschiebenSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string Auswahl { get; set; }

        public string AuswahlOptionen { get { return string.Format("GESP,{0};NGESP,{1};ALLE,{2};UPLOAD,{3}", Localize.Locked, Localize.NotLocked, Localize.All, Localize.Upload); } }

        [LocalizedDisplay(LocalizeConstants.WithComment)]
        public bool NurMitBemerkung { get; set; }
    }
}
