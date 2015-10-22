﻿using System.Collections.Generic;
﻿using CkgDomainLogic.Fahrzeuge.Models;
﻿using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface ICarporterfassungDataService : ICkgGeneralDataService
    {
        CarporterfassungModel LoadFahrzeugdaten(string kennzeichen, string bestandsnummer, string fin);

        List<CarporterfassungModel> SaveFahrzeuge(List<CarporterfassungModel> items, ref string errorMessage);

        List<CarportInfo> GetCarportAdressen(string adressKennung);

        IDictionary<string, string> GetCarportPdis();
    }
}
