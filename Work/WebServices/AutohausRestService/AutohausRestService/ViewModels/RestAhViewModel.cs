using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.ModelBinding;
using AutohausRestService.Contracts;
using AutohausRestService.Models;
using AutohausRestService.Services;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Services;
using WebTools.Services;

namespace AutohausRestService.ViewModels
{
    public enum WebLinkType
    {
        BestandZumFahrzeug,
        BestandDesKunden,
        Partnerverwaltung
    }

    public class RestAhViewModel
    {
        public IRestAhDataService DataService { get; private set; }

        private string WebPortalUrl { get; set; }

        public RestAhViewModel()
        {
            DataService = new RestAhDataServiceSap();
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

            List<Partner> partnerList;
            List<Fahrzeug> fzgList;

            var saveErg = DataService.SaveDatensatz(user, kunde, daten, out partnerList, out fzgList);

            if (!String.IsNullOrEmpty(saveErg))
            {
                modelState.AddModelError("", "Fehler beim Speichern: " + saveErg);
            }
            else
            {
                if (partnerList.Any())
                {
                    if (fzgList.Any())
                    {
                        if (fzgList.Count() == 1)
                        {
                            // 1 Partner & 1 Fahrzeug
                            linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.BestandZumFahrzeug, fzgList.First().FahrgestellNr));
                        }
                        else
                        {
                            // 1 Partner & n Fahrzeuge
                            linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.BestandDesKunden, partnerList.First().KundenNr));
                        }
                    }
                    else
                    {
                        // 1 Partner & 0 Fahrzeuge
                        linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.Partnerverwaltung, partnerList.First().KundenNr));
                    }
                }
                else if (fzgList.Any())
                {
                    // 0 Partner & 1 oder n Fahrzeug(e)
                    foreach (var fzg in fzgList)
                    {
                        linkListe.Add(GenerateWebLink(user.UrlRemoteLoginKey, WebLinkType.BestandZumFahrzeug, fzg.FahrgestellNr));
                    }
                }
            }

            return linkListe;
        }

        private string GenerateWebLink(string remoteLoginKey, WebLinkType linkTyp, string itemId)
        {
            var expToken = UserSecurityService.UrlRemoteEncryptHourAndDate(DateTime.Now.AddYears(10).ToString("HHddMMyyy"));
            var itemIdToken = HttpUtility.UrlEncode(CryptoMd5.Encrypt(itemId.Trim()));
            var appLink = "";

            switch (linkTyp)
            {
                case WebLinkType.BestandDesKunden:
                    appLink = "/Autohaus/Fahrzeugbestand/Index?pid=";
                    break;

                case WebLinkType.BestandZumFahrzeug:
                    appLink = "/Autohaus/Fahrzeugbestand/Index?fid=";
                    break;

                case WebLinkType.Partnerverwaltung:
                    appLink = "/Autohaus/Partner/Pflege?pid=";
                    break;
            }

            return String.Format("{0}{1}{2}&ra={3}&rb={4}", WebPortalUrl, appLink, itemIdToken, remoteLoginKey, expToken);
        }
    }
}