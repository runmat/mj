using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;
using SapORM.Models;
using Adresse = CkgDomainLogic.Uebfuehrg.Models.Adresse;
using AppModelMappings = CkgDomainLogic.Uebfuehrg.Models.AppModelMappings;
using Fahrzeug = CkgDomainLogic.Uebfuehrg.Models.Fahrzeug;

namespace CkgDomainLogic.Uebfuehrg.Services
{
    public class UebfuehrgDataServiceSAP : CkgGeneralDataServiceSAP, IUebfuehrgDataService
    {
        private string _kundenNr;
        public string KundenNr
        {
            get { return _kundenNr ?? (_kundenNr = LogonContext.KundenNr.ToSapKunnr()); }
            set { _kundenNr = value; }
        }

        public string AuftragGeber { get; set; }

        public string AuftragGeberOderKundenNr { get { return AuftragGeber.IsNotNullOrEmpty() ? AuftragGeber : KundenNr; } }
        

        public UebfuehrgDataServiceSAP(ISapDataService sap)
            : base(sap)
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
        }

        public List<Adresse> GetFahrtAdressen(string[] addressTypes)
        {
            // addressTypes <==> z.B. "ABHOLADRESSE", "AUSLIEFERUNG", "RÜCKHOLUNG", "HALTER"

            var id = 0;
            var adressen = addressTypes.SelectMany(type =>
                {
                    var sapItems = Z_M_IMP_AUFTRDAT_007.GT_WEB.GetExportListWithInitExecute(SAP,
                                                                                            "I_KUNNR, I_KENNUNG, I_NAME1, I_PSTLZ, I_ORT01",
                                                                                            LogonContext.KundenNr.ToSapKunnr(),
                                                                                            type, "*", "*", "*");
                    var webItems = AppModelMappings.Z_M_IMP_AUFTRDAT_007_GT_WEB_To_Adresse.Copy(sapItems).OrderBy(w => w.Name1).ToList();
                    webItems.ForEach(item =>
                                            {
                                                item.ID = ++id;
                                                item.AdressTyp = AdressenTyp.FahrtAdresse;
                                                if (item.Land.IsNullOrEmpty()) 
                                                    item.Land = "DE";
                                            });

                    return webItems;
                }).ToList();

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
            var webItems = AppModelMappings.Z_M_PARTNER_AUS_KNVP_LESEN_AUSGABE_To_Adresse.Copy(sapItems).OrderBy(w => w.Name1).ToList();

            if (webItems.None(item => item.SubTyp.IsNotNullOrEmpty()))
            {
                webItems.ForEach(item => item.SubTyp = "RE");
                var items = webItems.Copy((source, destination) => destination.SubTyp = "RG");
                webItems = webItems.Concat(items).ToList();
            }

            TryAddDefaultAddressOption(webItems, "RE");
            TryAddDefaultAddressOption(webItems, "RG");

            webItems.ForEach(item =>
                {
                    item.AdressTyp = AdressenTyp.RechnungsAdresse;
                    if (item.Land.IsNullOrEmpty())
                        item.Land = "DE";
                });

            return webItems;
        }

        private static void TryAddDefaultAddressOption(List<Adresse> webItems, string subTyp)
        {
            var items = webItems.Where(item => item.SubTyp == subTyp);
            if (items.Count() >= 2)
                webItems.Insert(0, new Adresse { Name1 = Localize.TranslateResourceKey(LocalizeConstants.DropdownDefaultOptionPleaseChoose), SubTyp = subTyp });
        }

        public void OnInit(ILogonContext logonContext, IAppSettings appSettings)
        {
            LogonContext = (ILogonContextDataService)logonContext;
            AppSettings = appSettings;
        }

        public List<UeberfuehrungsAuftragsPosition> Save(RgDaten rgDaten, List<CommonUiModel> stepModels, List<Fahrt> fahrten)
        {
            var returnList = new List<UeberfuehrungsAuftragsPosition>();
            var webUser = LogonContext.UserName;  
            var webUserEmail = "test@test.de";

            if (rgDaten.RgKundenNr.IsNullOrEmpty())
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Rechnungszahler angegeben" });
                return returnList;
            }
            if (rgDaten.ReKundenNr.IsNullOrEmpty())
            {
                returnList.Add(new UeberfuehrungsAuftragsPosition { AuftragsNr = "", Bemerkung = "Keine Rechnungsempfänger angegeben", FahrtIndex = "1" });
                return returnList;
            }

            var fahrtAdressen = stepModels.OfType<Adresse>().ToList();
            var i = 0; fahrtAdressen.ForEach(adresse => 
            { 
                adresse.FahrtIndexAktuellTmp = (i++).ToString();
                adresse.KundenNr = "";
            });
            var fahrzeuge = stepModels.OfType<Fahrzeug>().ToList();
            var dienstleistungsAuswahlen = stepModels.OfType<DienstleistungsAuswahl>();
            var dienstleistungen = dienstleistungsAuswahlen.SelectMany(a => a.GewaehlteDienstleistungen).ToList();
            var kurzBemerkungen = dienstleistungsAuswahlen.SelectMany(a => a.Bemerkungen.BemerkungAsKurzBemerkungen).ToList();
            var protokolle = dienstleistungsAuswahlen.SelectMany(a => a.UploadProtokolle).Where(up => !string.IsNullOrEmpty(up.Dateiname)).ToList();

            var vorgangsNr = SAP.GetExportParameterWithInitExecute("Z_UEB_NEXT_NUMBER_VORGANG_01", "E_VORGANG", "");
            if (vorgangsNr.IsNullOrEmpty())
                return returnList;

            fahrten.ForEach(fahrt => fahrt.VorgangsNummer = vorgangsNr);

            SAP.Init("Z_UEB_CREATE_ORDER_01",
                        "AG, RG, RE, WEB_USER, EMAIL_WEB_USER",
                        KundenNr.ToSapKunnr(), rgDaten.RgKundenNr.ToSapKunnr(), rgDaten.ReKundenNr.ToSapKunnr(), webUser, webUserEmail);

            fahrtAdressen.Add(CreateWebUserAdresse());

            var fahrtenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_FAHRTEN_To_Fahrt.CopyBack(fahrten).ToList();
            var adressenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_ADRESSEN_To_Adresse.CopyBack(fahrtAdressen).ToList();
            var fahrzeugeList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_FZG_To_Fahrzeug.CopyBack(fahrzeuge).ToList();
            var dienstleistungenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_DIENSTLSTGN_To_Dienstleistung.CopyBack(dienstleistungen).ToList();
            var bemerkungenList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_BEM_To_KurzBemerkung.CopyBack(kurzBemerkungen).ToList();
            var protokolleList = AppModelMappings.Z_UEB_CREATE_ORDER_01_GT_PROT_To_WebUploadProtokoll.CopyBack(protokolle).ToList();

            SAP.ApplyImport(fahrtenList);
            SAP.ApplyImport(adressenList);
            SAP.ApplyImport(fahrzeugeList);
            SAP.ApplyImport(dienstleistungenList);
            SAP.ApplyImport(bemerkungenList);
            SAP.ApplyImport(protokolleList);

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
                Name1 = LogonContext.FirstName,
                Ansprechpartner = LogonContext.LastName,
                Telefon = LogonContext.UserInfo.Telephone2,
                Email = LogonContext.UserInfo.Mail
            };
        }

        public List<HistoryAuftrag> GetHistoryAuftraege(HistoryAuftragSelector filter)
        {
            var importList = Z_V_UEBERF_AUFTR_KUND_PORT.T_SELECT.GetImportListWithInit(SAP);
            var sapFilter = AppModelMappings.Z_V_Ueberf_Auftr_Kund_Port_T_SELECT_To_HistoryAuftragSelector.CopyBack(filter);
            importList.Add(sapFilter);
            SAP.ApplyImport(importList);
            SAP.Execute();
            var sapAuftraege = Z_V_UEBERF_AUFTR_KUND_PORT.T_AUFTRAEGE.GetExportList(SAP);

            return AppModelMappings.Z_V_Ueberf_Auftr_Kund_Port_T_AUFTRAEGE_To_HistoryAuftrag.Copy(sapAuftraege).ToList();
        }

        public List<WebUploadProtokoll> GetProtokollArten()
        {
            Z_DPM_READ_TAB_PROT_01.Init(SAP, "I_KUNNR_AG", AuftragGeberOderKundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_READ_TAB_PROT_01_GT_OUT_To_WebUploadProtokoll.Copy(Z_DPM_READ_TAB_PROT_01.GT_OUT.GetExportListWithExecute(SAP).Where(x => x.WEB_UPLOAD.XToBool())).OrderBy(p => p.ProtokollartFormatted).ToList();
        }
    }
}
