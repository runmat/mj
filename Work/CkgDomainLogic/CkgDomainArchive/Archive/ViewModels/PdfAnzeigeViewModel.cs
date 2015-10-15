using System.Xml.Serialization;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Archive.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Archive.ViewModels
{
    public class PdfAnzeigeViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IPdfAnzeigeDataService DataService { get { return CacheGet<IPdfAnzeigeDataService>(); } }

        [XmlIgnore]
        public byte[] PdfDatei { get; private set; }

        public int CurrentAppID { get; set; }

        public void Init()
        {
            GetCurrentAppID();

            var serverPfad = ApplicationConfiguration.GetApplicationConfigValue("DateiPfad", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID);
            var carportIdVerwenden = ApplicationConfiguration.GetApplicationConfigValue("CarportIdVerwenden", CurrentAppID.ToString(), LogonContext.Customer.CustomerID, LogonContext.Group.GroupID).ToBool();
            var carportId = LogonContext.User.Reference;

            PdfDatei = DataService.GetPdf(serverPfad, carportIdVerwenden, carportId);
        }

        private void GetCurrentAppID()
        {
            CurrentAppID = LogonContext.GetAppIdCurrent();
        }
    }
}
