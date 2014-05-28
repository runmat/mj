using System;
using System.Globalization;
using System.Threading;
using AutoAct.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapORM.Models;

namespace AutoActTests.Bapi
{
    [TestClass]
    public class AuctionTests
    {
        DateTime _start;
        DateTime _end;
        private const string GivenTime = "111500";
        private const string FreigabePreis = "6.000";
        private const string AusrufePreis = "5.000";
        private Z_DPM_READ_AUTOACT_01.GT_OUT _quelle = null;


        [TestInitialize]
        public void Setup()
        {
            _start = DateTime.Now.Date;
            _end = _start.AddDays(10);
            _quelle = new Z_DPM_READ_AUTOACT_01.GT_OUT
                {
                    STARTDATUM = _start,
                    ENDDATUM = _end,
                    STARTUHRZEIT = GivenTime,
                    ENDUHRZEIT = GivenTime,
                    FREIGABEPREIS_C = FreigabePreis,
                    AUSRUFPREIS_C = AusrufePreis
                };
            // Achtung: Culture auf Invariant setzen damit das Parsen der Werte aus SAP funktioniert
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        [TestMethod]
        public void StartAndEndTimeIfAvailableAreAddedToStartAndEndDateDate()
        {
            // Arrange
            
            // Act
            var result = new Auction(_quelle);

            // Assert
            Assert.AreEqual(11, result.StartDate.Hour);
            Assert.AreEqual(11, result.EndDate.Hour);
            Assert.AreEqual(15, result.StartDate.Minute);
            Assert.AreEqual(15, result.EndDate.Minute);
            Assert.AreEqual(decimal.Parse(FreigabePreis), result.GrossClearancePrice);
            Assert.AreEqual(decimal.Parse(AusrufePreis), result.GrossStartingPrice);
        }
    }
}
