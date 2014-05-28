using AutoAct.Bapi;
using AutoAct.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapORM.Contracts;

namespace AutoActTests.Bapi
{
    [TestClass]
    public class AutoActBapiTests
    {
        private Mock<ISapDataService> _sapDataService;
        private AutoActBapi _autoActBapi;

        [TestInitialize]
        public void Setup()
        {
            _sapDataService = new Mock<ISapDataService>(MockBehavior.Strict);
            _autoActBapi = new AutoActBapi(_sapDataService.Object);
        }

        private void VerifyAll()
        {
            _sapDataService.VerifyAll();
        }

        [TestMethod]
        public void ReportVehicleExportSuccessInvokes_Z_DPM_SAVE_STATUS_AUTOACT_O1()
        {
            // Arrange
            const string belegNr = "ASDFASd(&(/";
            const string successStatus = "3";
            const string rueckAutoact = "";
            const long autoactId = 1234;
            _sapDataService.Setup(x => x.InitExecute(ApplicationStrings.Bapi_ReportVehicleExportSuccess, ApplicationStrings.Bapi_ReportVehicleExportSuccess_Params, belegNr, successStatus, rueckAutoact, autoactId.ToString())).Verifiable();

            // Act
            _autoActBapi.ReportVehicleExportSuccess(belegNr, autoactId);
            
            // Assert
            VerifyAll();
        }
    }
}
