using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.General.Services
{
    public class CkgGeneralDataServiceTest : Store, ICkgGeneralDataService
    {
        #region Administration + Infrastructure

        public string GroupName { get { return LogonContext.GroupName; } }

        public string UserName { get { return LogonContext.UserName; } }

        public string UserID { get { return LogonContext.UserID; } }

        public IAppSettings AppSettings { get; private set; }

        public ILogonContext LogonContext { get; private set; }

        #endregion


        #region General data + Business logic

        public List<KundeAusHierarchie> KundenAusHierarchie { get; private set; }

        public List<Land> Laender
        {
            get
            {
                return PropertyCacheGet(() =>
                    new List<Land>
                        {
                            new Land { ID = "", Name = "(bitte wählen)" },
                            new Land { ID = "DE", Name = "Deutschland" },
                            new Land { ID = "CH", Name = "Schweiz" },
                            new Land { ID = "AT", Name = "Österreich" },
                        })
                            .OrderBy(w => w.Name).ToList();
            }
        }

        public List<SelectItem> Versicherungen { get; private set; }

        // ReSharper disable ConvertClosureToMethodGroup
        public List<VersandOption> VersandOptionen
        {
            get
            {
                return PropertyCacheGet(() =>
                    new List<VersandOption>
                        {
                            new VersandOption { ID = "", Name = "(bitte wählen)" },
                            new VersandOption { ID = "PO", Name = "Post" },
                            new VersandOption { ID = "DH", Name = "DHL" },
                            new VersandOption { ID = "UP", Name = "UPS Standard" },
                        })
                        .OrderBy(w => w.Name).ToList();
            }
        }

        public List<ZulassungsOption> ZulassungsOptionen { get; private set; }
        public List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; private set; }

        public List<FahrzeugStatus> FahrzeugStatusWerte { get; private set; }

        public List<Hersteller> Hersteller { get { return new List<Hersteller>(); } }

        #endregion

        public string ToDataStoreKundenNr(string kundenNr)
        {
            return kundenNr;
        }

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
            AppSettings = appSettings;
            LogonContext = logonContext;
        }

        public string CountryPlzValidate(string country, string plz)
        {
            if (country.NotNullOrEmpty().ToUpper() == "DE" && plz.IsNotNullOrEmpty() && plz.Length != 5)
                return "Deutsche Postleitzahlen müssen 5-stellig sein";


            return "";
        }
        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            return "";
        }
    }
}

