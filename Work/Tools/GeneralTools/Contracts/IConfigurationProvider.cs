namespace GeneralTools.Contracts
{
    public interface IConfigurationProvider
    {
        string GetConfigValue(string context, string keyName);

        string GetConfigValueForCurrentCustomer(string keyName);
    }
}
