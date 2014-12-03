// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeugbestand.Contracts;
using CkgDomainLogic.Fahrzeugbestand.Models;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeugbestand.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Fahrzeugbestand.Services
{
    public class FahrzeugAkteBestandDataServiceSAP : AdressenDataServiceSAP, IFahrzeugAkteBestandDataService
    {
        public string AuftragGeber { get; set; }

        public string AuftragGeberOderKundenNr { get { return AuftragGeber.IsNotNullOrEmpty() ? AuftragGeber : KundenNr; } }
        

        public FahrzeugAkteBestandDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        #region Load Fahrzeug-Akte-Bestand

        public List<FahrzeugAkteBestand> GetFahrzeugeAkteBestand(FahrzeugAkteBestandSelektor model)
        {
            return
                AppModelMappings.Z_AHP_READ_FZGBESTAND_GT_WEBOUT_To_FahrzeugAkteBestand.Copy(
                    GetSapFahrzeugeAkteBestand(model)).ToList();
        }

        private IEnumerable<Z_AHP_READ_FZGBESTAND.GT_WEBOUT> GetSapFahrzeugeAkteBestand(
            FahrzeugAkteBestandSelektor model)
        {
            Z_AHP_READ_FZGBESTAND.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            
            if (model.FIN.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_FIN", model.FIN);

            if (model.Halter.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_HALTER", model.Halter);

            if (model.Kaeufer.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_KAEUFER", model.Kaeufer);

            SAP.Execute();

            return Z_AHP_READ_FZGBESTAND.GT_WEBOUT.GetExportList(SAP);
        }

        #endregion


        public string SaveFahrzeugAkteBestand(FahrzeugAkteBestand fahrzeugAkteBestand)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_AHP_CRE_CHG_FZG_AKT_BEST.Init(SAP);
                    SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_USER", LogonContext.UserName);

                    var fzgList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP.GetImportList(SAP);

                    CreateRowForFahrzeug(fzgList, fahrzeugAkteBestand);

                    SAP.ApplyImport(fzgList);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResultList = Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_OUT_ERR.GetExportList(SAP);

                    var sapResult = sapResultList.FirstOrDefault();
                    if (sapResult != null)
                        return string.Format("Fehler, folgendes Fahrzeug konnte nicht gespeichert werden: FIN {0}, Fin-ID {1}", sapResult.FIN, sapResult.FIN_ID);

                    return "";
                });

            return error;
        }

        private static void CreateRowForFahrzeug(List<Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP> fzgList, FahrzeugAkteBestand f)
        {
            var sapFahrzeug = new Z_AHP_CRE_CHG_FZG_AKT_BEST.GT_WEB_IMP
            {
                FIN_ID = f.FinID,
                FIN = f.FIN,

                // Fahrzeug Akte
                ZZFABRIKNAME = f.FabrikName,
                ZZHANDELSNAME = f.HandelsName,
                ZZTYP_SCHL = f.TypSchluessel,
                ZZHERSTELLER_SCH = f.HerstellerSchluessel,
                ZZVVS_SCHLUESSEL = f.VvsSchluessel,
                ZZTYP_VVS_PRUEF = f.VvsPruefZiffer,

                // Fahrzeug Bestand
                KAEUFER = f.Kaeufer,
                HALTER = f.Halter,
            };

            fzgList.Add(sapFahrzeug);
        }


        #region Partner-Adressen, overrides

        override protected Dictionary<string, string> FriendlyAdressenKennungTranslationDict
        {
            get
            {
                return new Dictionary<string, string> {
                    { "ZO01", "HALTER" },
                    { "ZO02", "KAEUFER" },
                };
            }
        }

        override public bool IsEqual(Adresse a1, Adresse a2)
        {
            return (a1.KundenNr.ToSapKunnr() == a2.KundenNr.ToSapKunnr());
        }

        override public List<Adresse> LoadFromSap(string internalKey = null, string kennungOverride = null, bool kundennrMitgeben = true)
        {
            Z_AHP_READ_PARTNER.Init(SAP);
            
            SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());

            if (kennungOverride.IsNotNullOrEmpty() || AdressenKennung.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_PARTART", TranslateFromFriendlyAdressenKennung(kennungOverride.IsNotNullOrEmpty() ? kennungOverride : AdressenKennung));

            var sapList = Z_AHP_READ_PARTNER.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_AHP_READ_PARTNER_GT_OUT_To_Adresse.Copy(sapList, (s, d) =>
                {
                    d.Kennung = AdressenKennung;
                }).ToList();
        }

        override protected Adresse StoreToSap(Adresse adresse, Action<string, string> addModelError, bool deleteOnly)
        {
            var insertMode = adresse.KundenNr.IsNullOrEmpty();

            var sapAdresse = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_GT_WEB_IMP_To_Adresse.CopyBack(adresse);
            sapAdresse.PARTART = TranslateFromFriendlyAdressenKennung(sapAdresse.PARTART);

            Z_AHP_CRE_CHG_PARTNER.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());

            try
            {
                var importList = Z_AHP_CRE_CHG_PARTNER.GT_WEB_IMP.GetImportList(SAP);
                importList.Add(sapAdresse);
                SAP.ApplyImport(importList);
                SAP.Execute();

                if (insertMode)
                {
                    var savedSapItem = Z_AHP_CRE_CHG_PARTNER.GT_OUT.GetExportList(SAP).FirstOrDefault();
                    if (savedSapItem != null)
                        adresse.KundenNr = savedSapItem.KUNNR;
                }
            }
            catch (Exception e)
            {
                if (addModelError != null)
                {
                    var errorPropertyName = e.Message.GetPartEnclosedBy('\'');
                    if (errorPropertyName.IsNullOrEmpty())
                        errorPropertyName = "SapError";

                    addModelError(errorPropertyName, e.Message);
                }
            }

            return adresse;
        }

        #endregion
    }
}
