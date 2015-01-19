using CkgDomainLogic.Services;
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

            DashboardService.InvokeViewModelForAppUrl("mvc/Autohaus/ZulassungsReport/Index", iocContainer);
        }
    }
}
