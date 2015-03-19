using System.Web;
using CkgDomainLogic.General.Services;
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

            DashboardAppUrlService.InvokeViewModelForAppUrl("mvc/Autohaus/ZulassungsReport/Index", "ZulassungenProKundeUndMonat", iocContainer);
        }
    }
}
