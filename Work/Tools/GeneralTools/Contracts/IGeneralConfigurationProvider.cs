using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IGeneralConfigurationProvider
    {
        string GetConfigVal(string context, string keyName);

        IDictionary<string, string> GetConfigAllServersVals(string context, string connectionString = null, string filterClause = null);


        void SetConfigVal(string context, string keyName, string value, string connectionString = null);
    }
}
