using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using System.Data;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Nacherfassung, Nacherfassung beauftragter und durchzuführende Versandzulassungen.
    /// </summary>
    public class NacherfZLD : SapOrmBusinessBase
    {
        #region "Declarations"

        private List<ZLDKopfdaten> _lstKopfdaten;
        private List<ZLDBankdaten> _lstBankdaten;
        private List<ZLDAdressdaten> _lstAdressen;
        private List<ZLDPosition> _lstPositionen;
        private List<Kundenname> _lstKundendaten; 

        #endregion

        #region "Properties"

        public ZLDVorgang AktuellerVorgang { get; private set; }

        public List<ZLDVorgangUINacherfassung> Vorgangsliste { get; private set; }

        public List<RechnungsanhangTemplates> RechnungUploadTemplates { get; private set; } 

        public List<NochNichtAbgesendeterVorgang> NochNichtAbgesendeteVorgaenge { get; private set; }

        public DataTable BestLieferanten { get; set; }
        public DataTable tblBarquittungen { get; set; }
        public DataTable AHVersandListe { get; set; }

        private DataTable _tblPrintDataForPdf;
        public DataTable tblPrintDataForPdf
        {
            get { return _tblPrintDataForPdf ?? (_tblPrintDataForPdf = ZLDCommon.CreatePrintTable()); }
            }

        // Selektion
        public String SelMatnr { get; set; }
        public String SelDatum { get; set; }
        public String SelDatumBis { get; set; }
        public String SelID { get; set; }
        public String SelKunde { get; set; }
        public String SelKreis { get; set; }
        public String SelKreisBis { get; set; }
        public String SelLief { get; set; }
        public String SelStatus { get; set; }
        public String SelVorgang { get; set; }
        public bool SelFlieger { get; set; }
        public bool SelAnnahmeAH { get; set; }
        public bool SelVersandAH { get; set; }
        public bool SelAenderungAngenommene { get; set; }
        public bool SelSofortabrechnung { get; set; }
        public bool SelEditDurchzufVersZul { get; set; }
        public bool SelUploadRechnungsanhaenge { get; set; }
        public string SelGroupTourID { get; set; }
        public String SelDZVKBUR { get; set; }
        public bool SelNochNichtAbgesendete { get; set; }

        public String Name1Hin { get; set; }
        public String Name2Hin { get; set; }
        public String StrasseHin { get; set; }
        public String PLZHin { get; set; }
        public String OrtHin { get; set; }
        public String DocRueck1 { get; set; }
        public String NameRueck1 { get; set; }
        public String NameRueck2 { get; set; }
        public String StrasseRueck { get; set; }
        public String PLZRueck { get; set; }
        public String OrtRueck { get; set; }
        public String Doc2Rueck { get; set; }
        public String Name1Rueck2 { get; set; }
        public String Name2Rueck2 { get; set; }
        public String Strasse2Rueck { get; set; }
        public String PLZ2Rueck { get; set; }
        public String Ort2Rueck { get; set; }

        public int MatError { get; set; }
        public String MatErrorText { get; set; }
        public string SofortabrechnungVerzeichnis { get; set; }

        public bool IsZLD { get; set; }
        public bool DataFilterActive { get; set; }
        public string DataFilterProperty { get; set; }
        public string DataFilterValue { get; set; }
        public int LastPageIndex { get; set; }
        public int LastPageSize { get; set; }

        #endregion

        #region "Methods"

        public NacherfZLD(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

            AktuellerVorgang = new ZLDVorgang(VKORG, VKBUR);
            Vorgangsliste = new List<ZLDVorgangUINacherfassung>();
        }

        public void CheckLieferant()
        {
            ExecuteSapZugriff(() =>
            {
                Z_ZLD_CHECK_ZLD.Init(SAP, "I_VKORG, I_VKBUR", "1010", AktuellerVorgang.Kopfdaten.LieferantenNr.TrimStart('0').Substring(2, 4));

                CallBapi();

                IsZLD = SAP.GetExportParameter("E_ZLD").XToBool();
            });
        }

        /// <summary>
        /// Liest Lieferanten zum Kreis
        /// </summary>
        /// <param name="abwKreis">abweichender Kreis für die Lieferantenermittlung, defaultmäßig wird SelKreis verwendet</param>
        public void getBestLieferant(string abwKreis = null)
        {
            ExecuteSapZugriff(() =>
            {
                Z_ZLD_EXPORT_INFOPOOL.Init(SAP, "I_KREISKZ", (abwKreis ?? SelKreis));

                CallBapi();

                BestLieferanten = SAP.GetExportTable("GT_EX_ZUSTLIEF");

                DataRow NewLief = BestLieferanten.NewRow();
                NewLief["LIFNR"] = "0";
                NewLief["NAME1"] = "";
                BestLieferanten.Rows.Add(NewLief);
            });
        }

        public void LoadVorgaengeFromSap(List<Materialstammdaten> materialStamm)
        {
            if (SelNochNichtAbgesendete)
                LoadNochNichtAbgesendeteVorgaengeFromSap();
            else
                LoadVorgaengeFromSapInner(materialStamm);
        }

        private void LoadVorgaengeFromSapInner(List<Materialstammdaten> materialStamm)
        {
            MatError = 0;
            MatErrorText = "";

            ExecuteSapZugriff(() =>
            {
                Vorgangsliste.Clear();

                if (SelSofortabrechnung)
                    Z_ZLD_EXPORT_SOFORT_ABRECH2.Init(SAP);
                else if (SelAenderungAngenommene)
                    Z_ZLD_MOB_EXPORT_ANGENOMMENE.Init(SAP);
                else
                    Z_ZLD_EXPORT_NACHERF2.Init(SAP);

                SAP.SetImportParameter("I_KUNNR", (String.IsNullOrEmpty(SelKunde) ? "" : SelKunde.ToSapKunnr()));
                SAP.SetImportParameter("I_VKORG", VKORG);
                SAP.SetImportParameter("I_VKBUR", VKBUR);
                SAP.SetImportParameter("I_ZZZLDAT", SelDatum);
                SAP.SetImportParameter("I_ZULBELN", (String.IsNullOrEmpty(SelID) ? "" : SelID.PadLeft0(10)));

                if (SelFlieger)
                    SAP.SetImportParameter("I_FLIEGER", "X");

                if (!SelSofortabrechnung)
                {
                    SAP.SetImportParameter("I_KREISKZ", SelKreis);
                    SAP.SetImportParameter("I_MATNR", SelMatnr);
                    SAP.SetImportParameter("I_BLTYP", SelStatus);
                    SAP.SetImportParameter("I_DZLD_VKBUR", SelDZVKBUR);

                    if (!String.IsNullOrEmpty(SelLief) && SelLief != "0")
                        SAP.SetImportParameter("I_LIFNR", SelLief);

                    if (SelAnnahmeAH)
                        SAP.SetImportParameter("I_AH_ANNAHME", "X");

                    if (!String.IsNullOrEmpty(SelGroupTourID))
                        SAP.SetImportParameter("I_GRUPPE", SelGroupTourID.PadLeft0(10));
                }

                if (SelUploadRechnungsanhaenge)
                {
                    if (!String.IsNullOrEmpty(SelDatumBis))
                        SAP.SetImportParameter("I_ZZZLDAT_BIS", SelDatumBis);
                    if (!String.IsNullOrEmpty(SelKreisBis))
                        SAP.SetImportParameter("I_KREISKZ_BIS", SelKreisBis);
                }

                CallBapi();

                if (SelSofortabrechnung)
                {
                    var sapKopfdaten = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BAK.GetExportList(SAP);
                    var sapBankdaten = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BANK.GetExportList(SAP);
                    var sapAdressen = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_ADRS.GetExportList(SAP);
                    var sapPositionen = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_POS.GetExportList(SAP);
                    var sapKundendaten = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_KUNDE.GetExportList(SAP);

                    _lstKopfdaten = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten).OrderBy(k => k.SapId.ToLong(0)).ToList();
                    _lstBankdaten = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_BANK_To_ZLDBankdaten.Copy(sapBankdaten).OrderBy(b => b.SapId.ToLong(0)).ToList();
                    _lstAdressen = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_ADRS_To_ZLDAdressdaten.Copy(sapAdressen).OrderBy(a => a.SapId.ToLong(0)).ToList();
                    _lstPositionen = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.SapId.ToLong(0)).ThenBy(p => p.PositionsNr.ToInt(0)).ToList();
                    _lstKundendaten = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_KUNDE_To_Kundenname.Copy(sapKundendaten).ToList();
                }
                else if (SelAenderungAngenommene)
                {
                    var sapKopfdaten = Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_BAK.GetExportList(SAP);
                    var sapBankdaten = Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_BANK.GetExportList(SAP);
                    var sapAdressen = Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_ADRS.GetExportList(SAP);
                    var sapPositionen = Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_POS.GetExportList(SAP);
                    var sapKundendaten = Z_ZLD_MOB_EXPORT_ANGENOMMENE.GT_EX_KUNDE.GetExportList(SAP);

                    _lstKopfdaten = AppModelMappings.Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten).OrderBy(k => k.SapId).ToList();
                    _lstBankdaten = AppModelMappings.Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_BANK_To_ZLDBankdaten.Copy(sapBankdaten).OrderBy(b => b.SapId).ToList();
                    _lstAdressen = AppModelMappings.Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_ADRS_To_ZLDAdressdaten.Copy(sapAdressen).OrderBy(a => a.SapId).ToList();
                    _lstPositionen = AppModelMappings.Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.SapId).ThenBy(p => p.PositionsNr).ToList();
                    _lstKundendaten = AppModelMappings.Z_ZLD_MOB_EXPORT_ANGENOMMENE_GT_EX_KUNDE_To_Kundenname.Copy(sapKundendaten).ToList();
                }
                else
                {
                    var sapKopfdaten = Z_ZLD_EXPORT_NACHERF2.GT_EX_BAK.GetExportList(SAP);
                    var sapBankdaten = Z_ZLD_EXPORT_NACHERF2.GT_EX_BANK.GetExportList(SAP);
                    var sapAdressen = Z_ZLD_EXPORT_NACHERF2.GT_EX_ADRS.GetExportList(SAP);
                    var sapPositionen = Z_ZLD_EXPORT_NACHERF2.GT_EX_POS.GetExportList(SAP);
                    var sapKundendaten = Z_ZLD_EXPORT_NACHERF2.GT_EX_KUNDE.GetExportList(SAP);

                    _lstKopfdaten = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten).OrderBy(k => k.SapId.ToLong(0)).ToList();
                    _lstBankdaten = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_BANK_To_ZLDBankdaten.Copy(sapBankdaten).OrderBy(b => b.SapId.ToLong(0)).ToList();
                    _lstAdressen = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_ADRS_To_ZLDAdressdaten.Copy(sapAdressen).OrderBy(a => a.SapId.ToLong(0)).ToList();
                    _lstPositionen = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.SapId.ToLong(0)).ThenBy(p => p.PositionsNr.ToInt(0)).ToList();
                    _lstKundendaten = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_KUNDE_To_Kundenname.Copy(sapKundendaten).ToList();
                }

                var delIds = new List<string>();
                foreach (var item in _lstKopfdaten)
                {
                    var kopfdaten = item;

                    var hauptPos = _lstPositionen.FirstOrDefault(p => p.SapId == kopfdaten.SapId && p.PositionsNr == "10");

                    // Auftrag ohne Hauptpositionen?
                    if (hauptPos == null)
                    {
                        delIds.Add(kopfdaten.SapId);
                    }
                    else
                    {
                        // Material einer Versandzulassung dem durchzuführenden ZLD zugewiesen?
                        if (materialStamm.None(m => m.MaterialNr == hauptPos.MaterialNr))
                        {
                            MatError = -4444;
                            MatErrorText += String.Format("ID {0}: Material {1} nicht freigeschaltet. \r\n", kopfdaten.SapId, hauptPos.MaterialNr);
                            delIds.Add(kopfdaten.SapId);
                        }
                    }
                }
                _lstKopfdaten.RemoveAll(k => delIds.Contains(k.SapId));

                foreach (var item in _lstKopfdaten)
                {
                    var kopfdaten = item;
                    var bankdaten = _lstBankdaten.FirstOrDefault(b => b.SapId == kopfdaten.SapId, new ZLDBankdaten());
                    var adresse = _lstAdressen.FirstOrDefault(a => a.SapId == kopfdaten.SapId, new ZLDAdressdaten());
                    var positionen = _lstPositionen.Where(p => p.SapId == kopfdaten.SapId).OrderBy(p => p.PositionsNr.ToInt(0)).ToList();

                    AddVorgangToVorgangsliste(kopfdaten, bankdaten, adresse, positionen);
                }
            });
        }

        private void AddVorgangToVorgangsliste(ZLDKopfdaten kopfdaten, ZLDBankdaten bankdaten, ZLDAdressdaten adresse, List<ZLDPosition> positionen, List<Kundenstammdaten> kundenStamm = null)
        {
            var kunde = _lstKundendaten.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
               
            if (kunde == null && kundenStamm != null)
                kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

            foreach (var pos in positionen.Where(p => p.WebMaterialart == "D"))
            {
                var gebuehrenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "G");
                var steuerPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "S");
                var kennzeichenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "K");

                string kennzTeil1;
                string kennzTeil2;
                ZLDCommon.KennzeichenAufteilen(kopfdaten.Kennzeichen, out kennzTeil1, out kennzTeil2);

                Vorgangsliste.Add(new ZLDVorgangUINacherfassung
                {
                    SapId = kopfdaten.SapId,
                    Belegart = kopfdaten.Belegart,
                    VkOrg = kopfdaten.VkOrg,
                    VkBur = kopfdaten.VkBur,
                    KundenNr = kopfdaten.KundenNr,
                    KundenName = (kunde != null ? kunde.Name1 : ""),
                    PositionsNr = pos.PositionsNr,
                    MaterialName = pos.MaterialName,
                    Zulassungsdatum = kopfdaten.Zulassungsdatum,
                    Referenz1 = kopfdaten.Referenz1,
                    Referenz2 = kopfdaten.Referenz2,
                    MaterialNr = pos.MaterialNr,
                    SdRelevant = pos.SdRelevant,
                    Menge = pos.Menge,
                    Preis = pos.Preis,
                    Gebuehr = (gebuehrenPos != null ? gebuehrenPos.Preis : 0),
                    GebuehrAmt = (gebuehrenPos != null ? gebuehrenPos.GebuehrAmt : 0),
                    Gebuehrenpaket = (gebuehrenPos != null && gebuehrenPos.Gebuehrenpaket.IsTrue()),
                    Steuer = (steuerPos != null ? steuerPos.Preis : 0),
                    PreisKennzeichen = (kennzeichenPos != null ? kennzeichenPos.Preis : 0),
                    Wunschkennzeichen = kopfdaten.Wunschkennzeichen,
                    KennzeichenReservieren = kopfdaten.KennzeichenReservieren,
                    Zahlart_EC = (!kopfdaten.Zahlart_Bar.IsTrue() && !kopfdaten.Zahlart_Rechnung.IsTrue()),
                    Zahlart_Bar = kopfdaten.Zahlart_Bar,
                    Zahlart_Rechnung = kopfdaten.Zahlart_Rechnung,
                    WebBearbeitungsStatus = (pos.Loeschkennzeichen == "L" ? "L" : pos.WebBearbeitungsStatus),
                    Landkreis = kopfdaten.Landkreis,
                    KennzeichenTeil1 = kennzTeil1,
                    KennzeichenTeil2 = kennzTeil2,
                    BarzahlungKunde = kopfdaten.BarzahlungKunde,
                    Bemerkung = kopfdaten.Bemerkung,
                    Flieger = kopfdaten.Flieger,
                    Nachbearbeiten = kopfdaten.Nachbearbeiten,
                    Infotext = kopfdaten.Infotext,
                    Name1 = adresse.Name1,
                    Name2 = adresse.Name2,
                    Strasse = adresse.Strasse,
                    Plz = adresse.Plz,
                    Ort = adresse.Ort,
                    Land = adresse.Land,
                    Kontoinhaber = bankdaten.Kontoinhaber,
                    SWIFT = bankdaten.SWIFT,
                    IBAN = bankdaten.IBAN,
                    Vorerfassungsdatum = kopfdaten.Vorerfassungsdatum,
                    Vorerfassungszeit = kopfdaten.Vorerfassungszeit
                });
            }
        }

        public void LoadVorgang(string sapId, List<Materialstammdaten> materialStamm, List<Stva> stvaStamm)
        {
            ClearError();

            try
            {
                ApplyVorgangslisteChangesToBaseLists(materialStamm, stvaStamm, false);

                AktuellerVorgang.Kopfdaten = ModelMapping.Copy(_lstKopfdaten.FirstOrDefault(k => k.SapId == sapId, new ZLDKopfdaten()));
                AktuellerVorgang.Bankdaten = ModelMapping.Copy(_lstBankdaten.FirstOrDefault(b => b.SapId == sapId, new ZLDBankdaten()));
                AktuellerVorgang.Adressdaten = ModelMapping.Copy(_lstAdressen.FirstOrDefault(a => a.SapId == sapId, new ZLDAdressdaten()));
                AktuellerVorgang.Positionen = ModelMapping.CopyList(_lstPositionen.Where(p => p.SapId == sapId).OrderBy(p => p.PositionsNr.ToInt(0))).ToList();
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        public void UpdateWebBearbeitungsStatus(string sapId, string posNr, string newStatus)
        {
            ClearError();

            try
            {
                if (posNr == "10")
                {
                    // Hauptposition -> kompletten Vorgang updaten
                    foreach (var item in Vorgangsliste.Where(v => v.SapId == sapId).ToList())
                    {
                        // "A" oder "O" nur setzen, wenn Unterpos. kein Loeschkz hat
                        if ((newStatus != "A" && newStatus != "O") || item.WebBearbeitungsStatus != "L")
                            item.WebBearbeitungsStatus = newStatus;
                    }
                }
                else
                {
                    // Unterposition -> nur Unterposition updaten
                    var vg = Vorgangsliste.FirstOrDefault(v => v.SapId == sapId && v.PositionsNr == posNr);
                    if (vg != null)
                        vg.WebBearbeitungsStatus = newStatus;
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        public void GetPreise(List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm, string userName)
        {
            ExecuteSapZugriff(() =>
            {
                var kopfdaten = AktuellerVorgang.Kopfdaten;

                var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

                var dlPositionen = AktuellerVorgang.Positionen.Where(p => p.WebMaterialart == "D").ToList();

                foreach (var p in dlPositionen)
                {
                    var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == p.MaterialNr);
                    if (mat != null)
                    {
                        // Gebührenposition
                        var gebuehrenPos = AktuellerVorgang.Positionen.FirstOrDefault(gp => gp.UebergeordnetePosition == p.PositionsNr && gp.WebMaterialart == "G");
                        if (kunde != null && gebuehrenPos != null && !String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
                        {
                            var matNr = (kunde.OhneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr);
                            var matName = (kunde.OhneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName);

                            var gebuehrenMat = materialStamm.FirstOrDefault(m => m.MaterialNr == matNr);

                            gebuehrenPos.MaterialNr = matNr;
                            gebuehrenPos.MaterialName = matName;
                            gebuehrenPos.NullpreisErlaubt = (gebuehrenMat != null && gebuehrenMat.NullpreisErlaubt);
                        }

                        // Kennzeichenposition
                        if (kunde != null && kunde.Pauschal)
                        {
                            AktuellerVorgang.Positionen.Where(kp => kp.UebergeordnetePosition == p.PositionsNr && kp.WebMaterialart == "K").ToList().ForEach(kp => kp.WebBearbeitungsStatus = "L");
                        }
                        else
                        {
                            var kennzeichenPos = AktuellerVorgang.Positionen.FirstOrDefault(kp => kp.UebergeordnetePosition == p.PositionsNr && kp.WebMaterialart == "K");
                            if (kennzeichenPos == null && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                            {
                                var neuePosNr = AktuellerVorgang.Positionen.Max(posMax => posMax.PositionsNr.ToInt(0)) + 10;
                                var kennzeichenMat = materialStamm.FirstOrDefault(m => m.MaterialNr == mat.KennzeichenMaterialNr);

                                AktuellerVorgang.Positionen.Add(new ZLDPosition
                                {
                                    SapId = kopfdaten.SapId,
                                    PositionsNr = neuePosNr.ToString(),
                                    UebergeordnetePosition = p.PositionsNr,
                                    MaterialNr = mat.KennzeichenMaterialNr,
                                    MaterialName = "",
                                    Menge = 1,
                                    WebMaterialart = "K",
                                    NullpreisErlaubt = (kennzeichenMat != null && kennzeichenMat.NullpreisErlaubt)
                                });
                            }
                        }

                        // Steuerposition
                        if (p.PositionsNr != "10")
                            AktuellerVorgang.Positionen.Where(sp => sp.UebergeordnetePosition == p.PositionsNr && sp.WebMaterialart == "S").ToList().ForEach(sp => sp.WebBearbeitungsStatus = "L");
                    }
                }

                Z_ZLD_PREISFINDUNG2.Init(SAP);

                var kopfListe = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_BAK_From_ZLDKopfdaten.CopyBack(new List<ZLDKopfdaten> { kopfdaten });
                SAP.ApplyImport(kopfListe);

                var posListe = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_POS_From_ZLDPosition.CopyBack(AktuellerVorgang.Positionen);
                SAP.ApplyImport(posListe);

                CallBapi();

                AktuellerVorgang.Positionen = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_POS_To_ZLDPosition.Copy(Z_ZLD_PREISFINDUNG2.GT_POS.GetExportList(SAP)).ToList();
            });
        }

        public void GetPreiseNewPositionen(List<ZLDPosition> neuePositionen, List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm)
        {
            ClearError();

            if (neuePositionen.None())
                return;

            ExecuteSapZugriff(() =>
            {
                var posListeWeb = new List<ZLDPosition>();

                var kopfdaten = AktuellerVorgang.Kopfdaten;

                var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

                var posNr = 0;

                for (var i = 0; i < neuePositionen.Count; i++)
                {
                    var p = neuePositionen[i];

                    var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == p.MaterialNr);
                    if (mat != null)
                    {
                        posNr += 10;

                        // erste neue Pos behält die mitgegebene posID, die nächsten werden neu generiert
                        if (i == 0)
                        {
                            posNr = p.PositionsNr.ToInt(posNr);
                        }
                        else
                        {
                            p.PositionsNr = posNr.ToString();
                        }
                        p.SapId = kopfdaten.SapId;
                        p.MaterialName = p.CombineBezeichnungMenge();
                        p.NullpreisErlaubt = mat.NullpreisErlaubt;
                        posListeWeb.Add(p);

                        // Gebühren
                        if (!String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
                        {
                            posNr += 10;

                            var ohneUst = (kunde != null && kunde.OhneUst);
                            var matNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr);
                            var matName = (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName);

                            var gebuehrenMat = materialStamm.FirstOrDefault(m => m.MaterialNr == matNr);

                            posListeWeb.Add(new ZLDPosition
                            {
                                SapId = kopfdaten.SapId,
                                PositionsNr = posNr.ToString(),
                                UebergeordnetePosition = p.PositionsNr,
                                MaterialNr = matNr,
                                MaterialName = matName,
                                Menge = 1,
                                WebMaterialart = "G",
                                NullpreisErlaubt = (gebuehrenMat != null && gebuehrenMat.NullpreisErlaubt)
                            });
                        }

                        // Kennzeichen
                        if ((kunde == null || !kunde.Pauschal) && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                        {
                            posNr += 10;

                            var kennzeichenMat = materialStamm.FirstOrDefault(m => m.MaterialNr == mat.KennzeichenMaterialNr);

                            posListeWeb.Add(new ZLDPosition
                            {
                                SapId = kopfdaten.SapId,
                                PositionsNr = posNr.ToString(),
                                UebergeordnetePosition = p.PositionsNr,
                                MaterialNr = mat.KennzeichenMaterialNr,
                                MaterialName = "",
                                Menge = 1,
                                WebMaterialart = "K",
                                NullpreisErlaubt = (kennzeichenMat != null && kennzeichenMat.NullpreisErlaubt)
                            });
                        }

                        // Steuern
                        if (p.PositionsNr == "10")
                        {
                            posNr += 10;

                            posListeWeb.Add(new ZLDPosition
                            {
                                SapId = kopfdaten.SapId,
                                PositionsNr = posNr.ToString(),
                                UebergeordnetePosition = p.PositionsNr,
                                MaterialNr = "591",
                                MaterialName = "",
                                Menge = 1,
                                WebMaterialart = "S"
                            });
                        }
                    }
                }

                Z_ZLD_PREISFINDUNG2.Init(SAP);

                var kopfListe = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_BAK_From_ZLDKopfdaten.CopyBack(new List<ZLDKopfdaten> { kopfdaten });
                SAP.ApplyImport(kopfListe);

                var posListe = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_POS_From_ZLDPosition.CopyBack(posListeWeb);
                SAP.ApplyImport(posListe);

                CallBapi();

                AktuellerVorgang.Positionen.AddRange(AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_POS_To_ZLDPosition.Copy(Z_ZLD_PREISFINDUNG2.GT_POS.GetExportList(SAP)).ToList());
            });
        }

        /// <summary>
        /// Führt für alle (ggf. gefilterten) Vorgänge eine Preisfindung durch und aktualisiert deren Preise
        /// </summary>
        public void DoPreisfindung(List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm, List<Stva> stvaStamm, string userName)
        {
            List<string> idList;

            if (DataFilterActive)
            {
                idList = Vorgangsliste.Where(vg => ZLDCommon.FilterData(vg, DataFilterProperty, DataFilterValue, true)).GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();
            }
            else
            {
                idList = Vorgangsliste.GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();
            }

            foreach (var id in idList)
            {
                LoadVorgang(id, materialStamm, stvaStamm);

                GetPreise(kundenStamm, materialStamm, userName);

                // Liste aktualisieren
                Vorgangsliste.RemoveAll(vg => vg.SapId == AktuellerVorgang.Kopfdaten.SapId);
                AddVorgangToVorgangsliste(AktuellerVorgang.Kopfdaten, AktuellerVorgang.Bankdaten, AktuellerVorgang.Adressdaten, AktuellerVorgang.Positionen, kundenStamm);
            }
        }

        public void SaveVorgangToSap(List<Kundenstammdaten> kundenStamm, string userName)
        {
            ExecuteSapZugriff(() =>
            {
                var kopfdaten = AktuellerVorgang.Kopfdaten;

                kopfdaten.Erfassungsdatum = DateTime.Now;
                kopfdaten.Erfasser = userName;

                if (String.IsNullOrEmpty(AktuellerVorgang.Bankdaten.Partnerrolle)) AktuellerVorgang.Bankdaten.Partnerrolle = "AG";

                AktuellerVorgang.Adressdaten.KundenNr = kopfdaten.KundenNr;
                if (String.IsNullOrEmpty(AktuellerVorgang.Adressdaten.Partnerrolle)) AktuellerVorgang.Adressdaten.Partnerrolle = "AG";

                foreach (var p in AktuellerVorgang.Positionen)
                {
                    if (p.WebMaterialart == "S" && p.UebergeordnetePosition != "10")
                        p.WebBearbeitungsStatus = "L";
                }

                ApplyAktuellerVorgangChangesToBaseLists();

                var kopfdatenRel = _lstKopfdaten.Where(k => k.SapId == kopfdaten.SapId);
                var bankdatenRel = _lstBankdaten.Where(b => b.SapId == kopfdaten.SapId && (!String.IsNullOrEmpty(b.Kontoinhaber) || b.Loeschkennzeichen == "L"));
                var adressdatenRel = _lstAdressen.Where(a => a.SapId == kopfdaten.SapId && (!String.IsNullOrEmpty(a.Name1) || a.Loeschkennzeichen == "L"));
                var positionenRel = _lstPositionen.Where(p => p.SapId == kopfdaten.SapId);

                Z_ZLD_SAVE_DATA2.Init(SAP);

                if (SelAnnahmeAH)
                    SAP.SetImportParameter("I_AH_ANNAHME", "X");

                if (SelSofortabrechnung)
                    SAP.SetImportParameter("I_SOFORTABRECHNUNG", "X");

                if (SelAenderungAngenommene)
                    SAP.SetImportParameter("I_AENDERUNG_ANGENOMMENE", "X");

                var kopfListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(kopfdatenRel);
                SAP.ApplyImport(kopfListe);

                var bankListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankdatenRel);
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressdatenRel);
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_POS_From_ZLDPosition.CopyBack(positionenRel);
                SAP.ApplyImport(posListe);

                CallBapi();

                var fehlerListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_SAVE_DATA2.GT_EX_ERRORS.GetExportList(SAP)).ToList();
                var ergPosListe = Z_ZLD_SAVE_DATA2.GT_IMP_POS.GetExportList(SAP);

                if (fehlerListe.Any(f => !String.IsNullOrEmpty(f.FehlerText) && f.FehlerText != "OK"))
                {
                    RaiseError(-9999, "Beim Speichern des Vorgangs in SAP sind Fehler aufgetreten");

                    foreach (var fehler in fehlerListe)
                    {
                        var pos = AktuellerVorgang.Positionen.FirstOrDefault(p => p.SapId == fehler.SapId && (String.IsNullOrEmpty(fehler.PositionsNr) || p.PositionsNr == fehler.PositionsNr));
                        if (pos != null)
                        {
                            if (!String.IsNullOrEmpty(fehler.FehlerText))
                                pos.FehlerText = fehler.FehlerText;
                            else
                                pos.FehlerText = "OK";
                        }
                    }
                }

                if (kopfdaten.Belegart == "OK")
                {
                    // Bei Belegart "OK" wurden die DL-Preise ggf. SAP-seitig angepasst
                    foreach (var pos in AktuellerVorgang.Positionen.Where(p => p.WebMaterialart == "D"))
                    {
                        var ergPos = ergPosListe.FirstOrDefault(p => p.ZULBELN.TrimStart('0') == kopfdaten.SapId && p.ZULPOSNR.TrimStart('0') == pos.PositionsNr);
                        if (ergPos != null)
                            pos.Preis = ergPos.PREIS;
                    }
                }

                ApplyAktuellerVorgangChangesToBaseLists(true);

                // Liste aktualisieren
                Vorgangsliste.RemoveAll(vg => vg.SapId == kopfdaten.SapId);
                if (kopfdaten.Flieger == SelFlieger)
                    AddVorgangToVorgangsliste(kopfdaten, AktuellerVorgang.Bankdaten, AktuellerVorgang.Adressdaten, AktuellerVorgang.Positionen, kundenStamm);
            });
        }

        public void SaveVorgaengeToSap(List<Materialstammdaten> materialStamm, List<Stva> stvaStamm, string userName, bool annahmeAhSend = false)
        {
            ClearError();

            var blnAnnahmeAhSenden = (SelAnnahmeAH && annahmeAhSend);

            List<ZLDVorgangUINacherfassung> vgList;

            if (DataFilterActive)
            {
                vgList = Vorgangsliste.Where(vg => ZLDCommon.FilterData(vg, DataFilterProperty, DataFilterValue, true)).ToList();
            }
            else
            {
                vgList = Vorgangsliste;
            }

            if (vgList.None() || (blnAnnahmeAhSenden && vgList.None(vg => vg.WebBearbeitungsStatus.In("A,L,V"))))
                return;

            ExecuteSapZugriff(() =>
            {
                Vorgangsliste.ForEach(vg => vg.FehlerText = "");

                ApplyVorgangslisteChangesToBaseLists(materialStamm, stvaStamm, blnAnnahmeAhSenden || SelAenderungAngenommene);

                List<string> idList;

                if (blnAnnahmeAhSenden)
                {
                    idList = vgList.Where(vg => vg.WebBearbeitungsStatus.In("A,L,V")).GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();
                }
                else
                {
                    idList = vgList.GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();
                }

                var bankdatenRel = _lstBankdaten.Where(b => idList.Contains(b.SapId) && (!String.IsNullOrEmpty(b.Kontoinhaber) || b.Loeschkennzeichen == "L")).ToList();
                var adressdatenRel = _lstAdressen.Where(a => idList.Contains(a.SapId) && (!String.IsNullOrEmpty(a.Name1) || a.Loeschkennzeichen == "L")).ToList();

                foreach (var item in _lstKopfdaten.Where(k => idList.Contains(k.SapId)))
                {
                    var kopf = item;

                    kopf.Erfassungsdatum = DateTime.Now;
                    kopf.Erfasser = userName;

                    var bankd = bankdatenRel.FirstOrDefault(b => b.SapId == kopf.SapId);
                    if (bankd != null)
                    {
                        if (String.IsNullOrEmpty(bankd.Partnerrolle)) bankd.Partnerrolle = "AG";
                    }

                    var adresse = adressdatenRel.FirstOrDefault(a => a.SapId == kopf.SapId);
                    if (adresse != null)
                    {
                        adresse.KundenNr = kopf.KundenNr;
                        if (String.IsNullOrEmpty(adresse.Partnerrolle)) adresse.Partnerrolle = "AG";
                    }

                    foreach (var p in _lstPositionen.Where(p => p.SapId == kopf.SapId))
                    {
                        if (blnAnnahmeAhSenden)
                        {
                            if (p.PositionsNr == "10")
                            {
                                switch (p.WebBearbeitungsStatus)
                                {
                                    case "A":
                                        kopf.Bearbeitungsstatus = "A";
                                        break;
                                    case "L":
                                        kopf.Bearbeitungsstatus = "L";
                                        break;
                                    case "V":
                                        kopf.Belegart = "AV";
                                        break;
                                    default:
                                        kopf.Bearbeitungsstatus = "1";
                                        break;
                                }
                            }

                            p.WebBearbeitungsStatus = "";
                        }
                        else if (SelAenderungAngenommene)
                        {
                            if (p.PositionsNr == "10")
                            {
                                switch (p.WebBearbeitungsStatus)
                                {
                                    case "L":
                                        kopf.Bearbeitungsstatus = "L";
                                        break;
                                    default:
                                        kopf.Bearbeitungsstatus = "A";
                                        break;
                                }
                            }

                            p.WebBearbeitungsStatus = "";
                        }

                        if (p.WebMaterialart == "S" && p.UebergeordnetePosition != "10")
                            p.WebBearbeitungsStatus = "L";
                    }
                }

                Z_ZLD_SAVE_DATA2.Init(SAP);

                if (SelAnnahmeAH)
                    SAP.SetImportParameter("I_AH_ANNAHME", "X");

                if (SelSofortabrechnung)
                    SAP.SetImportParameter("I_SOFORTABRECHNUNG", "X");

                if (SelAenderungAngenommene)
                    SAP.SetImportParameter("I_AENDERUNG_ANGENOMMENE", "X");

                var kopfListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(_lstKopfdaten.Where(k => idList.Contains(k.SapId)).ToList());
                SAP.ApplyImport(kopfListe);
                
                var bankListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankdatenRel);
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressdatenRel);
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_POS_From_ZLDPosition.CopyBack(_lstPositionen.Where(p => idList.Contains(p.SapId)).ToList());
                SAP.ApplyImport(posListe);

                CallBapi();

                var fehlerListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_SAVE_DATA2.GT_EX_ERRORS.GetExportList(SAP)).ToList();
                var ergPosListe = Z_ZLD_SAVE_DATA2.GT_IMP_POS.GetExportList(SAP);

                foreach (var vg in Vorgangsliste.Where(vg => idList.Contains(vg.SapId)))
                {
                    var fehler = fehlerListe.FirstOrDefault(f => f.SapId == vg.SapId && (String.IsNullOrEmpty(f.PositionsNr) || f.PositionsNr == vg.PositionsNr) && !String.IsNullOrEmpty(f.FehlerText));

                    if (fehler != null)
                        vg.FehlerText = fehler.FehlerText;
                    else
                        vg.FehlerText = "OK";

                    if (vg.Belegart == "OK")
                    {
                        // Bei Belegart "OK" wurden die DL-Preise ggf. SAP-seitig angepasst
                        var ergPos = ergPosListe.FirstOrDefault(p => p.ZULBELN.TrimStart('0') == vg.SapId && p.ZULPOSNR.TrimStart('0') == vg.PositionsNr);
                        if (ergPos != null)
                            vg.Preis = ergPos.PREIS;
                    }
                }
            });
        }

        public void SendVorgaengeToSap(List<Materialstammdaten> materialStamm, List<Stva> stvaStamm, string userName, string userVorname, string userNachname, bool versandZulDurchf = false)
        {
            ClearError();

            List<ZLDVorgangUINacherfassung> vgList;

            if (DataFilterActive)
            {
                vgList = Vorgangsliste.Where(vg => ZLDCommon.FilterData(vg, DataFilterProperty, DataFilterValue, true)).ToList();
            }
            else
            {
                vgList = Vorgangsliste;
            }

            if (vgList.None(vg => vg.WebBearbeitungsStatus.In("O,L")))
            {
                RaiseError(9999, "Es sind keine Vorgänge mit \"O\" oder \"L\" markiert");
                return;
            }

            ExecuteSapZugriff(() =>
            {
                Vorgangsliste.ForEach(vg => vg.FehlerText = "");

                ApplyVorgangslisteChangesToBaseLists(materialStamm, stvaStamm, true);

                var idList = vgList.Where(vg => vg.WebBearbeitungsStatus.In("O,L")).GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();

                var bankdatenRel = _lstBankdaten.Where(b => idList.Contains(b.SapId) && (!String.IsNullOrEmpty(b.Kontoinhaber) || b.Loeschkennzeichen == "L"));
                var adressdatenRel = _lstAdressen.Where(a => idList.Contains(a.SapId) && (!String.IsNullOrEmpty(a.Name1) || a.Loeschkennzeichen == "L"));

                foreach (var item in _lstKopfdaten.Where(k => idList.Contains(k.SapId)))
                {
                    var kopf = item;

                    if (!SelSofortabrechnung && !versandZulDurchf)
                    {
                        if (kopf.Belegart.In("VZ,VE,AV,AX"))
                        {
                            kopf.VersandzulassungErledigtDatum = DateTime.Now;
                            kopf.VersandzulassungBearbeitungsstatus = "VD";
                        }
                    }

                    kopf.Erfassungsdatum = DateTime.Now;
                    kopf.Erfasser = userName;

                    var bankd = bankdatenRel.FirstOrDefault(b => b.SapId == kopf.SapId);
                    if (bankd != null)
                    {
                        if (String.IsNullOrEmpty(bankd.Partnerrolle)) bankd.Partnerrolle = "AG";
                    }

                    var adresse = adressdatenRel.FirstOrDefault(a => a.SapId == kopf.SapId);
                    if (adresse != null)
                    {
                        adresse.KundenNr = kopf.KundenNr;
                        if (String.IsNullOrEmpty(adresse.Partnerrolle)) adresse.Partnerrolle = "AG";
                    }

                    foreach (var p in _lstPositionen.Where(p => p.SapId == kopf.SapId))
                    {
                        p.WebBearbeitungsStatus = "";

                        if ((p.WebMaterialart == "S" && (p.UebergeordnetePosition != "10" || !p.Preis.HasValue || p.Preis == 0))
                            || (p.WebMaterialart == "K" && (!p.Preis.HasValue || p.Preis == 0)))
                        {
                            p.Loeschkennzeichen = "L";
                        }
                    }
                }

                if (SelSofortabrechnung)
                {
                    Z_ZLD_IMPORT_SOFORT_ABRECH2.Init(SAP);

                    var kopfListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(_lstKopfdaten.Where(k => idList.Contains(k.SapId)));
                    SAP.ApplyImport(kopfListe);

                    var bankListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankdatenRel);
                    SAP.ApplyImport(bankListe);

                    var adressListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressdatenRel);
                    SAP.ApplyImport(adressListe);

                    var posListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_POS_From_ZLDPosition.CopyBack(_lstPositionen.Where(p => idList.Contains(p.SapId)));
                    SAP.ApplyImport(posListe);

                    var uDaten = new Userdaten
                    {
                        UserName = userName,
                        Vorname = userVorname,
                        Nachname = userNachname
                    };

                    var webUserListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_WEBUSER_DATEN_From_Userdaten.CopyBack(new List<Userdaten> { uDaten });
                    SAP.ApplyImport(webUserListe);
                }
                else if (versandZulDurchf)
                {
                    Z_ZLD_IMP_NACHERF_DZLD2.Init(SAP);

                    var kopfListe = AppModelMappings.Z_ZLD_IMP_NACHERF_DZLD2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(_lstKopfdaten.Where(k => idList.Contains(k.SapId)));
                    SAP.ApplyImport(kopfListe);

                    var posListe = AppModelMappings.Z_ZLD_IMP_NACHERF_DZLD2_GT_IMP_POS_From_ZLDPosition.CopyBack(_lstPositionen.Where(p => idList.Contains(p.SapId)));
                    SAP.ApplyImport(posListe);
                }
                else
                {
                    Z_ZLD_IMP_NACHERF2.Init(SAP);

                    var kopfListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(_lstKopfdaten.Where(k => idList.Contains(k.SapId)));
                    SAP.ApplyImport(kopfListe);

                    var bankListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankdatenRel);
                    SAP.ApplyImport(bankListe);

                    var adressListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressdatenRel);
                    SAP.ApplyImport(adressListe);

                    var posListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_POS_From_ZLDPosition.CopyBack(_lstPositionen.Where(p => idList.Contains(p.SapId)));
                    SAP.ApplyImport(posListe);
                }

                CallBapi();

                List<ZLDFehler> fehlerListe;

                if (SelSofortabrechnung)
                    fehlerListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_EX_ERRORS.GetExportList(SAP)).ToList();
                else if (versandZulDurchf)
                    fehlerListe = AppModelMappings.Z_ZLD_IMP_NACHERF_DZLD2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMP_NACHERF_DZLD2.GT_EX_ERRORS.GetExportList(SAP)).ToList();
                else
                    fehlerListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMP_NACHERF2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                foreach (var vg in Vorgangsliste.Where(vg => idList.Contains(vg.SapId)))
                {
                    var fehler = fehlerListe.FirstOrDefault(f => f.SapId == vg.SapId && (String.IsNullOrEmpty(f.PositionsNr) || f.PositionsNr == vg.PositionsNr) && !String.IsNullOrEmpty(f.FehlerText));

                    if (fehler != null)
                        vg.FehlerText = fehler.FehlerText;
                    else
                        vg.FehlerText = "OK";
                }

                if (SelSofortabrechnung)
                {
                    SofortabrechnungVerzeichnis = SAP.GetExportParameter("G_SA_PFAD");
                }
                else if (!versandZulDurchf)
                {
                    tblBarquittungen = SAP.GetExportTable("GT_BARQ");
                }
            });
        }

        public void LoadAHVersandVorgaengeFromSap()
        {
            ExecuteSapZugriff(() =>
            {
                Z_ZLD_AH_EX_VSZUL.Init(SAP);

                SAP.SetImportParameter("I_KUNNR", (String.IsNullOrEmpty(SelKunde) ? "" : SelKunde.ToSapKunnr()));
                SAP.SetImportParameter("I_VKBUR", VKBUR);
                SAP.SetImportParameter("I_VKORG", VKORG);
                SAP.SetImportParameter("I_ZZZLDAT", SelDatum);
                SAP.SetImportParameter("I_ZULBELN", (String.IsNullOrEmpty(SelID) ? "" : SelID.PadLeft0(10)));
                SAP.SetImportParameter("I_KREISKZ", SelKreis);

                CallBapi();

                AHVersandListe = SAP.GetExportTable("GT_OUT");
                AHVersandListe.Columns.Add("toDelete", typeof(String));

                foreach (DataRow itemRow in AHVersandListe.Rows)
                {
                    itemRow["ZULBELN"] = itemRow["ZULBELN"].ToString().TrimStart('0');
                    itemRow["KUNNR"] = itemRow["KUNNR"].ToString().TrimStart('0');
                    itemRow["ZULPOSNR"] = itemRow["ZULPOSNR"].ToString().TrimStart('0');
                    itemRow["toDelete"] = "";
                }
            });
        }

        public void LoadAHVersandVorgangDetailFromSap(string sapId)
        {
            ExecuteSapZugriff(() =>
            {
                Z_ZLD_GET_ORDER2.Init(SAP, "I_ZULBELN", sapId.PadLeft0(10));

                CallBapi();

                var sapKopfdaten = Z_ZLD_GET_ORDER2.GS_EX_BAK.GetExportList(SAP).FirstOrDefault(new Z_ZLD_GET_ORDER2.GS_EX_BAK());
                var sapBankdaten = Z_ZLD_GET_ORDER2.GT_EX_BANK.GetExportList(SAP).FirstOrDefault(new Z_ZLD_GET_ORDER2.GT_EX_BANK());
                var sapAdresse = Z_ZLD_GET_ORDER2.GT_EX_ADRS.GetExportList(SAP).FirstOrDefault(new Z_ZLD_GET_ORDER2.GT_EX_ADRS());
                var sapPositionen = Z_ZLD_GET_ORDER2.GT_EX_POS.GetExportList(SAP);

                if (sapKopfdaten == null)
                {
                    RaiseError(9999, "Kein Vorgang zur ID vorhanden");
                    return;
                }

                AktuellerVorgang.Kopfdaten = AppModelMappings.Z_ZLD_GET_ORDER2_GS_EX_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten);
                AktuellerVorgang.Bankdaten = AppModelMappings.Z_ZLD_GET_ORDER2_GT_EX_BANK_To_ZLDBankdaten.Copy(sapBankdaten);
                AktuellerVorgang.Adressdaten = AppModelMappings.Z_ZLD_GET_ORDER2_GT_EX_ADRS_To_ZLDAdressdaten.Copy(sapAdresse);
                AktuellerVorgang.Positionen = AppModelMappings.Z_ZLD_GET_ORDER2_GT_EX_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.PositionsNr.ToInt(0)).ToList();
            });
        }

        public void SaveAHVersandVorgangToSap(string userName)
        {
            ExecuteSapZugriff(() =>
            {
                var kopfdaten = AktuellerVorgang.Kopfdaten;

                kopfdaten.VersandzulassungBearbeitungsstatus = "LA";

                kopfdaten.Zahlart_EC = true;
                kopfdaten.Zahlart_Bar = false;
                kopfdaten.Zahlart_Rechnung = false;

                if (kopfdaten.LieferantenNr.NotNullOrEmpty().TrimStart('0').Substring(0, 2) == "56" && IsZLD)
                {
                    kopfdaten.VersandzulassungDurchfuehrendesVkBur = kopfdaten.LieferantenNr.TrimStart('0').Substring(2, 4);
                    kopfdaten.Belegart = "AV";
                }
                else
                {
                    kopfdaten.VersandzulassungDurchfuehrendesVkBur = "";
                    kopfdaten.Belegart = "AX";
                }

                kopfdaten.Bearbeitungsstatus = "A";

                kopfdaten.Erfassungsdatum = DateTime.Now;
                kopfdaten.Erfasser = userName;

                if (String.IsNullOrEmpty(AktuellerVorgang.Bankdaten.Partnerrolle)) AktuellerVorgang.Bankdaten.Partnerrolle = "AG";

                AktuellerVorgang.Adressdaten.KundenNr = kopfdaten.KundenNr;
                if (String.IsNullOrEmpty(AktuellerVorgang.Adressdaten.Partnerrolle)) AktuellerVorgang.Adressdaten.Partnerrolle = "AG";

                var adressListeWeb = new List<ZLDAdressdaten> { AktuellerVorgang.Adressdaten };

                if (!String.IsNullOrEmpty(DocRueck1))
                {
                    adressListeWeb.Add(new ZLDAdressdaten
                    {
                        SapId = kopfdaten.SapId,
                        KundenNr = kopfdaten.KundenNr,
                        Partnerrolle = "ZE",
                        Name1 = NameRueck1,
                        Name2 = NameRueck2,
                        Strasse = StrasseRueck,
                        Plz = PLZRueck,
                        Ort = OrtRueck,
                        Bemerkung = DocRueck1
                    });
                }

                if (!String.IsNullOrEmpty(Doc2Rueck))
                {
                    adressListeWeb.Add(new ZLDAdressdaten
                    {
                        SapId = kopfdaten.SapId,
                        KundenNr = kopfdaten.KundenNr,
                        Partnerrolle = "ZS",
                        Name1 = Name1Rueck2,
                        Name2 = Name2Rueck2,
                        Strasse = Strasse2Rueck,
                        Plz = PLZ2Rueck,
                        Ort = Ort2Rueck,
                        Bemerkung = Doc2Rueck
                    });
                }

                Z_ZLD_AH_VZ_SAVE2.Init(SAP);

                var kopfListe = AppModelMappings.Z_ZLD_AH_VZ_SAVE2_GT_BAK_From_ZLDKopfdaten.CopyBack(new List<ZLDKopfdaten> { kopfdaten });
                SAP.ApplyImport(kopfListe);

                var bankListe = AppModelMappings.Z_ZLD_AH_VZ_SAVE2_GT_BANK_From_ZLDBankdaten.CopyBack(new List<ZLDBankdaten> { AktuellerVorgang.Bankdaten });
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_AH_VZ_SAVE2_GT_ADRS_From_ZLDAdressdaten.CopyBack(adressListeWeb);
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_AH_VZ_SAVE2_GT_POS_From_ZLDPosition.CopyBack(AktuellerVorgang.Positionen);
                SAP.ApplyImport(posListe);

                CallBapi();

                var fehlerListe = AppModelMappings.Z_ZLD_AH_VZ_SAVE2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_AH_VZ_SAVE2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                if (fehlerListe.Any(f => !String.IsNullOrEmpty(f.FehlerText) && f.FehlerText != "OK"))
                {
                    RaiseError(-9999, "Beim Speichern des Vorgangs in SAP sind Fehler aufgetreten");

                    foreach (var fehler in fehlerListe)
                    {
                        var pos = AktuellerVorgang.Positionen.FirstOrDefault(p => p.SapId == fehler.SapId && (String.IsNullOrEmpty(fehler.PositionsNr) || p.PositionsNr == fehler.PositionsNr));
                        if (pos != null)
                        {
                            if (!String.IsNullOrEmpty(fehler.FehlerText))
                                pos.FehlerText = fehler.FehlerText;
                            else
                                pos.FehlerText = "OK";
                        }
                    }
                }
            });
        }

        public void SetSapLoekzForAHVersandVorgaenge()
        {
            foreach (DataRow item in AHVersandListe.Rows)
            {
                var SaveRow = item;

                if (SaveRow["toDelete"].ToString() == "L")
                {
                    ExecuteSapZugriff(() =>
                    {
                        Z_ZLD_SET_LOEKZ.Init(SAP, "I_ZULBELN", SaveRow["ZULBELN"].ToString().PadLeft0(10));

                        CallBapi();
                    });
                }
            }
        }

        public void DeleteVorgaengeOkAndDelFromLists()
        {
            ClearError();

            try
            {
                var idList = Vorgangsliste.GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();

                foreach (var item in idList)
                {
                    var id = item;

                    if (Vorgangsliste.None(v => v.SapId == id && v.FehlerText != "OK"))
                    {
                        Vorgangsliste.RemoveAll(v => v.SapId == id);
                        _lstKopfdaten.RemoveAll(k => k.SapId == id);
                        _lstBankdaten.RemoveAll(b => b.SapId == id);
                        _lstAdressen.RemoveAll(a => a.SapId == id);
                        _lstPositionen.RemoveAll(p => p.SapId == id);
                    }

                    var posDelList = Vorgangsliste.Where(v => v.SapId == id && v.WebBearbeitungsStatus == "L").Select(v => v.PositionsNr).ToList();

                    foreach (var pos in posDelList)
                    {
                        Vorgangsliste.RemoveAll(v => v.SapId == id && v.PositionsNr == pos);
                        _lstPositionen.RemoveAll(p => p.SapId == id && p.PositionsNr == pos);
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        /// <summary>
        /// Lädt das SDRelevant-Flag eines Gebührenmat.
        /// ist der Kunde ein Pauschalkunde,  Gebühr und Gebühr Amt unterschiedlich und 
        /// das Gebührenmaterial nicht SD relevant darf der Vorgang nicht abgesendet werden
        /// </summary>
        /// <param name="sapId"></param>
        /// <param name="posNr"></param>
        /// <param name="matNr"></param>
        /// <returns>SDRelevant</returns>
        public bool GetSDRelevantsGeb(string sapId, string posNr, string matNr)
        {
            ClearError();

            try
            {
                var gebuehrenPos = _lstPositionen.FirstOrDefault(p => 
                    p.SapId == sapId && p.UebergeordnetePosition == posNr &&
                    p.MaterialNr == matNr && p.WebMaterialart == "G");

                return (gebuehrenPos != null && gebuehrenPos.SdRelevant.IsTrue());
            }
            catch (Exception ex)
            {
                RaiseError(-9999, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Im aktuellen Vorgang vorgenommene Datenänderungen in Basislisten übernehmen
        /// </summary>
        private void ApplyAktuellerVorgangChangesToBaseLists(bool nurKopfUndPos = false)
        {
            var tmpKopfdaten = _lstKopfdaten.FirstOrDefault(k => k.SapId == AktuellerVorgang.Kopfdaten.SapId);
            var tmpBankdaten = _lstBankdaten.FirstOrDefault(b => b.SapId == AktuellerVorgang.Kopfdaten.SapId);
            var tmpAdressdaten = _lstAdressen.FirstOrDefault(a => a.SapId == AktuellerVorgang.Kopfdaten.SapId);
            var tmpPositionen = _lstPositionen.Where(p => p.SapId == AktuellerVorgang.Kopfdaten.SapId).OrderBy(p => p.PositionsNr.ToInt(0)).ToList();

            if (tmpKopfdaten != null)
                ModelMapping.Copy(AktuellerVorgang.Kopfdaten, tmpKopfdaten);

            if (!nurKopfUndPos)
            {
                if (tmpBankdaten != null)
                {
                    if (String.IsNullOrEmpty(AktuellerVorgang.Bankdaten.Kontoinhaber))
                        AktuellerVorgang.Bankdaten.Loeschkennzeichen = "L";
                    ModelMapping.Copy(AktuellerVorgang.Bankdaten, tmpBankdaten);
                }
                else
                {
                    if (!String.IsNullOrEmpty(AktuellerVorgang.Bankdaten.Kontoinhaber))
                        _lstBankdaten.Add(ModelMapping.Copy(AktuellerVorgang.Bankdaten));
                }

                if (tmpAdressdaten != null)
                {
                    if (String.IsNullOrEmpty(AktuellerVorgang.Adressdaten.Name1))
                        AktuellerVorgang.Adressdaten.Loeschkennzeichen = "L";
                    ModelMapping.Copy(AktuellerVorgang.Adressdaten, tmpAdressdaten);
                }
                else
                {
                    if (!String.IsNullOrEmpty(AktuellerVorgang.Adressdaten.Name1))
                        _lstAdressen.Add(ModelMapping.Copy(AktuellerVorgang.Adressdaten));
                }
            }

            foreach (var item in AktuellerVorgang.Positionen)
            {
                var pos = tmpPositionen.FirstOrDefault(p => p.PositionsNr == item.PositionsNr);
                if (pos != null)
                {
                    ModelMapping.Copy(item, pos);
                }
                else
                {
                    _lstPositionen.Add(ModelMapping.Copy(item));
                }
            }
        }

        /// <summary>
        /// In Vorgangsliste vorgenommene Datenänderungen in Basislisten übernehmen
        /// </summary>
        /// <param name="materialStamm"></param>
        /// <param name="stvaStamm"></param>
        /// <param name="setLoeschkennzeichen"></param>
        private void ApplyVorgangslisteChangesToBaseLists(List<Materialstammdaten> materialStamm, List<Stva> stvaStamm, bool setLoeschkennzeichen)
        {
            List<ZLDVorgangUINacherfassung> vgList;

            if (DataFilterActive)
            {
                vgList = Vorgangsliste.Where(vg => ZLDCommon.FilterData(vg, DataFilterProperty, DataFilterValue, true)).ToList();
            }
            else
            {
                vgList = Vorgangsliste;
            }

            var idList = vgList.GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();

            foreach (var item in _lstKopfdaten.Where(k => idList.Contains(k.SapId)))
            {
                var kopf = item;

                var hauptPos = Vorgangsliste.FirstOrDefault(v => v.SapId == kopf.SapId && v.PositionsNr == "10");
                if (hauptPos != null)
                {
                    kopf.Landkreis = hauptPos.Landkreis;

                    var amt = stvaStamm.FirstOrDefault(s => s.Landkreis == kopf.Landkreis);
                    kopf.KreisBezeichnung = (amt != null ? amt.KreisBezeichnung : "");

                    kopf.Kennzeichen = hauptPos.Kennzeichen;

                    kopf.Zahlart_EC = hauptPos.Zahlart_EC;
                    kopf.Zahlart_Bar = hauptPos.Zahlart_Bar;
                    kopf.Zahlart_Rechnung = hauptPos.Zahlart_Rechnung;

                    kopf.Zulassungsdatum = hauptPos.Zulassungsdatum;
                }

                var positionen = Vorgangsliste.Where(v => v.SapId == kopf.SapId);
                foreach (var pos in positionen)
                {
                    var dlPos = _lstPositionen.FirstOrDefault(p => p.SapId == pos.SapId && p.PositionsNr == pos.PositionsNr);
                    if (dlPos != null)
                    {
                        var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == dlPos.MaterialNr);

                        var loeschKz = (pos.WebBearbeitungsStatus == "L" ? "L" : "");

                        if (setLoeschkennzeichen)
                            dlPos.Loeschkennzeichen = loeschKz;

                        dlPos.Preis = pos.Preis;
                        dlPos.MaterialName = pos.MaterialName;
                        dlPos.MaterialName = dlPos.CombineBezeichnungMenge();
                        dlPos.WebBearbeitungsStatus = pos.WebBearbeitungsStatus;

                        // eingegebene Preise auf die entspr. Unterpositionen verteilen
                        var gebuehrenPos = _lstPositionen.FirstOrDefault(p => p.SapId == pos.SapId && p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "G" && p.Loeschkennzeichen != "L");
                        if (gebuehrenPos != null)
                        {
                            if (setLoeschkennzeichen)
                                gebuehrenPos.Loeschkennzeichen = loeschKz;

                            gebuehrenPos.Preis = pos.Gebuehr;
                            gebuehrenPos.GebuehrAmt = pos.GebuehrAmt;
                            gebuehrenPos.Gebuehrenpaket = pos.Gebuehrenpaket;
                        }

                        var steuerPos = _lstPositionen.FirstOrDefault(p => p.SapId == pos.SapId && p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "S" && p.Loeschkennzeichen != "L");
                        if (steuerPos != null)
                        {
                            if (setLoeschkennzeichen)
                                steuerPos.Loeschkennzeichen = loeschKz;

                            steuerPos.Preis = pos.Steuer;
                        }

                        var kennzeichenPos = _lstPositionen.FirstOrDefault(p => p.SapId == pos.SapId && p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "K" && p.Loeschkennzeichen != "L");
                        if (kennzeichenPos != null)
                        {
                            if (setLoeschkennzeichen)
                                kennzeichenPos.Loeschkennzeichen = loeschKz;

                            kennzeichenPos.Preis = pos.PreisKennzeichen;
                        }

                        // überflüssige Unterpositionen löschen
                        if (dlPos.WebMaterialart == "G" && mat != null && String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                            dlPos.Loeschkennzeichen = "L";
                        else if (dlPos.WebMaterialart == "S" && dlPos.UebergeordnetePosition != "10")
                            dlPos.Loeschkennzeichen = "L";
                    }
                }
            }
        }

        public void LoadRechnungsanhangTemplatesFromSql()
        {
            ClearError();

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                RechnungUploadTemplates = zldDataContext.RechnungsanhangTemplates.OrderBy(r => r.Bezeichnung).ToList();
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        public int InsertGebuehrenFromUploadData(List<Materialstammdaten> materialStamm, List<Stva> stvaStamm, List<RechnungsanhangDaten> uploadList)
        {
            var anzGefunden = 0;

            foreach (var item in uploadList)
            {
                var vorgang = Vorgangsliste.FirstOrDefault(v => v.Kennzeichen == item.Kennzeichen && v.PositionsNr == "10" && v.WebBearbeitungsStatus != "L");
                if (vorgang != null)
                {
                    vorgang.GebuehrAmt = item.Gebuehren.ToDecimal(0);

                    if (!vorgang.Gebuehrenpaket.IsTrue())
                        vorgang.Gebuehr = item.Gebuehren.ToDecimal(0);

                    DateTime tmpDate;
                    if (!String.IsNullOrEmpty(item.Zulassungsdatum) && DateTime.TryParseExact(item.Zulassungsdatum, "dd.MM.yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out tmpDate))
                        vorgang.Zulassungsdatum = tmpDate;

                    Vorgangsliste.Where(v => v.SapId == vorgang.SapId && v.WebBearbeitungsStatus != "L").ToList().ForEach(v => v.WebBearbeitungsStatus = "O");

                    anzGefunden++;
                }
            }

            ApplyVorgangslisteChangesToBaseLists(materialStamm, stvaStamm, false);

            return anzGefunden;
        }

        public void LoadNochNichtAbgesendeteVorgaengeFromSap()
        {
            ExecuteSapZugriff(() =>
            {
                Z_ZLD_EXPORT_AH_WARENKORB.Init(SAP);

                SAP.SetImportParameter("I_KUNNR", (String.IsNullOrEmpty(SelKunde) ? "" : SelKunde.ToSapKunnr()));
                SAP.SetImportParameter("I_VKORG", VKORG);
                SAP.SetImportParameter("I_VKBUR", VKBUR);
                SAP.SetImportParameter("I_ZZZLDAT", SelDatum);
                SAP.SetImportParameter("I_ZULBELN", (String.IsNullOrEmpty(SelID) ? "" : SelID.PadLeft0(10)));

                SAP.SetImportParameter("I_KREISKZ", SelKreis);

                if (!String.IsNullOrEmpty(SelGroupTourID))
                    SAP.SetImportParameter("I_GRUPPE", SelGroupTourID.PadLeft0(10));

                var sapList = Z_ZLD_EXPORT_AH_WARENKORB.GT_BAK.GetExportListWithExecute(SAP);

                NochNichtAbgesendeteVorgaenge = AppModelMappings.Z_ZLD_EXPORT_AH_WARENKORB_GT_BAK_To_NochNichtAbgesendeterVorgang.Copy(sapList).ToList();
            });
        }

        public void SelectNochNichtAbgesendetenVorgang(string sapId, bool newStatus)
        {
            ClearError();

            try
            {
                var vg = NochNichtAbgesendeteVorgaenge.FirstOrDefault(v => v.SapId == sapId);
                if (vg != null)
                    vg.IsSelected = newStatus;
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        public void SelectNochNichtAbgesendeteVorgaenge()
        {
            ClearError();

            try
            {
                var newStatus = NochNichtAbgesendeteVorgaenge.None(v => v.IsSelected);

                NochNichtAbgesendeteVorgaenge.ForEach(v => v.IsSelected = newStatus);
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        public void SendNochNichtAbgesendeteVorgaengeToSap()
        {
            var selektierteVorgaenge = NochNichtAbgesendeteVorgaenge.Where(v => v.IsSelected);

            if (selektierteVorgaenge.None())
            {
                RaiseError(9999, "Es sind keine Vorgänge markiert");
                return;
            }

            ExecuteSapZugriff(() =>
            {
                Z_ZLD_IMPORT_AH_WARENKORB.Init(SAP);

                var vorgaenge = AppModelMappings.Z_ZLD_IMPORT_AH_WARENKORB_GT_BAK_From_NochNichtAbgesendeterVorgang.CopyBack(selektierteVorgaenge);
                SAP.ApplyImport(vorgaenge);

                CallBapi();

                var ergListe = AppModelMappings.Z_ZLD_IMPORT_AH_WARENKORB_GT_BAK_To_NochNichtAbgesendeterVorgang.Copy(Z_ZLD_IMPORT_AH_WARENKORB.GT_BAK.GetExportList(SAP)).ToList();

                foreach (var erg in ergListe)
                {
                    var vg = NochNichtAbgesendeteVorgaenge.FirstOrDefault(v => v.SapId == erg.SapId);
                    if (vg != null)
                    {
                        if (!String.IsNullOrEmpty(erg.FehlerText))
                            vg.FehlerText = erg.FehlerText;
                        else
                            vg.FehlerText = "OK";
                    }
                }
            });
        }

        public void DeleteNochNichtAbgesendeteVorgaengeOkFromList()
        {
            ClearError();

            try
            {
                NochNichtAbgesendeteVorgaenge.RemoveAll(v => v.FehlerText == "OK");
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        #endregion
    }
}
