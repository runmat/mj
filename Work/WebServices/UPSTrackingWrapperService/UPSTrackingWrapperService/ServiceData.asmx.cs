using System;
using System.Web.Services;
using UPSTrackingWrapperService.UPSTrack;

namespace UPSTrackingWrapperService
{
    [WebService(Namespace = "http://kroschke.de/")]
    public class ServiceData : WebService
    {
        [WebMethod]
        public TrackResponse UPS_ProcessTrack(TrackRequest TrackRequest)
        {
            try
            {
                var securityToken = new UPSSecurity
                {
                    UsernameToken = new UPSSecurityUsernameToken { Username = Common.UpsUsername, Password = Common.UpsPassword },
                    ServiceAccessToken = new UPSSecurityServiceAccessToken { AccessLicenseNumber = Common.UpsAccessKey }
                };

                var service = new TrackService { Url = Common.UpsUrl, UPSSecurityValue = securityToken };

                return service.ProcessTrack(TrackRequest);
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                throw new Exception(String.Format("UPS_ProcessTrack, Fehler: {0} -> {1}", soapEx.Message, soapEx.Detail.InnerText));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("UPS_ProcessTrack, Fehler: {0}", ex.Message));
            }
        }

        #region Test

        [WebMethod]
        public TrackResponse UPS_ProcessTrackTest()
        {
            try
            {
                var securityToken = new UPSSecurity
                {
                    UsernameToken = new UPSSecurityUsernameToken { Username = Common.UpsUsername, Password = Common.UpsPassword },
                    ServiceAccessToken = new UPSSecurityServiceAccessToken { AccessLicenseNumber = Common.UpsAccessKey }
                };

                var service = new TrackService { Url = Common.UpsUrl, UPSSecurityValue = securityToken };

                var req = createTestTrackRequest();

                return service.ProcessTrack(req);
            }
            catch (System.Web.Services.Protocols.SoapException soapEx)
            {
                throw new Exception(String.Format("UPS_ProcessTrackTest, Fehler: {0} -> {1}", soapEx.Message, soapEx.Detail.InnerText));
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("UPS_ProcessTrackTest, Fehler: {0}", ex.Message));
            }
        }

        private TrackRequest createTestTrackRequest()
        {
            return new TrackRequest
            {
                InquiryNumber = "1ZV7974V6899897106"
            };
        }

        #endregion
    }
}
