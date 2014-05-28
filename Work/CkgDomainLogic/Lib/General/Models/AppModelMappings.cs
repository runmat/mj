﻿using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.General.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Common

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_Land_Plz_001.GT_WEB, Land> Z_M_Land_Plz_001_GT_WEB_To_Land
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_Land_Plz_001.GT_WEB, Land>(
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

        #endregion
    }
}