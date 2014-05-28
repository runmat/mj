//using CkgDomainLogic.DomainCommon.Contracts;
//using CkgDomainLogic.DomainCommon.Models;
//using CkgDomainLogic.DomainCommon.Services;
//using CkgDomainLogic.General.Contracts;
//using CkgDomainLogic.General.Services;
//using CkgDomainLogic.Services;
//using GeneralTools.Contracts;
//using GeneralTools.Services;
//using GeneralTools.Models;
//using NUnit.Framework;
//using SapORM.Contracts;
//using SapORM.Services;

//namespace CkgDomainLogic.UnitTests.DomainCommon.SAP
//{
//    [TestFixture]
//    public class AdressenPflegeTests : Store 
//    {
//        ISapDataService Sap { get { return PropertyCacheGet(() => new SapDataServiceTestSystemNoCacheFactory().Create()); } }

//        static IAppSettings TestAppSettings { get { return new CkgDomainAppSettings(); } }

//        ILogonContextDataService TestLogonContext { get { return PropertyCacheGet(() => new LogonContextTest(new Localize()) { KundenNr = "0000999998" }); } }

//        IAdressenDataService TestAdressenDataService 
//        { 
//            get 
//            { 
//                return PropertyCacheGet(() =>
//                    {
//                        var service = new AdressenDataServiceSAP(Sap);
//                        service.Init(TestAppSettings, TestLogonContext);
//                        return service;
//                    });
//            }
//        }

//        [Test]
//        public void AppModelMappings()
//        {
//            var mappingErrors = new AppModelMappings().GetMappingErrors();
//            Assert.IsTrue(mappingErrors.IsNullOrEmpty(), mappingErrors);
//        }

//        //[Test]
//        //public void AddressInsertWithProofReading()
//        //{
//        //    ViewModel.DataMarkForRefresh();
//        //    var adressen = ViewModel.Adressen;
//        //}
//    }
//}
