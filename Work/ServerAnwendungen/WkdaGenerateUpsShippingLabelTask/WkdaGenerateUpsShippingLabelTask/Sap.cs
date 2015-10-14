using SapORM.Contracts;
using SapORM.Services;

namespace WkdaGenerateUpsShippingLabelTask
{
    public class S
    {
        private static ISapDataService sapDataService;

        public static ISapDataService AP
        {
            get { return sapDataService ?? (sapDataService = SapDataServiceFactory.Create()); }
        }

        public static ISapDataServiceFactory SapDataServiceFactory
        {
            get { return new SapDataServiceFromConfigNoCacheFactory(); }
        }
    }
}
