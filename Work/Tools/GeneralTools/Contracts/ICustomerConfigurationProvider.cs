namespace GeneralTools.Contracts
{
    public interface ICustomerConfigurationProvider
    {
        string GetCustomerConfigVal(string keyName, int customerId);

        string GetCurrentCustomerConfigVal(string keyName);
    }
}
