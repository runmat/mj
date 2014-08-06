// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Uebfuehrg.Models.AppModelMappings;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Uebfuehrg.Services
{
    public class UebfuehrgDataServiceSAP : CkgGeneralDataServiceSAP, IUebfuehrgDataService
    {
        public string KundenNr { get { return LogonContext.KundenNr; } }

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
                                                                                            AuftragGeberOderKundenNr.ToSapKunnr(),
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
            LogonContext = logonContext;
            AppSettings = appSettings;
        }

        public List<UeberfuehrungsAuftragsPosition> Save(List<Fahrt> fahrten)
        {
            return null;
        }
    }
}
