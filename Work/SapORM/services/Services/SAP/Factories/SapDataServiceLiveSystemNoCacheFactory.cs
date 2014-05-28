using SapORM.Contracts;

namespace SapORM.Services
{
    public class SapDataServiceLiveSystemNoCacheFactory : ISapDataServiceFactory
    {
        public ISapDataService Create()
        {
            return new SapDataService(new SapConnectionLiveSystem(), new DynSapProxyErp35NoCacheFactory());
        }
    }
}
