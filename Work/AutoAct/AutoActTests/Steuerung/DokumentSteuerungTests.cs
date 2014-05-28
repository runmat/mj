using System.Collections.Generic;
using System.Text;
using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Rest;
using AutoAct.Steuerung;
using GeneralTools.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AutoActTests.Steuerung
{
    [TestClass]
    public class DokumentSteuerungTests
    {
        #region Privates, constructor, and init

        private const string Kundennummer = "123456";
        private const string Fin = "WAZasdfasdfasdfasdfasda";
        private const string FileName = "Dokument.doc";
        private const string FileName0 = "Mokument.doc";
        private const string FilePath = @"asdfasdfa\asdasd\Dokument.doc";
        private const string FilePath0 = @"asdfasdfa\asdasd\Mokument.doc";
        private const long InseratId = 12341234132413;
        private Vehicle _vehicle;

        private Mock<ILogService> _logService;
        private Mock<IAutoActRest> _autoActRest;
        private Mock<IFileHelper> _fileHelper;
        private Mock<IConsoleWrapper> _consoleWrapper;

        private DokumentSteuerung _dokumentSteuerung;

        [TestInitialize]
        public void Setup()
        {
            _logService = new Mock<ILogService>(MockBehavior.Strict);
            _autoActRest = new Mock<IAutoActRest>(MockBehavior.Strict);
            _fileHelper = new Mock<IFileHelper>(MockBehavior.Strict);
            _consoleWrapper = new Mock<IConsoleWrapper>(MockBehavior.Strict);

            _vehicle = new Vehicle
                {
                    Id = InseratId,
                    Vin = Fin
                };
            _dokumentSteuerung = new DokumentSteuerung(_logService.Object, _autoActRest.Object, _fileHelper.Object, _consoleWrapper.Object);
        }

        private void VerifyAll()
        {
            _logService.VerifyAll();
            _autoActRest.VerifyAll();
            _fileHelper.VerifyAll();
            _consoleWrapper.VerifyAll();
        }

        #endregion

        #region CheckDokument

        [TestMethod]
        public void CallingCheckDokumentForVehicleWithEmptyFileNameAppendsNoError()
        {
            // Arrange
            StringBuilder sb = new StringBuilder();
            List<Attachment> attachments =  new List<Attachment>
                {
                    new Attachment{ AttachmentType = AttachmentType.DAMAGE_REPORT, FileName = null },
                    new Attachment{ AttachmentType = AttachmentType.MAINTENANCE_MANUAL, FileName = null }
                }; 

            // Act
            _dokumentSteuerung.CheckDokumentsForVehicle(Kundennummer, Fin, attachments, sb);

            // Assert
            VerifyAll();
            Assert.AreEqual(0, sb.Length);
        }

        [TestMethod]
        public void CallingCheckDokumentWithExistingFileNameAppendsNoError()
        {
            // Arrange  
            List<Attachment> attachments = new List<Attachment>
                {
                    new Attachment{ AttachmentType = AttachmentType.DAMAGE_REPORT, FileName = FileName },
                    new Attachment{ AttachmentType = AttachmentType.MAINTENANCE_MANUAL, FileName = FileName0 }
                }; 
            _fileHelper.Setup(x => x.DeterminePathToFile(Kundennummer,Fin,FileName)).Returns(FilePath).Verifiable();
            _fileHelper.Setup(x => x.Exists(FilePath)).Returns(true).Verifiable();
            _fileHelper.Setup(x => x.DeterminePathToFile(Kundennummer,Fin,FileName0)).Returns(FilePath0).Verifiable();
            _fileHelper.Setup(x => x.Exists(FilePath0)).Returns(true).Verifiable();
            StringBuilder sb = new StringBuilder();

            // Act
            _dokumentSteuerung.CheckDokumentsForVehicle(Kundennummer, Fin, attachments, sb);

            // Assert
            VerifyAll();
            Assert.AreEqual(0, sb.Length);
        }

        [TestMethod]
        public void CallingCheckDokumentWithMissingFileNameAppendsError()
        {
            // Arrange  
            AttachmentType attachmentType = AttachmentType.DAMAGE_REPORT;
            Attachment attachment = new Attachment { AttachmentType = attachmentType, FileName = FileName };
            _fileHelper.Setup(x => x.DeterminePathToFile(Kundennummer, Fin, FileName)).Returns(FilePath).Verifiable();
            _fileHelper.Setup(x => x.Exists(FilePath)).Returns(false).Verifiable();
            _consoleWrapper.Setup(x => x.WriteInfo(It.IsAny<string>())).Verifiable();
            StringBuilder sb = new StringBuilder();

            // Act
            _dokumentSteuerung.CheckDokumentsForVehicle(Kundennummer, Fin, new List<Attachment>{ attachment }, sb);

            // Assert
            VerifyAll();
            Assert.AreNotEqual(0, sb.Length);
            Assert.IsTrue(sb.ToString().Contains(attachmentType.ToString()));
            Assert.IsTrue(sb.ToString().Contains(FileName));
        }

        #endregion

        #region LoadDokument

        [TestMethod]
        public void CallingLoadDokumentForVehicleWithEmptyFileNameAppendsNoError()
        {
            // Arrange
            Attachment attachment = new Attachment { AttachmentType = AttachmentType.DAMAGE_REPORT, FileName = string.Empty };
            StringBuilder sb = new StringBuilder();

            // Act
            _dokumentSteuerung.LoadDokumentForVehicle(_vehicle, Kundennummer, new List<Attachment>{ attachment }, sb);

            // Assert
            VerifyAll();
            Assert.AreEqual(0, sb.Length);
        }

        [TestMethod]
        public void CallingLoadDokumentForVehicleWithFile()
        {
            // Arrange
            List<Attachment> attachments = new List<Attachment>
                {
                    new Attachment { AttachmentType = AttachmentType.CUSTOM_DOCUMENT, FileName = FileName },
                    new Attachment { AttachmentType = AttachmentType.DAMAGE_REPORT, FileName = FileName0 }
                };
            Result<AttachmentsResult> result = new Result<AttachmentsResult>(); 
            _fileHelper.Setup(x => x.DeterminePathToFile(Kundennummer,Fin, FileName)).Returns(FilePath).Verifiable();
            _fileHelper.Setup(x => x.DeterminePathToFile(Kundennummer,Fin, FileName0)).Returns(FilePath0).Verifiable();
            _autoActRest.Setup(x => x.PostAttachment(InseratId.ToString(), attachments[0].AttachmentType, attachments[0].FileName, FilePath)).Returns(result).Verifiable();
            _autoActRest.Setup(x => x.PostAttachment(InseratId.ToString(), attachments[1].AttachmentType, attachments[1].FileName, FilePath0)).Returns(result).Verifiable();
            StringBuilder sb = new StringBuilder();

            // Act
            _dokumentSteuerung.LoadDokumentForVehicle(_vehicle, Kundennummer, attachments, sb);

            // Assert
            VerifyAll();
            Assert.AreEqual(0, sb.Length);
        }

        [TestMethod]
        public void FailingPostAttachmentWritesErrorBackToStringBuilder()
        {
            // Arrange
            const AttachmentType attachmentType = AttachmentType.DAMAGE_REPORT;
            Attachment attachment = new Attachment { AttachmentType = attachmentType, FileName = FileName };
            Result<AttachmentsResult> result = new Result<AttachmentsResult>();
            result.Errors.errors.Add(new Error
            { 
                code = "001",
                field = "field",
                message = new Message
                    {
                        de = "deutsche Fehlermeldung",
                        en = "englische Fehlermeldung"
                    },
                timestamp = "timestamp"
            });
            _fileHelper.Setup(x => x.DeterminePathToFile(Kundennummer, Fin, FileName)).Returns(FilePath).Verifiable();
            _autoActRest.Setup(x => x.PostAttachment(InseratId.ToString(), attachmentType, FileName, FilePath)).Returns(result).Verifiable();
            StringBuilder sb = new StringBuilder();

            // Act
            _dokumentSteuerung.LoadDokumentForVehicle(_vehicle, Kundennummer, new List<Attachment>{ attachment }, sb);

            // Assert
            VerifyAll();
            Assert.AreNotEqual(0, sb.Length);
        }

        #endregion
    }
}
