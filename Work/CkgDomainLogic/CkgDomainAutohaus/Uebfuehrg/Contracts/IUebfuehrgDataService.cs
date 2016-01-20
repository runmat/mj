using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Contracts;
using Adresse = CkgDomainLogic.Uebfuehrg.Models.Adresse;

namespace CkgDomainLogic.Uebfuehrg.Contracts
{
    public interface IUebfuehrgDataService : ICkgGeneralDataService 
    {
        string KundenNr { get; set; }

        string AuftragGeber { get; set; }

        string AuftragGeberOderKundenNr { get; }

        List<Adresse> GetFahrtAdressen(string[] addressTypes);

        List<Adresse> GetRechnungsAdressen();

        void GetTransportTypenAndDienstleistungen(out List<TransportTyp> transportTypen, out List<Dienstleistung> dienstleistungen);

        List<UeberfuehrungsAuftragsPosition> Save(RgDaten rgDaten, List<CommonUiModel> stepModels, List<Fahrt> fahrten);

        void OnInit(ILogonContext logonContext, IAppSettings appSettings);

        List<HistoryAuftrag> GetHistoryAuftraege(HistoryAuftragSelector filter);

        List<WebUploadProtokoll> GetProtokollArten();
    }
}
