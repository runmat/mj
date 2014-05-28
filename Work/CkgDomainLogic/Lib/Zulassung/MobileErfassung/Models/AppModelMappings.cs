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
                    new Dictionary<string, string> {
                        { "KREISKZ", "KurzBez" },
                        { "KREISBEZ", "Bezeichnung" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_STAMMD.GT_MAT, Dienstleistung> Z_ZLD_MOB_STAMMD_GT_MAT_To_Dienstleistung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_STAMMD.GT_MAT, Dienstleistung>(
                    new Dictionary<string, string> {
                        { "MATNR", "Id" },
                        { "MAKTX", "Bezeichnung" },
                        { "GEBMAT", "GebuehrenMaterial" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VGANZ, AmtVorgaenge> Z_ZLD_MOB_USER_GET_VG_GT_VGANZ_To_AmtVorgaenge
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VGANZ, AmtVorgaenge>(
                    new Dictionary<string, string> {
                        { "AMT", "KurzBez" },
                        { "KREISBEZ", "Bezeichnung" },
                        { "VG_ANZ", "AnzVorgaenge" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_KOPF, Vorgang> Z_ZLD_MOB_USER_GET_VG_GT_VG_KOPF_To_Vorgang
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_KOPF, Vorgang>(
                    new Dictionary<string, string> {
                        { "BLTYP", "BlTyp" },
                        { "ZULBELN", "Id" },
                        { "VKORG", "VkOrg" },
                        { "VKBUR", "VkBuero" },
                        { "KUNNR", "Kunnr" },
                        { "KUNDENNAME", "Kunname" },
                        { "REFERENZ1", "Referenz1" },
                        { "REFERENZ2", "Referenz2" },
                        { "KREISKZ", "Amt" },
                        { "ZZZLDAT", "ZulDat" },
                        { "ZZKENN", "Kennzeichen" },
                        { "BEB_STATUS", "Status" },
                        { "EC_JN", "ZahlartEC" },
                        { "BAR_JN", "ZahlartBar" },
                        { "RE_JN", "ZahlartRE" },
                        { "INFO_TEXT", "Bemerkung" },
                        { "NACHBEARBEITEN", "Nachbearbeiten" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_POS, VorgangPosition> Z_ZLD_MOB_USER_GET_VG_GT_VG_POS_To_VorgangPosition
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_GET_VG.GT_VG_POS, VorgangPosition>(
                    new Dictionary<string, string> {
                        { "ZULBELN", "KopfId" },
                        { "ZULPOSNR", "PosNr" },
                        { "MATNR", "DienstleistungId" },
                        { "MAKTX", "DienstleistungBez" },
                        { "GEB_AMT", "Gebuehr" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS, VorgangStatus> Z_ZLD_MOB_CHECK_BEB_STATUS_GT_BEB_STATUS_To_VorgangStatus
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_CHECK_BEB_STATUS.GT_BEB_STATUS, VorgangStatus>(
                    new Dictionary<string, string> {
                        { "ZULBELN", "Id" },
                        { "BEB_STATUS", "Status" },
                    }));
            }
        }

        #endregion


        #region Save to Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_KOPF, Vorgang> Z_ZLD_MOB_USER_PUT_VG_GT_VG_KOPF_To_Vorgang
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_KOPF, Vorgang>(
                    new Dictionary<string, string> {
                        { "BLTYP", "BlTyp" },
                        { "ZULBELN", "Id" },
                        { "VKORG", "VkOrg" },
                        { "VKBUR", "VkBuero" },
                        { "KUNNR", "Kunnr" },
                        { "KUNDENNAME", "Kunname" },
                        { "REFERENZ1", "Referenz1" },
                        { "REFERENZ2", "Referenz2" },
                        { "KREISKZ", "Amt" },
                        { "ZZZLDAT", "ZulDat" },
                        { "ZZKENN", "Kennzeichen" },
                        { "BEB_STATUS", "Status" },
                        { "EC_JN", "ZahlartEC" },
                        { "BAR_JN", "ZahlartBar" },
                        { "RE_JN", "ZahlartRE" },
                        { "INFO_TEXT", "Bemerkung" },
                        { "NACHBEARBEITEN", "Nachbearbeiten" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_POS, VorgangPosition> Z_ZLD_MOB_USER_PUT_VG_GT_VG_POS_To_VorgangPosition
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_MOB_USER_PUT_VG.GT_VG_POS, VorgangPosition>(
                    new Dictionary<string, string> {
                        { "ZULBELN", "KopfId" },
                        { "ZULPOSNR", "PosNr" },
                        { "MATNR", "DienstleistungId" },
                        { "MAKTX", "DienstleistungBez" },
                        { "GEB_AMT", "Gebuehr" },
                    }));
            }
        }

        #endregion
    }
}