using SapORM.Contracts;

namespace SapORM.Services
{
    public class SapDataServiceTestSystemNoCacheFactory : ISapDataServiceFactory
    {
        public ISapDataService Create()
        {
            return new SapDataService(new SapConnectionTestSystem(), new DynSapProxyErp35NoCacheFactory());
        }
    }
}
