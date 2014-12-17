using System;
using System.Configuration;
using CKGDatabaseAdminLib.Services;
using System.Linq;

namespace CkgServerTasks
{
    public class BapiCheck
    {
        public void CheckBapiDefinitionen()
        {
            try
            {
                var runBapiCheck = (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RunBapiCheck"]) && ConfigurationManager.AppSettings["RunBapiCheck"].ToUpper() == "TRUE");

                if (!runBapiCheck)
                    return;

                var connString = ConfigurationManager.AppSettings["BapiCheckSqlConnection"];
                var testSap = (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ProdSAP"]) || ConfigurationManager.AppSettings["ProdSAP"].ToUpper() != "TRUE");

                BapiCheckBapiDataService dataService = new BapiCheckBapiDataService(connString, testSap);

                var result = dataService.PerformBapiCheck();

                if (!String.IsNullOrEmpty(result))
                    throw new Exception(result);

                if (dataService.BapiCheckAbweichungen.Count > 0)
                {
                    Common.WriteLogEntry("Success", "Bapi-Prüfung erfolgreich ausgeführt, " + dataService.BapiCheckAbweichungen.Count.ToString() + " Abweichungen festgestellt.");

                    var mailText = "Beim Bapi-Check wurden folgende Abweichungen festgestellt:" + Environment.NewLine; 
                    mailText += String.Join(Environment.NewLine, dataService.BapiCheckAbweichungen.Select(
                        a => (a.BapiName + (a.DoesNotExistInSap ? " existiert nicht in SAP" : (a.IsNew ? " ist neu in SAP" : " hat sich geändert")))));

                    Common.SendMail("Bapi-Abweichungen", mailText);
                }
                else
                {
                    Common.WriteLogEntry("Success", "Bapi-Prüfung erfolgreich ausgeführt, keine Abweichungen festgestellt.");
                }
            }
            catch (Exception ex)
            {
                Common.WriteLogEntry("Fehler", ex.Message + ": " + (ex.InnerException != null ? ex.InnerException.Message : ""));
                Common.SendErrorMail(ex.Message);
            }
        }
    }
}
