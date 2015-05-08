using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_STAMMD.GT_KREISKZ, Amt> Z_ZLD_MOB_STAMMD_GT_KREISKZ_To_Amt
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_STAMMD.GT_KREISKZ, Amt>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Bezeichnung = s.KREISBEZ;
                        d.KurzBez = s.KREISKZ;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_STAMMD.GT_MAT, Dienstleistung> Z_ZLD_MOB_STAMMD_GT_MAT_To_Dienstleistung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_STAMMD.GT_MAT, Dienstleistung>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Bezeichnung = s.MAKTX;
                        d.GebuehrenMaterial = s.GEBMAT;
                        d.Id = s.MATNR;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VGANZ, AmtVorgaenge> Z_ZLD_MOB_USER_GET_VG_GT_VGANZ_To_AmtVorgaenge
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VGANZ, AmtVorgaenge>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.AnzVorgaenge = s.VG_ANZ;
                        d.Bezeichnung = s.KREISBEZ;
                        d.KurzBez = s.AMT;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_KOPF, Vorgang> Z_ZLD_MOB_USER_GET_VG_GT_VG_KOPF_To_Vorgang
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_KOPF, Vorgang>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Amt = s.KREISKZ;
                        d.Bemerkung = s.BEMERKUNG;
                        d.BlTyp = s.BLTYP;
                        d.DurchfVkBuero = s.VZD_VKBUR;
                        d.Id = s.ZULBELN;
                        d.Infotext = s.INFO_TEXT;
                        d.Kennzeichen = s.ZZKENN;
                        d.KennzeichenAnzahl = s.KENNZANZ;
                        d.KennzeichenGroesse = s.KENNZFORM;
                        d.Kunname = s.KUNDENNAME;
                        d.Kunnr = s.KUNNR;
                        d.Nachbearbeiten = s.NACHBEARBEITEN.XToBool();
                        d.NurEinKennzeichen = s.EINKENN_JN.XToBool();
                        d.Referenz1 = s.REFERENZ1;
                        d.Referenz2 = s.REFERENZ2;
                        d.Reserviert = s.RESERVKENN_JN.XToBool();
                        d.Status = s.BEB_STATUS;
                        d.TelefonNrDurchwahl = s.TEL_EXTENS;
                        d.TelefonNrVorwahl = s.TEL_NUMBER;
                        d.VkBuero = s.VKBUR;
                        d.VkOrg = s.VKORG;
                        d.VorerfasserName = s.VE_ERNAM;
                        d.Wunschkennzeichen = s.WUNSCHKENN_JN.XToBool();
                        d.ZahlartBar = s.BAR_JN.XToBool();
                        d.ZahlartEC = s.EC_JN.XToBool();
                        d.ZahlartRE = s.RE_JN.XToBool();
                        d.ZulDat = s.ZZZLDAT;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_POS, VorgangPosition> Z_ZLD_MOB_USER_GET_VG_GT_VG_POS_To_VorgangPosition
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_POS, VorgangPosition>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.DienstleistungBez = s.MAKTX;
                        d.DienstleistungId = s.MATNR;
                        d.Gebuehr = s.GEB_AMT;
                        d.KopfId = s.ZULBELN;
                        d.PosNr = s.ZULPOSNR;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS, VorgangStatus> Z_ZLD_MOB_CHECK_BEB_STATUS_GT_BEB_STATUS_To_VorgangStatus
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS, VorgangStatus>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Id = s.ZULBELN;
                        d.Status = s.BEB_STATUS;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_KUNDE, Kunde> Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_KUNDE_To_Kunde
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_KUNDE, Kunde>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.KundenNr = s.KUNNR;
                        d.Name1 = s.NAME1;
                        d.Namenserweiterung = s.EXTENSION1;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_MATERIAL, Dienstleistung> Z_ZLD_EXPORT_KUNDE_MAT_GT_EX_MATERIAL_To_Dienstleistung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_KUNDE_MAT.GT_EX_MATERIAL, Dienstleistung>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Bezeichnung = s.MAKTX;
                        d.GebuehrenMaterial = s.GEBMAT;
                        d.Id = s.MATNR;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL, Amt> Z_ZLD_EXPORT_ZULSTEL_GT_EX_ZULSTELL_To_Amt
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_EXPORT_ZULSTEL.GT_EX_ZULSTELL, Amt>(
                    new Dictionary<string, string>(),
                    (s, d) =>
                    {
                        d.Bezeichnung = s.KREISBEZ;
                        d.KurzBez = s.KREISKZ;
                    }
                ));
            }
        }

        #endregion


        #region Save to Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_KOPF, Vorgang> Z_ZLD_MOB_USER_PUT_VG_GT_VG_KOPF_From_Vorgang
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_KOPF, Vorgang>(
                    new Dictionary<string, string>(),
                    null,
                    (s, d) =>
                    {
                        d.BAR_JN = s.ZahlartBar.BoolToX();
                        d.BEB_STATUS = s.Status;
                        d.BEMERKUNG = s.Bemerkung;
                        d.BLTYP = s.BlTyp;
                        d.EC_JN = s.ZahlartEC.BoolToX();
                        d.EINKENN_JN = s.NurEinKennzeichen.BoolToX();
                        d.INFO_TEXT = s.Infotext;
                        d.KENNZANZ = s.KennzeichenAnzahl;
                        d.KENNZFORM = s.KennzeichenGroesse;
                        d.KREISKZ = s.Amt;
                        d.KUNDENNAME = s.Kunname;
                        d.KUNNR = s.Kunnr;
                        d.NACHBEARBEITEN = s.Nachbearbeiten.BoolToX();
                        d.REFERENZ1 = s.Referenz1;
                        d.REFERENZ2 = s.Referenz2;
                        d.RESERVKENN_JN = s.Reserviert.BoolToX();
                        d.RE_JN = s.ZahlartRE.BoolToX();
                        d.TEL_EXTENS = s.TelefonNrDurchwahl;
                        d.TEL_NUMBER = s.TelefonNrVorwahl;
                        d.VE_ERNAM = s.VorerfasserName;
                        d.VKBUR = s.VkBuero;
                        d.VKORG = s.VkOrg;
                        d.VZD_VKBUR = s.DurchfVkBuero;
                        d.WUNSCHKENN_JN = s.Wunschkennzeichen.BoolToX();
                        d.ZULBELN = s.Id;
                        d.ZZKENN = s.Kennzeichen;
                        d.ZZZLDAT = s.ZulDat;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_POS, VorgangPosition> Z_ZLD_MOB_USER_PUT_VG_GT_VG_POS_From_VorgangPosition
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_POS, VorgangPosition>(
                    new Dictionary<string, string>(),
                    null,
                    (s, d) =>
                    {
                        d.GEB_AMT = s.Gebuehr;
                        d.MAKTX = s.DienstleistungBez;
                        d.MATNR = s.DienstleistungId;
                        d.ZULBELN = s.KopfId;
                        d.ZULPOSNR = s.PosNr;
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS, VorgangStatus> Z_ZLD_MOB_CHECK_BEB_STATUS_GT_BEB_STATUS_From_VorgangStatus
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS, VorgangStatus>(
                    new Dictionary<string, string>(),
                    null,
                    (s, d) =>
                    {
                        d.ZULBELN = s.Id;
                    }
                ));
            }
        }

        #endregion
    }
}