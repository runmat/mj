using System;
using System.Collections.Generic;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using System.Data;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
	public class VorerfZLD : SapOrmBusinessBase
	{
		#region "Properties"

        public ZLDVorgangVorerfassung AktuellerVorgang { get; private set; }

        public List<ZLDVorgangUIVorerfassung> Vorgangsliste { get; private set; }

        public DataTable BestLieferanten { get; set; }
        public DataTable tblBarcodData { get; set; }
        public DataTable tblBarcodMaterial { get; set; }

        public bool IsZLD { get; set; }
        public int IDCount { get; set; }
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
        public Boolean ConfirmCPDAdress { get; set; }

		#endregion

		#region "Methods"

		public VorerfZLD(string userReferenz)
		{
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

            AktuellerVorgang = new ZLDVorgangVorerfassung(VKORG, VKBUR);
            Vorgangsliste = new List<ZLDVorgangUIVorerfassung>();
		}

        public void getDataFromBarcode()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_GET_DAD_SD_ORDER.Init(SAP, "I_VBELN", AktuellerVorgang.Kopfdaten.Barcode);

                    CallBapi();

                    tblBarcodData = SAP.GetExportTable("GS_DAD_ORDER");
                    tblBarcodMaterial = SAP.GetExportTable("GT_MAT");
                });
        }

        private void SetNewSapID()
        {
            var newSapId = "";

            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_BELNR.Init(SAP);

                    CallBapi();

                    newSapId = SAP.GetExportParameter("E_BELN").NotNullOrEmpty().TrimStart('0');
                });

            AktuellerVorgang.Kopfdaten.SapId = newSapId;
            AktuellerVorgang.Bankdaten.SapId = newSapId;
            AktuellerVorgang.Adressdaten.SapId = newSapId;
            AktuellerVorgang.Positionen.ForEach(p => p.SapId = newSapId);
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

		public void getBestLieferant()
		{
            ExecuteSapZugriff(() =>
                {
                    Z_ZLD_EXPORT_INFOPOOL.Init(SAP, "I_KREISKZ", AktuellerVorgang.Kopfdaten.Landkreis);

                    CallBapi();

                    BestLieferanten = SAP.GetExportTable("GT_EX_ZUSTLIEF");
                });
		}

        public void LoadVorgangFromSql(string sapId)
        {
            ClearError();

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == sapId);
                var tmpBank = zldDataContext.ZLDVorgangBank.FirstOrDefault(b => b.SapId == sapId);
                var tmpAdresse = zldDataContext.ZLDVorgangAdresse.FirstOrDefault(a => a.SapId == sapId);
                var tmpPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == sapId).ToList();

                AktuellerVorgang.Kopfdaten = new ModelMapping<ZLDVorgangKopf, ZLDKopfdaten>(new Dictionary<string, string>()).Copy(tmpKopf);
                AktuellerVorgang.Bankdaten = new ModelMapping<ZLDVorgangBank, ZLDBankdaten>(new Dictionary<string, string>()).Copy(tmpBank);
                AktuellerVorgang.Adressdaten = new ModelMapping<ZLDVorgangAdresse, ZLDAdressdaten>(new Dictionary<string, string>()).Copy(tmpAdresse);
                AktuellerVorgang.Positionen = new ModelMapping<ZLDVorgangPosition, ZLDPositionVorerfassung>(new Dictionary<string, string>()).Copy(tmpPositionen).ToList();
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

		public void SaveVorgangToSql(string userName)
		{
			ClearError();

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                var isNewVorgang = AktuellerVorgang.Kopfdaten.IsNewVorgang;

                if (isNewVorgang)
                {
                    SetNewSapID();

                    if (String.IsNullOrEmpty(AktuellerVorgang.Kopfdaten.SapId))
                    {
                        RaiseError(9999, "Fehler beim Erzeugen der Belegnummer!");
                        return;
                    }
                }

                var kopfdaten = AktuellerVorgang.Kopfdaten;

                kopfdaten.Vorgang = "V";
                kopfdaten.Vorerfassungsdatum = DateTime.Now;
                kopfdaten.Vorerfasser = userName;

                if (isNewVorgang)
                {
                    zldDataContext.ZLDVorgangKopf.InsertOnSubmit(new ModelMapping<ZLDVorgangKopf, ZLDKopfdaten>(new Dictionary<string, string>()).CopyBack(kopfdaten));
                    zldDataContext.ZLDVorgangBank.InsertOnSubmit(new ModelMapping<ZLDVorgangBank, ZLDBankdaten>(new Dictionary<string, string>()).CopyBack(AktuellerVorgang.Bankdaten));
                    zldDataContext.ZLDVorgangAdresse.InsertOnSubmit(new ModelMapping<ZLDVorgangAdresse, ZLDAdressdaten>(new Dictionary<string, string>()).CopyBack(AktuellerVorgang.Adressdaten));

                    foreach (var item in AktuellerVorgang.Positionen)
                    {
                        item.MaterialName = item.CombineBezeichnungMenge();

                        zldDataContext.ZLDVorgangPosition.InsertOnSubmit(new ModelMapping<ZLDVorgangPosition, ZLDPositionVorerfassung>(new Dictionary<string, string>()).CopyBack(item));
                    }
                }
                else
                {
                    var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == kopfdaten.SapId);
                    var tmpBank = zldDataContext.ZLDVorgangBank.FirstOrDefault(b => b.SapId == kopfdaten.SapId);
                    var tmpAdresse = zldDataContext.ZLDVorgangAdresse.FirstOrDefault(a => a.SapId == kopfdaten.SapId);

                    new ModelMapping<ZLDVorgangKopf, ZLDKopfdaten>(new Dictionary<string, string>()).CopyBack(kopfdaten, tmpKopf);
                    new ModelMapping<ZLDVorgangBank, ZLDBankdaten>(new Dictionary<string, string>()).CopyBack(AktuellerVorgang.Bankdaten, tmpBank);
                    new ModelMapping<ZLDVorgangAdresse, ZLDAdressdaten>(new Dictionary<string, string>()).CopyBack(AktuellerVorgang.Adressdaten, tmpAdresse);

                    var tmpVorhandenPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == kopfdaten.SapId).ToList();
                    foreach (var vorhandenePosition in tmpVorhandenPositionen)
                    {
                        var item = vorhandenePosition;

                        if (AktuellerVorgang.Positionen.None(p => p.SapId == item.SapId && p.PositionsNr == item.PositionsNr))
                            zldDataContext.ZLDVorgangPosition.DeleteOnSubmit(item);
                    }

                    foreach (var item in AktuellerVorgang.Positionen)
                    {
                        item.MaterialName = item.CombineBezeichnungMenge();

                        var tmpPosition = zldDataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == kopfdaten.SapId && p.PositionsNr == item.PositionsNr);

                        if (tmpPosition != null)
                            new ModelMapping<ZLDVorgangPosition, ZLDPositionVorerfassung>(new Dictionary<string, string>()).CopyBack(item, tmpPosition);
                        else
                            zldDataContext.ZLDVorgangPosition.InsertOnSubmit(new ModelMapping<ZLDVorgangPosition, ZLDPositionVorerfassung>(new Dictionary<string, string>()).CopyBack(item));
                    }
                }

                zldDataContext.SubmitChanges();
			}
			catch (Exception ex)
			{
                RaiseError(9999, ex.Message);
			}
		}

        public void LoadVorgaengeFromSql(List<Kundenstammdaten> kundenStamm, string userName)
        {
            ClearError();

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                Vorgangsliste.Clear();

                var ids = zldDataContext.ZLDVorgangKopf.Where(k => k.Vorgang == "V" && k.Vorerfasser == userName).Select(v => v.SapId);

                IDCount = ids.Count();

                foreach (var id in ids)
                {
                    var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == id);
                    var tmpPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == id).ToList();

                    var kopfdaten = new ModelMapping<ZLDVorgangKopf, ZLDKopfdaten>(new Dictionary<string, string>()).Copy(tmpKopf);
                    var positionen = new ModelMapping<ZLDVorgangPosition, ZLDPositionVorerfassung>(new Dictionary<string, string>()).Copy(tmpPositionen).ToList();

                    AddVorgangToVorgangsliste(kopfdaten, positionen, kundenStamm);
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        private void AddVorgangToVorgangsliste(ZLDKopfdaten kopfdaten, List<ZLDPositionVorerfassung> positionen, List<Kundenstammdaten> kundenStamm)
        {
            var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
            var kundenName = (kunde != null ? kunde.Name : "");

            foreach (var pos in positionen)
            {
                Vorgangsliste.Add(new ZLDVorgangUIVorerfassung
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
                    Kennzeichen = kopfdaten.Kennzeichen,
                    WebBearbeitungsStatus = kopfdaten.WebBearbeitungsStatus
                });
            }
        }

        public void DeleteVorgangPosition(string sapId, string posNr)
        {
            ClearError();

            try
            {
                if (posNr == "10")
                {
                    // Hauptposition -> kompletten Vorgang löschen
                    Vorgangsliste.RemoveAll(v => v.SapId == sapId);
                }
                else
                {
                    // Unterposition -> nur Unterposition löschen
                    Vorgangsliste.RemoveAll(v => v.SapId == sapId && v.PositionsNr == posNr);
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        public void SendVorgaengeToSap(List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm, string userName)
        {
            ClearError();

            if (Vorgangsliste.None())
                return;

            ExecuteSapZugriff(() =>
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                var kopfListeWeb = new List<ZLDKopfdaten>();
                var bankListeWeb = new List<ZLDBankdaten>();
                var adressListeWeb = new List<ZLDAdressdaten>();
                var posListeWeb = new List<ZLDPositionVorerfassung>();

                foreach (var vg in Vorgangsliste)
                {
                    var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == vg.SapId);
                    var tmpBank = zldDataContext.ZLDVorgangBank.FirstOrDefault(b => b.SapId == vg.SapId);
                    var tmpAdresse = zldDataContext.ZLDVorgangAdresse.FirstOrDefault(a => a.SapId == vg.SapId);
                    var tmpPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == vg.SapId).ToList();

                    var kopfdaten =
                        new ModelMapping<ZLDVorgangKopf, ZLDKopfdaten>(new Dictionary<string, string>()).Copy(tmpKopf);
                    var bankdaten =
                        new ModelMapping<ZLDVorgangBank, ZLDBankdaten>(new Dictionary<string, string>()).Copy(tmpBank);
                    var adressdaten =
                        new ModelMapping<ZLDVorgangAdresse, ZLDAdressdaten>(new Dictionary<string, string>()).Copy(
                            tmpAdresse);
                    var positionen =
                        new ModelMapping<ZLDVorgangPosition, ZLDPositionVorerfassung>(new Dictionary<string, string>())
                            .Copy(tmpPositionen).ToList();

                    var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
                    if (kunde != null)
                        kopfdaten.BarzahlungKunde = kunde.Bar;

                    kopfdaten.Erfassungsdatum = DateTime.Now;
                    kopfdaten.Erfasser = userName;

                    kopfListeWeb.Add(kopfdaten);
                    bankListeWeb.Add(bankdaten);
                    adressListeWeb.Add(adressdaten);

                    var posNr = 0;

                    foreach (var p in positionen)
                    {
                        var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == p.MaterialNr);
                        if (mat != null)
                        {
                            posNr += 10;

                            p.PositionsNr = posNr.ToString();
                            p.MaterialName = p.CombineBezeichnungMenge();
                            posListeWeb.Add(p);

                            if (p.WebMaterialart == "D")
                            {
                                // Gebühren
                                if (!String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
                                {
                                    posNr += 10;

                                    var ohneUst = (kunde != null && kunde.OhneUst);

                                    posListeWeb.Add(new ZLDPositionVorerfassung
                                    {
                                        SapId = kopfdaten.SapId,
                                        PositionsNr = posNr.ToString(),
                                        UebergeordnetePosition = p.PositionsNr,
                                        MaterialNr = (ohneUst ? mat.GebuehrenMaterialNr : mat.GebuehrenMitUstMaterialNr),
                                        MaterialName =
                                            (ohneUst ? mat.GebuehrenMaterialName : mat.GebuehrenMitUstMaterialName),
                                        Menge = 1,
                                        WebMaterialart = "G"
                                    });
                                }

                                // Kennzeichen
                                if ((kunde == null || !kunde.Pauschal) && !String.IsNullOrEmpty(mat.KennzeichenMaterialNr))
                                {
                                    posNr += 10;

                                    posListeWeb.Add(new ZLDPositionVorerfassung
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

                                    posListeWeb.Add(new ZLDPositionVorerfassung
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
                }

                Z_ZLD_IMPORT_ERFASSUNG2.Init(SAP);

                var kopfListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(kopfListeWeb);
                SAP.ApplyImport(kopfListe);

                var bankListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankListeWeb);
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressListeWeb);
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_POS_From_ZLDPositionVorerfassung.CopyBack(posListeWeb);
                SAP.ApplyImport(posListe);

                CallBapi();

                var fehlerListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMPORT_ERFASSUNG2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                foreach (var vg in Vorgangsliste)
                {
                    var fehler = fehlerListe.FirstOrDefault(f => f.SapId == vg.SapId && f.PositionsNr == vg.PositionsNr);

                    if (fehler != null && !String.IsNullOrEmpty(fehler.FehlerText))
                        vg.FehlerText = fehler.FehlerText;
                    else
                        vg.FehlerText = "OK";
                }
            });

            if (!ErrorOccured)
                DeleteVorgaengeOkFromSql();
        }

        private void DeleteVorgaengeOkFromSql()
        {
            ClearError();

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                var idList = Vorgangsliste.GroupBy(v => v.SapId).Select(grp => grp.First().SapId);

                foreach (var item in idList)
                {
                    var id = item;

                    if (Vorgangsliste.None(v => v.SapId == id && v.FehlerText != "OK"))
                    {
                        var vorgangToDel = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == id);

                        if (vorgangToDel != null)
                        {
                            zldDataContext.ZLDVorgangKopf.DeleteOnSubmit(vorgangToDel);
                            zldDataContext.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

		public void SendVersandVorgangToSap(List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm, string userName)
		{
			SetNewSapID();

            if (String.IsNullOrEmpty(AktuellerVorgang.Kopfdaten.SapId))
            {
                RaiseError(9999, "Fehler beim Erzeugen der Belegnummer!");
                return;
            }

            ExecuteSapZugriff(() =>
            {
                var kopfdaten = AktuellerVorgang.Kopfdaten;

                kopfdaten.StatusVersandzulassung = "N";

                var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
                if (kunde != null)
                    kopfdaten.BarzahlungKunde = kunde.Bar;

                kopfdaten.Zahlart_EC = true;
                kopfdaten.Zahlart_Bar = false;
                kopfdaten.Zahlart_Rechnung = false;

                if (kopfdaten.LieferantenNr.NotNullOrEmpty().TrimStart('0').Substring(0, 2) == "56" && IsZLD)
                {
                    kopfdaten.VersandzulassungDurchfuehrendesVkBur = kopfdaten.LieferantenNr.TrimStart('0').Substring(2, 4);
                }

                kopfdaten.Vorerfassungsdatum = DateTime.Now;
                kopfdaten.Vorerfasser = userName;
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

                var posListeWeb = new List<ZLDPositionVorerfassung>();

                int posNr = 0;

                foreach (var p in AktuellerVorgang.Positionen)
                {
                    var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == p.MaterialNr);
                    if (mat != null)
                    {
                        posNr += 10;

                        p.PositionsNr = posNr.ToString();
                        p.MaterialName = p.CombineBezeichnungMenge();
                        posListeWeb.Add(p);

                        if (p.WebMaterialart == "D")
                        {
                            // Gebühren
                            if (!String.IsNullOrEmpty(mat.GebuehrenMaterialNr))
                            {
                                posNr += 10;

                                var ohneUst = (kunde != null && kunde.OhneUst);

                                posListeWeb.Add(new ZLDPositionVorerfassung
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

                                posListeWeb.Add(new ZLDPositionVorerfassung
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

                                posListeWeb.Add(new ZLDPositionVorerfassung
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

                Z_ZLD_IMPORT_ERFASSUNG2.Init(SAP);

                var kopfListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(new List<ZLDKopfdaten> { kopfdaten });
                SAP.ApplyImport(kopfListe);

                var bankListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(new List<ZLDBankdaten> { AktuellerVorgang.Bankdaten });
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressListeWeb);
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_IMP_POS_From_ZLDPositionVorerfassung.CopyBack(posListeWeb);
                SAP.ApplyImport(posListe);

                CallBapi();

                var fehlerListe = AppModelMappings.Z_ZLD_IMPORT_ERFASSUNG2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMPORT_ERFASSUNG2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

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

		#endregion
    }
}
