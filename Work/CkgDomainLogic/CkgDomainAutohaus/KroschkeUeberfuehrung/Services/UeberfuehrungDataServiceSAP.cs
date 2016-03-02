using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Ueberfuehrung.Contracts;
using CkgDomainLogic.Ueberfuehrung.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Ueberfuehrung.Models.AppModelMappings;

namespace CkgDomainLogic.Ueberfuehrung.Services
{
    public class UeberfuehrungDataServiceSAP : CkgGeneralDataServiceSAP, IUeberfuehrungDataService
    {
        public string KundenNr { get { return LogonContext.KundenNr; } }

        public string AuftragGeber { get; set; }

        public string AuftragGeberOderKundenNr { get { return AuftragGeber.IsNotNullOrEmpty() ? AuftragGeber : KundenNr; } }
        


        private List<WebUploadProtokoll> _webUploadProtokoll;
        public List<WebUploadProtokoll> WebUploadProtokolle { get { return (_webUploadProtokoll ?? (_webUploadProtokoll = AppModelMappings.Z_DPM_READ_TAB_PROT_01_GT_OUT_To_WebUploadProtokoll.Copy(Z_DPM_READ_TAB_PROT_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_KUNNR_AG", AuftragGeberOderKundenNr.ToSapKunnr())).OrderBy(w => w.Protokollart).ToList())); } }

        public UeberfuehrungDataServiceSAP(ISapDataService sap) : base(sap)
        {

        }

        public void GetTransportTypenAndDienstleistungen(out List<TransportTyp> transportTypen, out List<Dienstleistung> dienstleistungen)
        {
            // Transport-Typen und AvailableDienstleistungen laden

            var importListAG = Z_DPM_READ_LV_001.GT_IN_AG.GetImportListWithInit(SAP, "I_VWAG", "X");
            importListAG.Add(new Z_DPM_READ_LV_001.GT_IN_AG { AG = AuftragGeberOderKundenNr.ToSapKunnr() });
            SAP.ApplyImport(importListAG);

            var importListProcess = Z_DPM_READ_LV_001.GT_IN_PROZESS.GetImportList(SAP);
            importListProcess.Add(new Z_DPM_READ_LV_001.GT_IN_PROZESS { SORT1 = "7" });
            SAP.ApplyImport(importListProcess);

            SAP.Execute();

            // TransportTypen (5 Stück)
            var sapTransportTypen = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(SAP).Where(d => d.ASNUM.IsNullOrEmpty() && d.KTEXT1_H2.IsNullOrEmpty());
            transportTypen = AppModelMappings.Z_DPM_READ_LV_001_GT_OUT_DL_To_TransportTyp.Copy(sapTransportTypen).ToList();

            // alle AvailableDienstleistungen
            var sapDienstleistungen = Z_DPM_READ_LV_001.GT_OUT_DL.GetExportList(SAP).Where(d => !d.ASNUM.IsNullOrEmpty() && d.KTEXT1_H2.IsNullOrEmpty());
            dienstleistungen = AppModelMappings.Z_DPM_READ_LV_001_GT_OUT_DL_To_Dienstleistung.Copy(sapDienstleistungen).ToList();

            
            
            // <TEST>

            //var preis = 650;
            dienstleistungen.Where(dl => !dl.IstStandard).ToList().ForEach(dl =>
            {
                //dl.Preis = preis;
                //preis += 50;
            });
            dienstleistungen.Where(dl => dl.IstStandard).ToList().ForEach(dl =>
            {
                dl.Preis = null;
            });

            //XmlService.XmlSerializeToFile(transportTypen, Path.Combine(AppSettings.RootPath, @"App_Data\XmlData\TransportTypen.xml"));
            //XmlService.XmlSerializeToFile(dienstleistungen, Path.Combine(AppSettings.RootPath, @"App_Data\XmlData\AvailableDienstleistungen.xml"));

            // </TEST>
        }

        public List<Adresse> GetFahrtAdressen(string[] addressTypes)
        {
            // addressTypes <==> z.B. "ABHOLADRESSE", "AUSLIEFERUNG", "RÜCKHOLUNG", "HALTER"

            var id = 0;
            var adressen = addressTypes.SelectMany(type =>
                {
                    var sapItems = Z_M_IMP_AUFTRDAT_007.GT_WEB.GetExportListWithInitExecute(SAP,
                                                                                            "I_KUNNR, I_KENNUNG, I_NAME1, I_PSTLZ, I_ORT01",
                                                                                            AuftragGeberOderKundenNr.ToSapKunnr(),
                                                                                            type, "*", "*", "*");
                    var webItems = AppModelMappings.Z_M_IMP_AUFTRDAT_007_GT_WEB_To_Adresse.Copy(sapItems).OrderBy(w => w.Firma).ToList();
                    webItems.ForEach(item =>
                                            {
                                                item.ID = ++id;
                                                item.SelectedID = item.ID;
                                                item.Typ = AdressenTyp.FahrtAdresse;
                                            });

                    return webItems;
                }).ToList();
            
            // <TEST>
            //XmlService.XmlSerializeToFile(adressen, Path.Combine(AppSettings.RootPath, @"App_Data\XmlData\FahrtAdressen.xml"));
            // </TEST>

            return adressen;
        }

        public List<Adresse> GetRechnungsAdressen()
        {
            SAP.Init("Z_M_PARTNER_AUS_KNVP_LESEN");
            SAP.SetImportParameter("KUNNR", KundenNr.ToSapKunnr());
            if (CkgDomainRules.IstKroschkeAutohaus(KundenNr))
                SAP.SetImportParameter("GRUPPE", GroupName);
            SAP.Execute();

            var sapItems = Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE.GetExportList(SAP);
            var webItems = AppModelMappings.Z_M_PARTNER_AUS_KNVP_LESEN_AUSGABE_To_Adresse.Copy(sapItems).OrderBy(w => w.Firma).ToList();
            if (webItems.Count > 1)
                webItems.Insert(0, new Adresse { NickName = "(Bitte wählen)" });

            if (webItems.None(item => item.SubTyp.IsNotNullOrEmpty()))
            {
                webItems.ForEach(item => item.SubTyp = "RE");
                var rgItems = webItems.Copy((source, destination) => destination.SubTyp = "RG");
                webItems = webItems.Concat(rgItems).ToList();
            }

            var id = 0;
            webItems.ForEach(item =>
            {
                item.ID = ++id;
                item.SelectedID = item.ID;
                item.Typ = AdressenTyp.RechnungsAdresse;
            });

            return webItems;
        }

        //public List<KclGruppe> GetKclGruppenDaten()
        //{
        //    var sapData = Z_V_Kcl_Gruppendaten.ZZGRUPPENDATEN.GetExportListWithInitExecute(SAP, "KUNNR, GRUPPE", LogonContext.KundenNr.ToSapKunnr(), LogonContext.GroupName);
        //    return AppModelMappings.Z_V_Kcl_Gruppendaten_ZZGRUPPENDATEN_To_KclGruppe.Copy(sapData).ToList();
        //}

        public List<HistoryAuftrag> GetHistoryAuftraege(HistoryAuftragFilter filter)
        {
            var importList = Z_V_UEBERF_AUFTR_KUND_PORT.T_SELECT.GetImportListWithInit(SAP);
            AppModelMappings.Z_V_Ueberf_Auftr_Kund_Port_T_SELECT_To_HistoryAuftragFilter.CopyBack(filter);
            importList.Add(AppModelMappings.Z_V_Ueberf_Auftr_Kund_Port_T_SELECT_To_HistoryAuftragFilter.CopyBack(filter));
            SAP.ApplyImport(importList);
            SAP.Execute();
            var sapAuftraege = Z_V_UEBERF_AUFTR_KUND_PORT.T_AUFTRAEGE.GetExportList(SAP);

            return AppModelMappings.Z_V_Ueberf_Auftr_Kund_Port_T_AUFTRAEGE_To_HistoryAuftrag.Copy(sapAuftraege).ToList();
        }

        public bool TryLoadFahrzeugFromFIN(ref Fahrzeug modelFahrzeug)
        {
            if (!modelFahrzeug.RequestLoadCarData)
                return false;

            var searchFIN = modelFahrzeug.FIN;
            var searchKennzeichen = modelFahrzeug.Kennzeichen;
            var searchReferenznummer = "";

            switch (modelFahrzeug.RequestLoadCarDataSource)
            {
                case "FIN":
                    searchKennzeichen = "";
                    break;
                case "Kennzeichen":
                    searchFIN = "";
                    break;
            }

            if (searchFIN.IsEmpty() && searchKennzeichen.IsEmpty() && searchReferenznummer.IsEmpty())
                return false;

            var sapItems = Z_DPM_READ_EQUI_003.GT_OUT.GetExportListWithInitExecute(SAP,
                                                                                    "I_KUNNR_AG, I_EQTYP, I_CHASSIS_NUM, I_LICENSE_NUM, I_LIZNR",
                                                                                    KundenNr.ToSapKunnr(),
                                                                                    "B",
                                                                                    searchFIN, searchKennzeichen, searchReferenznummer);
            if (sapItems.Count > 0)
            {
                var sapItem = sapItems.First();

                // Fahrzeugdaten ergänzen aus FIN:
                modelFahrzeug.FIN = sapItem.CHASSIS_NUM;
                if (sapItem.LICENSE_NUM.NotNullOrEmpty().Contains("-"))
                {
                    var kennzArray = sapItem.LICENSE_NUM.Split('-');
                    modelFahrzeug.KennzeichenOrt = kennzArray[0];
                    modelFahrzeug.KennzeichenRest = kennzArray[1];
                }

                modelFahrzeug.Typ = string.Format("{0} {1}", sapItem.ZZHERST_TEXT, sapItem.ZZHANDELSNAME);
                if (modelFahrzeug.Typ.IsValid() && modelFahrzeug.Typ.Length > 25)
                    modelFahrzeug.Typ = sapItem.ZZHANDELSNAME;

                if (sapItem.LIZNR.IsValid())
                    modelFahrzeug.Referenznummer = sapItem.LIZNR;
            }


            modelFahrzeug.RequestLoadCarData = false;
            return true;
        }

        public string GetUploadPathTemp()
        {
            return HttpContext.Current.Server.MapPath(string.Format(@"{0}/{1}", AppSettings.UploadFilePathTemp, UserID));
        }

        public void OnInit(ILogonContext logonContext, IAppSettings appSettings)
        {
            LogonContext = (ILogonContextDataService)logonContext;
            AppSettings = appSettings;

        }

        public bool SaveUploadFile(HttpPostedFileBase file, string fahrtIndex, out string fileName, out string errorMessage)
        {
            fileName = "";
            errorMessage = "";

            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.InputStream.CopyTo(ms);
                            var array = ms.GetBuffer();

                            var bytes = array.ToArray();
                            var webserverFilePath = GetUploadPathTemp();
                            if (!FileService.TryDirectoryCreate(webserverFilePath))
                                throw new IOException(string.Format("Fehler, Upload-Quellverzeichnis kann nicht erstellt werden: '{0}'", webserverFilePath));

                            fileName = string.Format("{0}-{1}{2}", Path.GetFileNameWithoutExtension(fileName), fahrtIndex, Path.GetExtension(fileName));
                            var webserverFileName = Path.Combine(webserverFilePath, fileName);

                            if (!FileService.TryFileCopyBytes(bytes, webserverFileName))
                                throw new IOException(string.Format("Fehler, Upload-Datei kann nicht kopiert werden nach: '{0}'", webserverFileName));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }

            return true;
        }

        public List<UeberfuehrungsAuftragsPosition> Save(Step[] steps, List<Fahrt> fahrten)
        {
            var returnList = new List<UeberfuehrungsAuftragsPosition>();
            var webUser = LogonContext.UserName;  // LogonContext.FullName
            var webUserEmail = "test@test.de";

            var fahrzeugStep = steps.FirstOrDefault(step => step.GroupName == "FAHRZEUGE");
            var fahrtenStep = steps.FirstOrDefault(s => s.GroupName == "FAHRTEN");
            var dienstleistungsStep = steps.FirstOrDefault(s => s.GroupName == "DIENSTLEISTUNGEN");
            if (fahrzeugStep == null)
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Fahrzeuge angegeben" });
                return returnList;
            }
            if (fahrtenStep == null)
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Fahrten vorhanden" });
                return returnList;
            }
            if (dienstleistungsStep == null)
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Dienstleistungen vorhanden" });
                return returnList;
            }


            var rAdressen = fahrzeugStep.CurrentSubStepForms.OfType<Adresse>().Where(a => a.Typ == AdressenTyp.RechnungsAdresse);
            if (rAdressen.None())
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Rechnungsadressen verfügbar" });
                return returnList;
            }
            var rgAdresse = rAdressen.FirstOrDefault(a => a.SubTyp == "RG");
            var reAdresse = rAdressen.FirstOrDefault(a => a.SubTyp == "RE");
            if (rgAdresse == null)
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Rechnungszahler angegeben" });
                return returnList;
            }
            if (reAdresse == null)
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Rechnungsempfänger angegeben", FahrtIndex = "1" });
                return returnList;
            }

            var fahrtAdressen = fahrtenStep.CurrentSubStepForms.OfType<Adresse>().Where(m => !m.IsEmpty).ToList();
            var i = 0; fahrtAdressen.ForEach(adresse => adresse.FahrtIndexAktuellTmp = (i++).ToString());
            var fahrzeuge = fahrzeugStep.AllSubStepForms.OfType<Fahrzeug>().Where(m => !m.IsEmpty).ToList();
            var dienstleistungen = dienstleistungsStep.AllSubStepForms.OfType<DienstleistungsAuswahl>().SelectMany(auswahl => auswahl.GewaehlteDienstleistungen).ToList();
            var protokolle = dienstleistungsStep.AllSubStepForms.OfType<UploadFiles>().SelectMany(u => u.UploadProtokolle).ToList();
            var kurzBemerkungen = dienstleistungsStep.AllSubStepForms.OfType<Bemerkungen>().SelectMany(u => u.BemerkungAsKurzBemerkungen).ToList();

            var vorgangsNr = SAP.GetExportParameterWithInitExecute("Z_UEB_NEXT_NUMBER_VORGANG_01", "E_VORGANG", "");
            if (vorgangsNr.IsNullOrEmpty())
                return returnList;

            // Fahrt "0" zur Abholadresse einfügen:
            var firstFahrt = fahrten.FirstOrDefault();
            fahrten.Insert(0, new Fahrt
            {
                FahrtIndex = "0",
                ZielAdresse = firstFahrt == null ? null : firstFahrt.StartAdresse,
            });

            fahrten.ForEach(fahrt => fahrt.VorgangsNummer = vorgangsNr);

            SAP.Init("Z_UEB_CREATE_ORDER_01",
                        "AG, RE, RG, WEB_USER, EMAIL_WEB_USER",
                        AuftragGeberOderKundenNr.ToSapKunnr(), reAdresse.KundenNr.ToSapKunnr(), rgAdresse.KundenNr.ToSapKunnr(), webUser, webUserEmail);

            fahrtAdressen.Add(CreateWebUserAdresse());

            var fahrtenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_FAHRTEN_To_Fahrt.CopyBack(fahrten).ToList();
            var adressenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_ADRESSEN_To_Adresse.CopyBack(fahrtAdressen).ToList();
            var fahrzeugeList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_FZG_To_Fahrzeug.CopyBack(fahrzeuge).ToList();
            var dienstleistungenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_DIENSTLSTGN_To_Dienstleistung.CopyBack(dienstleistungen).ToList();
            var protokolleList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_PROT_To_WebUploadProtokoll.CopyBack(protokolle).ToList();
            var bemerkungenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_BEM_To_KurzBemerkung.CopyBack(kurzBemerkungen).ToList();

            SAP.ApplyImport(fahrtenList);
            SAP.ApplyImport(adressenList);
            SAP.ApplyImport(fahrzeugeList);
            SAP.ApplyImport(dienstleistungenList);
            SAP.ApplyImport(protokolleList);
            SAP.ApplyImport(bemerkungenList);

            SAP.Execute();

            var sapReturnList = Z_UEB_CREATE_ORDER_01.GT_RET.GetExportList(SAP);

            returnList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_RET_To_UeberfuehrungsAuftrag.Copy(sapReturnList).ToList();

            return returnList;
        }

        Adresse CreateWebUserAdresse()
        {
            return new Adresse
            {
                FahrtIndexAktuellTmp = "AP",
                KundenNr = LogonContext.KundenNr.ToSapKunnr(),
                Firma = LogonContext.FirstName,
                Ansprechpartner = LogonContext.LastName,
                Telefon = LogonContext.UserInfo.Telephone2,
                Email = LogonContext.UserInfo.Mail
            };
        }
    }
}

