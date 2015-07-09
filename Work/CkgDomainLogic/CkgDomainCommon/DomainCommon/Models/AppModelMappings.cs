using GeneralTools.Contracts;
// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class AppModelMappings : ModelMappings
    {
        public static int TmpID;

        public static int CreateNewID()
        {
            return ++TmpID;
        }


        #region Versandaufträge (CoC, ZBII, etc)

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_FILL_VERSAUFTR.GT_IN, VersandAuftragsAnlage> Z_DPM_FILL_VERSAUFTR__GT_IN_From_VersandAuftragsAnlage
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FILL_VERSAUFTR.GT_IN, VersandAuftragsAnlage>(
                    new Dictionary<string, string> {
                        { "ZZKUNNR_AG", "KundenNr" },
                        { "CHASSIS_NUM", "VIN" },
                        //{ "ZZBRFVERS", "BriefVersand" },  not needed here - see CopyBack section of this mapping
                        //{ "ZZSCHLVERS", "SchluesselVersand" },  not needed here - see CopyBack section of this mapping
                        { "IDNRK", "StuecklistenKomponente" },
                        { "ZZABMELD", "AbmeldeKennzeichen" },
                        { "ABCKZ", "AbcKennzeichen" },
                        { "MATNR", "MaterialNr" },
                        { "ZZANFDT", "DadAnforderungsDatum" },
                        { "ZZNAME1_ZS", "Name1" },
                        { "ZZNAME2_ZS", "Name2" },
                        //{ "ZZSTRAS_ZS", "Strasse" },  not needed here - see CopyBack section of this mapping
                        //{ "ZZHAUSNR_ZS", "HausNr" },  not needed here - see CopyBack section of this mapping
                        { "ZZPSTLZ_ZS", "PLZ" },
                        { "ZZORT01_ZS", "Ort" },
                        { "ZZLAND_ZS", "Land" },
                        { "COUNTRY_ZS", "Land" },
                        { "ERNAM", "ErfassungsUserName" },
                        { "ZZBETREFF", "Bemerkung"},
                        { "ZZVGRUND", "Versandgrund"},
                        { "ZZ_MAHNA", "Mahnverfahren"}
                    },

                    // Init Copy  (from SAP)
                    null,

                    // CopyBack (to SAP)
                    (source, destination) =>  
                    {
                        // Address street + houseNo extraction:
                        AddressService.ApplyStreetAndHouseNo(source);
                        destination.ZZSTRAS_ZS = source.Strasse;
                        destination.ZZHAUSNR_ZS = source.HausNr;

                        // stupid SAP "NumC" conversions ....
                        destination.ZZBRFVERS = source.BriefVersand.ToNumC();
                        destination.ZZSCHLVERS = source.SchluesselVersand.ToNumC();

                        destination.NUR_MIT_ZB2 = source.SchluesselKombiVersand.BoolToX();
                    }
                ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_FILL_VERSAUFTR.GT_ERR, VersandAuftragsFehler> Z_DPM_FILL_VERSAUFTR__GT_ERR_To_VersandAuftragsFehler
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_FILL_VERSAUFTR.GT_ERR, VersandAuftragsFehler>(
                    new Dictionary<string, string> {
                        { "KUNNR_AG", "KundenNr" },
                        { "CHASSIS_NUM", "VIN" },
                        { "LICENSE_NUM", "Kennzeichen" },
                        { "BEMERKUNG", "Info" },
                    },

                    // Copy  (from SAP)
                    (source, destination) =>  
                    {
                    }
                ));
            }
        }
        #endregion


        #region Brief Versand (not used yet)

        static private readonly Dictionary<string, string> MapFahrzeugeFromSapDict = new Dictionary<string, string>
            {
                {"EQUNR", "EquiNr"},
                {"CHASSIS_NUM", "FIN"},
                {"LICENSE_NUM", "Kennzeichen"},
                {"ZZSTATUS_ZUG", "IstZugelassen"},
                {"ZZSTATUS_ABG", "IstAbgemeldet"},
                {"ZZSTATUS_IABG", "IstInAbmeldung"},
                {"FEHLERTEXT", "Info"},
            };

        static public ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR, Fahrzeug> MapFahrzeugeAbrufbarFromSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_ABRUFBAR, Fahrzeug>(MapFahrzeugeFromSapDict,
                    (source, destination) =>
                    {
                        destination.IstFehlerhaft = false;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_FEHLER, Fahrzeug> MapFahrzeugeFehlerhaftFromSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_FEHLER, Fahrzeug>(MapFahrzeugeFromSapDict,
                    (source, destination) =>
                    {
                        destination.IstFehlerhaft = true;
                    }));
            }
        }

        static public ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_IN, Fahrzeug> MapFahrzeugeImportToSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UNANGEF_ALLG_01.GT_IN, Fahrzeug>(new Dictionary<string, string>
                {
                    {"CHASSIS_NUM", "FIN"},
                    {"LICENSE_NUM", "Kennzeichen"},
               }));
            }
        }

        #endregion

        #region Domänenfestwerte

        static public ModelMapping<Z_DPM_DOMAENENFESTWERTE.GT_WEB, Domaenenfestwert> Z_DPM_DOMAENENFESTWERTE_GT_WEB_To_Domaenenfestwert
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_DOMAENENFESTWERTE.GT_WEB, Domaenenfestwert>(
                    new Dictionary<string, string> {
                        { "DOMVALUE_L", "Wert" },
                        { "DDTEXT", "Beschreibung" },
                    }));
            }
        }

        #endregion


        #region Haendler Adressen


        static public ModelMapping<Z_DPM_READ_LAND_02.GT_OUT, SelectItem> Z_DPM_READ_LAND_02__GT_OUT_To_SelectItem
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_LAND_02.GT_OUT, SelectItem>(
                                                 new Dictionary<string, string>(),
                                                 (s, d) =>
                                                 {
                                                     d.Key = s.LAND1_EXT;
                                                     d.Text = string.Format("{0}, {1} ({2})", s.LAND1, s.LANDX50, s.LAND1_EXT);
                                                 }));
            }
        }

        static public ModelMapping<Z_DPM_READ_REM_VERS_VORG_01.GT_OUT, HaendlerAdresse> Z_DPM_READ_MODELID_TAB__GT_OUT_To_HaendlerAdresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_REM_VERS_VORG_01.GT_OUT, HaendlerAdresse>(
                                                 new Dictionary<string, string>(),
                                                 (s, d) =>
                                                 {
                                                     d.HaendlerNr = s.HAENDLER;
                                                     d.LaenderCode = s.LAND_CODE;

                                                     d.SchluesselAdresseVerfuegbar = s.BRIEF_SCHLUE_ADR.XToBoolInverse();

                                                     d.Name1Brief = s.NAME1_B;
                                                     d.Name2Brief = s.NAME2_B;
                                                     d.StrasseBrief = s.STRASSE_B;
                                                     d.HausNrBrief = s.HAUSNR_B;
                                                     d.PlzBrief = s.PLZ_B;
                                                     d.OrtBrief = s.ORT_B;
                                                     d.LandBrief = "DE";

                                                     d.Name1Schluessel = s.NAME1_T;
                                                     d.Name2Schluessel = s.NAME2_T;
                                                     d.StrasseSchluessel = s.STRASSE_T;
                                                     d.HausNrSchluessel = s.HAUSNR_T;
                                                     d.PlzSchluessel = s.PLZ_T;
                                                     d.OrtSchluessel = s.ORT_T;
                                                     d.LandSchluessel = "DE";

                                                     d.VersandSperre = s.VERSANDSPERRE.XToBool();
                                                     d.ClientNr = s.CLIENT_NR;
                                                     d.ClientName = s.CLIENTNAME;
                                                 }));
            }
        }

        static public ModelMapping<Z_DPM_SAVE_REM_VERS_VORG_01.GT_TAB, HaendlerAdresse> Z_DPM_SAVE_MODELID_TAB__GT_TAB_To_HaendlerAdresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_SAVE_REM_VERS_VORG_01.GT_TAB, HaendlerAdresse>(
                                                 new Dictionary<string, string>(),
                                                 null,
                                                 (s, d) =>
                                                 {
                                                     d.HAENDLER = s.HaendlerNr;
                                                     d.LAND_CODE = s.LaenderCode;

                                                     d.BRIEF_SCHLUE_ADR = s.SchluesselAdresseVerfuegbar.BoolToXInverse();

                                                     d.NAME1_B = s.Name1Brief;
                                                     d.NAME2_B = s.Name2Brief;
                                                     d.STRASSE_B = s.StrasseBrief;
                                                     d.HAUSNR_B = s.HausNrBrief;
                                                     d.PLZ_B = s.PlzBrief;
                                                     d.ORT_B = s.OrtBrief;

                                                     d.NAME1_T = s.Name1Schluessel;
                                                     d.NAME2_T = s.Name2Schluessel;
                                                     d.STRASSE_T = s.StrasseSchluessel;
                                                     d.HAUSNR_T = s.HausNrSchluessel;
                                                     d.PLZ_T = s.PlzSchluessel;
                                                     d.ORT_T = s.OrtSchluessel;

                                                     d.VERSANDSPERRE = s.VersandSperre.BoolToX();
                                                     d.CLIENT_NR = s.ClientNr;
                                                     d.CLIENTNAME = s.ClientName;
                                                 }));
            }
        }

        #endregion
    }
}