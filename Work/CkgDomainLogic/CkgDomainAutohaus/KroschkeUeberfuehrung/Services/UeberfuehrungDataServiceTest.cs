using System.Collections.Generic;
using System.IO;
using System.Web;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using CkgDomainLogic.Ueberfuehrung.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.Ueberfuehrung.Services
{
    public class UeberfuehrungDataServiceTest : IUeberfuehrungDataService
    {
        public IAppSettings AppSettings { get; private set; }

        public ILogonContext LogonContext { get; private set; }

        public string AuftragGeber { get; set; }

        public string KundenNr { get; set; }

        public List<KundeAusHierarchie> KundenAusHierarchie { get; private set; }

        private List<Land> _laender;
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

        private List<WebUploadProtokoll> _webUploadProtokolle;
        public List<WebUploadProtokoll> WebUploadProtokolle
        {
            get
            {
                return (_webUploadProtokolle ?? (_webUploadProtokolle = new List<WebUploadProtokoll>
                                                                              {
                                                                                  new WebUploadProtokoll { Protokollart = "E-VW-AU", Kategorie = "1" },
                                                                                  new WebUploadProtokoll { Protokollart = "E-VW-RU", Kategorie = "1" },
                                                                                  new WebUploadProtokoll { Protokollart = "MIETVERTR", Kategorie = "1" },
                                                                                  new WebUploadProtokoll { Protokollart = "VW-DAD", Kategorie = "1" },
                                                                                  new WebUploadProtokoll { Protokollart = "VWPROTOK", Kategorie = "1" },
                                                                              }));
            }
        }

        public UeberfuehrungDataServiceTest(IAppSettings appSettings, ILogonContext logonContext)
        {
            AppSettings = appSettings;
            LogonContext = logonContext;
        }


        public void GetTransportTypenAndDienstleistungen(out List<TransportTyp> transportTypen, out List<Dienstleistung> dienstleistungen)
        {
            transportTypen = XmlService.XmlDeserializeFromFile<List<TransportTyp>>(Path.Combine(AppSettings.RootPath, @"App_Data\XmlData\TransportTypen.xml"));
            dienstleistungen = XmlService.XmlDeserializeFromFile<List<Dienstleistung>>(Path.Combine(AppSettings.RootPath, @"App_Data\XmlData\Dienstleistungen.xml"));
        }

        public List<Adresse> GetFahrtAdressen(string[] addressTypes)
        {
            return FahrtAdressen;
        }

        public List<Adresse> GetRechnungsAdressen()
        {
            return RechnungsAdressen;
        }

        public List<KclGruppe> GetKclGruppenDaten()
        {
            return new List<KclGruppe>();
        }

        public List<HistoryAuftrag> GetHistoryAuftraege(HistoryAuftragFilter filter)
        {
            return new List<HistoryAuftrag>();
        }

        public bool TryLoadFahrzeugFromFIN(ref Fahrzeug modelFahrzeug)
        {
            return true;
        }

        public List<Adresse> FahrtAdressen
        {
            get
            {
                return XmlService.XmlDeserializeFromFile<List<Adresse>>(Path.Combine(AppSettings.RootPath, @"App_Data\XmlData\FahrtAdressen.xml"));
            }
        }

        public List<Adresse> RechnungsAdressen
        {
            get
            {
                var webItems = new List<Adresse>
                           {
                                new Adresse
                                    {
                                        Firma = "Christoph Kroschke GmbH", Name2 = "", Strasse = "Ladestraße 1", PLZ = "22926", Ort = "Ahrensburg", SubTyp = "RE"
                                    },
                                new Adresse
                                    {
                                        Firma = "DAD - Deutscher Autodienst", Name2 = "", Strasse = "Tremsbütteler Weg 47", PLZ = "22941", Ort = "Bargteheide", SubTyp = "RE"
                                    },
                                new Adresse
                                    {
                                        Firma = "Volkswagen AG", Name2 = "", Strasse = "Berliner Ring 2", PLZ = "38440", Ort = "Wolfsburg", SubTyp = "RG"
                                    },
                                new Adresse
                                    {
                                        Firma = "AUDI AG", Name2 = "", Strasse = "Bayern Straße 2", PLZ = "84564", Ort = "Ingolstadt", SubTyp = "RG"
                                    },
                                new Adresse
                                    {
                                        Firma = "Daimler Benz AG", Name2 = "", Strasse = "Porsche Str. 122", PLZ = "78501", Ort = "Stuttgart", SubTyp = "RG"
                                    },
                           };
                
                var id = 0;
                webItems.ForEach(item =>
                {
                    item.ID = ++id;
                    item.SelectedID = item.ID;
                    item.Typ = AdressenTyp.RechnungsAdresse;
                });

                return webItems;
            }
        }

        public bool SaveUploadFile(HttpPostedFileBase file, string fahrtIndex, out string fileName, out string errorMessage)
        {
            fileName = file.FileName;
            errorMessage = "";

            return true;
        }

        public List<UeberfuehrungsAuftragsPosition> Save(Step[] steps, List<Fahrt> fahrten)
        {
            return new List<UeberfuehrungsAuftragsPosition>();
        }

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}/{1}", AppSettings.UploadFilePathTemp, LogonContext.UserID));
        }

        void IUeberfuehrungDataService.OnInit(ILogonContext logonContext, IAppSettings appSettings)
        {
        }

        public string ToDataStoreKundenNr(string kundenNr)
        {
            return kundenNr.ToSapKunnr();
        }

        public string GetZulassungskreisFromPostcodeAndCity(string postCode, string city)
        {
            throw new System.NotImplementedException();
        }

        public string CheckFahrgestellnummer(string fin, string pruefziffer)
        {
            throw new System.NotImplementedException();
        }

        public void Init(IAppSettings appSettings, ILogonContext logonContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
