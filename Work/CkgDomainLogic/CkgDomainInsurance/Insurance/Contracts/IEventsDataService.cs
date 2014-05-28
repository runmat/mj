using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Insurance.Contracts
{
    public interface IVersEventsDataService : ICkgGeneralDataService 
    {
        List<SelectItem> Versicherungen { get; }


        List<SchadenfallStatusArt> SchadenfallStatusArtenGet(string languageKey);

        List<SchadenfallStatus> SchadenfallStatusWerteGet(string languageKey, int? schadenfallID = null);

        bool SchadenfallStatusWertSave(SchadenfallStatus schadenfallStatus, Action<string, string> addModelError);


        List<Schadenfall> SchadenfaelleGet();

        Schadenfall SchadenfallAdd(Schadenfall item, Action<string, string> addModelError);

        bool SchadenfallDelete(int id);

        Schadenfall SchadenfallSave(Schadenfall item, Action<string, string> addModelError);


        List<TerminSchadenfall> TermineGet(Schadenfall selector = null, int boxID = -1);

        TerminSchadenfall TerminAdd(TerminSchadenfall item, Action<string, string> addModelError);
        
        bool TerminDelete(int id);
        
        TerminSchadenfall TerminSave(TerminSchadenfall item, Action<string, string> addModelError);


        
        List<VersEvent> VersEventsGet();

        VersEvent VersEventAdd(VersEvent item, Action<string, string> addModelError);

        bool VersEventDelete(int id);

        VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError);



        List<VersEventOrt> VersEventOrteGet(VersEvent versEvent = null);

        VersEventOrt VersEventOrtAdd(VersEventOrt item, Action<string, string> addModelError);

        bool VersEventOrtDelete(int id);

        VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError);



        List<VersEventOrtBox> VersEventOrtBoxenGet(VersEventOrt versEventOrt);

        VersEventOrtBox VersEventOrtBoxAdd(VersEventOrtBox item, Action<string, string> addModelError);

        bool VersEventOrtBoxDelete(int id);

        VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError);
    }
}
