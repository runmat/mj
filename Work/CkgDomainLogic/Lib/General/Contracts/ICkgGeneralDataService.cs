using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Contracts
{
    public interface ICkgGeneralDataService
    {
        List<KundeAusHierarchie> KundenAusHierarchie { get; }

        List<Land> Laender { get; }

        List<SelectItem> Versicherungen { get; }

        List<VersandOption> VersandOptionen { get; }

        List<ZulassungsOption> ZulassungsOptionen { get; }

        List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; }

        List<FahrzeugStatus> FahrzeugStatusWerte { get; }

        List<Hersteller> Hersteller { get; }

        string ToDataStoreKundenNr(string kundenNr);

        string GetZulassungskreisFromPostcodeAndCity(string postCode, string city);

        void Init(IAppSettings appSettings, ILogonContext logonContext);
    }
}
