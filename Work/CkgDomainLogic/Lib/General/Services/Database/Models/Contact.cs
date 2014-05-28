using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int ID { get; set; }

        public int? Mandant { get; set; }

        public string Anrede { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)] 
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fax)]
        public string Fax { get; set; }

        [LocalizedDisplay(LocalizeConstants.Mobile)]
        public string Mobile { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Mail { get; set; }

        public int? Hierarchie { get; set; }

        public string Abteilung { get; set; }

        public string Position { get; set; }

        public string PictureName { get; set; }
    }
}
