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

                    // Init Copy  (from SAP)
                    null,

                    // CopyBack (to SAP)
                    (source, destination) =>
                    {
                        destination.ZZKUNNR_AG = source.KundenNr;
                        destination.CHASSIS_NUM = source.VIN;
                        destination.IDNRK = source.StuecklistenKomponente;
                        destination.ZZABMELD = source.AbmeldeKennzeichen.BoolToX();
                        destination.ABCKZ = source.AbcKennzeichen;
                        destination.MATNR = source.MaterialNr;
                        destination.ZZANFDT = source.DadAnforderungsDatum;
                        destination.ZZVGRUND = source.Versandgrund.CropExactly(3);
                        destination.ZZ_MAHNA = source.Mahnverfahren;
                        destination.ZZPLFOR = source.PicklistenFormular;

                        destination.ZZNAME1_ZS = source.Name1.CropExactly(40);
                        destination.ZZNAME2_ZS = source.Name2.CropExactly(40);
                        destination.ZZNAME3_ZS = source.Ansprechpartner.CropExactly(40);

                        destination.ZZPSTLZ_ZS = source.PLZ.CropExactly(10);
                        destination.ZZLAND_ZS = source.Land.CropExactly(3);
                        destination.COUNTRY_ZS = source.Land.CropExactly(3);
                        destination.ZZORT01_ZS = source.Ort.CropExactly(40);

                        destination.ERNAM = source.ErfassungsUserName.CropExactly(12);
                        destination.ZZBETREFF = source.Bemerkung.CropExactly(60);

                        destination.LIZNR = source.Lizenz.CropExactly(20);

                        // Address street + houseNo extraction:
                        AddressService.ApplyStreetAndHouseNo(source);
                        if (source.HausNr.NotNullOrEmpty().Length < 10)
                        {
                            destination.ZZSTRAS_ZS = source.Strasse.CropExactly(60);
                            destination.ZZHAUSNR_ZS = source.HausNr.CropExactly(10);
                        }
                        else
                        {
                            destination.ZZSTRAS_ZS = string.Format("{0} {1}", source.Strasse, source.HausNr).CropExactly(60);
                            destination.ZZHAUSNR_ZS = "";
                        }

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


        static private readonly Dictionary<string, string> MapFahrzeugeFromSapDict = new Dictionary<string, string>
            {
                {"EQUNR", "EquiNr"},
                {"CHASSIS_NUM", "FIN"},
                {"LICENSE_NUM", "Kennzeichen"},
                {"ZZSTATUS_ZUG", "IstZugelassen"},
                {"ZZSTATUS_ABG", "IstAbgemeldet"},
                {"ZZSTATUS_IABG", "IstInAbmeldung"},
                {"FEHLERTEXT", "Info"},
                {"LIZNR", "VertragsNr"},
                {"TIDNR", "BriefNr"},
                {"ZZREFERENZ1", "Ref1"},
                {"ZZREFERENZ2", "Ref2"},
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
                    {"LIZNR", "VertragsNr"},
                    {"TIDNR", "BriefNr"},
                    {"ZZREFERENZ1", "Ref1"},
                    {"ZZREFERENZ2", "Ref2"},
               }));
            }
        }

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