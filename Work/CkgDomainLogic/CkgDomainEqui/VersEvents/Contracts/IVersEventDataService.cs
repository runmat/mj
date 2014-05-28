using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.VersEvents.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.VersEvents.Contracts
{
    public interface IVersEventsDataService : ICkgGeneralDataService 
    {
        List<SelectItem> Versicherungen { get; }



        List<Vorgang> VorgaengeGet();

        Vorgang VorgangAdd(Vorgang item);

        bool VorgangDelete(int id);

        Vorgang VorgangSave(Vorgang item, Action<string, string> addModelError);


        List<VorgangTermin> TermineGet(VorgangTerminSelector selector);
        
        VorgangTermin TerminAdd(VorgangTermin item);
        
        bool TerminDelete(int id);
        
        VorgangTermin TerminSave(VorgangTermin item, Action<string, string> addModelError);


        
        List<VersEvent> VersEventsGet(VersEventSelector selector = null);

        VersEvent VersEventAdd(VersEvent item);

        bool VersEventDelete(int id);

        VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError);



        List<VersEventOrt> VersEventOrteGet(VersEvent versEvent = null);

        VersEventOrt VersEventOrtAdd(VersEventOrt item);

        bool VersEventOrtDelete(int id);

        VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError);



        List<VersEventOrtBox> VersEventOrtBoxenGet(VersEventOrt versEventOrt);

        VersEventOrtBox VersEventOrtBoxAdd(VersEventOrtBox item);

        bool VersEventOrtBoxDelete(int id);

        VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError);
    }
}
