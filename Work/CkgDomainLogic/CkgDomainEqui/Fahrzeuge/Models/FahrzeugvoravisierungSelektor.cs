using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugvoravisierungSelektor 
    {
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]        
        public string Option { get; set; }

        public static string Optionen { get { return string.Format("volkswagen,{0};sonstige,{1}", "Volkswagen", Localize.Others); } }         
    }
}
