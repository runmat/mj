using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.General.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Common

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_LAND_PLZ_001.GT_WEB, Land> Z_M_Land_Plz_001_GT_WEB_To_Land
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_LAND_PLZ_001.GT_WEB, Land>(
                    new Dictionary<string, string> {
                        { "LAND1", "ID" },
                        { "LANDX", "Name" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, VersandOption> Z_DPM_READ_LV_001__GT_OUT_DL_To_VersandOption
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, VersandOption>(
                    new Dictionary<string, string> {
                        { "ASNUM", "ID" },
                        { "ASKTX", "Name" },
                        { "EAN11", "MaterialCode" },
                    },
                    (source, destination) =>
                    {
                        destination.IstEndgueltigerVersand = (source.EXTGROUP.NotNullOrEmpty() == "2");
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, ZulassungsOption> Z_DPM_READ_LV_001__GT_OUT_DL_To_ZulassungsOption
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, ZulassungsOption>(
                    new Dictionary<string, string> {
                        { "EXTGROUP", "ID" },
                        { "KTEXT1_H1", "Name" },
                        { "ASNUM", "ZulassungsCode" },
                    },
                    (source, destination) =>
                    {
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, ZulassungsDienstleistung> Z_DPM_READ_LV_001__GT_OUT_DL_To_ZulassungsDienstleistung
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_LV_001.GT_OUT_DL, ZulassungsDienstleistung>(
                    new Dictionary<string, string> {
                        { "ASNUM", "ID" },
                        { "ASKTX", "Name" },
                        { "EAN11", "Code" },
                    },
                    (source, destination) =>
                    {
                        destination.IstGewaehlt = source.VW_AG.IsNotNullOrEmpty();
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_ABM_STATUS_TXT.ET_STATUS, FahrzeugStatus> Z_DPM_CD_ABM_STATUS_TXT__ET_STATUS_To_FahrzeugStatus
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_ABM_STATUS_TXT.ET_STATUS, FahrzeugStatus>(
                    new Dictionary<string, string> {
                        { "DOMVALUE_L", "ID" },
                        { "DDTEXT", "Name" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_READ_VERS_GRUND_KUN_01.GT_OUT, VersandGrund> Z_DPM_READ_VERS_GRUND_KUN_01_GT_OUT_To_VersandGrund
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_VERS_GRUND_KUN_01.GT_OUT, VersandGrund>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Code = s.ZZVGRUND;
                        d.Bezeichnung = s.VGRUND_TEXT;
                        d.IstEndgueltigerVersand = (s.ABCKZ == "2");
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB, KundeAusHierarchie> Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE_GT_DEB_To_KundeAusHierarchie
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_KUNDEN_ZUR_HIERARCHIE.GT_DEB, KundeAusHierarchie>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.Barkunde = s.BARKUNDE.XToBool();
                        d.CpdMitEinzugsermaechtigung = s.XCPDEIN.XToBool();
                        d.Cpdkunde = s.XCPDK.XToBool();
                        d.HausNr = s.HOUSE_NUM1;
                        d.KundenNr = s.KUNNR;
                        d.Name1 = s.NAME1;
                        d.Name2 = s.NAME2;
                        d.OhneUmsatzsteuer = s.OHNEUST.XToBool();
                        d.Ort = s.CITY1;
                        d.Pauschalkunde = s.ZZPAUSCHAL.XToBool();
                        d.Plz = s.POST_CODE1;
                        d.Strasse = s.STREET;
                        d.VkBur = s.VKBUR;
                        d.VkOrg = s.VKORG;
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_HERSTELLERGROUP.T_HERST, Hersteller> Z_M_HERSTELLERGROUP_T_HERST_To_Hersteller
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_HERSTELLERGROUP.T_HERST, Hersteller>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                        {
                            d.Code = s.HERST_GROUP;
                            d.Name = s.HERST_T;
                        }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_AHP_READ_VERSUNTERNEHMEN.GT_OUT, EvbInfo> Z_AHP_READ_VERSUNTERNEHMEN_GT_OUT_To_EvbInfo
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_AHP_READ_VERSUNTERNEHMEN.GT_OUT, EvbInfo>(
                    new Dictionary<string, string>()
                    , (s, d) =>
                    {
                        d.EvbNr = s.EVB2;
                        d.Fax = s.FAX_NUMBER;
                        d.HausNr = s.HOUSE_NUM1;
                        d.Land = s.COUNTRY;
                        d.Ort = s.CITY1;
                        d.Plz = s.POST_CODE1;
                        d.Strasse = s.STREET;
                        d.Telefon = s.TEL_NUMBER;
                        d.Versicherung = s.NAME;
                        d.VsuNr = s.VSU_NR;
                    }));
            }
        }

        #endregion
    }
}