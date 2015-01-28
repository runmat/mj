using System.Collections.Generic;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Autohaus.Models
{
    public partial class AppModelMappings : ModelMappings
    {
        #region FromSap

        static public ModelMapping<Z_ZLD_AH_KUNDE_MAT.GT_DEB, Kundenstammdaten> Z_ZLD_AH_KUNDE_MAT_GT_DEB_To_Kundenstammdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_KUNDE_MAT.GT_DEB, Kundenstammdaten>(
                    new Dictionary<string, string> {
                        { "VKORG", "Verkaufsorganisation" },
                        { "VKBUR", "Verkaufsbuero" },
                        { "GRUPPE", "Kundengruppe" },
                        { "KUNNR", "Kundennummer" },
                        { "NAME1", "Name1" },
                        { "NAME2", "Name2" },
                        { "CITY1", "Ort" },
                        { "POST_CODE1", "PLZ" },
                        { "STREET", "Strasse" },
                        { "HOUSE_NUM1", "Hausnummer" },
                        { "EXTENSION1", "Erweiterung" },
                        { "ZZPAUSCHAL", "Pauschalkunde" },
                        { "OHNEUST", "OhneUmsatzsteuer" },
                        { "XCPDK", "CPDKunde" },
                        { "XCPDEIN", "CPDKundeMitEinzugsermaechtigung" },
                        { "KUNNR_LF", "Kundennummer_LBV" },
                        { "BARKUNDE", "Barkunde" },
                        { "ZULUPFLICHT", "ZulassungsunterlagenPflicht" },
                        { "REF_NAME_01", "ReferenzFeldname1" },
                        { "REF_NAME_02", "ReferenzFeldname2" },
                        { "REF_NAME_03", "ReferenzFeldname3" },
                        { "REF_NAME_04", "ReferenzFeldname4" },
                        { "HALTER", "Halter" },
                        { "DAUER_EVB", "DauerEVB" },
                    }));
            }
        }

        static public ModelMapping<Z_ZLD_AH_KUNDE_MAT.GT_MAT, Materialstammdaten> Z_ZLD_AH_KUNDE_MAT_GT_MAT_To_Materialstammdaten
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_ZLD_AH_KUNDE_MAT.GT_MAT, Materialstammdaten>(
                    new Dictionary<string, string> {
                        { "VKORG", "Verkaufsorganisation" },
                        { "VKBUR", "Verkaufsbuero" },
                        { "MATNR", "Materialnummer" },
                        { "ZUONR", "ZuordnungMenue" },
                        { "BLTYP", "Belegtyp" },
                        { "ZDEFAULT", "Default" },
                        { "MAKTX", "Bezeichnung" },
                        { "DOKZUORD", "ZuordnungDokument" },
                    }));
            }
        }

        #endregion


        #region ToSap

        #endregion
    }
}