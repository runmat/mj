using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;

namespace CkgDomainLogic.Insurance.ViewModels
{
    public class SchadenakteViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IVersEventsDataService DataService { get { return CacheGet<IVersEventsDataService>(); } }

        public Schadenakte Schadenakte { get; set; }

        public SchadenakteDocsViewModel DocsViewModel { get; set; }

        //[XmlIgnore]
        //public List<VersEvent> Events { get { return DataService.Events; } }

        public void LoadSchadenakte(Schadenfall schadenfall)
        {
            Schadenakte = new Schadenakte
                {
                    Schadenfall = schadenfall
                };
            //Schadenakte.Schadenfall.EventName = Events.Find(e => e.ID == Schadenakte.Schadenfall.EventID).EventName;

            DocsViewModel.LoadSchadenakteDocs(schadenfall.ID);
        }
    }
}
