using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Insurance.Models;

namespace CkgDomainLogic.Insurance.Contracts
{
    public interface ISchadenakteDataService : ICkgGeneralDataService
    {
        List<SchadenfallStatusArt> SchadenfallStatusArtenGet(string languageKey);

        List<SchadenfallStatus> SchadenfallStatusWerteGet(string languageKey, int? schadenfallID = null);

        bool SchadenfallStatusWertSave(SchadenfallStatus schadenfallStatus, Action<string, string> addModelError);


        List<Schadenfall> SchadenfaelleGet();

        Schadenfall SchadenfallAdd(Schadenfall item, Action<string, string> addModelError);

        bool SchadenfallDelete(int id);

        Schadenfall SchadenfallSave(Schadenfall item, Action<string, string> addModelError);
    }
}
