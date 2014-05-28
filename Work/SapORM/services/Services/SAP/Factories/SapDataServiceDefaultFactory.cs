using SapORM.Contracts;

namespace SapORM.Services
{
    public class SapDataServiceDefaultFactory : ISapDataServiceFactory 
    {
        public ISapDataService Create()
        {
            return new SapDataService(new SapConnectionFromConfig(), new DynSapProxyErp35SqlCacheFactory());
        }
    }
}
