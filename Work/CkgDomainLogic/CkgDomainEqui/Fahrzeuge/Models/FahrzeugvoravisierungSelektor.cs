using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeugvoravisierungSelektor 
    {
        public string Option { get; set; }

        public static string Optionen { get { return string.Format("volkswagen,{0};sonstige,{1}", "Volkswagen", Localize.Others); } }         
    }
}
