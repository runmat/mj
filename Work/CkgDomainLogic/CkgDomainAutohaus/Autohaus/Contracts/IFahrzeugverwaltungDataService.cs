using System;
using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Autohaus.Models;

namespace CkgDomainLogic.Autohaus.Contracts
{
    public interface IFahrzeugverwaltungDataService : ICkgGeneralDataService
    {
        List<Fahrzeug> FahrzeugeGet();

        Fahrzeug FahrzeugAdd(Fahrzeug item, Action<string, string> addModelError);

        bool FahrzeugDelete(int id);

        Fahrzeug FahrzeugSave(Fahrzeug item, Action<string, string> addModelError);
    }
}
