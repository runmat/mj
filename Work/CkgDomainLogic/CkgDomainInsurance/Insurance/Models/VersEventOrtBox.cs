using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Insurance.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Web;

namespace CkgDomainLogic.Insurance.Models
{
    public class VersEventOrtBox
    {
        [Key]
        [SelectListKey]
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
        [SelectListText]
        public string BoxName { get; set; }

        [LocalizedDisplay(LocalizeConstants.BoxType)]
        [GridHidden]
        public string BoxArt { get; set; }

        [LocalizedDisplay(LocalizeConstants.BoxType)]
        [NotMapped]
        public string BoxArtAsText
        {
            get
            {
                var boxArtEntity = BoxArten.ToSelectList().FirstOrDefault(art => art.Value == BoxArt);
                if (boxArtEntity == null) return "";
                
                return boxArtEntity.Text;
            }
        }

        [LocalizedDisplay(LocalizeConstants.InsuranceNo)]
        public string VersicherungID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Insurance)]
        public string VersicherungName
        {
            get
            {
                if (BoxArt == "RE")
                    return "";

                var versEntity = Versicherungen.ToSelectList().FirstOrDefault(vers => vers.Value == VersicherungID);
                if (versEntity == null) return "";

                return versEntity.Text;
            }
        }

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

        [NotMapped]
        public static Func<VersEventsViewModel> GetViewModel { get; set; }
    }
}
