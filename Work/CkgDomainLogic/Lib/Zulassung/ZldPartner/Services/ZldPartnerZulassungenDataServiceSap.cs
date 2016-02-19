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
        public List<StornoGrund> Gruende { get; private set; }

        public List<Material> Materialien { get; private set; }

        public ZldPartnerZulassungenDataServiceSap(ISapDataService sap)
            : base(sap)
        {
            Gruende = new List<StornoGrund>();
            Materialien = new List<Material>();
        }

        public void LoadStammdaten()
        {
            Z_ZLD_PP_STAMMDATEN.Init(SAP);

            SAP.Execute();

            Gruende = AppModelMappings.Z_ZLD_PP_STAMMDATEN_EXP_GRUENDE_To_StornoGrund.Copy(Z_ZLD_PP_STAMMDATEN.EXP_GRUENDE.GetExportList(SAP)).ToList();
            Materialien = AppModelMappings.Z_ZLD_PP_STAMMDATEN_EXP_MATERIAL_To_Material.Copy(Z_ZLD_PP_STAMMDATEN.EXP_MATERIAL.GetExportList(SAP)).ToList();
        }

        public List<OffeneZulassung> LoadOffeneZulassungen()
        {
            Z_ZLD_PP_GET_PO_01.Init(SAP, "I_LIFNR", LogonContext.User.Reference.NotNullOrEmpty().ToSapKunnr());

            var sapItems = AppModelMappings.Z_ZLD_PP_GET_PO_01_GT_BESTELLUNGEN_To_OffeneZulassung.Copy(Z_ZLD_PP_GET_PO_01.GT_BESTELLUNGEN.GetExportListWithExecute(SAP)).ToList();

            sapItems.ForEach(s =>
            {
                if (!string.IsNullOrEmpty(s.StornoBemerkungLangtextNr))
                    s.StornoBemerkung = ReadLangtext(s.StornoBemerkungLangtextNr);
            });

            return sapItems;
        }

        public List<OffeneZulassung> SaveOffeneZulassungen(bool nurSpeichern, List<OffeneZulassung> zulassungen)
        {
            zulassungen.ForEach(z =>
            {
                if (string.IsNullOrEmpty(z.StornoBemerkung))
                {
                    if (!string.IsNullOrEmpty(z.StornoBemerkungLangtextNr))
                    {
                        DeleteLangtext(z.StornoBemerkungLangtextNr);
                        z.StornoBemerkungLangtextNr = "";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(z.StornoBemerkungLangtextNr))
                        z.StornoBemerkungLangtextNr = InsertLangtext(z.StornoBemerkung);
                    else
                        UpdateLangtext(z.StornoBemerkungLangtextNr, z.StornoBemerkung);
                }
            });

            Z_ZLD_PP_SAVE_PO_01.Init(SAP, "I_MODUS", (nurSpeichern ? "S" : "A"));

            var listeVorhandenePositionen = AppModelMappings.Z_ZLD_PP_SAVE_PO_01_GT_BESTELLUNGEN_From_OffeneZulassung.CopyBack(zulassungen.Where(z => !z.NeuePosition)).ToList();
            SAP.ApplyImport(listeVorhandenePositionen);

            var listeNeuePositionen = AppModelMappings.Z_ZLD_PP_SAVE_PO_01_GT_MATERIALIEN_From_OffeneZulassung.CopyBack(zulassungen.Where(z => z.NeuePosition)).ToList();
            SAP.ApplyImport(listeNeuePositionen);

            SAP.Execute();

            var ergList = Z_ZLD_PP_SAVE_PO_01.GT_BESTELLUNGEN.GetExportList(SAP);

            foreach (var item in zulassungen)
            {
                if (ergList.Any(e => e.EBELN == item.BelegNr && e.EBELP == item.BelegPosition))
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

        public string LoadDurchgefuehrteZulassungen(DurchgefuehrteZulassungenSuchparameter suchparameter, out List<DurchgefuehrteZulassung> zulassungen)
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

            if (SAP.ResultCode != 0)
            {
                zulassungen = new List<DurchgefuehrteZulassung>();
                return SAP.ResultMessage;
            }

            zulassungen = AppModelMappings.Z_ZLD_PP_GET_ZULASSUNGEN_01_GT_BESTELL_LISTE_To_DurchgefuehrteZulassung.Copy(Z_ZLD_PP_GET_ZULASSUNGEN_01.GT_BESTELL_LISTE.GetExportList(SAP)).ToList();

            return "";
        }
    }
}
