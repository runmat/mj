using SapORM.Contracts;
using SapORM.Services;

namespace CarDocuWebService.SAP
{
    public class Sap
    {
        public static ISapDataService CreateDataService()
        {
            return new SapDataServiceDefaultFactory().Create();
        }
    }
}