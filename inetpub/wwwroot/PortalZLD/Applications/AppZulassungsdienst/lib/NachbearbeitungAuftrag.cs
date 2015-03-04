using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class NachbearbeitungAuftrag : SapOrmBusinessBase
    {
        #region "Properties"

        public string SucheId { get; set; }
        public string SucheAuftragsnummer { get; set; }

        public ZLDVorgang AktuellerVorgang { get; private set; }

        public DataTable tblStornogruende { get; private set; }
        public DataTable tblOffeneStornos { get; private set; }
        public DataTable tblBarquittungen { get; private set; }

        public string Stornogrund { get; set; }
        public string StornoKundennummer { get; set; }
        public string StornoBegruendung { get; set; }
        public string StornoStva { get; set; }
        public string StornoKennzeichen { get; set; }

        #endregion

        #region "Methods"

        public NachbearbeitungAuftrag(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

            AktuellerVorgang = new ZLDVorgang(VKORG, VKBUR);
        }

        public void StornogruendeLaden()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_STO_STORNOGRUENDE.Init(SAP);

                    CallBapi();

                    tblStornogruende = SAP.GetExportTable("GT_GRUENDE");
                });
        }

        public void VorgangPruefen()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_STO_STORNO_CHECK.Init(SAP);

                    SAP.SetImportParameter("I_VKORG", VKORG);
                    SAP.SetImportParameter("I_VKBUR", VKBUR);

                    if (!String.IsNullOrEmpty(SucheAuftragsnummer))
                        SAP.SetImportParameter("I_VBELN", SucheAuftragsnummer.PadLeft0(10));

                    if (!String.IsNullOrEmpty(SucheId))
                        SAP.SetImportParameter("I_ZULBELN", SucheId.PadLeft0(10));

                    CallBapi();

                    if (!ErrorOccured)
                        AktuellerVorgang.Kopfdaten.SapId = SAP.GetExportParameter("E_ZULBELN");
                });
        }

        public void VorgangLaden()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_STO_GET_ORDER2.Init(SAP, "I_ZULBELN", AktuellerVorgang.Kopfdaten.SapId.PadLeft0(10));

                    CallBapi();

                    if (!ErrorOccured)
                    {
                        var sapKopfdaten = Z_ZLD_STO_GET_ORDER2.ES_BAK.GetExportList(SAP).FirstOrDefault(new Z_ZLD_STO_GET_ORDER2.ES_BAK());
                        var sapBankdaten = Z_ZLD_STO_GET_ORDER2.ES_BANK.GetExportList(SAP).FirstOrDefault(new Z_ZLD_STO_GET_ORDER2.ES_BANK());
                        var sapAdresse = Z_ZLD_STO_GET_ORDER2.GT_ADRS.GetExportList(SAP).FirstOrDefault(new Z_ZLD_STO_GET_ORDER2.GT_ADRS());
                        var sapPositionen = Z_ZLD_STO_GET_ORDER2.GT_POS.GetExportList(SAP);

                        AktuellerVorgang.Kopfdaten = AppModelMappings.Z_ZLD_STO_GET_ORDER2_ES_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten);
                        AktuellerVorgang.Bankdaten = AppModelMappings.Z_ZLD_STO_GET_ORDER2_ES_BANK_To_ZLDBankdaten.Copy(sapBankdaten);
                        AktuellerVorgang.Adressdaten = AppModelMappings.Z_ZLD_STO_GET_ORDER2_GT_ADRS_To_ZLDAdressdaten.Copy(sapAdresse);
                        AktuellerVorgang.Positionen = AppModelMappings.Z_ZLD_STO_GET_ORDER2_GT_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.PositionsNr).ToList();
                    }
                });
        }

        public void VorgangStornieren(string userName)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_STO_STORNO_ORDER.Init(SAP);

                    SAP.SetImportParameter("I_ZULBELN", AktuellerVorgang.Kopfdaten.SapId.PadLeft0(10));
                    SAP.SetImportParameter("I_ERNAM", userName);
                    SAP.SetImportParameter("I_STORNOGRUND", Stornogrund);

                    if (!String.IsNullOrEmpty(StornoKundennummer))
                        SAP.SetImportParameter("I_KUNNR", StornoKundennummer.PadLeft0(10));

                    if (!String.IsNullOrEmpty(StornoBegruendung))
                        SAP.SetImportParameter("I_BEGRUENDUNG", StornoBegruendung);

                    if (!String.IsNullOrEmpty(StornoStva))
                        SAP.SetImportParameter("I_KREISKZ", StornoStva);

                    if (!String.IsNullOrEmpty(StornoKennzeichen))
                        SAP.SetImportParameter("I_ZZKENN", StornoKennzeichen);

                    CallBapi();

                    if (!ErrorOccured)
                    {
                        var newSapId = SAP.GetExportParameter("E_ZULBELN_NEU");
                        AktuellerVorgang.Kopfdaten = new ZLDKopfdaten { VkOrg = VKORG, VkBur = VKBUR, SapId = newSapId };
                        tblBarquittungen = SAP.GetExportTable("GT_BARQ");
                    }
                });
        }

        public void VorgangAbsenden()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_IMP_NACHERF2.Init(SAP);

                    var kopfListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(new List<ZLDKopfdaten> { AktuellerVorgang.Kopfdaten });
                    SAP.ApplyImport(kopfListe);

                    var bankListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(new List<ZLDBankdaten> { AktuellerVorgang.Bankdaten });
                    SAP.ApplyImport(bankListe);

                    var adressListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(new List<ZLDAdressdaten> { AktuellerVorgang.Adressdaten });
                    SAP.ApplyImport(adressListe);

                    var posListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_POS_From_ZLDPosition.CopyBack(AktuellerVorgang.Positionen);
                    SAP.ApplyImport(posListe);

                    CallBapi();

                    tblBarquittungen = SAP.GetExportTable("GT_BARQ");

                    var fehlerListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMP_NACHERF2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                    if (fehlerListe.Any(f => f.FehlerText != "OK"))
                    {
                        RaiseError(-9999, "Beim Speichern des Vorgangs in SAP sind Fehler aufgetreten");

                        foreach (var fehler in fehlerListe)
                        {
                            var pos = AktuellerVorgang.Positionen.FirstOrDefault(p => p.SapId == fehler.SapId && p.PositionsNr == fehler.PositionsNr);
                            if (pos != null)
                                pos.FehlerText = fehler.FehlerText;
                        }
                    }
                });
        }

        public void OffeneStornosLaden(List<Kundenstammdaten> kundenStamm)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_STO_STORNO_LISTE.Init(SAP, "I_VKORG, I_VKBUR", VKORG, VKBUR);

                    CallBapi();

                    if (!ErrorOccured)
                    {
                        tblOffeneStornos = SAP.GetExportTable("GT_LISTE");

                        tblOffeneStornos.Columns.Add("Kunde");
                        tblOffeneStornos.Columns.Add("Erfasst");

                        foreach (DataRow row in tblOffeneStornos.Rows)
                        {
                            row["ZULBELN"] = row["ZULBELN"].ToString().TrimStart('0');
                            row["ZULBELN_ALT"] = row["ZULBELN_ALT"].ToString().TrimStart('0');

                            var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == row["KUNNR"].ToString().TrimStart('0'));
                            if (kunde != null)
                                row["Kunde"] = kunde.Name1;

                            string strErfasst = "";
                            if (!String.IsNullOrEmpty(row["VE_ERDAT"].ToString()))
                            {
                                DateTime tmpDat;
                                if (DateTime.TryParse(row["VE_ERDAT"].ToString(), out tmpDat))
                                {
                                    strErfasst = tmpDat.ToShortDateString();

                                    string strZeit = row["VE_ERZEIT"].ToString();
                                    if (!String.IsNullOrEmpty(strZeit) && strZeit.Length == 6)
                                    {
                                        strErfasst += " " + strZeit.Substring(0, 2) + ":" + strZeit.Substring(2, 2) + ":" + strZeit.Substring(4, 2);
                                    }
                                }
                            }
                            row["Erfasst"] = strErfasst;
                        }

                        tblOffeneStornos.AcceptChanges();
                    }
                });
        }

        /// <summary>
        /// Daten "auf Anfang" zurücksetzen
        /// </summary>
        public void ResetData(bool resetSuchparameter)
        {
            if (resetSuchparameter)
            {
                SucheId = "";
                SucheAuftragsnummer = "";
            }
          
            AktuellerVorgang = new ZLDVorgang(VKORG, VKBUR);

            Stornogrund = "";
            StornoKundennummer = "";
            StornoBegruendung = "";
            StornoStva = "";
            StornoKennzeichen = "";
        }

        #endregion
    }
}