using System.Collections.Generic;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.CoC.Contracts
{
    public interface IZulassungDataService : ICkgGeneralDataService
    {
        #region Zulassung

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

        #endregion


        #region Sendungsaufträge

        List<SendungsAuftrag> GetSendungsAuftraege(SendungsAuftragSelektor model);

        #endregion

        #region Sendungsaufträge, nach ID

        List<SendungsAuftrag> GetSendungsAuftraegeId(SendungsAuftragIdSelektor model);

        #endregion

        #region Sendungsaufträge, nach Docs

        List<SendungsAuftrag> GetSendungsAuftraegeDocs(SendungsAuftragDocsSelektor model);

        #endregion

        #region Sendungsaufträge, nach Fin

        List<SendungsAuftrag> GetSendungsAuftraegeFin(SendungsAuftragFinSelektor model);

        #endregion
    }
}
