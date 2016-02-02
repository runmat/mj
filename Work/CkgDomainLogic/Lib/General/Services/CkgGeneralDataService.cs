using System.Collections.Generic;
using System.Security;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class CkgGeneralDataService : Store, ICkgGeneralDataService
    {
        #region Administration + Infrastructure

        public string GroupName { get { return LogonContext.GroupName; } }

        public string UserName { get { return LogonContext.UserName; } }

        public string UserID { get { return LogonContext.UserID; } }

        public IAppSettings AppSettings { get; private set; }

        public ILogonContextDataService LogonContext { get; private set; }

        #endregion


        #region General data + Business logic

        public List<KundeAusHierarchie> KundenAusHierarchie { get; private set; }
        public List<Land> Laender { get { return null; } }
        public List<SelectItem> Versicherungen { get; private set; }

        public List<VersandOption> VersandOptionen { get { return null; } }
        public List<ZulassungsOption> ZulassungsOptionen { get; private set; }
        public List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; private set; }

        public List<FahrzeugStatus> FahrzeugStatusWerte { get { return null; } }

        public List<Hersteller> Hersteller { get { return new List<Hersteller>();} }

        #endregion


        public string ToDataStoreKundenNr(string kundenNr)
        {
            return kundenNr;
        }

        public string CheckFahrgestellnummer(string fin, string pruefziffer)
        {
            throw new System.NotImplementedException();
        }

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
            AppSettings = appSettings;
            LogonContext = (ILogonContextDataService)logonContext;
        }

        public string CountryPlzValidate(string country, string plz)
        {
            if (country.NotNullOrEmpty().ToUpper() == "DE" && plz.IsNotNullOrEmpty() && plz.Length != 5)
                return "Deutsche Postleitzahlen müssen 5-stellig sein";

            return "";
        }

        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            throw new System.NotImplementedException();
        }
    }
}

