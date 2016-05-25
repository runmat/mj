using System.Collections.Generic;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Equi.Contracts
{
    public interface IEquiGrunddatenDataService
    {
        List<SelectItem> GetZielorte();

        List<SelectItem> GetStandorte();

        List<SelectItem> GetBetriebsnummern();

        List<EquiGrunddaten> GetEquis(EquiGrunddatenSelektor suchparameter);
    }
}
