namespace GeneralTools.Contracts
{
    public interface ICustomerConfigurationProvider
    {
        string GetCustomerConfigVal(string keyName, int customerId);

        void SetCustomerConfigVal(string keyName, string value, int customerId);

        string GetCurrentCustomerConfigVal(string keyName);
        string GetCurrentBusinessCustomerConfigVal(string keyName);

        void SetCurrentCustomerConfigVal(string keyName, string value);
        void SetCurrentBusinessCustomerConfigVal(string keyName, string value);
    }
}
