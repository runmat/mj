// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using CkgDomainLogic.FzgModelle.Contracts;
using CkgDomainLogic.FzgModelle.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.FzgModelle.Models.AppModelMappings;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.FzgModelle.Services
{
    public class ModellIdDataServiceSQL : IModellIdDataService
    {
        IPersistanceService PService { get { return ((LogonContextDataServiceBase)LogonContext).PersistanceService; } }
        const string PGroupKey = "ModellIdDataService";
        private string POwnerKey { get { return LogonContext.UserName; } }

        public List<ModellId> GetModellIds()
        {
            var containerList = PService.GetObjectContainers(POwnerKey, PGroupKey);
            var objectList = containerList.Select(container => (container.Object as ModellId)).ToListOrEmptyList();

            return objectList;
        }

        public string SaveModellId(ModellId modellId)
        {
            modellId = (ModellId)PService.SaveObject(modellId.ObjectKey, POwnerKey, PGroupKey, LogonContext.UserName, modellId);
            
            var error = "";

            return error;
        }


        #region ICkgGeneralDataService

        public IAppSettings AppSettings { get; private set; }

        public ILogonContext LogonContext { get; private set; }

        public string AuftragGeber { get; set; }

        public string KundenNr { get; set; }

        private List<Land> _laender;
        public List<KundeAusHierarchie> KundenAusHierarchie { get; private set; }

        public List<Land> Laender
        {
            get
            {
                return (_laender ?? (_laender = new List<Land>
                                                                              {
                                                                                  new Land { ID = "DE", Name = "Deutschland" },
                                                                                  new Land { ID = "DK", Name = "Dänemark" },
                                                                                  new Land { ID = "NL", Name = "Niederlande" },
                                                                                  new Land { ID = "PL", Name = "Polen" },
                                                                                  new Land { ID = "SE", Name = "Schweden" },
                                                                              }));
            }
        }

        public List<SelectItem> Versicherungen { get; private set; }


        private List<VersandOption> _versandOptionen;
        public List<VersandOption> VersandOptionen
        {
            get
            {
                return (_versandOptionen ?? (_versandOptionen = new List<VersandOption>
                                                                              {
                                                                                  new VersandOption { ID = "PO", Name = "Postversand" },
                                                                                  new VersandOption { ID = "DH", Name = "DHL" },
                                                                                  new VersandOption { ID = "HE", Name = "Hermes" },
                                                                              }));
            }
        }

        public List<ZulassungsOption> ZulassungsOptionen { get; private set; }
        public List<ZulassungsDienstleistung> ZulassungsDienstleistungen { get; private set; }

        public List<FahrzeugStatus> FahrzeugStatusWerte { get; private set; }

        public List<Hersteller> Hersteller { get { return new List<Hersteller>(); } }


        public string ToDataStoreKundenNr(string kundenNr)
        {
            return kundenNr.ToSapKunnr();
        }

        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            return "";
        }

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
            AppSettings = appSettings;
            LogonContext = logonContext;
        }

        #endregion
    }
}
