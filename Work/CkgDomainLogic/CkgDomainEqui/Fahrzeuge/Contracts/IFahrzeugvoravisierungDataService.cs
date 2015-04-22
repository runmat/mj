using System.Collections.Generic;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface IFahrzeugvoravisierungDataService
    {
               
        //TreuhandverwaltungSelektor GetBerechtigungenFromSap(TreuhandverwaltungSelektor selector);

        //void ValidateUpload(TreuhandverwaltungSelektor selector);

        bool SaveUploadItems(List<FahrzeugvoravisierungUploadModel> uploadItems);

      
    }
}
