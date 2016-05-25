using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.DomainCommon.Services;
using CkgDomainLogic.Partner.Contracts;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Fahrzeugbestand.Models.AppModelMappings;

namespace CkgDomainLogic.Partner.Services
{
    public class PartnerDataServiceSAP : AdressenDataServiceSAP, IPartnerDataService 
    {
        public string AuftragGeber { get; set; }

        public string AuftragGeberOderKundenNr { get { return AuftragGeber.IsNotNullOrEmpty() ? AuftragGeber : KundenNr; } }


        public PartnerDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        #region Partner-Adressen, overrides

        override protected Dictionary<string, string> FriendlyAdressenKennungTranslationDict
        {
            get
            {
                return new Dictionary<string, string> 
                {
                    { "ZO01", "HALTER" },
                    { "ZO02", "KAEUFER" },
                    { "ZO03", "ZAHLERKFZSTEUER" },
                };
            }
        }

        override public bool IsEqual(Adresse a1, Adresse a2)
        {
            return (a1.KundenNr.ToSapKunnr() == a2.KundenNr.ToSapKunnr());
        }


        public override List<Adresse> LoadFromSap(string internalKey = null, string kennungOverride = null,
            bool kundennrMitgeben = true)
        {           
            List<Adresse> result = null;

            AdressenKennung = "HALTER";
            var list = LoadFromSapPrivate(internalKey, kennungOverride);
            AdressenKennung = "KAEUFER";
            result = list.Concat(LoadFromSapPrivate(internalKey, kennungOverride)).ToList();
            AdressenKennung = "ZAHLERKFZSTEUER";
            return result.Concat(LoadFromSapPrivate(internalKey, kennungOverride)).ToList();
        }

        List<Adresse> LoadFromSapPrivate(string internalKey = null, string kennungOverride = null, bool kundennrMitgeben = true)
        {
            Z_AHP_READ_PARTNER.Init(SAP);

            SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());
            if (SubKundennr.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_KUNNR_PARVW", SubKundennr.ToSapKunnr());

            if (kennungOverride.IsNotNullOrEmpty() || AdressenKennung.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_PARTART", TranslateFromFriendlyAdressenKennung(kennungOverride.IsNotNullOrEmpty() ? kennungOverride : AdressenKennung));

            var sapList = Z_AHP_READ_PARTNER.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_AHP_READ_PARTNER_GT_OUT_To_Adresse.Copy(sapList, (s, d) =>
            {
                d.Kennung = AdressenKennung;
            }).ToList();
        }

        override protected List<Adresse> StoreToSap(List<Adresse> adressen, Action<string, string> addModelError, bool deleteOnly)
        {
            var insertMode = adressen.First().KundenNr.IsNullOrEmpty();

            foreach (var adr in adressen)
            {
                adr.Kennung = TranslateFromFriendlyAdressenKennung(adr.Kennung);
            }

            Z_AHP_CRE_CHG_PARTNER.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", KundenNr.ToSapKunnr());

            try
            {
                var adressenSap = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_GT_WEB_IMP_To_Adresse.CopyBack(adressen);
                SAP.ApplyImport(adressenSap);
                SAP.Execute();

                if (insertMode)
                {
                    adressen = AppModelMappings.Z_AHP_CRE_CHG_PARTNER_GT_OUT_To_Adresse.Copy(Z_AHP_CRE_CHG_PARTNER.GT_OUT.GetExportList(SAP)).ToList();
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

            return adressen;
        }

        #endregion
    }
}
