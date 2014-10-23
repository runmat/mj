using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Equi.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IAbweichungenDataService : ICkgGeneralDataService
    {
        #region Halterabweichungen

        List<Halterabweichung> Halterabweichungen { get; }

        void MarkForRefreshHalterabweichungen();

        List<Halterabweichung> SaveHalterabweichungen(List<Halterabweichung> vorgaenge, ref string message);

        #endregion


        #region Versandabweichungen

        List<Fahrzeugbrief> VersandAbweichungen { get; }

        string SaveVersandAbweichungMemo(string equiNr, string memo, DateTime? ausgangsDatum);

        string SaveVersandAbweichungAsErledigt(string equiNr, string memo, DateTime? ausgangsDatum);

        #endregion
    }
}
