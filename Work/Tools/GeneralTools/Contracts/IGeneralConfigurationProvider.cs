namespace GeneralTools.Contracts
{
    public interface IGeneralConfigurationProvider
    {
        string GetConfigVal(string context, string keyName);

        void SetConfigVal(string context, string keyName, string value);
    }
}
