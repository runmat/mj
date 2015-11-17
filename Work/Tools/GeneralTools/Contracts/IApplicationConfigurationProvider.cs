namespace GeneralTools.Contracts
{
    public interface IApplicationConfigurationProvider
    {
        string GetApplicationConfigVal(string keyName, string appID, int customerID = 0, int groupID = 0);
    }
}
