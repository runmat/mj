using System.Collections.Generic;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.General.Contracts
{
    public interface IGridAdminDataService : ICkgGeneralDataService
    {
        TranslatedResource TranslatedResourceLoad(string resourceKey);
        TranslatedResourceCustom TranslatedResourceCustomerLoad(string resourceKey, int customerID);
        
        List<Customer> GetCustomers();
        List<User> GetUsersForCustomer(Customer customer);
        string GetAppUrl(int appId);
        string GetAppFriendlyName(int appId);

        void TranslatedResourceUpdate(TranslatedResource r);
        void TranslatedResourceDelete(TranslatedResource r);

        void TranslatedResourceCustomerUpdate(TranslatedResourceCustom r);
        void TranslatedResourceCustomerDelete(TranslatedResourceCustom r);

        void TranslationsMarkForRefresh();

        void SetCurrentBusinessCustomerConfigVal(string keyName, string value);
    }
}
