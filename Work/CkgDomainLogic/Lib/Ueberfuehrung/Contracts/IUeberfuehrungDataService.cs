using System.Collections.Generic;
using System.Web;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Ueberfuehrung.Models;
using GeneralTools.Contracts;

namespace CkgDomainLogic.Ueberfuehrung.Contracts
{
    public interface IUeberfuehrungDataService : ICkgGeneralDataService
    {
        string AuftragGeber { get; set; }

        List<WebUploadProtokoll> WebUploadProtokolle { get; }

        List<Adresse> GetFahrtAdressen(string[] addressTypes);

        List<Adresse> GetRechnungsAdressen();

        //List<KclGruppe> GetKclGruppenDaten();

        List<HistoryAuftrag> GetHistoryAuftraege(HistoryAuftragFilter filter);

        void GetTransportTypenAndDienstleistungen(out List<TransportTyp> transportTypen,
                                                  out List<Dienstleistung> dienstleistungen);

        bool TryLoadFahrzeugFromFIN(ref Fahrzeug modelFahrzeug);

        bool SaveUploadFile(HttpPostedFileBase file, string fahrtIndex, out string fileName, out string errorMessage);

        List<UeberfuehrungsAuftragsPosition> Save(Step[] steps, List<Fahrt> fahrten);

        string GetUploadPathTemp();
        void OnInit(ILogonContext logonContext, IAppSettings appSettings);
    }
}
