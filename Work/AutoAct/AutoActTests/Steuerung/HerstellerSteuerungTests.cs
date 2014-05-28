using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Rest;
using AutoAct.Steuerung;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoActTests.Steuerung
{
    [TestClass]
    public class HerstellerSteuerungTests
    {
        private Mock<IAutoActRest> _autoActRest;
        private Mock<IConsoleWrapper> _consoleWrapper;
        private HerstellerSteuerung _herstellerSteuerung;
        private const string LocalizedName = "Audi";
        private const string Make = "AUDI";
        private const string Model = "A3";
        private AutoActMake _make0 = new AutoActMake{ LanguageCode = "de", LocalizedName = LocalizedName, Make = Make };
        private readonly Result<AutoActMakesResult> _makes = new Result<AutoActMakesResult>{ Value = new AutoActMakesResult() };
            
        [TestInitialize]
        public void Setup()
        {
            _makes.Value.Add(new AutoActMake{ LanguageCode = "de", LocalizedName = LocalizedName, Make = Make });
            _autoActRest = new Mock<IAutoActRest>(MockBehavior.Strict);
            _consoleWrapper = new Mock<IConsoleWrapper>(MockBehavior.Loose);
            _herstellerSteuerung = new HerstellerSteuerung(_autoActRest.Object, _consoleWrapper.Object);
        }

        private void VerifyAll()
        {
            _autoActRest.VerifyAll();
        }

        [TestMethod]
        public void InvokingGetHerstellerAndModelWithEmptyStringWritesErrorToStringbuilder()
        {
            // Arrange
            var sb = new StringBuilder();

            // Act
            _herstellerSteuerung.GetHerstellerAndModel(string.Empty, new Vehicle(), sb);

            // Assert
            VerifyAll();
            Assert.AreNotEqual(0, sb.Length);
        }

        [TestMethod]
        public void InvokingGetHerstellerAndModelForUnavailableMakeWritesErrorToStringBuilder()
        {
            // Arrange
            var sb = new StringBuilder();
            var zzfabrikname = string.Concat("KKKK", " ", "ZZZZ YYYY");
            _autoActRest.Setup(x => x.GetMakes(VehicleType.Car)).Returns(_makes).Verifiable();

            // Act
            _herstellerSteuerung.GetHerstellerAndModel(zzfabrikname, new Vehicle(), sb);

            // Assert
            VerifyAll();
            Assert.AreNotEqual(0, sb.Length);
        }
    }
}
