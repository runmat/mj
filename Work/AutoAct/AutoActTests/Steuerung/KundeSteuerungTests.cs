using System;
using System.Collections.Generic;
using AutoAct.Bapi;
using AutoAct.Interfaces;
using AutoAct.Steuerung;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapORM.Models;

namespace AutoActTests.Steuerung
{
    [TestClass]
    public class KundeSteuerungTests
    {
        private Mock<ILogService> _logService;
        private Mock<IAutoActRest> _autoActRest;
        private Mock<IAutoActBapi> _autoActBapi;
        private Mock<IFahrzeugSteuerung> _fahrzeugSteuerung;
        private Mock<IConsoleWrapper> _consoleWrapper;

        private const string AnmeldeName = "ASDF/(%&/(%&";
        private const string Passwort = "jurtzhdgf";
        private const string Kunnr = "1234123412341";

        private KundeSteuerung _kundeSteuerung;
        private Kunde _kunde;

        [TestInitialize]
        public void Setup()
        {
            _logService = new Mock<ILogService>(MockBehavior.Strict);
            _autoActRest = new Mock<IAutoActRest>(MockBehavior.Strict);
            _autoActBapi = new Mock<IAutoActBapi>(MockBehavior.Strict);
            _fahrzeugSteuerung = new Mock<IFahrzeugSteuerung>(MockBehavior.Strict);
            _consoleWrapper = new Mock<IConsoleWrapper>(MockBehavior.Strict);
            _kundeSteuerung = new KundeSteuerung(_logService.Object, _autoActRest.Object, _autoActBapi.Object, _fahrzeugSteuerung.Object,  _consoleWrapper.Object);
            _kunde = new Kunde
                {
                    Anmeldename = AnmeldeName,
                    Passwort = Passwort,
                    Nummer = Kunnr
                };
        }
        
        private void VerifyAll()
        {
            _logService.VerifyAll();
            _autoActRest.VerifyAll();
            _autoActBapi.VerifyAll();
            _fahrzeugSteuerung.VerifyAll();
            _consoleWrapper.VerifyAll();
        }

        [TestMethod]
        public void InvokingExecuteCallsIsAliveAndAbortsIfNotAlive()
        {
            // Arrange
            _consoleWrapper.Setup(x => x.WriteInfo(It.IsAny<string>())).Verifiable();
            _autoActRest.Setup(x => x.SetDiegstAuthenticator(AnmeldeName, Passwort)).Verifiable();
            _autoActRest.Setup(x => x.IsAlive()).Returns(false).Verifiable();
            _logService.Setup(x => x.LogError(It.Is<ApplicationException>(ex => ex.Message.Contains(Kunnr)), null, null)).Returns(new LogItem()).Verifiable();
            _consoleWrapper.Setup(x => x.WriteError(It.Is<string>(ex => ex.Contains(Kunnr)))).Verifiable();


            // Act
            _kundeSteuerung.Execute(_kunde);

            // Assert
            VerifyAll();
        }

        [TestMethod]
        public void InvokingExecuteCallsIsAliveFetchesFahrzeugeAndCallsFahzeugSteuerung()
        {
            // Arrange
            _consoleWrapper.Setup(x => x.WriteInfo(It.IsAny<string>())).Verifiable();
            var fahrzeuge = new List<Z_DPM_READ_AUTOACT_01.GT_OUT>();
            _autoActRest.Setup(x => x.SetDiegstAuthenticator(AnmeldeName, Passwort)).Verifiable();
            _autoActRest.Setup(x => x.IsAlive()).Returns(true).Verifiable();
            _autoActBapi.Setup(x => x.GetVehiclesToExportPerKunde(Kunnr)).Returns(fahrzeuge).Verifiable();
            _fahrzeugSteuerung.Setup(x => x.Execute(_kunde, fahrzeuge)).Verifiable();

            // Act
            _kundeSteuerung.Execute(_kunde);

            // Assert
            VerifyAll();
        }
    }
}
