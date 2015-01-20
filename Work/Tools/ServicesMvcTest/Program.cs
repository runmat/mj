using System.Web;
using CkgDomainLogic.Services;
using MvcTools.Web;
using SapORM.Services;
using ServicesMvc;

namespace ServicesMvcTest
{
    class Program
    {
        static void Main()
        {
            var sap = new SapDataServiceDefaultFactory().Create();
            var iocContainer = IocConfig.CreateIocContainerAndRegisterTypes(sap);

            HttpContext.Current = SessionHelper.FakeHttpContext();

            DashboardService.InvokeViewModelForAppUrl("mvc/Autohaus/ZulassungsReport/Index", iocContainer);
        }
    }
}
