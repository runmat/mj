using System.Collections.Generic;
using CkgDomainLogic.Fahrer.Models;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.Fahrer.Contracts
{
    public interface IFahrerDataService : ICkgGeneralDataService
    {
        string FahrerID { get; }

        List<FahrerTagBelegung> FahrerTagBelegungen { get; }

        void MarkDataForRefresh();

        void SaveFahrerTagBelegungen(IEnumerable<FahrerTagBelegung> fahrerTagBelegungen);

        IEnumerable<FahrerAuftrag> LoadFahrerAuftraege(string auftragsStatus);

        IEnumerable<IFahrerAuftragsFahrt> LoadFahrerAuftragsFahrten();
        IEnumerable<IFahrerAuftragsFahrt> LoadFahrerAuftragsProtokolle();
        IFahrerAuftragsFahrt LoadFahrerAuftragsEinzelProtokoll(string auftragsnr, string fahrtTyp);
        
        string SetFahrerAuftragsStatus(string auftragsNr, string status);

        byte[] GetAuftragsPdfBytes(string auftragsNr);


        int QmFahrerRankingCount { get; set; }

        List<QmFahrer> QmFahrerList { get; set; }

        List<QmFleetMonitor> QmFleetMonitorList { get; set; }

        bool LoadQmReportFleetData(DateRange dateRange);

        List<SelectItem> QmCodes { get; }

        string SaveProtokollAndQmDaten(ProtokollEditModel item);

        List<string> GetProtokollArchivierungMailAdressenAndReferenz(FahrerAuftragsProtokoll protokoll);
        List<FahrerAuftrag> LoadFreieAuftraege();
    }
}
