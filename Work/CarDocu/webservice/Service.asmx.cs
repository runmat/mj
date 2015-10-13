using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using CarDocuWebService.SAP;
using GeneralTools.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace CarDocuWebService
{
    [WebService(Namespace = "https://webservices.dad.de/CarDocu/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Service : WebService
    {
        [WebMethod]
        public bool IsOnline()
        {
            return true;
        }


        #region CarDocu

        [WebMethod]
        public bool ProcessArchivMeldung(string kundennr, string documentID, string id, string standortCode, string commaSeparatedDocumentCodes, 
                                         out string commaSeparatedReturnCodes, out string commaSeparatedReturnMessages)
        {
            var success = false;

            commaSeparatedReturnCodes = "";
            commaSeparatedReturnMessages = "";

            Exception exception = null;
            try
            {
                var sap = Sap.CreateDataService();

                var importList = Z_DPM_CD_OPTISCH_ARCHIVIERT.GT_WEB.GetImportListWithInit(sap, "I_AG", kundennr.ToSapKunnr());

                importList
                    .AddRange(commaSeparatedDocumentCodes.Split(',')
                                                         .Select(documentCode => new Z_DPM_CD_OPTISCH_ARCHIVIERT.GT_WEB
                                                         {
                                                             INDEXNR = id, //"IT12345612",
                                                             QMCOD = documentCode, //"KALK",
                                                             STORT = standortCode, //"1600",
                                                             SUBRC = 9999,
                                                             MESSAGE = "---"
                                                         }));

                sap.ApplyImport(importList);

                sap.Execute();

                var processedList = Z_DPM_CD_OPTISCH_ARCHIVIERT.GT_WEB.GetExportList(sap);
                if (processedList != null && processedList.Count > 0)
                {
                    commaSeparatedReturnCodes = string.Join(",",
                                                            processedList.Select(
                                                                e => e.SUBRC.GetValueOrDefault().ToString()).ToArray());
                    commaSeparatedReturnMessages = string.Join(",", processedList.Select(e => e.MESSAGE).ToArray());
                    success = true;
                }
            }
            catch (Exception e)
            {
                success = false;
                exception = e;
            }

            LogArchivMeldung("CarDocu Strafzettel", kundennr, documentID, id, standortCode, commaSeparatedDocumentCodes,
                             commaSeparatedReturnCodes, commaSeparatedReturnMessages, exception, success);

            return success;
        }

        #endregion


        #region VW Leasing

        [WebMethod]
        public bool ProcessVwlKlaerfallMeldung(string kundennr, string documentID, string id, string standortCode, string commaSeparatedDocumentCodes,
                                               out string commaSeparatedReturnCodes, out string commaSeparatedReturnMessages)
        {
            var success = false;

            commaSeparatedReturnCodes = "";
            commaSeparatedReturnMessages = "";

            Exception exception = null;
            try
            {
                var sap = Sap.CreateDataService();

                var importList = Z_DPM_SET_DAT_ABM_STATUS_01.GT_WEB.GetImportListWithInit(sap, "I_KUNNR_AG", kundennr.ToSapKunnr());

                importList
                    .AddRange(commaSeparatedDocumentCodes.Split(',')
                                                         .Select(documentCode => new Z_DPM_SET_DAT_ABM_STATUS_01.GT_WEB
                                                         {
                                                             CHASSIS_NUM = id, //"IT12345612",
                                                             SAP_CODE = documentCode, //"KALK",
                                                         }));

                sap.ApplyImport(importList);

                sap.Execute();

                var processedList = Z_DPM_SET_DAT_ABM_STATUS_01.GT_WEB.GetExportList(sap);
                if (processedList != null && processedList.Count > 0)
                {
                    commaSeparatedReturnCodes = string.Join(",",
                                                            processedList.Select(
                                                            e => e.RET_BEM.IsNullOrEmpty() ? "0" : "-1").ToArray());
                    commaSeparatedReturnMessages = string.Join(",", processedList.Select(e => e.RET_BEM).ToArray());
                    success = true;
                }
            }
            catch (Exception e)
            {
                success = false;
                exception = e;
            }

            LogArchivMeldung("VW Leasing Klärfälle", kundennr, documentID, id, standortCode, commaSeparatedDocumentCodes,
                             commaSeparatedReturnCodes, commaSeparatedReturnMessages, exception, success);

            return success;
        }

        #endregion

        #region WKDA Wiesbaden

        [WebMethod]
        public bool ProcessWkdaWiesbaden(string kundennr, string documentID, string id, string standortCode,
                                            string commaSeparatedDocumentCodes,
                                            out string commaSeparatedReturnCodes, out string commaSeparatedReturnMessages)
        {
            commaSeparatedReturnCodes = "";
            commaSeparatedReturnMessages = "";

            return true;
        }

        #endregion

        #region Misc

        private static void LogArchivMeldung(string meldungsTyp, string kundennr, string documentID, string id, string standortCode,
                                             string commaSeparatedDocumentCodes, string commaSeparatedReturnCodes,
                                             string commaSeparatedReturnMessages, Exception exception, bool success)
        {
            try
            {
                var applicationRootFolder = HttpContext.Current.Server.MapPath("~/");
                var fileName = Path.Combine(applicationRootFolder,
                                            string.Format("logs/{0}____{1}.txt",
                                                          DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss_ms"), documentID));
                if (!FileService.TryFileDelete(fileName))
                    return;

                var sb = new StringBuilder();
                sb.AppendLine("_________________________________________________________________");
                sb.AppendLine();
                sb.AppendLine(string.Format("Meldungs-Typ\t{0}", meldungsTyp));
                sb.AppendLine();
                sb.AppendLine(string.Format("Datum\t\t{0}", DateTime.Now.ToString("dd.MM.yyyy HH:mm")));
                sb.AppendLine(string.Format("ID\t\t{0}", id));
                sb.AppendLine(string.Format("Kunden-Nr\t{0}", kundennr));
                sb.AppendLine(string.Format("Standort\t{0}", standortCode));
                sb.AppendLine("_________________________________________________________________");
                sb.AppendLine();
                sb.AppendLine(string.Format("Doc. Codes\t{0}", commaSeparatedDocumentCodes));
                sb.AppendLine();
                sb.AppendLine(string.Format("Return Codes\t{0}", commaSeparatedReturnCodes));
                sb.AppendLine();
                var returnCodes = commaSeparatedReturnMessages.Split(',');
                if (returnCodes.Any())
                {
                    sb.AppendLine(string.Format("SAP Return Messages"));
                    returnCodes.ToList().ForEach(c => sb.AppendLine(string.Format("\t\t- {0}", c)));
                }

                sb.AppendLine();
                sb.AppendLine(success
                                  ? "*** SAP Verarbeitung technisch fehlerfrei ***"
                                  : "!!!!!! SAP Verarbeitung MIT TECHNISCHEM FEHLER !!!!!!");
                sb.AppendLine();

                if (exception != null)
                {
                    sb.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    sb.AppendLine();
                    sb.AppendLine(exception.Message);
                    sb.AppendLine();
                    sb.AppendLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }

                using (var outfile = new StreamWriter(fileName))
                {
                    outfile.Write(sb.ToString());
                }
            }
            catch
            {
            }
        }

        #endregion

    }
}