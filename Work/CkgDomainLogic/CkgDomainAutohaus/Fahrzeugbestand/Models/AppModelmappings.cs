// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Fahrzeugbestand.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_IMP_AUFTRDAT_007.GT_WEB, Adresse> Z_M_IMP_AUFTRDAT_007_GT_WEB_To_Adresse
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_IMP_AUFTRDAT_007.GT_WEB, Adresse>(
                    new Dictionary<string, string> {
                        { "MANDT", "Mandant" },
                        { "KUNNR", "KundenNr" },
                        { "NAME1", "Name1" },
                        { "NAME2", "Ansprechpartner" },
                        { "STRAS", "Strasse" },
                        { "PSTLZ", "PLZ" },
                        { "ORT01", "Ort" },
                        { "TELNR", "Telefon" },
                        { "EMAIL", "Email" },
                        { "FAXNR", "Fax" },
                        { "KENNUNG", "SubTyp" },
                    }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE, Adresse> Z_M_PARTNER_AUS_KNVP_LESEN_AUSGABE_To_Adresse
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE, Adresse>(
                    new Dictionary<string, string> {
                        { "KUNNR", "KundenNr" },
                        { "NAME1", "Name1" },
                        { "NAME2", "Ansprechpartner" },
                        //{ "NICK_NAME", "NickName" },
                        { "STREET", "Strasse" },
                        { "HOUSE_NUM1", "HausNr" },
                        { "POST_CODE1", "PLZ" },
                        { "CITY1", "Ort" },
                        { "TEL_NUMBER", "Telefon" },
                        { "TELFX", "Fax" },
                        { "PARVW", "SubTyp" },
                    }));
            }
        }

        #endregion
   }
}