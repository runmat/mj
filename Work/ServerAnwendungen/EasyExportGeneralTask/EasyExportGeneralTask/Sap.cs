using SapORM.Contracts;
using SapORM.Services;

namespace EasyExportGeneralTask
{
    public class S
    {
        private static ISapDataService sapDataService;

        public static ISapDataService AP
        {
            get 
            { 
                if (sapDataService == null)
                {
                    sapDataService = SapDataServiceFactory.Create();
                }

                return sapDataService;
            }
        }

        public static ISapDataServiceFactory SapDataServiceFactory
        {
            get { return new SapDataServiceFromConfigNoCacheFactory(); }
        }
    }
}
