using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using AppZulassungsdienst.lib.Models;
using System.Data;
using CKG.Base.Business;
using System.Configuration;
using GeneralTools.Models;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Kompletterfassung.
    /// </summary>
    public class KomplettZLD : SapOrmBusinessBase
    {
        #region "Properties"

        public ZLDVorgang AktuellerVorgang { get; private set; }

        public List<ZLDVorgangUIKompletterfassung> Vorgangsliste { get; private set; }

        public DataTable tblUser { get; set; }
        public DataTable tblBarcodData { get; set; }
        public DataTable tblBarcodMaterial { get; set; }
        public DataTable tblBarquittungen { get; set; }
     
        public bool ConfirmCPDAdress { get; set; }
        public bool DataFilterActive { get; set; }
        public string DataFilterProperty { get; set; }
        public string DataFilterValue { get; set; }
        public int LastPageIndex { get; set; }
        public int LastPageSize { get; set; }

        #endregion

        #region "Methods"

        public KomplettZLD(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

            AktuellerVorgang = new ZLDVorgang(VKORG, VKBUR);
            Vorgangsliste = new List<ZLDVorgangUIKompletterfassung>();
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

        public void LoadVorgangFromSql(string sapId)
        {
            ClearError();

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == sapId, new ZLDVorgangKopf());
                var tmpBank = zldDataContext.ZLDVorgangBank.FirstOrDefault(b => b.SapId == sapId, new ZLDVorgangBank());
                var tmpAdresse = zldDataContext.ZLDVorgangAdresse.FirstOrDefault(a => a.SapId == sapId, new ZLDVorgangAdresse());
                var tmpPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == sapId).ToList();

                AktuellerVorgang.Kopfdaten = ModelMapping.Copy<ZLDVorgangKopf, ZLDKopfdaten>(tmpKopf);
                AktuellerVorgang.Bankdaten = ModelMapping.Copy<ZLDVorgangBank, ZLDBankdaten>(tmpBank);
                AktuellerVorgang.Adressdaten = ModelMapping.Copy<ZLDVorgangAdresse, ZLDAdressdaten>(tmpAdresse);
                AktuellerVorgang.Positionen = ModelMapping.Copy<ZLDVorgangPosition, ZLDPosition>(tmpPositionen).ToList();

                // Änderungen aus Liste übernehmen
                var kopfL = Vorgangsliste.FirstOrDefault(vg => vg.SapId == AktuellerVorgang.Kopfdaten.SapId);
                if (kopfL != null)
                {
                    var kopfdaten = AktuellerVorgang.Kopfdaten;

                    kopfdaten.Kennzeichen = kopfL.Kennzeichen;

                    kopfdaten.Zahlart_EC = kopfL.Zahlart_EC;
                    kopfdaten.Zahlart_Bar = kopfL.Zahlart_Bar;
                    kopfdaten.Zahlart_Rechnung = kopfL.Zahlart_Rechnung;
                }

                foreach (var item in AktuellerVorgang.Positionen)
                {
                    var pos = Vorgangsliste.FirstOrDefault(vg => vg.SapId == item.SapId && vg.PositionsNr == item.PositionsNr);
                    var uebergeordnetePos = Vorgangsliste.FirstOrDefault(vg => vg.SapId == item.SapId && vg.PositionsNr == item.UebergeordnetePosition);

                    switch (item.WebMaterialart)
                    {
                        case "D":
                            if (pos != null)
                                item.Preis = pos.Preis;
                            break;

                        case "G":
                            if (uebergeordnetePos != null)
                            {
                                item.Preis = uebergeordnetePos.Gebuehr;
                                item.GebuehrAmt = uebergeordnetePos.GebuehrAmt;
                            }
                            break;

                        case "S":
                            if (uebergeordnetePos != null)
                            {
                                item.Preis = uebergeordnetePos.Steuer;
                            }
                            break;

                        case "K":
                            if (uebergeordnetePos != null)
                            {
                                item.Preis = uebergeordnetePos.PreisKennzeichen;
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

        public void SaveVorgangToSql(List<Kundenstammdaten> kundenStamm, string userName)
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

                kopfdaten.Vorgang = "K";
                kopfdaten.Vorerfassungsdatum = DateTime.Now;
                kopfdaten.Vorerfasser = userName;

                if (isNewVorgang)
                {
                    zldDataContext.ZLDVorgangKopf.InsertOnSubmit(ModelMapping.Copy<ZLDKopfdaten, ZLDVorgangKopf>(kopfdaten));
                    zldDataContext.ZLDVorgangBank.InsertOnSubmit(ModelMapping.Copy<ZLDBankdaten, ZLDVorgangBank>(AktuellerVorgang.Bankdaten));
                    zldDataContext.ZLDVorgangAdresse.InsertOnSubmit(ModelMapping.Copy<ZLDAdressdaten, ZLDVorgangAdresse>(AktuellerVorgang.Adressdaten));

                    foreach (var item in AktuellerVorgang.Positionen)
                    {
                        if (item.WebMaterialart == "D")
                            item.MaterialName = item.CombineBezeichnungMenge();

                        zldDataContext.ZLDVorgangPosition.InsertOnSubmit(ModelMapping.Copy<ZLDPosition, ZLDVorgangPosition>(item));
                    }
                }
                else
                {
                    var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == kopfdaten.SapId, new ZLDVorgangKopf());
                    var tmpBank = zldDataContext.ZLDVorgangBank.FirstOrDefault(b => b.SapId == kopfdaten.SapId, new ZLDVorgangBank());
                    var tmpAdresse = zldDataContext.ZLDVorgangAdresse.FirstOrDefault(a => a.SapId == kopfdaten.SapId, new ZLDVorgangAdresse());

                    ModelMapping.Copy(kopfdaten, tmpKopf);
                    ModelMapping.Copy(AktuellerVorgang.Bankdaten, tmpBank);
                    ModelMapping.Copy(AktuellerVorgang.Adressdaten, tmpAdresse);

                    var tmpVorhandenPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == kopfdaten.SapId).ToList();
                    foreach (var vorhandenePosition in tmpVorhandenPositionen)
                    {
                        var item = vorhandenePosition;

                        if (AktuellerVorgang.Positionen.None(p => p.SapId == item.SapId && p.PositionsNr == item.PositionsNr))
                            zldDataContext.ZLDVorgangPosition.DeleteOnSubmit(item);
                    }

                    foreach (var item in AktuellerVorgang.Positionen)
                    {
                        if (item.WebMaterialart == "D")
                            item.MaterialName = item.CombineBezeichnungMenge();

                        var tmpPosition = zldDataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == kopfdaten.SapId && p.PositionsNr == item.PositionsNr);
                        if (tmpPosition != null)
                            ModelMapping.Copy(item, tmpPosition);
                        else
                            zldDataContext.ZLDVorgangPosition.InsertOnSubmit(ModelMapping.Copy<ZLDPosition, ZLDVorgangPosition>(item));
                    }
                }

                zldDataContext.SubmitChanges();

                // Liste aktualisieren
                Vorgangsliste.RemoveAll(vg => vg.SapId == kopfdaten.SapId);
                AddVorgangToVorgangsliste(kopfdaten, AktuellerVorgang.Positionen, kundenStamm);
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

                var ids = zldDataContext.ZLDVorgangKopf.Where(k => k.Vorgang == "K" && k.Vorerfasser == userName).Select(v => v.SapId);

                foreach (var item in ids)
                {
                    var id = item;

                    var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == id, new ZLDVorgangKopf());
                    var tmpPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == id).ToList();

                    var kopfdaten = ModelMapping.Copy<ZLDVorgangKopf, ZLDKopfdaten>(tmpKopf);
                    var positionen = ModelMapping.Copy<ZLDVorgangPosition, ZLDPosition>(tmpPositionen).ToList();

                    positionen.ForEach(p => p.WebBearbeitungsStatus = (tmpKopf.WebBearbeitungsStatus == "L" ? "L" : ""));

                    AddVorgangToVorgangsliste(kopfdaten, positionen, kundenStamm);
                }
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        private void AddVorgangToVorgangsliste(ZLDKopfdaten kopfdaten, List<ZLDPosition> positionen, List<Kundenstammdaten> kundenStamm)
        {
            var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

            foreach (var pos in positionen.Where(p => p.WebMaterialart == "D"))
            {
                var gebuehrenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "G");
                var steuerPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "S");
                var kennzeichenPos = positionen.FirstOrDefault(p => p.UebergeordnetePosition == pos.PositionsNr && p.WebMaterialart == "K");

                string kennzTeil1;
                string kennzTeil2;
                ZLDCommon.KennzeichenAufteilen(kopfdaten.Kennzeichen, out kennzTeil1, out kennzTeil2);

                Vorgangsliste.Add(new ZLDVorgangUIKompletterfassung
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
                    WebBearbeitungsStatus = pos.WebBearbeitungsStatus,
                    Landkreis = kopfdaten.Landkreis,
                    KennzeichenTeil1 = kennzTeil1,
                    KennzeichenTeil2 = kennzTeil2,
                    BarzahlungKunde = kopfdaten.BarzahlungKunde
                });
            }
        }

        public void DeleteVorgangPosition(string sapId, string posNr)
        {
            ClearError();

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                if (posNr == "10")
                {
                    // Hauptposition -> Löschkennzeichen für kompletten Vorgang setzen
                    Vorgangsliste.Where(v => v.SapId == sapId).ToList().ForEach(v => v.WebBearbeitungsStatus = (v.WebBearbeitungsStatus == "L" ? "" : "L"));
                }
                else
                {
                    // Unterposition -> Unterposition löschen
                    Vorgangsliste.RemoveAll(v => v.SapId == sapId && v.PositionsNr == posNr);

                    var positionToDel = zldDataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == sapId && p.PositionsNr == posNr);
                    if (positionToDel != null)
                    {
                        zldDataContext.ZLDVorgangPosition.DeleteOnSubmit(positionToDel);
                        zldDataContext.SubmitChanges();
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

        public void GetPreise(List<Kundenstammdaten> kundenStamm, List<Materialstammdaten> materialStamm)
        {
            ExecuteSapZugriff(() =>
            {
                var posListeWeb = new List<ZLDPosition>();

                var kopfdaten = AktuellerVorgang.Kopfdaten;

                // Preisfindung erfordert eine (Dummy-)ID
                if (String.IsNullOrEmpty(kopfdaten.SapId))
                    kopfdaten.SapId = "999";

                var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);

                var posNr = 0;

                foreach (var item in AktuellerVorgang.Positionen)
                {
                    var p = item;

                    var mat = materialStamm.FirstOrDefault(m => m.MaterialNr == p.MaterialNr);

                    if (p.WebMaterialart == "D" && mat != null)
                    {
                        posNr += 10;

                        var dlPosNr = posNr.ToString();

                        posListeWeb.Add(new ZLDPosition
                        {
                            SapId = kopfdaten.SapId,
                            PositionsNr = dlPosNr,
                            UebergeordnetePosition = (posNr == 10 ? "" : "10"),
                            Menge = (p.Menge.HasValue && p.Menge > 0 ? p.Menge : 1),
                            MaterialNr = p.MaterialNr,
                            MaterialName = p.CombineBezeichnungMenge(),
                            WebMaterialart = p.WebMaterialart,
                            NullpreisErlaubt = mat.NullpreisErlaubt
                        });

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
                                UebergeordnetePosition = dlPosNr,
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
                                UebergeordnetePosition = dlPosNr,
                                MaterialNr = mat.KennzeichenMaterialNr,
                                MaterialName = "",
                                Menge = 1,
                                WebMaterialart = "K",
                                NullpreisErlaubt = (kennzeichenMat != null && kennzeichenMat.NullpreisErlaubt)
                            });
                        }

                        // Steuern
                        if (dlPosNr == "10")
                        {
                            posNr += 10;

                            posListeWeb.Add(new ZLDPosition
                            {
                                SapId = kopfdaten.SapId,
                                PositionsNr = posNr.ToString(),
                                UebergeordnetePosition = dlPosNr,
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

                // Preisfindung erfordert eine (Dummy-)ID
                if (String.IsNullOrEmpty(kopfdaten.SapId))
                    kopfdaten.SapId = "999";

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

                        if (p.WebMaterialart == "D")
                        {
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

        public void SaveVorgaengeToSql(string userName)
        {
            ClearError();

            List<ZLDVorgangUIKompletterfassung> vgList;

            if (DataFilterActive)
            {
                vgList = Vorgangsliste.Where(vg => ZLDCommon.FilterData(vg, DataFilterProperty, DataFilterValue, true)).ToList();
            }
            else
            {
                vgList = Vorgangsliste;
            }

            if (vgList.None())
                return;

            try
            {
                var zldDataContext = new ZLDTableClassesDataContext();

                foreach (var vg in vgList)
                {
                    var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == vg.SapId);
                    if (tmpKopf != null)
                    {
                        tmpKopf.Kennzeichen = vg.Kennzeichen;

                        tmpKopf.Zahlart_EC = vg.Zahlart_EC;
                        tmpKopf.Zahlart_Bar = vg.Zahlart_Bar;
                        tmpKopf.Zahlart_Rechnung = vg.Zahlart_Rechnung;

                        tmpKopf.WebBearbeitungsStatus = vg.WebBearbeitungsStatus;
                        
                        tmpKopf.Vorerfassungsdatum = DateTime.Now;
                        tmpKopf.Vorerfasser = userName;
                    }

                    var tmpPos = zldDataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == vg.SapId && p.PositionsNr == vg.PositionsNr);
                    if (tmpPos != null)
                    {
                        tmpPos.Preis = vg.Preis;

                        // eingegebene Preise auf die entspr. Unterpositionen verteilen
                        var gebuehrenPos = zldDataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "G");
                        if (gebuehrenPos != null)
                        {
                            gebuehrenPos.Preis = vg.Gebuehr;
                            gebuehrenPos.GebuehrAmt = vg.GebuehrAmt;
                        }

                        var steuerPos = zldDataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "S");
                        if (steuerPos != null)
                            steuerPos.Preis = vg.Steuer;

                        var kennzeichenPos = zldDataContext.ZLDVorgangPosition.FirstOrDefault(p => p.SapId == vg.SapId && p.UebergeordnetePosition == vg.PositionsNr && p.WebMaterialart == "K");
                        if (kennzeichenPos != null)
                            kennzeichenPos.Preis = vg.PreisKennzeichen;
                    }
                }

                zldDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                RaiseError(9999, ex.Message);
            }
        }

        public void SendVorgaengeToSap(List<Kundenstammdaten> kundenStamm, string userName)
        {
            ClearError();

            List<ZLDVorgangUIKompletterfassung> vgList;

            if (DataFilterActive)
            {
                vgList = Vorgangsliste.Where(vg => ZLDCommon.FilterData(vg, DataFilterProperty, DataFilterValue, true)).ToList();
            }
            else
            {
                vgList = Vorgangsliste;
            }

            if (vgList.None())
                return;

            ExecuteSapZugriff(() =>
            {
                Vorgangsliste.ForEach(vg => vg.FehlerText = "");

                var zldDataContext = new ZLDTableClassesDataContext();

                var kopfListeWeb = new List<ZLDKopfdaten>();
                var bankListeWeb = new List<ZLDBankdaten>();
                var adressListeWeb = new List<ZLDAdressdaten>();
                var posListeWeb = new List<ZLDPosition>();

                var idList = vgList.GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();

                foreach (var item in idList)
                {
                    var id = item;

                    var tmpKopf = zldDataContext.ZLDVorgangKopf.FirstOrDefault(k => k.SapId == id, new ZLDVorgangKopf());
                    var tmpBank = zldDataContext.ZLDVorgangBank.FirstOrDefault(b => b.SapId == id, new ZLDVorgangBank());
                    var tmpAdresse = zldDataContext.ZLDVorgangAdresse.FirstOrDefault(a => a.SapId == id, new ZLDVorgangAdresse());
                    var tmpPositionen = zldDataContext.ZLDVorgangPosition.Where(p => p.SapId == id).ToList();

                    var kopfdaten = ModelMapping.Copy<ZLDVorgangKopf, ZLDKopfdaten>(tmpKopf);
                    var bankdaten = ModelMapping.Copy<ZLDVorgangBank, ZLDBankdaten>(tmpBank);
                    var adressdaten = ModelMapping.Copy<ZLDVorgangAdresse, ZLDAdressdaten>(tmpAdresse);
                    var positionen = ModelMapping.Copy<ZLDVorgangPosition, ZLDPosition>(tmpPositionen).ToList();

                    var kunde = kundenStamm.FirstOrDefault(k => k.KundenNr == kopfdaten.KundenNr);
                    if (kunde != null)
                        adressdaten.Land = kunde.Land;

                    kopfdaten.Erfassungsdatum = DateTime.Now;
                    kopfdaten.Erfasser = userName;
                    kopfdaten.Loeschkennzeichen = (tmpKopf.WebBearbeitungsStatus == "L" ? "L" : "");

                    kopfListeWeb.Add(kopfdaten);

                    if (!String.IsNullOrEmpty(bankdaten.Kontoinhaber))
                    {
                        if (String.IsNullOrEmpty(bankdaten.Partnerrolle)) bankdaten.Partnerrolle = "AG";
                        bankListeWeb.Add(bankdaten);
                    }

                    if (!String.IsNullOrEmpty(adressdaten.Name1))
                    {
                        adressdaten.KundenNr = kopfdaten.KundenNr;
                        if (String.IsNullOrEmpty(adressdaten.Partnerrolle)) adressdaten.Partnerrolle = "AG";
                        adressListeWeb.Add(adressdaten);
                    }

                    positionen.RemoveAll(p => p.WebMaterialart == "S" && (p.UebergeordnetePosition != "10" || !p.Preis.HasValue || p.Preis == 0));
                    positionen.RemoveAll(p => p.WebMaterialart == "K" && (!p.Preis.HasValue || p.Preis == 0));
                    positionen.Where(p => p.WebMaterialart == "D").ToList().ForEach(p => p.MaterialName = p.CombineBezeichnungMenge());
                    positionen.ForEach(p => p.WebBearbeitungsStatus = "");
                    if (kopfdaten.Loeschkennzeichen == "L")
                        positionen.ForEach(p => p.Loeschkennzeichen = "L");

                    posListeWeb.AddRange(positionen);
                }

                Z_ZLD_IMP_KOMPER2.Init(SAP);

                var kopfListe = AppModelMappings.Z_ZLD_IMP_KOMPER2_GT_IMP_BAK_From_ZLDKopfdaten.CopyBack(kopfListeWeb);
                SAP.ApplyImport(kopfListe);

                var bankListe = AppModelMappings.Z_ZLD_IMP_KOMPER2_GT_IMP_BANK_From_ZLDBankdaten.CopyBack(bankListeWeb);
                SAP.ApplyImport(bankListe);

                var adressListe = AppModelMappings.Z_ZLD_IMP_KOMPER2_GT_IMP_ADRS_From_ZLDAdressdaten.CopyBack(adressListeWeb);
                SAP.ApplyImport(adressListe);

                var posListe = AppModelMappings.Z_ZLD_IMP_KOMPER2_GT_IMP_POS_From_ZLDPosition.CopyBack(posListeWeb);
                SAP.ApplyImport(posListe);

                CallBapi();

                // sind in den Aufträgen Barkunden dabei kommen aus SAP Pfade 
                // zu den Barquittungen in diese Tabelle
                tblBarquittungen = SAP.GetExportTable("GT_BARQ");

                var fehlerListe = AppModelMappings.Z_ZLD_IMP_KOMPER2_GT_EX_ERRORS_To_ZLDFehler.Copy(Z_ZLD_IMP_KOMPER2.GT_EX_ERRORS.GetExportList(SAP)).ToList();

                foreach (var vg in Vorgangsliste.Where(vg => idList.Contains(vg.SapId)))
                {
                    var fehler = fehlerListe.FirstOrDefault(f => f.SapId == vg.SapId && (String.IsNullOrEmpty(f.PositionsNr) || f.PositionsNr == vg.PositionsNr) && !String.IsNullOrEmpty(f.FehlerText));

                    if (fehler != null && fehlerListe.None(f => f.SapId == fehler.SapId && f.FehlerText.StartsWith("SD-Auftrag ist bereits angelegt")))
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

        public void DeleteVorgaengeOkFromList()
        {
            ClearError();

            try
            {
                var idList = Vorgangsliste.GroupBy(v => v.SapId).Select(grp => grp.First().SapId).ToList();

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

        /// <summary>
        /// Benutzer der gleichen Filiale laden, die nicht angemeldet sind
        /// </summary>
        public void LadeBenutzer(string userName, string userReferenz)
        {
            ClearError();

            var connection = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]
                };
            
            try
            {
                tblUser = new DataTable();

                var command = new SqlCommand();
                var adapter = new SqlDataAdapter();

                command.CommandText = "SELECT Username FROM dbo.WebUser  " +
                                      "WHERE Reference = @Reference AND LoggedOn = 0 AND NOT Username = @Username " +
                                      " ORDER BY dbo.WebUser.Username";

                command.Parameters.AddWithValue("@Reference", userReferenz);
                command.Parameters.AddWithValue("@Username", userName);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                adapter.SelectCommand = command; 
                adapter.Fill(tblUser);
            }
            catch (Exception ex)
            {
                RaiseError(9999, "Fehler beim Laden der Benutzerliste: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Vor dem ziehen der Daten von anderen Benutzern , prüfen 
        /// ob diese angemeldet sind
        /// </summary>
        public string CheckBenutzerOnline(string userName)
        {
            ClearError();
            
            var connection = new SqlConnection
                {
                    ConnectionString = ConfigurationManager.AppSettings["Connectionstring"]
                };
            var abgemeldet = "";
            
            try
            {
                var command = new SqlCommand
                    {
                        CommandText = "SELECT LoggedOn FROM dbo.WebUser  " +
                                      "WHERE UserName = @UserName"
                    };

                command.Parameters.AddWithValue("@UserName", userName);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                abgemeldet = command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                RaiseError(9999,"Fehler beim überprüfen des Benutzers: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return abgemeldet;
        }

        #endregion
    }
}
  