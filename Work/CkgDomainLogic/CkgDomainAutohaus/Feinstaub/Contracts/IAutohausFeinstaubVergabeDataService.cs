using System.Collections.Generic;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Feinstaub.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Feinstaub.Contracts
{
    public interface IAutohausFeinstaubVergabeDataService : ICkgGeneralDataService
    {
        List<Kundenstammdaten> Kundenstamm { get; }

        List<Domaenenfestwert> Kraftstoffcodes { get; }

        List<Domaenenfestwert> Plakettenarten { get; }

        void MarkForRefreshKundenstamm();

        void MarkForRefreshKraftstoffcodes();

        void MarkForRefreshPlakettenarten();

        string CheckFeinstaubVergabe(FeinstaubCheck pruefKriterien, out string plakettenart);

        string SaveFeinstaubVergabe(FeinstaubVergabe feinstaubDaten);
    }
}

