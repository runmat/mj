using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zanf.Contracts;
using CkgDomainLogic.Zanf.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Zanf.Models.AppModelMappings;

namespace CkgDomainLogic.Zanf.Services
{
    public class ZanfReportDataServiceSAP : CkgGeneralDataServiceSAP, IZanfReportDataService
    {
        public ZulassungsAnforderungSuchparameter Suchparameter { get; set; }

        public List<ZulassungsAnforderung> ZulassungsAnforderungen { get { return PropertyCacheGet(() => LoadZulassungsAnforderungenFromSap().ToList()); } }

        public ZanfReportDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new ZulassungsAnforderungSuchparameter { Auswahl = "A" };
        }

        public void MarkForRefreshZulassungsAnforderungen()
        {
            PropertyCacheClear(this, m => m.ZulassungsAnforderungen);
        }

        private IEnumerable<ZulassungsAnforderung> LoadZulassungsAnforderungenFromSap()
        {
            Z_ZANF_READ_DATEN_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.AnlageDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_ERDAT_VON", Suchparameter.AnlageDatumRange.StartDate);
                SAP.SetImportParameter("I_ERDAT_BIS", Suchparameter.AnlageDatumRange.EndDate);
            }

            if (Suchparameter.AusfuehrungsDatumRange.IsSelected)
            {
                SAP.SetImportParameter("I_ADATUM_VON", Suchparameter.AusfuehrungsDatumRange.StartDate);
                SAP.SetImportParameter("I_ADATUM_BIS", Suchparameter.AusfuehrungsDatumRange.EndDate);
            }

            if (!String.IsNullOrEmpty(Suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_FAHRG", Suchparameter.FahrgestellNr.ToUpper());

            if (!String.IsNullOrEmpty(Suchparameter.ReferenzNr))
                SAP.SetImportParameter("I_REFNR", Suchparameter.ReferenzNr);

            switch (Suchparameter.Auswahl)
            {
                case "A":
                    SAP.SetImportParameter("I_OFFEN", "X");
                    SAP.SetImportParameter("I_ERL", "X");
                    SAP.SetImportParameter("I_KLAERF", "X");
                    break;

                case "O":
                    SAP.SetImportParameter("I_OFFEN", "X");
                    break;

                case "D":
                    SAP.SetImportParameter("I_ERL", "X");
                    break;

                case "K":
                    SAP.SetImportParameter("I_KLAERF", "X");
                    break;
            }

            SAP.Execute();

            var zanfList = AppModelMappings.Z_ZANF_READ_DATEN_01_GT_DATEN_To_ZulassungsAnforderung.Copy(Z_ZANF_READ_DATEN_01.GT_DATEN.GetExportList(SAP)).ToList();
            var textList = Z_ZANF_READ_DATEN_01.GT_KLAERFALLTEXT.GetExportList(SAP);
            var adrsList = AppModelMappings.Z_ZANF_READ_DATEN_01_GT_ADRESS_To_ZanfAdresse.Copy(Z_ZANF_READ_DATEN_01.GT_ADRESS.GetExportList(SAP)).ToList();
            
            foreach (var item in zanfList)
            {
                var zanf = item;

                List<string> textZeilen;
                List<ZanfAdresse> adrsZeilen;

                if (zanf.AnforderungsNr.IsNotNullOrEmpty())
                {
                    textZeilen = textList.Where(t => t.ORDERID == zanf.AnforderungsNr).OrderBy(t => t.ZEILENNR).Select(t => t.BEMERKUNG).ToList();
                    adrsZeilen = adrsList.Where(t => t.AnforderungsNr == zanf.AnforderungsNr).ToList();
                }
                else
                {
                    textZeilen = textList.Where(t => t.VBELN == zanf.AuftragsNr).OrderBy(t => t.ZEILENNR).Select(t => t.BEMERKUNG).ToList();
                    adrsZeilen = adrsList.Where(t => t.AuftragsNr == zanf.AuftragsNr).ToList();
                }

                zanf.KlaerfallText = String.Join("<br/>", textZeilen);
                zanf.KlaerfallTextPreview = textZeilen.FirstOrDefault();

                var halterAdresse = adrsZeilen.FirstOrDefault(a => a.Partnerrolle == "ZH");
                if (halterAdresse != null)
                    zanf.Halter = halterAdresse;

                var haendlerAdresse = adrsZeilen.FirstOrDefault(a => a.Partnerrolle == "ZE");
                if (haendlerAdresse != null)
                    zanf.Haendler = haendlerAdresse;
            }

            return zanfList;
        }
    }
}
