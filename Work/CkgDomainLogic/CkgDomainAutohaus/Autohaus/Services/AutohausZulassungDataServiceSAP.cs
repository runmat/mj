using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappingsDomainCommon = CkgDomainLogic.Autohaus.Models.AppModelMappings;

namespace CkgDomainLogic.Autohaus.Services
{
    public class AutohausZulassungDataServiceSAP : CkgGeneralDataServiceSAP, IAutohausZulassungDataService
    {
        public string AuftragsNummer { get; set; }

        public AutohausZulassungDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

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
                            CreateRowForAuftrag(auftraegeList, auftrag, halterAdresse, zulassungsOptionen, versicherungsdaten);

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

        private void CreateRowForOnePartner(List<Z_DPM_WEB_ZULASSUNG_01.GT_PARTNER> partnerList, Adresse adresse, string partnerKennung, VinWunschkennzeichen auftrag, bool useCustomerNumberOnly = false)
        {
            var partnerNumber = partnerKennung.StartsWith("Z") ? "" : (adresse.KundenNr.IsNotNullOrEmpty() ? adresse.KundenNr : LogonContext.KundenNr).ToSapKunnr();

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

        private static void CreateRowsForDienstleistungen(List<Z_DPM_WEB_ZULASSUNG_01.GT_DIENSTL> dienstleistungenList, VinWunschkennzeichen auftrag, ZulassungsDienstleistungen zulassungsDienstleistungen)
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
    }
}
