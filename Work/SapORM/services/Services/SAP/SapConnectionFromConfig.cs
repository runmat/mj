using System;
using System.Configuration;
using System.Web;
using SapORM.Contracts;
// ReSharper disable RedundantUsingDirective
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace SapORM.Services
{
    public class SapConnectionFromConfig : ISapConnection 
    {
        public bool ProdSAP 
        { 
            get
            {
                var prodSapForWebLogonUser = TryGetProdSapForWebLogonUser();
                if (prodSapForWebLogonUser != null)
                    return prodSapForWebLogonUser.GetValueOrDefault();

                return (ConfigurationManager.AppSettings["ProdSAP"].ToLower() == "true");
            }
        }

        public string SAPAppServerHost
        {
            get { return ConfigurationManager.AppSettings[ProdSAP ? "SAPAppServerHost" : "TESTSAPAppServerHost"]; }
        }

        public int SAPSystemNumber
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings[ProdSAP ? "SAPSystemNumber" : "TESTSAPSystemNumber"]); }
        }

        public string SAPClient 
        {
            get { return ConfigurationManager.AppSettings[ProdSAP ? "SAPClient" : "TESTSAPClient"]; }
        }

        public string SAPUsername 
        {
            get { return ConfigurationManager.AppSettings[ProdSAP ? "SAPUsername" : "TESTSAPUsername"]; }
        }

        public string SAPPassword
        {
            get { return ConfigurationManager.AppSettings[ProdSAP ? "SAPPassword" : "TESTSAPPassword"]; }
        }

        public string ErpConnectLicense
        {
            get { return ConfigurationManager.AppSettings["ErpConnectLicense"]; }
        }

        public string SqlServerConnectionString
        {
            get { return ConfigurationManager.AppSettings["Connectionstring"]; }
        }


        public bool? TryGetProdSapForWebLogonUser()
        {
            if (HttpContext.Current == null || HttpContext.Current.Session == null)
                return null;

            var webLogonUserOnProdDataSystem = HttpContext.Current.Session["WebLogonUserOnProdDataSystem"];
            if (webLogonUserOnProdDataSystem == null)
                return null;

            return (bool)webLogonUserOnProdDataSystem;
        }
    }
}
