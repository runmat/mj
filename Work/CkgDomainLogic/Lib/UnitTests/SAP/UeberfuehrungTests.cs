//using System;
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

//namespace CkgDomainLogic.UnitTests.Ueberfuehrung.SAP
//{
//    [TestFixture]
//    public class UeberfuehrungTests : Store
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
//                        service.Init(TestAppSettings, TestLogonContext);
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

//        public UeberfuehrungHistoryViewModel HistoryViewModel
//        {
//            get
//            {
//                return PropertyCacheGet(() => new UeberfuehrungHistoryViewModel(TestUeberfuehrungDataService, TestAppSettings, TestLogonContext));
//            }
//        }


//        [Test]
//        public void AppModelMappings()
//        {
//            var mappingErrors = new AppModelMappings().GetMappingErrors();
//            Assert.IsTrue(mappingErrors.IsNullOrEmpty(), mappingErrors);
//        }

//        [Test]
//        public void GetHistoryAuftraege()
//        {
//            var filter = new HistoryAuftragFilter
//                {
//                    KundenNr = TestLogonContext.KundenNr.ToSapKunnr(),
//                    ErfassungsDatumVon = DateTime.Now.AddDays(-60),
//                    ErfassungsDatumBis = DateTime.Now.AddDays(0),
//                    AlleOrganisationen = true,
//                    AuftragsArt = "A",
//                };
//            var auftraege = HistoryViewModel.GetHistoryAuftraege(filter);
//            Assert.AreNotEqual(0, auftraege.Count);
//        }
//    }
//}
