using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.AppUserOverview.Models
{
    public class AppUserOverviewSelektor : Store
    {
        /// <summary>
        /// Default value = Nur aktive anzeigen ("Active")
        /// </summary>
        [LocalizedDisplay("Nur aktive")]    //[LocalizedDisplay(LocalizeConstants.VIN17)]
        public string ShowActiveOnly { get { return PropertyCacheGet(() => "Active"); } set { PropertyCacheSet(value); } }  

        /// <summary>
        /// Inhalte der Radiobuttons für Auswahl bereitstellen
        /// </summary>
        public string ShowActiveOnlyWerte
        {
            get
            {
                return string.Format("{0},{1};{2},{3}",
                    "Active", "Ja",    // Localize.VehiclesAll,
                    "All", "Nein");    // Localize.VehiclesOnlyWithoutFirstRegistration);
            }
        }

    }
}
