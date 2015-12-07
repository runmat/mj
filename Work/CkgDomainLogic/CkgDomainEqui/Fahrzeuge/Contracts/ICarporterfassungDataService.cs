﻿using System.Collections.Generic;
﻿using CkgDomainLogic.Fahrzeuge.Models;
﻿using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Fahrzeuge.Contracts
{
    public interface ICarporterfassungDataService : ICkgGeneralDataService
    {
        List<CarporterfassungModel> SaveFahrzeuge(List<CarporterfassungModel> items, ref string errorMessage, bool nacherfassung);

        List<CarportInfo> GetCarportAdressen(string adressKennung);

        IDictionary<string, string> GetCarportPdis(string adressKennung);

        List<CarporterfassungModel> GetFahrzeuge(CarportnacherfassungSelektor selektor);

        IDictionary<string, string> GetCustomerOrganizations();
    }
}
