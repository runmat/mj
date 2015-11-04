using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.ZldPartner.Contracts;
using CkgDomainLogic.ZldPartner.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.ZldPartner.Models.AppModelMappings;

namespace CkgDomainLogic.ZldPartner.Services
{
    public class ZldPartnerZulassungenDataServiceSap : CkgGeneralDataServiceSAP, IZldPartnerZulassungenDataService
    {
        public ZldPartnerZulassungenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
        }

        public List<OffeneZulassung> LoadOffeneZulassungen()
        {
            Z_ZLD_PP_GET_PO_01.Init(SAP, "I_LIFNR", LogonContext.User.Reference.NotNullOrEmpty().ToSapKunnr());

            return AppModelMappings.Z_ZLD_PP_GET_PO_01_GT_BESTELLUNGEN_To_OffeneZulassung.Copy(Z_ZLD_PP_GET_PO_01.GT_BESTELLUNGEN.GetExportListWithExecute(SAP)).ToList();
        }

        public List<OffeneZulassung> SaveOffeneZulassungen(bool nurSpeichern, List<OffeneZulassung> zulassungen)
        {
            Z_ZLD_PP_SAVE_PO_01.Init(SAP, "I_MODUS", (nurSpeichern ? "S" : "A"));

            var zulList = AppModelMappings.Z_ZLD_PP_SAVE_PO_01_GT_BESTELLUNGEN_From_OffeneZulassung.CopyBack(zulassungen).ToList();
            SAP.ApplyImport(zulList);

            SAP.Execute();

            var ergList = Z_ZLD_PP_SAVE_PO_01.GT_BESTELLUNGEN.GetExportList(SAP);

            foreach (var item in zulassungen)
            {
                if (ergList != null && ergList.Any(e => e.EBELN == item.BelegNr && e.EBELP == item.BelegPosition))
                {
                    item.SaveMessage = ergList.First(e => e.EBELN == item.BelegNr && e.EBELP == item.BelegPosition).MESSAGE;
                }
                else
                {
                    item.SaveMessage = "";
                }
            }

            return zulassungen;
        }

        public List<DurchgefuehrteZulassung> LoadDurchgefuehrteZulassungen(DurchgefuehrteZulassungenSuchparameter suchparameter)
        {
            Z_ZLD_PP_GET_ZULASSUNGEN_01.Init(SAP, "I_LIFNR", LogonContext.User.Reference.NotNullOrEmpty().ToSapKunnr());

            if (!String.IsNullOrEmpty(suchparameter.Kunde))
                SAP.SetImportParameter("I_KUNDE", suchparameter.Kunde);

            if (suchparameter.ZulassungsDatumRange.IsSelected)
            {
                if (suchparameter.ZulassungsDatumRange.StartDate.HasValue)
                    SAP.SetImportParameter("I_ZZZLDAT_VON", suchparameter.ZulassungsDatumRange.StartDate.Value);

                if (suchparameter.ZulassungsDatumRange.EndDate.HasValue)
                    SAP.SetImportParameter("I_ZZZLDAT_BIS", suchparameter.ZulassungsDatumRange.EndDate.Value);
            }

            if (!String.IsNullOrEmpty(suchparameter.Auswahl))
                SAP.SetImportParameter("I_AUSWAHL", suchparameter.Auswahl);

            if (!String.IsNullOrEmpty(suchparameter.Kennzeichen))
                SAP.SetImportParameter("I_ZZKENN", suchparameter.Kennzeichen);

            if (!String.IsNullOrEmpty(suchparameter.Halter))
                SAP.SetImportParameter("I_HALTER", suchparameter.Halter);

            SAP.Execute();

            return AppModelMappings.Z_ZLD_PP_GET_ZULASSUNGEN_01_GT_BESTELL_LISTE_To_DurchgefuehrteZulassung.Copy(Z_ZLD_PP_GET_ZULASSUNGEN_01.GT_BESTELL_LISTE.GetExportList(SAP)).ToList();
        }
    }
}
