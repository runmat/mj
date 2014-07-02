using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IBriefversandDataService : ICkgGeneralDataService
    {
        List<Fahrzeugbrief> Fahrzeugbriefe { get; }

        void MarkForRefreshFahrzeugbriefe();

        //string AuftragsNummer { get; set; }

        //string SaveZulassung(
        //    Adresse auftraggeberAdresse,
        //    Adresse halterAdresse,
        //    Adresse reguliererAdresse,
        //    Adresse rechnungsEmpfaengerAdresse,
        //    Adresse versicherungsNehmerAdresse,
        //    Adresse versandScheinSchilderAdresse,
        //    Adresse versandZb2CocAdresse,

        //    ZulassungsOptionen zulassungsOptionen,
        //    ZulassungsDienstleistungen zulassungsDienstleistungen,
        //    Versicherungsdaten versicherungsdaten,
        //    WunschkennzeichenOptionen wunschkennzeichen
        //    );
    }
}
