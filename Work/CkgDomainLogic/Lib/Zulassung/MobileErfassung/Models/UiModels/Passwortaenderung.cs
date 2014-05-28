using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models.UiModels
{
    public class Passwortaenderung
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Altes Kennwort")]
        public string AltesPasswort { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Neues Kennwort")]
        public string NeuesPasswort { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Neues Kennwort bestätigen")]
        public string NeuesPasswortConfirm { get; set; }
    }
}
