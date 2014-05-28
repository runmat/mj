using System.Xml.Serialization;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using CkgDomainLogic.General.ViewModels;

namespace CkgDomainLogic.Equi.ViewModels
{
    public class EquiHistorieViewModel : CkgBaseViewModel 
    {
        [XmlIgnore]
        public IEquiHistorieDataService DataService { get { return CacheGet<IEquiHistorieDataService>(); } }

        public string Fahrgestellnummer { get; set; }

        public EquiHistorie EquipmentHistorie { get; set; }

        /// <summary>
        /// Equi-Historiendaten laden
        /// </summary>
        /// <returns></returns>
        public void LoadHistorie(string fin)
        {
            Fahrgestellnummer = fin;
            EquipmentHistorie = DataService.GetEquiHistorie(Fahrgestellnummer);
        }
    }
}
