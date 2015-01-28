using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http.ModelBinding;
using AutohausRestService.Contracts;
using AutohausRestService.Models;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using WebTools.Services;

namespace AutohausRestService.Services
{
    public enum WebLinkType
    {
        BestandZurFahrzeugId,
        BestandDesKunden,
        Partnerverwaltung
    }

    public class RestAhDataServiceSap : IRestAhDataService
    {
        private string WebPortalUrl { get; set; }

        public RestAhDataServiceSap()
        {
            WebPortalUrl = GeneralConfiguration.GetConfigValue("AutohausRestService", "WebPortalUrl");
        }

        public List<string> SaveDatensatz(Datensatz daten, AuthenticationHeaderValue authHeader, ModelStateDictionary modelState)
        {
            List<string> linkListe = new List<string>();

            string uname;
            string pword;
            RestAuthentication.GetUsernameAndPasswordFromAuthorizationHeader(authHeader, out uname, out pword);

            var dbContext = new DomainDbContext(ConfigurationService.DbConnection, uname);

            var user = dbContext.User;
            var kunde = dbContext.GetCustomerFromUserName(uname);

            if (kunde == null)
            {
                modelState.AddModelError("", "Kundeninformationen konnten nicht ermittelt werden.");
                return linkListe;
            }

            if (!kunde.AllowUrlRemoteLogin.HasValue || !kunde.AllowUrlRemoteLogin.Value || String.IsNullOrEmpty(user.UrlRemoteLoginKey))
            {
                modelState.AddModelError("", "Die Webportal-Anmeldung per Link ist für diesen Kunden/User nicht möglich.");
                return linkListe;
            }

            var objS = new S();

            Z_AHP_CRE_CHG_PARTNER_FZGDATEN.Init(objS.AP);

            objS.AP.SetImportParameter("I_KUNNR", kunde.KUNNR.ToSapKunnr());
            objS.AP.SetImportParameter("I_USER", uname);

            if (daten.Partnerdaten != null)
            {
                var pImportList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_IMP_From_Partner.CopyBack(new List<Partner> { daten.Partnerdaten }).ToList();
                objS.AP.ApplyImport(pImportList);
            }

            if (daten.Fahrzeuge.Count > 0)
            {
                var fzgImportList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_IMP_From_Fahrzeug.CopyBack(daten.Fahrzeuge).ToList();
                objS.AP.ApplyImport(fzgImportList);
            }

            objS.AP.Execute();

            if (objS.AP.ResultCode == 0)
            {
                var pExportList = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_PARTNER_OUT.GetExportList(objS.AP);
                var pList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_PARTNER_OUT_To_Partner.Copy(pExportList);

                var fzgExportList = Z_AHP_CRE_CHG_PARTNER_FZGDATEN.GT_WEB_FZG_OUT.GetExportList(objS.AP);
                var fzgList = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_FZGDATEN_GT_WEB_FZG_OUT_To_Fahrzeug.Copy(fzgExportList);

                if (pList.Any())
                {
                    if (fzgList.Any())
                    {
                        if (fzgList.Count() == 1)
                        {
                            // 1 Partner & 1 Fahrzeug
                            linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.BestandZurFahrzeugId, fzgList.First().FahrzeugID));
                        }
                        else
                        {
                            // 1 Partner & n Fahrzeuge
                            linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.BestandDesKunden, pList.First().KundenNr));
                        }
                    }
                    else
                    {
                        // 1 Partner & 0 Fahrzeuge
                        linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.Partnerverwaltung, pList.First().KundenNr));
                    }
                }
                else if (fzgList.Any())
                {
                    // 0 Partner & 1 oder n Fahrzeug(e)
                    foreach (var fzg in fzgList)
                    {
                        linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.BestandZurFahrzeugId, fzg.FahrzeugID));
                    }
                }
            }
            else
            {
                modelState.AddModelError("", "Fehler beim Speichern: " + objS.AP.ResultMessage);
            }

            return linkListe;
        }

        private string GenerateWebLink(string remoteLoginKey, WebLinkType linkTyp, string itemId)
        {
            // Link-Gültigkeit erstmal pauschal auf 2 Jahre festgelegt, muss später ggf. noch angepasst werden
            var expToken = UserSecurityService.UrlRemoteEncryptHourAndDate(DateTime.Now.AddYears(2).ToString("HHddMMyyy"));
            var appLink = "";

            switch (linkTyp)
            {
                case WebLinkType.BestandDesKunden:
                    appLink = "/Autohaus/Fahrzeugbestand/Index?partnerid=";
                    break;

                case WebLinkType.BestandZurFahrzeugId:
                    appLink = "/Autohaus/Fahrzeugbestand/Index?fzgid=";
                    break;

                case WebLinkType.Partnerverwaltung:
                    appLink = "/Autohaus/Partner/Pflege?partnerid=";
                    break;
            }

            return String.Format("{0}{1}{2}&ra={3}&rb={4}", WebPortalUrl, appLink, itemId.Trim(), remoteLoginKey, expToken);
        }
    }
}