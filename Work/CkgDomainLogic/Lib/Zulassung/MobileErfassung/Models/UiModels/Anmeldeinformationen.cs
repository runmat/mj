using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models.UiModels
{
    public class Anmeldeinformationen
    {
        [Display(Name = "Benutzername")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kennwort")]
        public string Password { get; set; }
    }
}
