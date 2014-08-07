using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Contracts;

namespace CkgDomainLogic.Uebfuehrg.Contracts
{
    public interface IUebfuehrgDataService : ICkgGeneralDataService 
    {
        string AuftragGeber { get; set; }

        List<Adresse> GetFahrtAdressen(string[] addressTypes);

        List<Adresse> GetRechnungsAdressen();

        void GetTransportTypenAndDienstleistungen(out List<TransportTyp> transportTypen, out List<Dienstleistung> dienstleistungen);

        List<UeberfuehrungsAuftragsPosition> Save(List<Fahrt> fahrten);

        void OnInit(ILogonContext logonContext, IAppSettings appSettings);
    }
}
