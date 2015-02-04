// ReSharper disable RedundantUsingDirective
// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Services;
using SapORM.Contracts;
using SapORM.Models;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class AddressModelMappings : ModelMappings
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

    }
}