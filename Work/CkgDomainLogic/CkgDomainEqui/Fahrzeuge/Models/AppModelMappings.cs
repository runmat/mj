using System.Collections.Generic;
// ReSharper disable RedundantUsingDirective
using CkgDomainLogic.General.Models;
// ReSharper restore RedundantUsingDirective
using GeneralTools.Models;
using SapORM.Models;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class AppModelMappings : ModelMappings
    {
        #region Load from Repository

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_ABM_LIST.ET_ABM_LIST, AbgemeldetesFahrzeug> Z_DPM_CD_ABM_LIST__ET_ABM_LIST_To_AbgemeldetesFahrzeug
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_ABM_LIST.ET_ABM_LIST, AbgemeldetesFahrzeug>(
                    new Dictionary<string, string> ()
                    ,(sap, business) =>
                        {
                            business.Status = sap.STATUS;
                            business.StatusBezeichnung = sap.STATUS_BEZ;
                            business.FIN = sap.FIN;
                            business.RueckgabeDatum = sap.DAT_ABM_REP;
                            business.Standort = sap.ORT_ABM_REP;
                            business.Art = sap.TODO_TXT;
                            business.Abteilung = sap.ABTEILUNG;
                            business.Kilometer = sap.KM;
                            business.Betriebsnummer = sap.BETRIEB;
                            business.Bemerkung = sap.BEMERKUNG_TXT;
                            business.Modell = sap.MODELL;
                            business.FIN10 = sap.FIN_10;
                            business.AbmeldeAuftragDatum = sap.DAT_ABM_AUFTR;
                            business.AbmeldeDatum = sap.EXPIRY_DATE;
                            business.Kostenstelle = sap.KOSTST;
                            business.Zielort = sap.ZIELORT;
                            business.AbteilungsLeiter = sap.ABT_LEITER_VNAME.IsNullOrEmpty() ? sap.ABT_LEITER_NAME : string.Format("{0}, {1}", sap.ABT_LEITER_NAME, sap.ABT_LEITER_VNAME);
                        }));
            }
        }

        // ReSharper disable InconsistentNaming
        static public ModelMapping<Z_DPM_CD_ABM_HIST.ET_ABM_HIST, AbmeldeHistorie> Z_DPM_CD_ABM_HIST__ET_ABM_HIST_To_AbmeldeHistorie
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_ABM_HIST.ET_ABM_HIST, AbmeldeHistorie>(
                    new Dictionary<string, string> {
                        //{ "FIN", "FIN" },
                        { "CDKFN", "AbmeldeVorgangNr" },
                        { "ERDAT", "ErfassungsDatum" },
                        { "ERNAM", "ErfassungsUser" },
                        { "DATBI", "GueltigkeitsEndeDatum" },
                        { "TODO_TXT", "GeplanteAktionen" },
                        { "BEMERKUNG_TXT", "Bemerkung" },
                    }));
            }
        }

        #endregion


        #region Save to Repository

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_DPM_CD_ABM_LIST.IT_STATUS, FahrzeugStatus> Z_DPM_CD_ABM_LIST__IT_STATUS_To_FahrzeugStatus
            // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_CD_ABM_LIST.IT_STATUS, FahrzeugStatus>(
                                                 new Dictionary<string, string>
                                                     {
                                                         {"STATUS", "ID"},
                                                     },
                                                     null,  // Init Copy
                                                     (business, sap) =>   // Init Copy Back
                                                         {
                                                             sap.STATUS = business.ID.NotNullOrEmpty().TrimStart('0').PadLeft(2, '0');
                                                         }));
            }
        }


        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_DPM_IMP_FEHLTEILETIK_01.GT_DATEN, FehlteilEtikett> Z_DPM_IMP_FEHLTEILETIK_01__GT_DATEN_To_FehlteilEtikett
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_IMP_FEHLTEILETIK_01.GT_DATEN, FehlteilEtikett>(
                                                 new Dictionary<string, string>
                                                     {
                                                         {"CHASSIS_NUM", "VIN"},

                                                         {"INHALT_1", "Content1"},
                                                         {"UEBERSCHRIFT_1", "Header1"},
                                                         
                                                         {"INHALT_2", "Content2"},
                                                         {"UEBERSCHRIFT_2", "Header2"},
                                                         
                                                         {"INHALT_3", "Content3"},
                                                         {"UEBERSCHRIFT_3", "Header3"},
                                                         
                                                         {"INHALT_4", "Content4"},
                                                         {"UEBERSCHRIFT_4", "Header4"},
                                                         
                                                         {"INHALT_5", "Content5"},
                                                         {"UEBERSCHRIFT_5", "Header5"},
                                                         
                                                         {"INHALT_6", "Content6"},
                                                         {"UEBERSCHRIFT_6", "Header6"},
                                                         
                                                         {"INHALT_7", "Content7"},
                                                         {"UEBERSCHRIFT_7", "Header7"},
                                                         
                                                         {"INHALT_8", "Content8"},
                                                         {"UEBERSCHRIFT_8", "Header8"},
                                                         
                                                         {"INHALT_9", "Content9"},
                                                         {"UEBERSCHRIFT_9", "Header9"},
                                                         
                                                         {"INHALT_10", "Content10"},
                                                         {"UEBERSCHRIFT_10", "Header10"},
                                                     },
                                                     null,  // Init Copy
                                                     (business, sap) =>   // Init Copy Back
                                                         {
                                                             sap.CHASSIS_NUM = business.VIN.NotNullOrEmpty().ToUpper();
                                                         }));
            }
        }

        // ReSharper disable InconsistentNaming
        public static ModelMapping<Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT, FehlteilEtikett> Z_DPM_DRUCK_FEHLTEILETIK_GT_ETIKETT_To_FehlteilEtikett
        // ReSharper restore InconsistentNaming
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_DRUCK_FEHLTEILETIK.GT_ETIKETT, FehlteilEtikett>(
                                                 new Dictionary<string, string>
                                                     {
                                                         {"CHASSIS_NUM", "VIN"},
                                                         {"LICENSE_NUM", "Kennzeichen"},

                                                         {"INHALT_1", "Content1"},
                                                         {"UEBERSCHRIFT_1", "Header1"},
                                                         
                                                         {"INHALT_2", "Content2"},
                                                         {"UEBERSCHRIFT_2", "Header2"},
                                                         
                                                         {"INHALT_3", "Content3"},
                                                         {"UEBERSCHRIFT_3", "Header3"},
                                                         
                                                         {"INHALT_4", "Content4"},
                                                         {"UEBERSCHRIFT_4", "Header4"},
                                                         
                                                         {"INHALT_5", "Content5"},
                                                         {"UEBERSCHRIFT_5", "Header5"},
                                                         
                                                         {"INHALT_6", "Content6"},
                                                         {"UEBERSCHRIFT_6", "Header6"},
                                                         
                                                         {"INHALT_7", "Content7"},
                                                         {"UEBERSCHRIFT_7", "Header7"},
                                                         
                                                         {"INHALT_8", "Content8"},
                                                         {"UEBERSCHRIFT_8", "Header8"},
                                                         
                                                         {"INHALT_9", "Content9"},
                                                         {"UEBERSCHRIFT_9", "Header9"},
                                                         
                                                         {"INHALT_10", "Content10"},
                                                         {"UEBERSCHRIFT_10", "Header10"},
                                                     },
                                                     null,  // Init Copy
                                                     (business, sap) =>   // Init Copy Back
                                                     {
                                                         sap.CHASSIS_NUM = business.VIN.NotNullOrEmpty().ToUpper();
                                                     }));
            }
        }

        /// <summary>
        /// Upload Fahrzeugeinsteuerung
        /// </summary>
        static public ModelMapping<Z_DPM_UPLOAD_GRUDAT_TIP_01.GT_IN, FahrzeugeinsteuerungUploadModel> Z_DPM_UPLOAD_GRUDAT_TIP_01_GT_IN_From_FahrzeugeinsteuerungUploadModel
        {
            get
            {
                return EnsureSingleton(() => new ModelMapping<Z_DPM_UPLOAD_GRUDAT_TIP_01.GT_IN, FahrzeugeinsteuerungUploadModel>(
                    new Dictionary<string, string>()
                    , null
                    , (source, destination) =>
                    {
                        destination.CHASSIS_NUM = source.Fahrgestellnummer;
                        destination.ZZREFERENZ1 = source.Flottennummer;
                    }
                ));
            }
        }

        #endregion
    }
}