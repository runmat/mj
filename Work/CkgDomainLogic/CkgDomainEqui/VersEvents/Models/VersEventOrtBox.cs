using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.VersEvents.Models
{
    public class VersEventOrtBox
    {
        [Key]
        public int ID { get; set; }

        [GridHidden]
        public int VersOrtID { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        [GridHidden]
        public DateTime? LoeschDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        [GridHidden]
        public string LoeschUser { get; set; }


        [LocalizedDisplay(LocalizeConstants.BoxNo)]
        public int BoxNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.BoxName)]
        [Required]
        public string BoxName { get; set; }

        [LocalizedDisplay(LocalizeConstants.BoxType)]
        public string BoxArt { get; set; }

        [LocalizedDisplay(LocalizeConstants.InsuranceCompany)]
        public string VersicherungID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Technician)]
        public string TechnikerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.TimeGrid)]
        public int TaktungMinuten { get; set; }



        [XmlIgnore]
        public static string BoxArten
        {
            get { return "GU,Gutachten;RE,Reparatur"; }
        }

        [XmlIgnore]
        public static List<SelectItem> Versicherungen { get; set; }
    }
}
