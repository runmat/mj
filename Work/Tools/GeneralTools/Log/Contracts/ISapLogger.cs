namespace GeneralTools.Log.Contracts
{
    interface ISapLogger
    {
        void Log(string anmeldeName, string bapi, string importParameter, string importTable, string dataContext, string logonContext, bool status);
        void Log(string anmeldeName, string bapi, string importParameter, string importTable, string dataContext, string logonContext, bool status, string appName);
    }
}
