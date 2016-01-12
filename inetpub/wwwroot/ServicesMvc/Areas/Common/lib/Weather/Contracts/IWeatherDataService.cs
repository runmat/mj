using GeneralTools.Models;

namespace CkgDomainLogic.General.Contracts
{
    public interface IWeatherDataService
    {
        string ConfigurationContextKey { get; }

        JsonItemsPackage RequestGetWeatherData(string cityAndCountry);
    }
}
