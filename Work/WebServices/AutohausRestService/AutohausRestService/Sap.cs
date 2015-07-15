using SapORM.Contracts;
using SapORM.Services;

namespace AutohausRestService
{
    public class S
    {
        private ISapDataService _sapDataService;

        public ISapDataService AP
        {
            get { return _sapDataService ?? (_sapDataService = SapDataServiceFactory.Create()); }
        }

        public ISapDataServiceFactory SapDataServiceFactory
        {
            get { return new SapDataServiceFromConfigNoCacheFactory(); }
        }
    }
}