using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeugSperrenVerschiebenDataService : ICkgGeneralDataService
    {
        List<FahrzeuguebersichtPDI> GetPDIStandorte();

        List<Fahrzeuguebersicht> GetFahrzeuge();

        int FahrzeugeSperren(bool sperren, string sperrText, ref List<Fahrzeuguebersicht> fahrzeuge);

        int FahrzeugeVerschieben(string zielPdi, ref List<Fahrzeuguebersicht> fahrzeuge);

        int FahrzeugeTexteErfassen(string bemerkungIntern, string bemerkungExtern, ref List<Fahrzeuguebersicht> fahrzeuge);



        List<FahrzeugVersand> GetFahrzeugVersendungen(string landCode, bool? gesperrte);

        string FahrzeugeVersendungenSperren(bool sperren, List<FahrzeugVersand> fahrzeuge);
    }
}
