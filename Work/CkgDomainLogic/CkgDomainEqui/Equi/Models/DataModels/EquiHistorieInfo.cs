using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorieInfo
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equipmentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        public string Referenznummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? Anlagedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingDate)]
        public DateTime? Versanddatum { get; set; }

        public string AbcKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.StorageLocation)]
        public string Lagerort
        {
            get
            {
                string erg = "";

                var bukrsName = (GetViewModel != null ? GetViewModel().LogonContext.Customer.AccountingAreaName : "DAD");

                switch (AbcKennzeichen)
                {
                    case "":
                        erg = bukrsName;
                        break;
                    case "0":
                        erg = bukrsName;
                        break;
                    case "1":
                        if ((Versanddatum == null) || (Versanddatum == DateTime.MinValue))
                        {
                            erg = "temporär angefordert";
                        }
                        else
                        {
                            erg = "temporär versendet";
                        }
                        break;
                    case "2":
                        if ((Versanddatum == null) || (Versanddatum == DateTime.MinValue))
                        {
                            erg = "endgültig angefordert";
                        }
                        else
                        {
                            erg = "endgültig versendet";
                        }
                        break;
                }

                return erg;
            }
        }

        [LocalizedDisplay(LocalizeConstants.PartnerNo)]
        public string Partnernummer { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<EquiHistorieViewModel> GetViewModel { get; set; }
    }
}
