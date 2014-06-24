using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Autohaus.Models;

namespace CkgDomainLogic.Autohaus.ViewModels
{
    public class FahrzeugakteViewModel : CkgBaseViewModel
    {
        public Fahrzeug Fahrzeug { get; set; }

        public FahrzeugakteDocsViewModel DocsViewModel { get; set; }

        public void LoadFahrzeugakte(Fahrzeug fahrzeug)
        {
            Fahrzeug = fahrzeug;

            DocsViewModel.LoadFahrzeugakteDocs(fahrzeug.ID);
        }
    }
}
