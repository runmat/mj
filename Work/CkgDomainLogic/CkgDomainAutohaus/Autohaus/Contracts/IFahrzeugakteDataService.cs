using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Autohaus.Models;

namespace CkgDomainLogic.Autohaus.Contracts
{
    public interface IFahrzeugakteDataService : ICkgGeneralDataService
    {
        List<BeauftragteZulassung> BeauftragteZulassungenGet(int fahrzeugId);

        void BeauftragteZulassungSave(int fahrzeugId, string referenzNr, string fahrgestellNr, string zb2Nr, DateTime? zulassungsDatum);
    }
}
