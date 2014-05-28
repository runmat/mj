using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoAct.Entities;
using AutoAct.Enums;
using AutoAct.Interfaces;
using AutoAct.Resources;
using AutoAct.Rest;
using GeneralTools.Contracts;
using GeneralTools.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace AutoActTests
{
    [TestClass]
    public class AutoActRestTests
    {
        #region Privates and Constructor

        private Mock<IRestClient> _client;
        private Mock<IRestRequest> _request;
        private Mock<IFileHelper> _fileHelper;
        private Mock<ILogService> _logService;
        private AutoActRest _autoActRest;
        private const string UserName = "asdfasdi2341)8";
        private const string Password = "asdf(&aSD34";
        private const string BaseUrl = "http://asdfads/";
        private readonly CustomConverter _serializer = new CustomConverter { ContentType = "application/json" };

        [TestInitialize]
        public void Setup()
        {
            _client = new Mock<IRestClient>(MockBehavior.Strict);
            _request = new Mock<IRestRequest>(MockBehavior.Strict);
            _fileHelper = new Mock<IFileHelper>(MockBehavior.Strict);
            _logService = new Mock<ILogService>(MockBehavior.Strict);
            _client.SetupSet(x => x.BaseUrl = BaseUrl).Verifiable();
            _autoActRest = new AutoActRest(_logService.Object, BaseUrl, _client.Object, _fileHelper.Object);
        }

        private void VerifyAll()
        {
            _client.VerifyAll();
            _request.VerifyAll();
            _fileHelper.VerifyAll();
            _logService.VerifyAll();
        }

        #endregion

        #region SetDiegstAutheticator

        [TestMethod]
        public void InvokingSetDigestAuthenticatorSetsIAuthenticatorOfClient()
        {
            // Arrange
            IAuthenticator authenticator = null;
            _client.SetupSet(x => x.Authenticator = It.IsAny<IAuthenticator>()).Callback<IAuthenticator>(a => authenticator = a).Verifiable();

            // Act
            _autoActRest.SetDiegstAuthenticator(UserName, Password);

            // Assert
            VerifyAll();
            Assert.IsNotNull(authenticator);
        }

        #endregion

        #region IsAlive

        [TestMethod]
        public void InvokingIsAliveCallsIsAliveOnAutoAct()
        {
            // Arrange
            const string responseContent = "";
            IRestRequest currentRequest = null;
            Mock<IRestResponse<Vehicle>> response = new Mock<IRestResponse<Vehicle>>(MockBehavior.Strict);
            _client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Callback<IRestRequest>(a => currentRequest = a).Returns(response.Object).Verifiable();
            response.SetupGet(x => x.Content).Returns(responseContent).Verifiable();

            // Act
            var result = _autoActRest.IsAlive();

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(DataFormat.Json, currentRequest.RequestFormat);
            Assert.AreEqual(ApplicationStrings.IsAliveResource, currentRequest.Resource);
            Assert.AreEqual(responseContent == string.Empty, result);
        }

        #endregion

        #region POST Vehicle

        [TestMethod]
        public void PostVehicleCreatesPayloadAndPostsReturningResultOnSuccess()
        {
            // Arrange
            Vehicle vehiclePosted = new Vehicle();
            Vehicle vehicleReceived = new Vehicle();
            Mock<IRestResponse<Vehicle>> response = new Mock<IRestResponse<Vehicle>>(MockBehavior.Strict);
            IRestRequest request = null;
            _client.Setup(x => x.Execute<Vehicle>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            response.SetupGet(x => x.Data).Returns(vehicleReceived).Verifiable();

            // Act
            var result = _autoActRest.PostVehicle(vehiclePosted);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(Method.POST, request.Method);
            Assert.AreEqual(ApplicationStrings.PostVehicleResource, request.Resource);
            Assert.AreEqual(ApplicationStrings.PostVehicleResource, request.Resource);
            Assert.AreEqual(vehicleReceived, result.Value);
            Assert.IsFalse(result.Errors.errors.Any());
        }

        [TestMethod]
        public void PostVehicleCreatesPayloadAndPostsReturningErrorMessageOnFailure()
        {
            // Arrange
            const string errorMessage0 = "ASDFASD$&$%&/$%&";
            const string errorMessage1 = "ASDFASD$&ZTG%$(";
            ErrorRootObject responseContent = new ErrorRootObject();
            responseContent.errors.Add(new Error { message = new Message { de = errorMessage0, en = errorMessage0 } });
            responseContent.errors.Add(new Error { message = new Message { de = errorMessage1, en = errorMessage1 } });
            Vehicle vehiclePosted = new Vehicle();
            Mock<IRestResponse<Vehicle>> response = new Mock<IRestResponse<Vehicle>>(MockBehavior.Strict);
            IRestRequest request = null;
            _client.Setup(x => x.Execute<Vehicle>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Forbidden).Verifiable();
            response.SetupGet(x => x.Content).Returns(_serializer.Serialize(responseContent)).Verifiable();

            // Act
            var result = _autoActRest.PostVehicle(vehiclePosted);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(Method.POST, request.Method);
            Assert.AreEqual(ApplicationStrings.PostVehicleResource, request.Resource);
            Assert.IsTrue(result.ErrorSummary.Contains(errorMessage0));
            Assert.IsTrue(result.ErrorSummary.Contains(errorMessage1));
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public void PostVehicleCreatesPayloadAndPostsAndREturnsErrorObjectWhenNoExplicitErrorWasReturned()
        {
            // Arrange
            Vehicle vehiclePosted = new Vehicle();
            Mock<IRestResponse<Vehicle>> response = new Mock<IRestResponse<Vehicle>>(MockBehavior.Strict);
            IRestRequest request = null;
            _client.Setup(x => x.Execute<Vehicle>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Forbidden).Verifiable();
            response.SetupGet(x => x.Content).Returns(_serializer.Serialize(null)).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.Unauthorized).Verifiable();

            // Act
            var result = _autoActRest.PostVehicle(vehiclePosted);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(Method.POST, request.Method);
            Assert.AreEqual(ApplicationStrings.PostVehicleResource, request.Resource);
            Assert.IsTrue(result.ErrorSummary.Contains(HttpStatusCode.Unauthorized.ToString()));
            Assert.IsNull(result.Value);
        }

        #endregion

        #region POST Attachment

        [TestMethod]
        public void InvokingPostAttachmentReadsBytesAndReturnsUploadedDocumentOnSuccess()
        {
            // Arrange
            const string vehicleid = "ASdfad%&§$&/$%&";
            const string filename = "WTZERT%&/()&/()&";
            const string filenameWithPath = "WTZERT%&/()&/()&%&/()&/(";
            const AttachmentType attachmentType = AttachmentType.DAMAGE_REPORT;
            IRestRequest request = null;
            byte[] bytearray = new byte[0];
            var responseResult = new AttachmentsResult();
            Mock<IRestResponse<AttachmentsResult>> response = new Mock<IRestResponse<AttachmentsResult>>(MockBehavior.Strict);
            _fileHelper.Setup(x => x.ReadAllBytes(filenameWithPath)).Returns(bytearray).Verifiable();
            _client.Setup(x => x.Execute<AttachmentsResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            response.SetupGet(x => x.Data).Returns(responseResult).Verifiable();

            // Act
            var result = _autoActRest.PostAttachment(vehicleid, attachmentType, filename, filenameWithPath);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsNotNull(request);
            var getOrPostParams = request.Parameters.Where(x => x.Type == ParameterType.GetOrPost).ToList();
            Assert.AreEqual(1, getOrPostParams.Count());
            Assert.AreEqual(ApplicationStrings.PostAttachmentFormPartAttachmentType, getOrPostParams.First().Name);
            Assert.AreEqual(attachmentType.ToString(), getOrPostParams.First().Value.ToString());
            Assert.AreEqual(1, request.Files.Count);
            Assert.AreEqual(filename, request.Files[0].FileName);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.errors.Count == 0);
            Assert.AreEqual(responseResult, result.Value);
        }

        [TestMethod]
        public void InvokingPostAttachmentReadsBytesAndReturnsErrorOnFailure()
        {
            // Arrange
            const string vehicleid = "ASdfad%&§$&/$%&";
            const string filename = "WTZERT%&/()&/()&";
            const string filenameWithPath = "WTZERT%&/()&/()&%&/()&/(";
            const string deErrorMessage = "§$§$%&§$%&/(%&)";
            const AttachmentType attachmentType = AttachmentType.DAMAGE_REPORT;
            IRestRequest request = null;
            byte[] bytearray = new byte[0];
            var responseResult = new ErrorRootObject();
            responseResult.errors.Add(new Error { message = new Message { de = deErrorMessage } });
            Mock<IRestResponse<AttachmentsResult>> response = new Mock<IRestResponse<AttachmentsResult>>(MockBehavior.Strict);
            _fileHelper.Setup(x => x.ReadAllBytes(filenameWithPath)).Returns(bytearray).Verifiable();
            _client.Setup(x => x.Execute<AttachmentsResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadGateway).Verifiable();
            response.SetupGet(x => x.Content).Returns(_serializer.Serialize(responseResult)).Verifiable();

            // Act
            var result = _autoActRest.PostAttachment(vehicleid, attachmentType, filename, filenameWithPath);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsNotNull(request);
            var getOrPostParams = request.Parameters.Where(x => x.Type == ParameterType.GetOrPost).ToList();
            Assert.AreEqual(1, getOrPostParams.Count());
            Assert.AreEqual(ApplicationStrings.PostAttachmentFormPartAttachmentType, getOrPostParams.First().Name);
            Assert.AreEqual(attachmentType.ToString(), getOrPostParams.First().Value.ToString());
            Assert.AreEqual(1, request.Files.Count);
            Assert.AreEqual(filename, request.Files[0].FileName);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.errors.Count == 1);
            Assert.AreEqual(null, result.Value);
        }

        #endregion

        #region DELETE Vehicle

        [TestMethod]
        public void InvokeDeleteVehicleAndReturnSuccess()
        {
            // Arrange
            Int64 id = 123412341234132;
            IRestRequest request = null;
            Mock<IRestResponse> response = new Mock<IRestResponse>(MockBehavior.Strict);
            _client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.NoContent).Verifiable();

            // Act
            var result = _autoActRest.DeleteVehicle(id);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsTrue(result.Value);
            Assert.IsTrue(request.Resource.Contains(id.ToString()));
            Assert.AreEqual(Method.DELETE, request.Method);
        }

        [TestMethod]
        public void InvokeDeleteVehicleAndReturnError()
        {
            // Arrange
            const long id = 123412341234132;
            const string errormessage = "ASDFASdf§$%§$%&§$%&§$%&";
            IRestRequest request = null;
            Mock<IRestResponse> response = new Mock<IRestResponse>(MockBehavior.Strict);
            _client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadGateway).Verifiable();
            ApplicationException ex = new ApplicationException();
            response.SetupGet(x => x.ErrorException).Returns(ex).Verifiable();
            response.SetupGet(x => x.ErrorMessage).Returns(errormessage).Verifiable();
            ApplicationException loggedException = null;
            _logService.Setup(x => x.LogError(It.IsAny<ApplicationException>(), null, null)).Returns(new LogItem()).Callback<Exception, ILogonContext, object>((a, b, c) => loggedException = (ApplicationException)a).Verifiable();

            // Act
            var result = _autoActRest.DeleteVehicle(id);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsFalse(result.Value);
            Assert.IsTrue(request.Resource.Contains(id.ToString()));
            Assert.AreEqual(Method.DELETE, request.Method);
            Assert.AreEqual(ex, loggedException.InnerException);
            Assert.AreEqual(errormessage, loggedException.Message);
        }

        #endregion

        #region DELETE Pictures

        [TestMethod]
        public void InvokeDeletePicturesAndReturnSuccess()
        {
            // Arrange
            Int64 id = 123412341234132;
            IRestRequest request = null;
            Mock<IRestResponse> response = new Mock<IRestResponse>(MockBehavior.Strict);
            _client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.NoContent).Verifiable();

            // Act
            var result = _autoActRest.DeleteVehicle(id);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsTrue(result.Value);
            Assert.IsTrue(request.Resource.Contains(id.ToString()));
            Assert.AreEqual(Method.DELETE, request.Method);
        }

        [TestMethod]
        public void InvokeDeletePicturesAndReturnError()
        {
            // Arrange
            const long id = 123412341234132;
            const string errormessage = "ASDFASdf§$%§$%&§$%&§$%&";
            IRestRequest request = null;
            Mock<IRestResponse> response = new Mock<IRestResponse>(MockBehavior.Strict);
            _client.Setup(x => x.Execute(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadGateway).Verifiable();
            ApplicationException ex = new ApplicationException();
            response.SetupGet(x => x.ErrorException).Returns(ex).Verifiable();
            response.SetupGet(x => x.ErrorMessage).Returns(errormessage).Verifiable();
            ApplicationException loggedException = null;
            _logService.Setup(x => x.LogError(It.IsAny<ApplicationException>(), null, null)).Returns(new LogItem()).Callback<Exception, ILogonContext, object>((a, b, c) => loggedException = (ApplicationException)a).Verifiable();

            // Act
            var result = _autoActRest.DeleteVehicle(id);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsFalse(result.Value);
            Assert.IsTrue(request.Resource.Contains(id.ToString()));
            Assert.AreEqual(Method.DELETE, request.Method);
            Assert.AreEqual(ex, loggedException.InnerException);
            Assert.AreEqual(errormessage, loggedException.Message);
        }

        #endregion

        #region GET Vehicle

        [TestMethod]
        public void InvokingGetVehiclesWithSuccessReturnsResult()
        {
            // Arrange
            VehiclesResult vehicles = new VehiclesResult { Vehicles = new List<Vehicle>() };
            IRestRequest request = null;
            Mock<IRestResponse<VehiclesResult>> response = new Mock<IRestResponse<VehiclesResult>>(MockBehavior.Strict);
            _client.Setup(x => x.Execute<VehiclesResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            response.SetupGet(x => x.Data).Returns(vehicles).Verifiable();

            // Act
            var result = _autoActRest.GetVehicles();

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(ApplicationStrings.GetVehicleResource, request.Resource);
            Assert.AreEqual(Method.GET, request.Method);
            Assert.AreEqual(0, result.Errors.errors.Count);
        }

        [TestMethod]
        public void InvokingGetVehicleWithErrorREturnsErrorCollection()
        {
            // Arrange
            var errormessage = "1A5DE4B1-D8EE-49F1-9DBF-8E5BC5DCA0AB";
            ErrorRootObject errorRootObject = new ErrorRootObject();
            errorRootObject.errors.Add(new Error
                {
                    message = new Message
                        {
                            de = errormessage,
                            en = errormessage
                        }
                });
            IRestRequest request = null;
            Mock<IRestResponse<VehiclesResult>> response = new Mock<IRestResponse<VehiclesResult>>(MockBehavior.Strict);
            _client.Setup(x => x.Execute<VehiclesResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadGateway).Verifiable();
            response.SetupGet(x => x.Content).Returns(_serializer.Serialize(errorRootObject)).Verifiable();

            // Act
            var result = _autoActRest.GetVehicles();

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(ApplicationStrings.GetVehicleResource, request.Resource);
            Assert.AreEqual(Method.GET, request.Method);
            Assert.AreEqual(1, result.Errors.errors.Count);
        }

        #endregion

        #region GET MAKE & MODEL

        [TestMethod]
        public void InvokingGetMakesWithSuccessReturnsResult()
        {
            // Arrange
            const VehicleType vehicleType = VehicleType.Car;
            var resource = string.Format(ApplicationStrings.GetMakeResource, vehicleType.ToString());
            AutoActMakesResult makes = new AutoActMakesResult();
            IRestRequest request = null;
            Mock<IRestResponse<AutoActMakesResult>> response = new Mock<IRestResponse<AutoActMakesResult>>(MockBehavior.Strict);
            _client.Setup(x => x.Execute<AutoActMakesResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            response.SetupGet(x => x.Data).Returns(makes).Verifiable();

            // Act
            var result = _autoActRest.GetMakes(vehicleType);

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(resource, request.Resource);
            Assert.AreEqual(Method.GET, request.Method);
            Assert.AreEqual(0, result.Errors.errors.Count);
        }

        [TestMethod]
        public void InvokingGetMakeWithErrorREturnsErrorCollection()
        {
            // Arrange
            const VehicleType vehicleType = VehicleType.Car;
            var resource = string.Format(ApplicationStrings.GetMakeResource, vehicleType.ToString());
            var errormessage = "1A5DE4B1-D8EE-49F1-9DBF-8E5BC5DCA0AB";
            ErrorRootObject errorRootObject = new ErrorRootObject();
            errorRootObject.errors.Add(new Error
            {
                message = new Message
                {
                    de = errormessage,
                    en = errormessage
                }
            });
            IRestRequest request = null;
            Mock<IRestResponse<AutoActMakesResult>> response = new Mock<IRestResponse<AutoActMakesResult>>(MockBehavior.Strict);
            _client.Setup(x => x.Execute<AutoActMakesResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadGateway).Verifiable();
            response.SetupGet(x => x.Content).Returns(_serializer.Serialize(errorRootObject)).Verifiable();

            // Act
            var result = _autoActRest.GetMakes(vehicleType);
             
            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.AreEqual(resource, request.Resource);
            Assert.AreEqual(Method.GET, request.Method);
            Assert.AreEqual(1, result.Errors.errors.Count);
        }

        #endregion

        #region POST Image

        [TestMethod]
        public void InvokingPostImageReadsBytesAndReturnsUploadedDocumentOnSuccess()
        {
            // Arrange
            const string vehicleid = "ASdfad%&§$&/$%&";
            const string filename0 = "WTZERT%&/()&/()&";
            const string filename1 = "UTIOZUIZ§$%&/$%";
            const string filename2 = "§$%&§$%&§$%§$%";
            IRestRequest request = null;
            byte[] bytearray = new byte[0];
            var responseResult = new PicturesResult();
            Mock<IRestResponse<PicturesResult>> response = new Mock<IRestResponse<PicturesResult>>(MockBehavior.Strict);
            _fileHelper.Setup(x => x.ReadAllBytes(filename0)).Returns(bytearray).Verifiable();
            _fileHelper.Setup(x => x.ReadAllBytes(filename1)).Returns(bytearray).Verifiable();
            _client.Setup(x => x.Execute<PicturesResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            response.SetupGet(x => x.Data).Returns(responseResult).Verifiable();

            // Act
            var result = _autoActRest.PostPictures(vehicleid, new[] { filename0, filename1 });

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsNotNull(request);
            Assert.AreEqual(2, request.Files.Count);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.errors.Count == 0);
            Assert.AreEqual(responseResult, result.Value);
        }

        [TestMethod]
        public void InvokingPostImageReadsBytesAndReturnsErrorOnFailure()
        {
            // Arrange
            const string vehicleid = "ASdfad%&§$&/$%&";
            const string filename0 = "WTZERT%&/()&/()&";
            const string filename1 = "UTIOZUIZ§$%&/$%";
            const string deErrorMessage = "182E2B14-3687-461D-BAC7-18F31771CE2A";
            IRestRequest request = null;
            byte[] bytearray = new byte[0];
            var responseResult = new ErrorRootObject();
            responseResult.errors.Add(new Error { message = new Message { de = deErrorMessage } });
            Mock<IRestResponse<PicturesResult>> response = new Mock<IRestResponse<PicturesResult>>(MockBehavior.Strict);
            _fileHelper.Setup(x => x.ReadAllBytes(filename0)).Returns(bytearray).Verifiable();
            _fileHelper.Setup(x => x.ReadAllBytes(filename1)).Returns(bytearray).Verifiable();
            _client.Setup(x => x.Execute<PicturesResult>(It.IsAny<IRestRequest>())).Returns(response.Object).Callback<IRestRequest>(a => request = a).Verifiable();
            response.SetupGet(x => x.StatusCode).Returns(HttpStatusCode.BadGateway).Verifiable();
            response.SetupGet(x => x.Content).Returns(_serializer.Serialize(responseResult)).Verifiable();

            // Act
            var result = _autoActRest.PostPictures(vehicleid, new[] { filename0, filename1 });

            // Assert
            VerifyAll();
            response.VerifyAll();
            Assert.IsNotNull(request);
            Assert.AreEqual(2, request.Files.Count);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Errors.errors.Count == 1);
        }

        [TestMethod]
        public void InvokingPostPicturesWithEmptyArrayExits()
        {
            // Arrange

            // Act
            var result = _autoActRest.PostPictures("", new string[0]);

            // Assert
            VerifyAll();
        }

        #endregion

    }
}
