using System;
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IAdressenDataService : ICkgGeneralDataService
    {
        string AdressenKennung { get; set; }
        string TranslateFromFriendlyAdressenKennung(string friendlyKennung);

        List<Adresse> Adressen { get; }

        List<Adresse> RgAdressen { get; }
        List<Adresse> ReAdressen { get; }
        Adresse AgAdresse { get; }

        string KundennrOverride { get; set; }
        string SubKundennr { get; set; }

        List<Adresse> ZulassungsStellen { get; }


        void MarkForRefreshAdressen();

        List<Adresse> SaveAdressen(List<Adresse> adressen, Action<string, string> addModelError);

        void DeleteAdresse(Adresse adresse);

        EvbInfo GetEvbVersInfo(string evb, out string message, out bool isValid);
    }
}
