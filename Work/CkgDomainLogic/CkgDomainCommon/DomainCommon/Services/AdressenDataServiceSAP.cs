using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using Adresse = CkgDomainLogic.DomainCommon.Models.Adresse;
using AppModelMappings = CkgDomainLogic.DomainCommon.Models.AppModelMappings;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class AdressenDataServiceSAP : CkgGeneralDataServiceSAP, IAdressenDataService
    {
        public List<Adresse> Adressen { get { return PropertyCacheGet(() => LoadFromSap()); } }

        public List<Adresse> RgAdressen { get { return PropertyCacheGet(() => GetCustomerAdressen("RG")); } }
        public List<Adresse> ReAdressen { get { return PropertyCacheGet(() => GetCustomerAdressen("RE")); } }
        public Adresse AgAdresse { get { return PropertyCacheGet(() => GetCustomerAdressen("AG").FirstOrDefault()); } }


        public AdressenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshAdressen()
        {
            PropertyCacheClear(this, m => m.Adressen);
            PropertyCacheClear(this, m => m.RgAdressen);
            PropertyCacheClear(this, m => m.ReAdressen);
            PropertyCacheClear(this, m => m.AgAdresse);
        }

        public Adresse SaveAdresse(Adresse adresse, Action<string, string> addModelError)
        {
            var storedItem = StoreToSap(adresse, addModelError, false);
            return ModelMapping.Copy(storedItem, Adressen.First(c => c.ID == storedItem.ID));
        }

        public void DeleteAdresse(Adresse adresse)
        {
            StoreToSap(adresse, null, true);
            Adressen.RemoveAll(c => c.ID == adresse.ID);
        }

        public List<Adresse> LoadFromSap(string internalKey = null, string kennung = null, bool kundennrMitgeben = true)
        {
            Z_DPM_READ_ZDAD_AUFTR_006.Init(SAP);
            if (kundennrMitgeben)
                SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            if (internalKey.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_POS_KURZTEXT", internalKey);
            if (kennung.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_KENNUNG", kennung);

            var sapList = Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB.GetExportListWithExecute(SAP);

            return AppModelMappings.MapAdressenFromSAP.Copy(sapList).ToList();
        }

        Adresse StoreToSap(Adresse adresse, Action<string, string> addModelError, bool deleteOnly)
        {
            var insertMode = adresse.InternalKey.IsNullOrEmpty();
            if (insertMode)
            {
                // need to store a web private key here (InternalKey2), 
                // because we don't know the public key (InternalKey) our data store (SAP) will generate
                adresse.InternalKey2 = Guid.NewGuid().ToString();
            }

            var sapAdresse = AppModelMappings.MapAdressenToSAP.CopyBack(adresse);

            Z_DPM_PFLEGE_ZDAD_AUFTR_006.Init(SAP);
            SAP.SetImportParameter("I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            var verarbeitungsKennzeichen = deleteOnly ? "D" : insertMode ? "N" : "U";
            SAP.SetImportParameter("I_VERKZ", verarbeitungsKennzeichen);

            try
            {
                var importList = Z_DPM_PFLEGE_ZDAD_AUFTR_006.GT_WEB.GetImportList(SAP);
                importList.Add(sapAdresse);
                SAP.ApplyImport(importList);
                SAP.Execute();
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

            //
            // prüflesen:
            //
            if (!deleteOnly)
            {
                var savedID = adresse.ID;
                Adresse savedAdresse;
                if (insertMode)
                {
                    MarkForRefreshAdressen();
                    // need to use our web private key here (InternalKey2), 
                    // because we don't know the public key (InternalKey) our data store (SAP) generated
                    savedAdresse = Adressen.FirstOrDefault(a => a.InternalKey2 == adresse.InternalKey2);

                    if (savedAdresse != null)
                        // As long we are in insert mode, let's transfer the public key from our data store (SAP) to our origin item:
                        adresse.InternalKey = savedAdresse.InternalKey;
                }
                else
                    savedAdresse = LoadFromSap(adresse.InternalKey).FirstOrDefault();

                if (savedAdresse != null)
                    savedAdresse.ID = savedID;

                if (addModelError != null)
                    ModelMapping.Differences(adresse, savedAdresse).ForEach(
                        differentPropertyName => addModelError(differentPropertyName, "Ihr Wert wurde aus unbekannten Gründen nicht gespeichert."));

                //adresse = savedAdresse;
            }

            return adresse;
        }

        public List<Adresse> GetCustomerAdressen(string addressType)
        {
            SAP.Init("Z_M_PARTNER_AUS_KNVP_LESEN");
            SAP.SetImportParameter("KUNNR", LogonContext.KundenNr.ToSapKunnr());
            if (CkgDomainRules.IstKroschkeAutohaus(LogonContext.KundenNr))
                SAP.SetImportParameter("GRUPPE", GroupName);
            SAP.Execute();

            var sapItems = Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE.GetExportList(SAP);
            var webItems = AppModelMappings.Z_M_PARTNER_AUS_KNVP_LESEN_AUSGABE_To_Adresse.Copy(sapItems).Where(a => a.Kennung == addressType).OrderBy(w => w.Name1).ToList();

            var id = 0;
            webItems.ForEach(item =>
            {
                item.ID = ++id;
            });

            return webItems;
        }
    }
}
