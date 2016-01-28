using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.Autohaus.Contracts
{
    public interface ICocAnforderungDataService : ICkgGeneralDataService
    {
        string GetEmpfaengerEmailAdresse();
    }
}
