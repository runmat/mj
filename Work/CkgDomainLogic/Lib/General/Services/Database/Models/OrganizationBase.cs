using System.ComponentModel.DataAnnotations;

namespace CkgDomainLogic.General.Database.Models
{
    public class OrganizationBase
    {
        [Key]
        public int OrganizationID { get; set; }

        public string OrganizationName { get; set; }

        public int CustomerID { get; set; }

        public string OrganizationReference { get; set; }

        public string OrganizationReference2 { get; set; }

        public bool AllOrganizations { get; set; }
    }
}