using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoAct.Bapi;
using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Resources;
using AutoAct.Rest;
using AutoAct.Steuerung;
using GeneralTools.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SapORM.Models;

namespace AutoActTests.Steuerung
{
    [TestClass]
    public class FahrzeugSteuerungTests
    {
        #region Privates, Setup

        private Mock<ILogService> _logService;
        private Mock<IAutoActRest> _autoActRest;
        private Mock<IAutoActBapi> _autoActBapi;
        private Mock<IDokumentSteuerung> _dokumentSteuerung;
        private Mock<IFileHelper> _fileHelper;
        private Mock<IHerstellerSteuerung> _herstellerSteuerung;

        private const string Belegnummer = "2ABEFB62-08A0-49DB-8EF5-3F2E21D016BB";
        private const string Kundennummer = "DCB64DB4-5219-487B-AA89-84CB378A752E";
        private const string Adresse = "8F938512-6380-4A89-88A2-63227324BD83";
        private const string Vin = "0C6DD972-97AD-4FB2-86D6-AE0E602DC799";
        private const string Zustandsbericht = "3A191A68-EA57-4394-AD30-D634B8438F65";
        private const string Schadensbericht = "E5B22FB9-11BD-4D4F-8ED4-BC0D4275B793";
        private const string Wartungssbericht = "BD3CB9FA-C9EE-43DB-A32A-7DD4370DD285";
        private const string Extrasbericht = "A2757684-65A6-46D5-AA96-155579A4850D";
        private const string Make = "Audi";
        private const string Model = "A3";

        private const Int64 AutoactId = 123456789;

        private FahrzeugSteuerung _fahrzeugSteuerung;
        private IEnumerable<Z_DPM_READ_AUTOACT_01.GT_OUT> _fahrzeuge;


        private Kunde _kunde;

        [TestInitialize]
        public void Setup()
        {
            _logService = new Mock<ILogService>(MockBehavior.Strict);
            _autoActRest = new Mock<IAutoActRest>(MockBehavior.Strict);
            _autoActBapi = new Mock<IAutoActBapi>(MockBehavior.Strict);
            _fileHelper = new Mock<IFileHelper>(MockBehavior.Strict);
            _dokumentSteuerung = new Mock<IDokumentSteuerung>(MockBehavior.Strict);
            _herstellerSteuerung = new Mock<IHerstellerSteuerung>(MockBehavior.Strict);
            _fahrzeugSteuerung = new FahrzeugSteuerung(_logService.Object, _autoActRest.Object, _autoActBapi.Object, _dokumentSteuerung.Object, _fileHelper.Object, _herstellerSteuerung.Object);

            // Daten aufbereiten
            var fahrzeug = new Z_DPM_READ_AUTOACT_01.GT_OUT
                {
                    BELEGNR = Belegnummer,
                    ANGEBOTSART = "1", // Bieterverfahren
                    ANZ_HALTER = "1",
                    AUSRUFPREIS_C = "10000",
                    CHASSIS_NUM = Vin,
                    ENDDATUM = DateTime.Now.AddDays(10),
                    ENDUHRZEIT = "110000",
                    FREIGABEPREIS_C = "11000",
                    STARTUHRZEIT = "110000",
                    STARTDATUM = DateTime.Now.Date,
                    KUNNR_AG = Kundennummer,
                    REFERENZ = "REMA",
                    ERSTZULDAT = DateTime.Now.AddMonths(-10),
                    ZZFABRIKNAME = string.Concat(Make, " ", Model)
                };

            _kunde = new Kunde
                {
                    Adresse = Adresse,
                    Nummer = Kundennummer,
                    AnzahlDerBilder = 3
                };

            _fahrzeuge = new List<Z_DPM_READ_AUTOACT_01.GT_OUT> { fahrzeug };

        }

        private void VerifyAll()
        {
            _logService.VerifyAll();
            _autoActRest.VerifyAll();
            _autoActBapi.VerifyAll();
            _dokumentSteuerung.VerifyAll();
            _fileHelper.VerifyAll();
            _herstellerSteuerung.VerifyAll();
        }

        #endregion

        #region Execute

        [TestMethod]
        public void InvokingVehicleWithInvalidAuctionDateWritesErrorBackToSap()
        {
            // Arrange
            _fahrzeuge.First().STARTDATUM = DateTime.Now.AddDays(-2);
            _autoActBapi.Setup(x => x.ReportVehilceExportFailure(Belegnummer, ApplicationStrings.Startdatum_darf_nicht_in_der_Vergangheit_liegen)).Verifiable();

            // Act
            _fahrzeugSteuerung.Execute(_kunde, _fahrzeuge);

            // Assert
            VerifyAll();
        }

        [TestMethod]
        public void InvokingExecuteWithVehicleWithAttachmentsAndPicturesInvokesRestAndWritsBackToBapi()
        {
            // Arrange
            Result<Vehicle> response = new Result<Vehicle>();
            Vehicle vehicle = null;
            _dokumentSteuerung.Setup(x => x.CheckDokumentsForVehicle(Kundennummer, Vin, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Verifiable();
            _herstellerSteuerung.Setup(x => x.GetHerstellerAndModel(_fahrzeuge.First().ZZFABRIKNAME, It.IsAny<Vehicle>(), It.IsAny<StringBuilder>())).Verifiable();
            _autoActRest.Setup(x => x.PostVehicle(It.IsAny<Vehicle>())).Returns(response).Callback<Vehicle>(a =>
                {
                    vehicle = a;
                    a.Id = AutoactId; // Aufruf an Execute setzt die Id des Inserats
                    response.Value = a;
                }).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehicleExportSuccess(Belegnummer, AutoactId)).Verifiable();
            IEnumerable<Attachment> attachments = null;
            _dokumentSteuerung.Setup(x => x.LoadDokumentForVehicle(It.IsAny<Vehicle>(), Kundennummer, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Callback<Vehicle,string,IEnumerable<Attachment>,StringBuilder>((a,b,c,d) => attachments = c ) .Verifiable();
            string[] listOfImages = new string[0];
            _fileHelper.Setup(x => x.GetImageNamesForFahrzeug(Kundennummer, Vin)).Returns(listOfImages).Verifiable();
            _autoActRest.Setup(x => x.PostPictures(AutoactId.ToString(), listOfImages)).Returns(new Result<PicturesResult>()).Verifiable();
            _fahrzeuge.First().ZUSTANDSBERICHT = Zustandsbericht;
            _fahrzeuge.First().UNTERLAGEN1 = Wartungssbericht;
            _fahrzeuge.First().UNTERLAGEN2 = Schadensbericht;
            _fahrzeuge.First().UNTERLAGEN3 = Extrasbericht;

            // Act
            _fahrzeugSteuerung.Execute(_kunde, _fahrzeuge);

            // Assert
            VerifyAll();
            Assert.IsNotNull(vehicle);
            Assert.AreEqual(Zustandsbericht, attachments.Single(x => x.AttachmentType == AttachmentType.STATUS_REPORT).FileName);
            Assert.AreEqual(Schadensbericht, attachments.Single(x => x.AttachmentType == AttachmentType.DAMAGE_REPORT).FileName);
            Assert.AreEqual(Wartungssbericht, attachments.Single(x => x.AttachmentType == AttachmentType.MAINTENANCE_MANUAL).FileName);
            Assert.AreEqual(Extrasbericht , attachments.Single(x => x.AttachmentType == AttachmentType.CUSTOM_DOCUMENT).FileName);
        }

        [TestMethod]
        public void InvokingExecuteWithWithFailingAttachmentExportAbortsForVehicleAndWritesError()
        {
            // Arrange
            const string error = "Dochwaspassiert";
            _fahrzeuge.First().ZUSTANDSBERICHT = Zustandsbericht;
            const string path = "fileAndPath";
            Result<Vehicle> response = new Result<Vehicle>();
            Vehicle vehicle = null;
            _dokumentSteuerung.Setup(x => x.CheckDokumentsForVehicle(Kundennummer, Vin, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Verifiable();
            _herstellerSteuerung.Setup(x => x.GetHerstellerAndModel(_fahrzeuge.First().ZZFABRIKNAME, It.IsAny<Vehicle>(), It.IsAny<StringBuilder>())).Verifiable();
            _autoActRest.Setup(x => x.PostVehicle(It.IsAny<Vehicle>())).Returns(response).Callback<Vehicle>(a =>
            {
                vehicle = a;
                a.Id = AutoactId; // Aufruf an Execute setzt die Id des Inserats
                response.Value = a;
            }).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehicleExportSuccess(Belegnummer, AutoactId)).Verifiable();
            _dokumentSteuerung.Setup(x => x.LoadDokumentForVehicle(It.IsAny<Vehicle>(), Kundennummer, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Callback<Vehicle, string, IEnumerable<Attachment>, StringBuilder>((a, b, c, d) => d.Append(error)).Verifiable();
            string[] listOfImages = new string[0];
            _fileHelper.Setup(x => x.GetImageNamesForFahrzeug(Kundennummer, Vin)).Returns(listOfImages).Verifiable();
            _autoActRest.Setup(x => x.PostPictures(AutoactId.ToString(), listOfImages)).Returns(new Result<PicturesResult>()).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehilceAttachmentOrImageExportFailure(Belegnummer, It.Is<string>(message => message.Contains(error)))).Verifiable();

            // Act
            _fahrzeugSteuerung.Execute(_kunde, _fahrzeuge);

            // Assert
            VerifyAll();
        }

        [TestMethod]
        public void InvokingExecuteWithVehicleExportErrorIsReportedToSap()
        {
            // Arrange
            const string error = "Fehler beim Export des Fahrzeugs";
            Result<Vehicle> response = new Result<Vehicle>();
            response.Errors.errors.Add(new Error { message = new Message { de = error } });
            Vehicle vehicle = null;
            _dokumentSteuerung.Setup(x => x.CheckDokumentsForVehicle(Kundennummer, Vin, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Verifiable();
            _herstellerSteuerung.Setup(x => x.GetHerstellerAndModel(_fahrzeuge.First().ZZFABRIKNAME, It.IsAny<Vehicle>(), It.IsAny<StringBuilder>())).Verifiable();
            _autoActRest.Setup(x => x.PostVehicle(It.IsAny<Vehicle>())).Returns(response).Callback<Vehicle>(a =>
            {
                vehicle = a;
                a.Id = AutoactId; // Aufruf an Execute setzt die Id des Inserats
                response.Value = a;
            }).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehilceExportFailure(Belegnummer, It.Is<string>(message => message.Contains(error)))).Verifiable();

            // Act
            _fahrzeugSteuerung.Execute(_kunde, _fahrzeuge);

            // Assert
            VerifyAll();
            Assert.IsNotNull(vehicle);
        }

        [TestMethod]
        public void InvokingExecuteWithMissingHerstellerAbortsExportAndReportsErrorToSap()
        {
            // Arrange
            const string error = "Fehler beim der Ermittlung des Herstellers";
            _dokumentSteuerung.Setup(x => x.CheckDokumentsForVehicle(Kundennummer, Vin, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Verifiable();
            _herstellerSteuerung.Setup(x => x.GetHerstellerAndModel(_fahrzeuge.First().ZZFABRIKNAME, It.IsAny<Vehicle>(), It.IsAny<StringBuilder>())).Callback<string,Vehicle,StringBuilder>((a,b,c) => c.Append(error)).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehilceExportFailure(Belegnummer, It.Is<string>(message => message.Contains(error)))).Verifiable();

            // Act
            _fahrzeugSteuerung.Execute(_kunde, _fahrzeuge);

            // Assert
            VerifyAll();
        }

        [TestMethod]
        public void InvokingExecuteWithNonExistentAttachmentAbortsExportAndReportsErrorToSap()
        {
            // Arrange
            const string error = "Anhang gibt es nicht!";
            _dokumentSteuerung.Setup(x => x.CheckDokumentsForVehicle(Kundennummer, Vin, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Callback<string,string,IEnumerable<Attachment>,StringBuilder>((a,b,c,d) => d.Append(error)) .Verifiable();
            _herstellerSteuerung.Setup(x => x.GetHerstellerAndModel(_fahrzeuge.First().ZZFABRIKNAME, It.IsAny<Vehicle>(), It.IsAny<StringBuilder>())).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehilceExportFailure(Belegnummer, It.Is<string>(message => message.Contains(error)))).Verifiable();

            // Act
            _fahrzeugSteuerung.Execute(_kunde, _fahrzeuge);

            // Assert
            VerifyAll();
        }

        [TestMethod]
        public void InvokingExecuteAndFailingExportOfImagesReportsErrorToSap()
        {
            // Arrange
            const string error = "Dochwaspassiert";
            _fahrzeuge.First().ZUSTANDSBERICHT = Zustandsbericht;
            const string path = "fileAndPath";
            Result<Vehicle> response = new Result<Vehicle>();
            Vehicle vehicle = null;
            _dokumentSteuerung.Setup(x => x.CheckDokumentsForVehicle(Kundennummer, Vin, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Verifiable();
            _herstellerSteuerung.Setup(x => x.GetHerstellerAndModel(_fahrzeuge.First().ZZFABRIKNAME, It.IsAny<Vehicle>(), It.IsAny<StringBuilder>())).Verifiable();
            _autoActRest.Setup(x => x.PostVehicle(It.IsAny<Vehicle>())).Returns(response).Callback<Vehicle>(a =>
            {
                vehicle = a;
                a.Id = AutoactId; // Aufruf an Execute setzt die Id des Inserats
                response.Value = a;
            }).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehicleExportSuccess(Belegnummer, AutoactId)).Verifiable();
            _dokumentSteuerung.Setup(x => x.LoadDokumentForVehicle(It.IsAny<Vehicle>(), Kundennummer, It.IsAny<IEnumerable<Attachment>>(), It.IsAny<StringBuilder>())).Verifiable();
            string[] listOfImages = new string[0];
            _fileHelper.Setup(x => x.GetImageNamesForFahrzeug(Kundennummer, Vin)).Returns(listOfImages).Verifiable();
            var pictureExportResult = new Result<PicturesResult>();
            pictureExportResult.Errors.errors.Add(new Error{ message = new Message{ de = error }});
            _autoActRest.Setup(x => x.PostPictures(AutoactId.ToString(), listOfImages)).Returns(pictureExportResult).Verifiable();
            _autoActBapi.Setup(x => x.ReportVehilceAttachmentOrImageExportFailure(Belegnummer, It.Is<string>(message => message.Contains(error)))).Verifiable();

            // Act
            _fahrzeugSteuerung.Execute(_kunde, _fahrzeuge);

            // Assert
            VerifyAll();
        }

        #endregion

    }
}