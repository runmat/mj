using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Organization")]
    public class UserOrganization : Organization
    {
        public bool OrganizationAdmin { get; set; }
    }
}
