using System;
using System.Net;
using System.Web.Services;
using AgisWebService.Agis;

namespace AgisWebService
{
    [WebService(Namespace = "http://kroschke.de/")]
    public class ServiceData : WebService
    {
        [WebMethod]
        public GetVehicleStatusResponse Agis_GetFahrzeugInfo(string fin, string username)
        {
            try
            {
                var service = new Agis.CarDocuWebService { Url = Common.AgisUrl };
                service.ClientCertificates.Add(Common.AgisCert);
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };

                return service.GetVehicleStatus(new GetVehicleStatus
                    {
                        VIN17 = fin,
                        SysUser = username
                    }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Agis_GetFahrzeugInfo, Fehler :  " + ex.Message);
            }
        }
    }
}
