namespace GeneralTools.Contracts
{
    public interface IGeneralConfigurationProvider
    {
        string GetConfigVal(string context, string keyName);
    }
}
