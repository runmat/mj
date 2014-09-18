using System;
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.DomainCommon.Contracts
{
    public interface IAdressenDataService : ICkgGeneralDataService
    {
        List<Adresse> Adressen { get; }

        List<Adresse> RgAdressen { get; }
        List<Adresse> ReAdressen { get; }
        Adresse AgAdresse { get; }

        List<Adresse> ZulassungsStellen { get; }


        void MarkForRefreshAdressen();

        Adresse SaveAdresse(Adresse adresse, Action<string, string> addModelError);

        void DeleteAdresse(Adresse adresse);
    }
}
