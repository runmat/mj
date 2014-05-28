// ReSharper disable RedundantUsingDirective
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective
using NUnit.Framework;
using SapORM.Contracts;
using SapORM.Models;
using SapORM.Services;

namespace SapORM.UnitTests
{
    [TestFixture]
    public class Tests
    {
        private static ISapDataService _sap;
        static ISapDataService Sap { get { return (_sap ?? (_sap = new SapDataServiceFromConfigNoCacheFactory().Create())); } }

        public Tests()
        {
            ConfigurationMerger.MergeTestWebConfigAppSettings();
        }


        #region Tests

        [Test]
        public void SimpleTest()
        {
            var a = 0;
            var b = 0;

            Assert.AreEqual(a, b);
        }

        [Test]
        public void SapTest()
        {
            var list = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportListWithInitExecute(Sap,
                "I_KUNNR, I_KENNUNG, I_POS_KURZTEXT",
                "0000315638", "", "10");
            if (list == null)
            {
                Assert.IsNull(list);
                return;
            }
            Assert.AreEqual(1, list.Count);
        }

        #endregion
    }
}
