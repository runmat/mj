using System;
using System.Web;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
using MvcTools.Web;
using SapORM.Services;
using ServicesMvc;

namespace ServicesMvcTest
{
    class Program
    {
        static void Main()
        {
            var dateRange = new DateRange(DateRangeType.Last3Months);

            var sDateRange = XmlService.XmlSerializeToString(dateRange);
            sDateRange = sDateRange.Replace("12-01", "12-02");

            var oDateRange = XmlService.XmlDeserializeFromString<DateRange>(sDateRange);
            
            var rangeType = dateRange.RangeType;
            
            dateRange.StartDate = DateTime.Today.AddDays(2);
            dateRange.EndDate = DateTime.Today.AddDays(2);
            rangeType = dateRange.RangeType;

            dateRange.StartDate = DateTime.Today.AddDays(-1);
            dateRange.EndDate = DateTime.Today.AddDays(-1);
            rangeType = dateRange.RangeType;

            dateRange.RangeType = DateRangeType.Last7Days;
            rangeType = dateRange.RangeType;

            return;


            var sap = new SapDataServiceDefaultFactory().Create();
            var iocContainer = IocConfig.CreateIocContainerAndRegisterTypes(sap);

            HttpContext.Current = SessionHelper.FakeHttpContext();

            DashboardAppUrlService.InvokeViewModelForAppUrl("mvc/Autohaus/ZulassungsReport/Index", "ZulassungenProKundeUndMonat", iocContainer);
        }
    }
}
