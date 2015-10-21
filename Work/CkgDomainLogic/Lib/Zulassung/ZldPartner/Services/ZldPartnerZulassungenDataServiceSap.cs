using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.ZldPartner.Contracts;
using CkgDomainLogic.ZldPartner.Models;
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
            Z_ZLD_PP_GET_PO_01.Init(SAP, "I_LIFNR", LogonContext.User.Reference);

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
            throw new NotImplementedException();
        }
    }
}
