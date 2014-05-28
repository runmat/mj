using MvcTools.Web;
using SapORM.Contracts;
using SapORM.Services;

namespace CkMobileMvcTools.Services
{
    public class S
    {
        public static ISapDataService AP
        {
            get { return SessionStore<ISapDataService>.GetModel(SapDataServiceFactory.Create); }
        }

        public static ISapDataServiceFactory SapDataServiceFactory
        {
            get { return new SapDataServiceDefaultFactory(); }
        }
    }
}

