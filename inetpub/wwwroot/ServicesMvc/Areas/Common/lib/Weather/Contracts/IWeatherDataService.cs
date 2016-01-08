using System.Collections.Generic;

namespace CkgDomainLogic.General.Contracts
{
    public interface IWeatherDataService
    {
        string GetWeatherData(string cityAndCountry);
    }
}
