using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Zanf.Contracts;
using CkgDomainLogic.Zanf.Models;
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
            Suchparameter = new ZulassungsAnforderungSuchparameter { NurKlaerfaelle = true };
        }

        public void MarkForRefreshZulassungsAnforderungen()
        {
            PropertyCacheClear(this, m => m.ZulassungsAnforderungen);
        }

        private IEnumerable<ZulassungsAnforderung> LoadZulassungsAnforderungenFromSap()
        {
            Z_ZANF_READ_KLAERF_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(Suchparameter.AnforderungsNr))
                SAP.SetImportParameter("I_ORDERID", Suchparameter.AnforderungsNr.PadLeft10());

            if (!String.IsNullOrEmpty(Suchparameter.FahrgestellNr))
                SAP.SetImportParameter("I_FAHRG", Suchparameter.FahrgestellNr.ToUpper());

            if (!String.IsNullOrEmpty(Suchparameter.AuftragsNr))
                SAP.SetImportParameter("I_VBELN", Suchparameter.AuftragsNr.PadLeft10());

            if (!String.IsNullOrEmpty(Suchparameter.KundenreferenzNr))
                SAP.SetImportParameter("I_REFNR", Suchparameter.KundenreferenzNr);

            if (Suchparameter.Anlagedatum.HasValue)
                SAP.SetImportParameter("I_ERDAT", Suchparameter.Anlagedatum);

            if (Suchparameter.Ausfuehrungsdatum.HasValue)
                SAP.SetImportParameter("I_ADATUM", Suchparameter.Ausfuehrungsdatum);

            if (Suchparameter.NurKlaerfaelle)
                SAP.SetImportParameter("I_KLAERF", "X");

            SAP.Execute();

            var sapList = Z_ZANF_READ_KLAERF_01.GT_DATEN.GetExportList(SAP);

            var webList = AppModelMappings.Z_ZANF_READ_KLAERF_01_GT_DATEN_To_ZulassungsAnforderung.Copy(sapList).ToList();

            var textList = Z_ZANF_READ_KLAERF_01.GT_KLAERFALLTEXT.GetExportList(SAP);

            foreach (var webItem in webList)
            {
                var zanf = webItem;
                var zeilen = textList.Where(t => t.ORDERID == zanf.AnforderungsNr && t.HPPOS == zanf.HauptpositionsNr).OrderBy(t => t.ZEILENNR).Select(t => t.BEMERKUNG);

                //zanf.KlaerfallTextPreview = zeilen.FirstOrDefault();
                zanf.KlaerfallText = String.Join("<br/>", zeilen);
            }

            return webList;
        }
    }
}
