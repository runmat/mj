//using CkgDomainLogic.General.Contracts;
//using CkgDomainLogic.General.Services;
//using CkgDomainLogic.Services;
//using CkgDomainLogic.Ueberfuehrung.Contracts;
//using CkgDomainLogic.Ueberfuehrung.Models;
//using CkgDomainLogic.Ueberfuehrung.Services;
//using CkgDomainLogic.Ueberfuehrung.ViewModels;
//using GeneralTools.Contracts;
//using GeneralTools.Models;
//using GeneralTools.Services;
//using NUnit.Framework;
//using SapORM.Contracts;
//using SapORM.Services;
//using GeneralAppModelMappings = CkgDomainLogic.General.Models;

//namespace CkgDomainLogic.UnitTests.Ueberfuehrung.SAP
//{
//    [TestFixture]
//    public class GeneralTests : Store
//    {
//        ISapDataService Sap { get { return PropertyCacheGet(() => new SapDataServiceTestSystemNoCacheFactory().Create()); } }

//        static IAppSettings TestAppSettings { get { return new CkgDomainAppSettings(); } }

//        ILogonContextDataService TestLogonContext 
//        { 
//            get 
//            { 
//                return PropertyCacheGet(() => new LogonContextTest(new Localize())
//                {
//                    KundenNr = "10000649".ToSapKunnr(),
//                    GroupName = "ABD_AlbertBauerDig"
//                }); 
//            } 
//        }

//        IUeberfuehrungDataService TestUeberfuehrungDataService
//        {
//            get
//            {
//                return PropertyCacheGet(() =>
//                    {
//                        var service = new UeberfuehrungDataServiceSAP(Sap);
//                        service .Init(TestAppSettings, TestLogonContext);
//                        return service;
//                    });
//            }
//        }

//        public UeberfuehrungViewModel ViewModel
//        {
//            get
//            {
//                return PropertyCacheGet(() => new UeberfuehrungViewModel(TestUeberfuehrungDataService, TestAppSettings, TestLogonContext));
//            }
//        }


//        [Test]
//        public void AppModelMappings()
//        {
//            var mappingErrors = new GeneralAppModelMappings.AppModelMappings().GetMappingErrors();
//            Assert.IsTrue(mappingErrors.IsNullOrEmpty(), mappingErrors);
//        }
//    }
//}
