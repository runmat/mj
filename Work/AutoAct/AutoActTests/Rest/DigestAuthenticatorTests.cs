using System.Net;
using AutoAct.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace AutoActTests.Rest
{
    [TestClass]
    public class DigestAuthenticatorTests
    {
        private const string username = "AEF30E9D-3C9B-48EF-B063-F38B3A6B8430";
        private const string password = "45DC6C11-469E-4C15-9768-B1C4C2864C1D";

        private DigestAuthenticator _digestAuthenticator;

        [TestInitialize]
        public void Setup()
        {
            _digestAuthenticator = new DigestAuthenticator(username, password);
        }

        [TestMethod]
        public void InvokingAuthenticateSetsCredentialsToRequest()
        {
            // Arrange
            Mock<IRestClient> client = new Mock<IRestClient>(MockBehavior.Strict);
            Mock<IRestRequest> request = new Mock<IRestRequest>(MockBehavior.Strict);
            ICredentials networkCredential = null;
            request.SetupSet(x => x.Credentials = It.IsAny<NetworkCredential>()).Callback<ICredentials>(a => networkCredential = a).Verifiable();

            // Act
            _digestAuthenticator.Authenticate(client.Object, request.Object);

            // Assert
            client.VerifyAll();
            request.VerifyAll();
            Assert.IsNotNull(networkCredential);
        }
    }
}
