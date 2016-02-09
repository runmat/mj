// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;
using CoCAppModelMappings = CkgDomainLogic.CoC.Models.AppModelMappings;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.CoC.Services
{
    public class ZulassungDataServiceSAP : CkgGeneralDataServiceSAP, IZulassungDataService
    {
        public string AuftragsNummer { get; set; }

        public ZulassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }


        #region Zulassung

        public string SaveZulassung(
            Adresse auftraggeberAdresse,
            Adresse halterAdresse,
            Adresse reguliererAdresse,
            Adresse rechnungsEmpfaengerAdresse,
            Adresse versicherungsNehmerAdresse,
            Adresse versandScheinSchilderAdresse,
            Adresse versandZb2CocAdresse,

            ZulassungsOptionen zulassungsOptionen,
            ZulassungsDienstleistungen zulassungsDienstleistungen,
            Versicherungsdaten versicherungsdaten,
            WunschkennzeichenOptionen wunschkennzeichen
            )
        {
            AuftragsNummer = "";

            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                    {
                        Z_DPM_WEB_ZULASSUNG_01.Init(SAP);
                        SAP.SetImportParameter("AG", LogonContext.KundenNr.ToSapKunnr());
                        SAP.SetImportParameter("WEB_USER", LogonContext.UserName);

                        var auftragsList = wunschkennzeichen.WunschkennzeichenList.ToList();
                        var auftraegeList = Z_DPM_WEB_ZULASSUNG_01.GT_AUF.GetImportList(SAP);
                        var partnerList = Z_DPM_WEB_ZULASSUNG_01.GT_PARTNER.GetImportList(SAP);
                        var dienstleistungenList = Z_DPM_WEB_ZULASSUNG_01.GT_DIENSTL.GetImportList(SAP);

                        for (var i = 0; i < auftragsList.Count; i++)
                        {
                            var auftrag = auftragsList[i];

                            // Auftrag allgemein
                            CreateRowForAuftrag(auftraegeList, auftrag, halterAdresse, zulassungsOptionen,
                                                versicherungsdaten);

                            // Partner
                            CreateRowsForPartner(partnerList,
                                                 auftrag,
                                                 halterAdresse,
                                                 reguliererAdresse,
                                                 rechnungsEmpfaengerAdresse,
                                                 versicherungsNehmerAdresse,
                                                 versandScheinSchilderAdresse,
                                                 versandZb2CocAdresse);

                            // Dienstleistungen
                            CreateRowsForDienstleistungen(dienstleistungenList,
                                                          auftrag,
                                                          zulassungsDienstleistungen);
                        }

                        SAP.ApplyImport(auftraegeList);
                        SAP.ApplyImport(partnerList);
                        SAP.ApplyImport(dienstleistungenList);

                        SAP.Execute();
                    },

                // SAP custom error handling:
                () =>
                    {
                        var sapResultList = Z_DPM_WEB_ZULASSUNG_01.GT_RETURN.GetExportList(SAP);

                        var sapResult = sapResultList.FirstOrDefault();
                        if (sapResult != null)
                        {
                            AuftragsNummer = sapResult.VBELN;
                            return sapResult.MESSAGE;
                        }

                        return "Fehler: Es wurde kein Auftrag angelegt.";
                    });

            return error;
        }

        private static void CreateRowForAuftrag(
            List<Z_DPM_WEB_ZULASSUNG_01.GT_AUF> auftraegeList,
            VinWunschkennzeichen auftrag,
            Adresse halterAdresse,
            ZulassungsOptionen zulassungsOptionen,
            Versicherungsdaten versicherungsdaten)
        {
            var sapAuftrag = new Z_DPM_WEB_ZULASSUNG_01.GT_AUF
                {
                    // Fahrzeug Info
                    ZZFAHRG = auftrag.VIN,
                    ZZBRIEF = auftrag.ZBII,
                    ZZREFNR = auftrag.AuftragsReferenz,

                    ZFAHRZEUGART = "",

                    // Zulassungs-Optionen
                    SFV_FZG = zulassungsOptionen.ZulassungsOption.Name,
                    ZULDAT = zulassungsOptionen.AuslieferDatum,
                    ZUL_DEZ = halterAdresse.Land.NotNullOrEmpty().ToUpper() == "DE" ? "1" : "0",
                    ZUL_AUSLAND = halterAdresse.Land.NotNullOrEmpty().ToUpper() == "DE" ? "0" : "1",
                    ZUL_EXPORT = "0",

                    // Wunschkennzeichen
                    WUNSCHKENNZ = auftrag.WunschKennzeichenAsString.Replace("<br />", "; "),

                    // Versicherung
                    VERSICHERUNG = versicherungsdaten.VersicherungsGesellschaft,
                    EVBNR = versicherungsdaten.EvbNummer,
                };

            auftraegeList.Add(sapAuftrag);
        }

        private void CreateRowsForPartner(
            List<Z_DPM_WEB_ZULASSUNG_01.GT_PARTNER> partnerList,
            VinWunschkennzeichen auftrag,
            Adresse halterAdresse,
            Adresse reguliererAdresse,
            Adresse rechnungsEmpfaengerAdresse,
            Adresse versicherungsNehmerAdresse,
            Adresse versandScheinSchilderAdresse,
            Adresse versandZb2CocAdresse)
        {
            // Halter
            CheckForTeslaPatchKundenNrHalterAdresse(halterAdresse);
            CreateRowForOnePartner(partnerList, halterAdresse, "ZH", auftrag);

            // Rechnungsempfänger
            CreateRowForOnePartner(partnerList, rechnungsEmpfaengerAdresse, "RE", auftrag, true);

            // Regulierer
            CreateRowForOnePartner(partnerList, reguliererAdresse, "RG", auftrag, true);

            // Versicherungs-Nehmer
            CreateRowForOnePartner(partnerList, versicherungsNehmerAdresse, "ZC", auftrag);

            // Versand Schein + Schilder 
            CreateRowForOnePartner(partnerList, versandScheinSchilderAdresse, "ZE", auftrag);

            // Versand ZB2 + CoC
            CreateRowForOnePartner(partnerList, versandZb2CocAdresse, "ZS", auftrag);
        }

        private void CreateRowForOnePartner(List<Z_DPM_WEB_ZULASSUNG_01.GT_PARTNER> partnerList, Adresse adresse,
                                            string partnerKennung, VinWunschkennzeichen auftrag,
                                            bool useCustomerNumberOnly = false)
        {
            var partnerNumber = partnerKennung.StartsWith("Z")
                                    ? ""
                                    : (adresse.KundenNr.IsNotNullOrEmpty() ? adresse.KundenNr : LogonContext.KundenNr)
                                          .ToSapKunnr();

            var partner = new Z_DPM_WEB_ZULASSUNG_01.GT_PARTNER
                {
                    ZZFAHRG = auftrag.VIN,
                    ZZBRIEF = auftrag.ZBII,
                    ZZREFNR = auftrag.AuftragsReferenz,

                    PARTN_ROLE = partnerKennung,
                    PARTN_NUMB = partnerNumber,
                };

            if (!useCustomerNumberOnly)
            {
                partner.NAME = adresse.Name1;
                partner.NAME_2 = adresse.Name2;
                partner.STREET = adresse.StrasseHausNr;
                partner.POSTL_CODE = adresse.PLZ;
                partner.CITY = adresse.Ort;
                partner.COUNTRY = adresse.Land;
            }

            partnerList.Add(partner);
        }

        private static void CreateRowsForDienstleistungen(List<Z_DPM_WEB_ZULASSUNG_01.GT_DIENSTL> dienstleistungenList,
                                                          VinWunschkennzeichen auftrag,
                                                          ZulassungsDienstleistungen zulassungsDienstleistungen)
        {
            for (var i = 0; i < zulassungsDienstleistungen.GewaehlteDienstleistungen.Count; i++)
            {
                var dienstleistung = zulassungsDienstleistungen.GewaehlteDienstleistungen[i];

                var sapDienstleistung = new Z_DPM_WEB_ZULASSUNG_01.GT_DIENSTL
                    {
                        ZZFAHRG = auftrag.VIN,
                        ZZBRIEF = auftrag.ZBII,
                        ZZREFNR = auftrag.AuftragsReferenz,

                        DIENSTL_NR = dienstleistung.ID,
                        DIENSTL_TEXT = dienstleistung.Name,
                        MATNR = dienstleistung.Code,
                    };

                dienstleistungenList.Add(sapDienstleistung);
            }
        }

        private void CheckForTeslaPatchKundenNrHalterAdresse(Adresse halterAdresse)
        {
            if (LogonContext.KundenNr.ToSapKunnr() == "0010010753" && halterAdresse.KundenNr.IsNullOrEmpty())
                halterAdresse.KundenNr = "0000349980";
        }

        #endregion


        #region Sendungsaufträge

        public List<SendungsAuftrag> GetSendungsAuftraege(SendungsAuftragSelektor model)
        {
            return CoCAppModelMappings.Z_DPM_GET_ZZSEND2_GT_WEB_To_SendungsAuftrag.Copy(GetSapSendungsAuftraege(model)).ToList();
        }

        private IEnumerable<Z_DPM_GET_ZZSEND2.GT_WEB> GetSapSendungsAuftraege(SendungsAuftragSelektor model)
        {
            Z_DPM_GET_ZZSEND2.Init(SAP);

            SAP.SetImportParameter("KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (model.DatumRange.IsSelected)
            {
                SAP.SetImportParameter("ERDAT_VON", model.DatumRange.StartDate);
                SAP.SetImportParameter("ERDAT_BIS", model.DatumRange.EndDate);
            }

            if (model.FilterNurMitSendungsNummer)
                SAP.SetImportParameter("CHECK_SEND2", "X");

            SAP.Execute();

            return Z_DPM_GET_ZZSEND2.GT_WEB.GetExportList(SAP);
        }

        #endregion        
        

        #region Sendungsaufträge, nach ID

        public List<SendungsAuftrag> GetSendungsAuftraegeId(SendungsAuftragIdSelektor model)
        {
            return CoCAppModelMappings.Z_DPM_READ_SENDTAB_03_GT_OUT_To_SendungsAuftrag.Copy(GetSapSendungsAuftraegeId(model)).ToList();
        }

        private IEnumerable<Z_DPM_READ_SENDTAB_03.GT_OUT> GetSapSendungsAuftraegeId(SendungsAuftragIdSelektor model)
        {
            Z_DPM_READ_SENDTAB_03.Init(SAP);

            SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (model.DatumRangeIds.IsSelected)
            {
                SAP.SetImportParameter("I_ZZLSDAT_VON", model.DatumRangeIds.StartDate);
                SAP.SetImportParameter("I_ZZLSDAT_BIS", model.DatumRangeIds.EndDate);
            }

            SAP.SetImportParameter("I_CHECK_TRACK", "X");
            SAP.SetImportParameter("I_POOLGROUP", "X");

            SAP.Execute();

            return Z_DPM_READ_SENDTAB_03.GT_OUT.GetExportList(SAP);
        }

        #endregion


        #region Sendungsaufträge, nach Docs

        public List<SendungsAuftrag> GetSendungsAuftraegeDocs(SendungsAuftragDocsSelektor model)
        {
            return CoCAppModelMappings.Z_DPM_READ_SENDTAB_03_GT_OUT_To_SendungsAuftrag.Copy(GetSapSendungsAuftraegeDocs(model)).ToList();
        }

        private IEnumerable<Z_DPM_READ_SENDTAB_03.GT_OUT> GetSapSendungsAuftraegeDocs(SendungsAuftragDocsSelektor model)
        {
            Z_DPM_READ_SENDTAB_03.Init(SAP);

            SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (model.DatumRangeDocs.IsSelected)
            {
                SAP.SetImportParameter("I_ZZLSDAT_VON", model.DatumRangeDocs.StartDate);
                SAP.SetImportParameter("I_ZZLSDAT_BIS", model.DatumRangeDocs.EndDate);
            }

            if (model.NurMitSendungsID)
                SAP.SetImportParameter("I_CHECK_TRACK", "X");

            SAP.Execute();

            return Z_DPM_READ_SENDTAB_03.GT_OUT.GetExportList(SAP);
        }

        public List<SendungsAuftrag> GetSendungsAuftraegeFin(SendungsAuftragFinSelektor model)
        {
            Z_DPM_READ_SENDTAB_03.Init(SAP);

            SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (model.Fin.IsNotNullOrEmpty())            
                SAP.SetImportParameter("I_CHASSIS_NUM", model.Fin);
                           
            if (model.Vertragsnummer.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_ZZREFNR", model.Vertragsnummer);

            if (model.Kennzeichen.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_ZZKENN", model.Kennzeichen);

            if (model.Referenz1.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_ZZREFERENZ1", model.Referenz1);

            if (model.Referenz2.IsNotNullOrEmpty())
                SAP.SetImportParameter("I_ZZREFERENZ2", model.Referenz2);

            SAP.Execute();

            var liste = Z_DPM_READ_SENDTAB_03.GT_OUT.GetExportList(SAP).ToList();
            
            return CoCAppModelMappings.Z_DPM_READ_SENDTAB_03_GT_OUT_To_SendungsAuftrag.Copy(liste).ToList();
        }

        #endregion
    }
}
