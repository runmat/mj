using SapORM.Contracts;

namespace SapORM.Services
{
    public class SapDataServiceFromConfigNoCacheFactory : ISapDataServiceFactory
    {
        public ISapDataService Create()
        {
            return new SapDataService(new SapConnectionFromConfig(), new DynSapProxyErp35NoCacheFactory());
        }
    }
}
