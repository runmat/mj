using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.CoC.Contracts
{
    public interface IZulassungDataService : ICkgGeneralDataService
    {
        string AuftragsNummer { get; set; }

        string SaveZulassung(
            Adresse auftraggeberAdresse, 
            Adresse halterAdresse,
            Adresse reguliererAdresse,
            Adresse rechnungsEmpfaengerAdresse,
            Adresse versicherungsNehmerAdresse, 
            Adresse versandScheinSchilderAdresse, 
            Adresse versandZb2CocAdresse,

            ZulassungsOptionen zulassungsOptionen,
            ZulassungsDienstleistungen zulassungsDienstleistungen,
            Versicherungsdaten versicherungsdaten,
            WunschkennzeichenOptionen wunschkennzeichen
            );
    }
}
