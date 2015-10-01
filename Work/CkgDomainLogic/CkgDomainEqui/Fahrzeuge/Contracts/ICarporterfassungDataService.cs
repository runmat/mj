﻿using System.Collections.Generic;
﻿using CkgDomainLogic.Fahrzeuge.Models;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface ICarporterfassungDataService
    {
        CarporterfassungModel LoadFahrzeugdaten(string kennzeichen, string bestandsnummer, string fin);

        List<CarporterfassungModel> SaveFahrzeuge(List<CarporterfassungModel> items);

        CarportInfo GetCarportInfo(string carportId);

        IEnumerable<string> GetCarportPdis();
    }
}
