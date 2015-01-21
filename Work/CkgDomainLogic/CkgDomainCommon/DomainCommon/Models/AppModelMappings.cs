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


        #region Adressen

        static public ModelMapping<Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB, Adresse> MapAdressenFromSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB, Adresse>(
                    new Dictionary<string, string> {
                        { "KUNNR", "KundenNr" },

                        { "POS_KURZTEXT", "InternalKey" },
                        { "POS_TEXT", "InternalKey2" },

                        { "NAME1", "Name1" },
                        { "NAME2", "Name2" },
                        { "STRAS", "Strasse" },
                        { "PSTLZ", "PLZ" },
                        { "ORT01", "Ort" },
                        { "TELNR", "Telefon" },
                        { "FAXNR", "Fax" },
                        { "EMAIL", "Email" },
                        { "LAND1", "Land" },
                        { "KENNUNG", "Kennung" },
                    },

                    // Init Copy  (from SAP)
                    (source, destination) =>
                    {
                        destination.ID = CreateNewID();

                        if (destination.HausNr.IsNullOrEmpty())
                        {
                            //AddressService.ApplyStreetAndHouseNo(destination);
                        }
                    }
                    ));
            }
        }

        static public ModelMapping<Z_DPM_PFLEGE_ZDAD_AUFTR_006.GT_WEB, Adresse> MapAdressenToSAP
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_PFLEGE_ZDAD_AUFTR_006.GT_WEB, Adresse>(
                    new Dictionary<string, string> {
                        { "POS_KURZTEXT", "InternalKey" },
                        { "POS_TEXT", "InternalKey2" },

                        { "NAME1", "Name1" },
                        { "NAME2", "Name2" },
                        { "STRAS", "Strasse" },
                        { "PSTLZ", "PLZ" },
                        { "ORT01", "Ort" },
                        { "TELNR", "Telefon" },
                        { "FAXNR", "Fax" },
                        { "EMAIL", "Email" },
                        { "LAND1", "Land" },
                        { "KENNUNG", "Kennung" },
                    },

                    // Init Copy  (from SAP)
                    null,

                    // Init CopyBack: (to SAP)
                    (source, destination) =>
                        {
                            //destination.STRAS = AddressService.FormatStreetAndHouseNo(source);
                        }
                    ));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE, Adresse> Z_M_PARTNER_AUS_KNVP_LESEN_AUSGABE_To_Adresse
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_M_PARTNER_AUS_KNVP_LESEN.AUSGABE, Adresse>(
                                                 new Dictionary<string, string>
                                                     {
                                                         {"KUNNR", "KundenNr"},
                                                         {"NAME1", "Name1"},
                                                         {"NAME2", "Name2"},
                                                         {"STREET", "Strasse"},
                                                         {"POST_CODE1", "PLZ"},
                                                         {"CITY1", "Ort"},
                                                         {"TEL_NUMBER", "Telefon"},
                                                         {"TELFX", "Fax"},
                                                         {"PARVW", "Kennung"},
                                                     },

                                                 // Init Copy  (from SAP)
                                                (sapModel, businessModel) =>
                                                    {
                                                        businessModel.Land = sapModel.DEFPA.NotNullOrEmpty().ToUpper() == "X" ? "DE" : "";
                                                        //if (businessModel.Land.IsNullOrEmpty())
                                                        //    businessModel.Land = "-";
                                                        businessModel.IsDefaultPartner = sapModel.DEFPA.NotNullOrEmpty().ToUpper() == "X";
                                                    },

                                                 // Init CopyBack: (to SAP)
                                                 (businessModel, sapModel) =>
                                                     {
                                                         //destination.STRAS = AddressService.FormatStreetAndHouseNo(source);
                                                     }));
            }
        }

        static public ModelMapping<Z_DPM_READ_ADRESSPOOL_01.GT_ZULAST, Adresse> Z_DPM_READ_ADRESSPOOL_01_GT_ZULAST__To__Adresse
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_READ_ADRESSPOOL_01.GT_ZULAST, Adresse>(
                    new Dictionary<string, string>(),

                    // Init Copy  (from SAP)
                    (sap, business) =>
                    {
                        business.ID = CreateNewID();

                        business.Name1 = sap.NAME1;
                        business.Name2 = sap.NAME2;
                        business.Strasse = sap.STRAS;
                        business.HausNr = "";
                        business.PLZ = sap.PSTLZ;
                        business.Ort = sap.ORT01;
                        business.Land = "DE";
                        business.Typ = sap.LIFNR;

                        business.Kennung = "ZULASSUNG";
                    }
                 ));
            }
        }

        #endregion


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
    }
}