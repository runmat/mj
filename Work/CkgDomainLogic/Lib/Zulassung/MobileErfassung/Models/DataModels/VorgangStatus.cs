using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.Zulassung.MobileErfassung.Models
{
    /// <summary>
    /// Statusinformation (BEB-Status) zu einer Vorgangsnummer
    /// </summary>
    public class VorgangStatus
    {
        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        public VorgangStatus()
        {
            this.Id = "";
            this.Status = "";
        }

        public VorgangStatus(string id, string status)
        {
            this.Id = id;
            this.Status = status;
        }
    }
}
