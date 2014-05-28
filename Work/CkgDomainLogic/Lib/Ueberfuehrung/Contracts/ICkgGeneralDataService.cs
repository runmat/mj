using System.Collections.Generic;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Contracts
{
    public interface ICkgGeneralDataService
    {
        List<Land> Laender { get; }

        List<VersandOption> VersandOptionen { get; }

        List<ZulassungsOption> ZulassungsOptionen { get; }

        List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; }

        List<FahrzeugStatus> FahrzeugStatusWerte { get; }

        string ToDataStoreKundenNr(string kundenNr);

        string GetZulassungskreisFromPostcodeAndCity(string postCode, string city);

        void Init(IAppSettings appSettings, ILogonContext logonContext);
    }
}
