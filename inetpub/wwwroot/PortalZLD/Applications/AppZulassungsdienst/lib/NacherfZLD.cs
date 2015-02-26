using System;
using System.Collections.Generic;
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

        public DataTable BestLieferanten { get; set; }
        public DataTable tblBarquittungen { get; set; }
        public DataTable AHVersandListe { get; set; }

        // Selektion
        public String SelMatnr { get; set; }
        public String SelDatum { get; set; }
        public String SelID { get; set; }
        public String SelKunde { get; set; }
        public String SelKreis { get; set; }
        public String SelLief { get; set; }
        public String SelStatus { get; set; }
        public String SelVorgang { get; set; }
        public bool SelFlieger { get; set; }
        public bool SelAnnahmeAH { get; set; }
        public bool SelSofortabrechnung { get; set; }
        public bool SelEditDurchzufVersZul { get; set; }
        public string SelGroupTourID { get; set; }
        public String SelDZVKBUR { get; set; }

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
            MatError = 0;
            MatErrorText = "";

            ExecuteSapZugriff(() =>
            {
                Vorgangsliste.Clear();

                if (SelSofortabrechnung)
                    Z_ZLD_EXPORT_SOFORT_ABRECH2.Init(SAP);
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

                CallBapi();

                if (SelSofortabrechnung)
                {
                    var sapKopfdaten = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BAK.GetExportList(SAP);
                    var sapBankdaten = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_BANK.GetExportList(SAP);
                    var sapAdressen = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_ADRS.GetExportList(SAP);
                    var sapPositionen = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_POS.GetExportList(SAP);
                    var sapKundendaten = Z_ZLD_EXPORT_SOFORT_ABRECH2.GT_EX_KUNDE.GetExportList(SAP);

                    _lstKopfdaten = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten).OrderBy(k => k.SapId).ToList();
                    _lstBankdaten = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_BANK_To_ZLDBankdaten.Copy(sapBankdaten).OrderBy(b => b.SapId).ToList();
                    _lstAdressen = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_ADRS_To_ZLDAdressdaten.Copy(sapAdressen).OrderBy(a => a.SapId).ToList();
                    _lstPositionen = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.SapId).ThenBy(p => p.PositionsNr).ToList();
                    _lstKundendaten = AppModelMappings.Z_ZLD_EXPORT_SOFORT_ABRECH2_GT_EX_KUNDE_To_Kundenname.Copy(sapKundendaten).ToList();
                }
                else
                {
                    var sapKopfdaten = Z_ZLD_EXPORT_NACHERF2.GT_EX_BAK.GetExportList(SAP);
                    var sapBankdaten = Z_ZLD_EXPORT_NACHERF2.GT_EX_BANK.GetExportList(SAP);
                    var sapAdressen = Z_ZLD_EXPORT_NACHERF2.GT_EX_ADRS.GetExportList(SAP);
                    var sapPositionen = Z_ZLD_EXPORT_NACHERF2.GT_EX_POS.GetExportList(SAP);
                    var sapKundendaten = Z_ZLD_EXPORT_NACHERF2.GT_EX_KUNDE.GetExportList(SAP);

                    _lstKopfdaten = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten).OrderBy(k => k.SapId).ToList();
                    _lstBankdaten = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_BANK_To_ZLDBankdaten.Copy(sapBankdaten).OrderBy(b => b.SapId).ToList();
                    _lstAdressen = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_ADRS_To_ZLDAdressdaten.Copy(sapAdressen).OrderBy(a => a.SapId).ToList();
                    _lstPositionen = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.SapId).ThenBy(p => p.PositionsNr).ToList();
                    _lstKundendaten = AppModelMappings.Z_ZLD_EXPORT_NACHERF2_GT_EX_KUNDE_To_Kundenname.Copy(sapKundendaten).ToList();
                }
                
                foreach (var item in _lstKopfdaten)
                {
                    var kopfdaten = item;

                    var hauptPos = _lstPositionen.FirstOrDefault(p => p.SapId == kopfdaten.SapId && p.PositionsNr == "10");

                    // Auftrag ohne Hauptpositionen ?
                    if (hauptPos == null)
                    {
                        _lstKopfdaten.Remove(kopfdaten);
                    }
                    else
                    {
                        // Material einer Versandzulassung dem durchzuführenden ZLD zugewiesen?
                        if (materialStamm.None(m => m.MaterialNr == hauptPos.MaterialNr))
                        {
                            MatError = -4444;
                            MatErrorText += String.Format("ID {0}: Material {1} nicht freigeschaltet. \r\n", kopfdaten.SapId, hauptPos.MaterialNr);
                            _lstKopfdaten.Remove(kopfdaten);
                        }
                    }
                }

                foreach (var item in _lstKopfdaten)
                {
                    var kopfdaten = item;
                    var bankdaten = _lstBankdaten.FirstOrDefault(b => b.SapId == kopfdaten.SapId);
                    var adresse = _lstAdressen.FirstOrDefault(a => a.SapId == kopfdaten.SapId);
                    var positionen = _lstPositionen.Where(p => p.SapId == kopfdaten.SapId).OrderBy(p => p.PositionsNr).ToList();

                    AddVorgangToVorgangsliste(kopfdaten, bankdaten, adresse, positionen);
                }
            });
        }

        private void AddVorgangToVorgangsliste(ZLDKopfdaten kopfdaten, ZLDBankdaten bankdaten, ZLDAdressdaten adresse, List<ZLDPosition> positionen)
        {
            var kunde = _lstKundendaten.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
            var kundenName = (kunde != null ? kunde.Name : "");

            foreach (var pos in positionen)
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
                    VkOrg = kopfdaten.VkOrg,
                    VkBur = kopfdaten.VkBur,
                    KundenNr = kopfdaten.KundenNr,
                    KundenName = kundenName,
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
                    Gebuehrenpaket = pos.Gebuehrenpaket,
                    Steuer = (steuerPos != null ? steuerPos.Preis : 0),
                    PreisKennzeichen = (kennzeichenPos != null ? kennzeichenPos.Preis : 0),
                    Wunschkennzeichen = kopfdaten.Wunschkennzeichen,
                    KennzeichenReservieren = kopfdaten.KennzeichenReservieren,
                    Zahlart_EC = kopfdaten.Zahlart_EC,
                    Zahlart_Bar = kopfdaten.Zahlart_Bar,
                    Zahlart_Rechnung = kopfdaten.Zahlart_Rechnung,
                    WebBearbeitungsStatus = kopfdaten.WebBearbeitungsStatus,
                    Landkreis = kopfdaten.Landkreis,
                    KennzeichenTeil1 = kennzTeil1,
                    KennzeichenTeil2 = kennzTeil2,
                    Bemerkung = kopfdaten.Bemerkung,
                    Flieger = kopfdaten.Flieger,
                    Nachbearbeiten = kopfdaten.Nachbearbeiten,
                    Infotext = kopfdaten.Infotext,
                    Name1 = adresse.Name1,
                    Name2 = adresse.Name2,
                    Strasse = adresse.Strasse,
                    Plz = adresse.Plz,
                    Ort = adresse.Ort,
                    Kontoinhaber = bankdaten.Kontoinhaber,
                    SWIFT = bankdaten.SWIFT,
                    IBAN = bankdaten.IBAN,
                    Vorerfassungsdatum = kopfdaten.Vorerfassungsdatum,
                    VersandzulassungDurchfuehrendesVkBur = kopfdaten.VersandzulassungDurchfuehrendesVkBur
                });
            }
        }

        public void LoadVorgang(string sapId)
        {
            ClearError();

            try
            {
                AktuellerVorgang.Kopfdaten = _lstKopfdaten.FirstOrDefault(k => k.SapId == sapId);
                AktuellerVorgang.Bankdaten = _lstBankdaten.FirstOrDefault(b => b.SapId == sapId);
                AktuellerVorgang.Adressdaten = _lstAdressen.FirstOrDefault(a => a.SapId == sapId);
                AktuellerVorgang.Positionen = _lstPositionen.Where(p => p.SapId == sapId).OrderBy(p => p.PositionsNr).ToList();

                // Änderungen aus Liste übernehmen
                var kopfL = Vorgangsliste.FirstOrDefault(vg => vg.SapId == AktuellerVorgang.Kopfdaten.SapId);
                if (kopfL != null)
                {
                    var kopfdaten = AktuellerVorgang.Kopfdaten;

                    if (kopfdaten != null)
                    {
                        kopfdaten.Kennzeichen = kopfL.KennzeichenTeil1;
                        if (!String.IsNullOrEmpty(kopfL.KennzeichenTeil2))
                            kopfdaten.Kennzeichen += "-" + kopfL.KennzeichenTeil2;

                        kopfdaten.Zahlart_EC = kopfL.Zahlart_EC;
                        kopfdaten.Zahlart_Bar = kopfL.Zahlart_Bar;
                        kopfdaten.Zahlart_Rechnung = kopfL.Zahlart_Rechnung;

                        kopfdaten.Zulassungsdatum = kopfL.Zulassungsdatum;
                    }
                }

                foreach (var item in AktuellerVorgang.Positionen)
                {
                    switch (item.WebMaterialart)
                    {
                        case "D":
                            var posL = Vorgangsliste.FirstOrDefault(vg => vg.SapId == item.SapId && vg.PositionsNr == item.PositionsNr);
                            if (posL != null)
                            {
                                item.Preis = posL.Preis;
                                item.MaterialName = posL.MaterialName;
                            }
                            break;

                        case "G":
                            var gebuehrenPos = Vorgangsliste.FirstOrDefault(vg => vg.SapId == item.SapId && vg.PositionsNr == item.UebergeordnetePosition);
                            if (gebuehrenPos != null)
                            {
                                item.Preis = gebuehrenPos.Gebuehr;
                                item.GebuehrAmt = gebuehrenPos.GebuehrAmt;
                            }
                            break;

                        case "S":
                            var steuerPos = Vorgangsliste.FirstOrDefault(vg => vg.SapId == item.SapId && vg.PositionsNr == item.UebergeordnetePosition);
                            if (steuerPos != null)
                            {
                                item.Preis = steuerPos.Steuer;
                            }
                            break;

                        case "K":
                            var kennzeichenPos = Vorgangsliste.FirstOrDefault(vg => vg.SapId == item.SapId && vg.PositionsNr == item.UebergeordnetePosition);
                            if (kennzeichenPos != null)
                            {
                                item.Preis = kennzeichenPos.PreisKennzeichen;
                            }
                            break;
                    }
                }
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
                    Vorgangsliste.Where(v => v.SapId == sapId).ToList().ForEach(v => v.WebBearbeitungsStatus = newStatus);
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
                var posListeWeb = new List<ZLDPosition>();

                var kopfdaten = AktuellerVorgang.Kopfdaten;

                var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

                var posNr = 0;

                foreach (var item in AktuellerVorgang.Positionen)
                {
                    var p = item;

                    var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == p.MaterialNr);
                    if (mat != null)
                    {
                        posNr += 10;

                        var loeschKz = p.Loeschkennzeichen;

                        if (p.WebMaterialart == "G" && String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                            loeschKz = "X";
                        else if (p.WebMaterialart == "S" && p.UebergeordnetePosition != "10")
                            loeschKz = "X";

                        var impPos = new ZLDPosition
                            {
                                SapId = p.SapId,
                                PositionsNr = p.PositionsNr,
                                UebergeordnetePosition = p.UebergeordnetePosition,
                                Loeschkennzeichen = loeschKz,
                                Menge = (p.Menge.HasValue && p.Menge > 0 ? p.Menge : 1),
                                MaterialNr = p.MaterialNr,
                                MaterialName = p.MaterialName,
                                WebMaterialart = p.WebMaterialart,
                                Preis = (p.WebMaterialart == "K" ? 0 : p.Preis),
                                GebuehrAmt = (p.WebMaterialart == "G" && !SelAnnahmeAH ? p.GebuehrAmt : 0),
                                GebuehrAmtAdd = p.GebuehrAmtAdd,
                                Gebuehrenpaket = (p.WebMaterialart == "G" && p.Gebuehrenpaket.IsTrue())
                            };

                        impPos.MaterialName = impPos.CombineBezeichnungMenge();
                        posListeWeb.Add(impPos);

                        if (p.WebMaterialart == "D")
                        {
                            // Gebühren
                            if (!String.IsNullOrEmpty(mat.GebuehrenMaterialNr)
                                && AktuellerVorgang.Positionen.None(ap => ap.SapId == p.SapId && ap.UebergeordnetePosition == p.PositionsNr && ap.WebMaterialart == "G"))
                            {
                                posNr += 10;

                                var ohneUst = (kunde != null && kunde.OhneUst);

                                posListeWeb.Add(new ZLDPosition
                                {
                                    SapId = kopfdaten.SapId,
                                    PositionsNr = posNr.ToString(),
                                    UebergeordnetePosition = p.PositionsNr,
                                    MaterialNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr),
                                    MaterialName = (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName),
                                    Menge = 1,
                                    WebMaterialart = "G"
                                });
                            }

                            // Kennzeichen
                            if ((kunde == null || !kunde.Pauschal) && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr)
                                && AktuellerVorgang.Positionen.None(ap => ap.SapId == p.SapId && ap.UebergeordnetePosition == p.PositionsNr && ap.WebMaterialart == "K"))
                            {
                                posNr += 10;

                                posListeWeb.Add(new ZLDPosition
                                {
                                    SapId = kopfdaten.SapId,
                                    PositionsNr = posNr.ToString(),
                                    UebergeordnetePosition = p.PositionsNr,
                                    MaterialNr = mat.KennzeichenMaterialNr,
                                    MaterialName = "",
                                    Menge = 1,
                                    WebMaterialart = "K"
                                });
                            }

                            // Steuern
                            if (p.PositionsNr == "10"
                                && AktuellerVorgang.Positionen.None(ap => ap.SapId == p.SapId && ap.UebergeordnetePosition == p.PositionsNr && ap.WebMaterialart == "S"))
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
                }

                Z_ZLD_PREISFINDUNG2.Init(SAP);

                var kopfListe = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_BAK_From_ZLDKopfdaten.CopyBack(new List<ZLDKopfdaten> { kopfdaten });
                SAP.ApplyImport(kopfListe);

                var posListe = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_POS_From_ZLDPosition.CopyBack(posListeWeb);
                SAP.ApplyImport(posListe);

                CallBapi();

                AktuellerVorgang.Positionen = AppModelMappings.Z_ZLD_PREISFINDUNG2_GT_POS_To_ZLDPosition.Copy(Z_ZLD_PREISFINDUNG2.GT_POS.GetExportList(SAP)).ToList();
            });
        }

        public void GetPreiseNewPositionen(List<ZLDPosition> neuePositionen, List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm, string userName)
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
                        p.MaterialName = p.CombineBezeichnungMenge();
                        posListeWeb.Add(p);

                        // Gebühren
                        if (!String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
                        {
                            posNr += 10;

                            var ohneUst = (kunde != null && kunde.OhneUst);

                            posListeWeb.Add(new ZLDPosition
                            {
                                SapId = kopfdaten.SapId,
                                PositionsNr = posNr.ToString(),
                                UebergeordnetePosition = p.PositionsNr,
                                MaterialNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr),
                                MaterialName = (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName),
                                Menge = 1,
                                WebMaterialart = "G"
                            });
                        }

                        // Kennzeichen
                        if ((kunde == null || !kunde.Pauschal) && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                        {
                            posNr += 10;

                            posListeWeb.Add(new ZLDPosition
                            {
                                SapId = kopfdaten.SapId,
                                PositionsNr = posNr.ToString(),
                                UebergeordnetePosition = p.PositionsNr,
                                MaterialNr = mat.KennzeichenMaterialNr,
                                MaterialName = "",
                                Menge = 1,
                                WebMaterialart = "K"
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
        public void DoPreisfindung(List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm, string userName)
        {
            List<ZLDVorgangUINacherfassung> liste;

            if (DataFilterActive)
            {
                liste = Vorgangsliste.Where(vg =>
                    ZLDCommon.FilterData(vg, DataFilterProperty, DataFilterValue, true)).ToList();
            }
            else
            {
                liste = Vorgangsliste;
            }

            foreach (var item in liste)
            {
                // aktuellen Vorgang laden
                LoadVorgang(item.SapId);

                // Preisfindung durchführen
                GetPreise(kundenStamm, materialStamm, userName);

                // Liste aktualisieren
                Vorgangsliste.RemoveAll(vg => vg.SapId == AktuellerVorgang.Kopfdaten.SapId);
                AddVorgangToVorgangsliste(AktuellerVorgang.Kopfdaten, AktuellerVorgang.Bankdaten, AktuellerVorgang.Adressdaten, AktuellerVorgang.Positionen);
            }
        }

        public void SaveVorgangToSap(List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm, string userName)
        {
            ExecuteSapZugriff(() =>
            {
                var posListeWeb = new List<ZLDPosition>();

                var kopfdaten = AktuellerVorgang.Kopfdaten;

                var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

                var posNr = 0;

                if (SelAnnahmeAH)
                {
                    // für "neue AH-Vorgänge" den beb_status aktualisieren
                    switch (kopfdaten.WebBearbeitungsStatus)
                    {
                        case "A":
                            kopfdaten.Bearbeitungsstatus = "A";
                            break;
                        case "L":
                            kopfdaten.Bearbeitungsstatus = "L";
                            break;
                        default:
                            kopfdaten.Bearbeitungsstatus = "1";
                            break;
                    }
                }
                else if (kopfdaten.Bearbeitungsstatus == "F" && !kopfdaten.Flieger.IsTrue())
                {
                    // Nachbearbeitete fehlgeschlagene (Flieger) wieder auf "Angenommen" setzen, wenn Flieger-Flag raus ist
                    kopfdaten.Bearbeitungsstatus = "A";
                    kopfdaten.MobilUser = "";
                }
                
                kopfdaten.Erfassungsdatum = DateTime.Now;
                kopfdaten.Erfasser = userName;

                foreach (var item in AktuellerVorgang.Positionen)
                {
                    var p = item;

                    var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == p.MaterialNr);
                    if (mat != null)
                    {
                        posNr += 10;

                        var loeschKz = p.Loeschkennzeichen;

                        if (p.WebMaterialart == "G" && String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                            loeschKz = "X";
                        else if (p.WebMaterialart == "S" && p.UebergeordnetePosition != "10")
                            loeschKz = "X";

                        var impPos = new ZLDPosition
                        {
                            SapId = p.SapId,
                            PositionsNr = p.PositionsNr,
                            UebergeordnetePosition = p.UebergeordnetePosition,
                            Loeschkennzeichen = loeschKz,
                            Menge = (p.Menge.HasValue && p.Menge > 0 ? p.Menge : 1),
                            MaterialNr = p.MaterialNr,
                            MaterialName = p.MaterialName,
                            WebMaterialart = p.WebMaterialart,
                            Preis = (p.WebMaterialart == "K" ? 0 : p.Preis),
                            GebuehrAmt = (p.WebMaterialart == "G" && !SelAnnahmeAH ? p.GebuehrAmt : 0),
                            GebuehrAmtAdd = p.GebuehrAmtAdd,
                            Gebuehrenpaket = (p.WebMaterialart == "G" && p.Gebuehrenpaket.IsTrue())
                        };

                        impPos.MaterialName = impPos.CombineBezeichnungMenge();
                        posListeWeb.Add(impPos);

                        if (p.WebMaterialart == "D")
                        {
                            // Gebühren
                            if (!String.IsNullOrEmpty(mat.GebuehrenMaterialNr)
                                && AktuellerVorgang.Positionen.None(ap => ap.SapId == p.SapId && ap.UebergeordnetePosition == p.PositionsNr && ap.WebMaterialart == "G"))
                            {
                                posNr += 10;

                                var ohneUst = (kunde != null && kunde.OhneUst);

                                posListeWeb.Add(new ZLDPosition
                                {
                                    SapId = kopfdaten.SapId,
                                    PositionsNr = posNr.ToString(),
                                    UebergeordnetePosition = p.PositionsNr,
                                    MaterialNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr),
                                    MaterialName = (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName),
                                    Menge = 1,
                                    WebMaterialart = "G"
                                });
                            }

                            // Kennzeichen
                            if ((kunde == null || !kunde.Pauschal) && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr)
                                && AktuellerVorgang.Positionen.None(ap => ap.SapId == p.SapId && ap.UebergeordnetePosition == p.PositionsNr && ap.WebMaterialart == "K"))
                            {
                                posNr += 10;

                                posListeWeb.Add(new ZLDPosition
                                {
                                    SapId = kopfdaten.SapId,
                                    PositionsNr = posNr.ToString(),
                                    UebergeordnetePosition = p.PositionsNr,
                                    MaterialNr = mat.KennzeichenMaterialNr,
                                    MaterialName = "",
                                    Menge = 1,
                                    WebMaterialart = "K"
                                });
                            }

                            // Steuern
                            if (p.PositionsNr == "10"
                                && AktuellerVorgang.Positionen.None(ap => ap.SapId == p.SapId && ap.UebergeordnetePosition == p.PositionsNr && ap.WebMaterialart == "S"))
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
                }

                Z_ZLD_SAVE_DATA2.Init(SAP);

                if (SelAnnahmeAH)
                    SAP.SetImportParameter("I_AH_ANNAHME", "X");

                if (SelSofortabrechnung)
                    SAP.SetImportParameter("I_SOFORTABRECHNUNG", "X");

                var kopfListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(new List<ZLDKopfdaten> { kopfdaten });
                SAP.ApplyImport(kopfListe);

                var bankListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(new List<ZLDBankdaten> { AktuellerVorgang.Bankdaten });
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(new List<ZLDAdressdaten> { AktuellerVorgang.Adressdaten });
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_POS_From_ZLDPosition.CopyBack(posListeWeb);
                SAP.ApplyImport(posListe);

                CallBapi();

                var fehlerListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_SAVE_DATA2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                if (fehlerListe.Any())
                {
                    RaiseError(-9999, "Beim Speichern des Vorgangs in SAP sind Fehler aufgetreten");

                    foreach (var fehler in fehlerListe)
                    {
                        var pos = AktuellerVorgang.Positionen.FirstOrDefault(p => p.SapId == fehler.SapId && p.PositionsNr == fehler.PositionsNr);

                        if (pos != null)
                            pos.FehlerText = fehler.FehlerText;
                    }
                }

                // Liste aktualisieren
                Vorgangsliste.RemoveAll(vg => vg.SapId == kopfdaten.SapId);
                AddVorgangToVorgangsliste(kopfdaten, AktuellerVorgang.Bankdaten, AktuellerVorgang.Adressdaten, AktuellerVorgang.Positionen);
            });
        }

        public void SaveVorgaengeToSap(List<Materialstammdaten> materialStamm, List<Stva> stvaStamm, string userName, bool annahmeAhSend = false)
        {
            ClearError();

            if (Vorgangsliste.None())
                return;

            ExecuteSapZugriff(() =>
            {
                var kopfListeWeb = new List<ZLDKopfdaten>();
                var bankListeWeb = new List<ZLDBankdaten>();
                var adressListeWeb = new List<ZLDAdressdaten>();
                var posListeWeb = new List<ZLDPosition>();

                foreach (var vg in Vorgangsliste)
                {
                    var tmpKopf = _lstKopfdaten.FirstOrDefault(k => k.SapId == vg.SapId);
                    if (tmpKopf != null)
                    {
                        var kreisBez = tmpKopf.KreisBezeichnung;
                        if (tmpKopf.Landkreis != vg.Landkreis)
                        {
                            var amt = stvaStamm.FirstOrDefault(s => s.Landkreis == vg.Landkreis);
                            if (amt != null)
                                kreisBez = amt.KreisBezeichnung;
                        }

                        tmpKopf.Landkreis = vg.Landkreis;
                        tmpKopf.KreisBezeichnung = kreisBez;

                        tmpKopf.Kennzeichen = vg.KennzeichenTeil1;
                        if (!String.IsNullOrEmpty(vg.KennzeichenTeil2))
                            tmpKopf.Kennzeichen += "-" + vg.KennzeichenTeil2;

                        tmpKopf.Zahlart_EC = vg.Zahlart_EC;
                        tmpKopf.Zahlart_Bar = vg.Zahlart_Bar;
                        tmpKopf.Zahlart_Rechnung = vg.Zahlart_Rechnung;
                        tmpKopf.Zulassungsdatum = vg.Zulassungsdatum;

                        if (SelAnnahmeAH && annahmeAhSend)
                        {
                            // für "neue AH-Vorgänge" den beb_status aktualisieren
                            switch (tmpKopf.WebBearbeitungsStatus)
                            {
                                case "A":
                                    tmpKopf.Bearbeitungsstatus = "A";
                                    break;
                                case "L":
                                    tmpKopf.Bearbeitungsstatus = "L";
                                    break;
                                default:
                                    tmpKopf.Bearbeitungsstatus = "1";
                                    break;
                            }
                        }
                        else if (tmpKopf.Bearbeitungsstatus == "F" && !tmpKopf.Flieger.IsTrue())
                        {
                            // Nachbearbeitete fehlgeschlagene (Flieger) wieder auf "Angenommen" setzen, wenn Flieger-Flag raus ist
                            tmpKopf.Bearbeitungsstatus = "A";
                            tmpKopf.MobilUser = "";
                        }

                        tmpKopf.Erfassungsdatum = DateTime.Now;
                        tmpKopf.Erfasser = userName;

                        kopfListeWeb.Add(tmpKopf);
                    }

                    var tmpBank = _lstBankdaten.FirstOrDefault(b => b.SapId == vg.SapId);
                    if (tmpBank != null && !String.IsNullOrEmpty(tmpBank.Kontoinhaber))
                        bankListeWeb.Add(tmpBank);

                    var tmpAdresse = _lstAdressen.FirstOrDefault(a => a.SapId == vg.SapId);
                    if (tmpAdresse != null && !String.IsNullOrEmpty(tmpAdresse.Name1))
                        adressListeWeb.Add(tmpAdresse);

                    var tmpPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.PositionsNr == vg.PositionsNr);
                    if (tmpPos != null)
                    {
                        var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == tmpPos.MaterialNr);

                        var loeschKz = tmpPos.Loeschkennzeichen;

                        if (tmpPos.WebMaterialart == "G" && mat != null && String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                            loeschKz = "X";
                        else if (tmpPos.WebMaterialart == "S" && tmpPos.UebergeordnetePosition != "10")
                            loeschKz = "X";

                        var impPos = new ZLDPosition
                        {
                            SapId = tmpPos.SapId,
                            PositionsNr = tmpPos.PositionsNr,
                            UebergeordnetePosition = tmpPos.UebergeordnetePosition,
                            Loeschkennzeichen = loeschKz,
                            Menge = (tmpPos.Menge.HasValue && tmpPos.Menge > 0 ? tmpPos.Menge : 1),
                            MaterialNr = tmpPos.MaterialNr,
                            MaterialName = tmpPos.MaterialName,
                            WebMaterialart = tmpPos.WebMaterialart,
                            Preis = (tmpPos.WebMaterialart == "K" ? 0 : tmpPos.Preis),
                            GebuehrAmt = (tmpPos.WebMaterialart == "G" && !SelAnnahmeAH ? tmpPos.GebuehrAmt : 0),
                            GebuehrAmtAdd = tmpPos.GebuehrAmtAdd,
                            Gebuehrenpaket = (tmpPos.WebMaterialart == "G" && tmpPos.Gebuehrenpaket.IsTrue())
                        };

                        impPos.MaterialName = impPos.CombineBezeichnungMenge();
                        posListeWeb.Add(impPos);
                    }

                    // bei der Hauptposition eingegebene Preise auf die entspr. Unterpositionen verteilen
                    if (vg.PositionsNr == "10")
                    {
                        var gebuehrenPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "G");

                        if (gebuehrenPos != null)
                        {
                            gebuehrenPos.Preis = vg.Gebuehr;
                            gebuehrenPos.GebuehrAmt = vg.GebuehrAmt;
                        }

                        var steuerPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "S");

                        if (steuerPos != null)
                            steuerPos.Preis = vg.Steuer;

                        var kennzeichenPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "K");

                        if (kennzeichenPos != null)
                            kennzeichenPos.Preis = vg.PreisKennzeichen;
                    }
                }

                Z_ZLD_SAVE_DATA2.Init(SAP);

                if (SelAnnahmeAH)
                    SAP.SetImportParameter("I_AH_ANNAHME", "X");

                if (SelSofortabrechnung)
                    SAP.SetImportParameter("I_SOFORTABRECHNUNG", "X");

                var kopfListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(kopfListeWeb);
                SAP.ApplyImport(kopfListe);

                var bankListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankListeWeb);
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressListeWeb);
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_IMP_POS_From_ZLDPosition.CopyBack(posListeWeb);
                SAP.ApplyImport(posListe);

                CallBapi();

                var fehlerListe = AppModelMappings.Z_ZLD_SAVE_DATA2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_SAVE_DATA2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                foreach (var vg in Vorgangsliste)
                {
                    var fehler = fehlerListe.FirstOrDefault(f => f.SapId == vg.SapId && f.PositionsNr == vg.PositionsNr);

                    if (fehler != null && !String.IsNullOrEmpty(fehler.FehlerText))
                        vg.FehlerText = fehler.FehlerText;
                    else
                        vg.FehlerText = "OK";
                }
            });
        }

        public void SendVorgaengeToSap(List<Materialstammdaten> materialStamm, string userName, string userVorname, string userNachname, bool versandZul = false)
        {
            ClearError();

            if (Vorgangsliste.None(vg => vg.WebBearbeitungsStatus == "O" || vg.WebBearbeitungsStatus == "L"))
            {
                RaiseError(9999, "Es sind keine Vorgänge mit \"O\" oder \"L\" markiert");
                return;
            }

            ExecuteSapZugriff(() =>
            {
                var kopfListeWeb = new List<ZLDKopfdaten>();
                var bankListeWeb = new List<ZLDBankdaten>();
                var adressListeWeb = new List<ZLDAdressdaten>();
                var posListeWeb = new List<ZLDPosition>();

                foreach (var vg in Vorgangsliste.Where(vg => vg.WebBearbeitungsStatus == "O" || vg.WebBearbeitungsStatus == "L"))
                {
                    var tmpKopf = _lstKopfdaten.FirstOrDefault(k => k.SapId == vg.SapId);
                    if (tmpKopf != null)
                    {
                        tmpKopf.Kennzeichen = vg.KennzeichenTeil1;
                        if (!String.IsNullOrEmpty(vg.KennzeichenTeil2))
                            tmpKopf.Kennzeichen += "-" + vg.KennzeichenTeil2;

                        tmpKopf.Zahlart_EC = vg.Zahlart_EC;
                        tmpKopf.Zahlart_Bar = vg.Zahlart_Bar;
                        tmpKopf.Zahlart_Rechnung = vg.Zahlart_Rechnung;
                        tmpKopf.Zulassungsdatum = vg.Zulassungsdatum;

                        if (!SelSofortabrechnung)
                        {
                            if (tmpKopf.Belegart == "VZ" || tmpKopf.Belegart == "VE" || tmpKopf.Belegart == "AV" || tmpKopf.Belegart == "AX")
                            {
                                tmpKopf.VersandzulassungErledigtDatum = DateTime.Now;
                                tmpKopf.VersandzulassungBearbeitungsstatus = "VD";
                            }
                        }

                        tmpKopf.Loeschkennzeichen = (vg.WebBearbeitungsStatus == "L" ? "X" : "");

                        tmpKopf.Erfassungsdatum = DateTime.Now;
                        tmpKopf.Erfasser = userName;

                        kopfListeWeb.Add(tmpKopf);
                    }

                    var tmpBank = _lstBankdaten.FirstOrDefault(b => b.SapId == vg.SapId);
                    if (tmpBank != null && !String.IsNullOrEmpty(tmpBank.Kontoinhaber))
                        bankListeWeb.Add(tmpBank);

                    var tmpAdresse = _lstAdressen.FirstOrDefault(a => a.SapId == vg.SapId);
                    if (tmpAdresse != null && !String.IsNullOrEmpty(tmpAdresse.Name1))
                        adressListeWeb.Add(tmpAdresse);

                    var tmpPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.PositionsNr == vg.PositionsNr);
                    if (tmpPos != null)
                    {
                        var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == tmpPos.MaterialNr);

                        var loeschKz = tmpPos.Loeschkennzeichen;

                        if (tmpPos.WebMaterialart == "G" && mat != null && String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                            loeschKz = "X";
                        else if (tmpPos.WebMaterialart == "S" && tmpPos.UebergeordnetePosition != "10")
                            loeschKz = "X";

                        var impPos = new ZLDPosition
                        {
                            SapId = tmpPos.SapId,
                            PositionsNr = tmpPos.PositionsNr,
                            UebergeordnetePosition = tmpPos.UebergeordnetePosition,
                            Loeschkennzeichen = loeschKz,
                            Menge = (tmpPos.Menge.HasValue && tmpPos.Menge > 0 ? tmpPos.Menge : 1),
                            MaterialNr = tmpPos.MaterialNr,
                            MaterialName = tmpPos.MaterialName,
                            WebMaterialart = tmpPos.WebMaterialart,
                            Preis = (tmpPos.WebMaterialart == "K" ? 0 : tmpPos.Preis),
                            GebuehrAmt = (tmpPos.WebMaterialart == "G" && !SelAnnahmeAH ? tmpPos.GebuehrAmt : 0),
                            GebuehrAmtAdd = tmpPos.GebuehrAmtAdd,
                            Gebuehrenpaket = (tmpPos.WebMaterialart == "G" && tmpPos.Gebuehrenpaket.IsTrue())
                        };

                        impPos.MaterialName = impPos.CombineBezeichnungMenge();
                        posListeWeb.Add(impPos);
                    }

                    // bei der Hauptposition eingegebene Preise auf die entspr. Unterpositionen verteilen
                    if (vg.PositionsNr == "10")
                    {
                        var gebuehrenPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "G");

                        if (gebuehrenPos != null)
                        {
                            gebuehrenPos.Preis = vg.Gebuehr;
                            gebuehrenPos.GebuehrAmt = vg.GebuehrAmt;
                        }

                        var steuerPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "S");

                        if (steuerPos != null)
                            steuerPos.Preis = vg.Steuer;

                        var kennzeichenPos = _lstPositionen.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "K");

                        if (kennzeichenPos != null)
                            kennzeichenPos.Preis = vg.PreisKennzeichen;
                    }
                }


                if (SelSofortabrechnung)
                {
                    Z_ZLD_IMPORT_SOFORT_ABRECH2.Init(SAP);

                    SAP.SetImportParameter("I_SOFORTABRECHNUNG", "X");

                    var kopfListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(kopfListeWeb);
                    SAP.ApplyImport(kopfListe);

                    var bankListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankListeWeb);
                    SAP.ApplyImport(bankListe);

                    var adressListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressListeWeb);
                    SAP.ApplyImport(adressListe);

                    var posListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_IMP_POS_From_ZLDPosition.CopyBack(posListeWeb);
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
                else if (versandZul)
                {
                    Z_ZLD_IMP_NACHERF_DZLD2.Init(SAP);

                    var kopfListe = AppModelMappings.Z_ZLD_IMP_NACHERF_DZLD2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(kopfListeWeb);
                    SAP.ApplyImport(kopfListe);

                    var posListe = AppModelMappings.Z_ZLD_IMP_NACHERF_DZLD2_GT_IMP_POS_From_ZLDPosition.CopyBack(posListeWeb);
                    SAP.ApplyImport(posListe);
                }
                else
                {
                    Z_ZLD_IMP_NACHERF2.Init(SAP);

                    if (SelAnnahmeAH)
                        SAP.SetImportParameter("I_AH_ANNAHME", "X");

                    var kopfListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(kopfListeWeb);
                    SAP.ApplyImport(kopfListe);

                    var bankListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankListeWeb);
                    SAP.ApplyImport(bankListe);

                    var adressListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressListeWeb);
                    SAP.ApplyImport(adressListe);

                    var posListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_IMP_POS_From_ZLDPosition.CopyBack(posListeWeb);
                    SAP.ApplyImport(posListe);
                }

                CallBapi();

                List<ZLDFehler> fehlerListe;

                if (SelSofortabrechnung)
                    fehlerListe = AppModelMappings.Z_ZLD_IMPORT_SOFORT_ABRECH2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMPORT_SOFORT_ABRECH2.GT_EX_ERRORS.GetExportList(SAP)).ToList();
                else if (versandZul)
                    fehlerListe = AppModelMappings.Z_ZLD_IMP_NACHERF_DZLD2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMP_NACHERF_DZLD2.GT_EX_ERRORS.GetExportList(SAP)).ToList();
                else
                    fehlerListe = AppModelMappings.Z_ZLD_IMP_NACHERF2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMP_NACHERF2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                foreach (var vg in Vorgangsliste)
                {
                    var fehler = fehlerListe.FirstOrDefault(f => f.SapId == vg.SapId && f.PositionsNr == vg.PositionsNr);

                    if (fehler != null && !String.IsNullOrEmpty(fehler.FehlerText))
                        vg.FehlerText = fehler.FehlerText;
                    else
                        vg.FehlerText = "OK";
                }

                if (SelSofortabrechnung)
                {
                    SofortabrechnungVerzeichnis = SAP.GetExportParameter("G_SA_PFAD");
                }
                else if (!versandZul)
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

                var sapKopfdaten = Z_ZLD_GET_ORDER2.GS_EX_BAK.GetExportList(SAP).FirstOrDefault();
                var sapBankdaten = Z_ZLD_GET_ORDER2.GT_EX_BANK.GetExportList(SAP).FirstOrDefault();
                var sapAdresse = Z_ZLD_GET_ORDER2.GT_EX_ADRS.GetExportList(SAP).FirstOrDefault();
                var sapPositionen = Z_ZLD_GET_ORDER2.GT_EX_POS.GetExportList(SAP);

                if (sapKopfdaten == null)
                {
                    RaiseError(9999, "Kein Vorgang zur ID vorhanden");
                    return;
                }

                AktuellerVorgang.Kopfdaten = AppModelMappings.Z_ZLD_GET_ORDER2_GS_EX_BAK_To_ZLDKopfdaten.Copy(sapKopfdaten);
                AktuellerVorgang.Bankdaten = AppModelMappings.Z_ZLD_GET_ORDER2_GT_EX_BANK_To_ZLDBankdaten.Copy(sapBankdaten);
                AktuellerVorgang.Adressdaten = AppModelMappings.Z_ZLD_GET_ORDER2_GT_EX_ADRS_To_ZLDAdressdaten.Copy(sapAdresse);
                AktuellerVorgang.Positionen = AppModelMappings.Z_ZLD_GET_ORDER2_GT_EX_POS_To_ZLDPosition.Copy(sapPositionen).OrderBy(p => p.PositionsNr).ToList();
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

                if (fehlerListe.Any())
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

        public void DeleteVorgaengeOkFromList()
        {
            ClearError();

            try
            {
                var idList = Vorgangsliste.GroupBy(v => v.SapId).Select(grp => grp.First().SapId);

                foreach (var item in idList)
                {
                    var id = item;

                    if (Vorgangsliste.None(v => v.SapId == id && v.FehlerText != "OK"))
                        Vorgangsliste.RemoveAll(v => v.SapId == id);
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
                var ZLD_DataContext = new ZLDTableClassesDataContext();

                var gebuehrenPos = ZLD_DataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == sapId &&
                                                                                          p.UebergeordnetePosition == posNr &&
                                                                                          p.MaterialNr == matNr &&
                                                                                          p.WebMaterialart == "G");

                return (gebuehrenPos != null && gebuehrenPos.SdRelevant.IsTrue());
            }
            catch (Exception ex)
            {
                RaiseError(-9999, ex.Message);
                return false;
            }
        }

        #endregion
    }
}
