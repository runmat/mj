using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CkgDomainLogic.General.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("vwApplicationWebUser")]
    public class ApplicationUser : IApplicationUserMenuItem
    {
        public int UserID { get; set; }

        [Key]
        public int AppID { get; set; }

        public string AppName { get; set; }

        public string AppFriendlyName { get; set; }

        public string AppURL { get; set; }

        public int AppRank { get; set; }

        public bool AppInMenu { get; set; }

        public bool AppIsMvcFavorite { get; set; }

        
        #region Menu Group

        public string AppType { get; set; }

        [NotMapped]
        public int AppTypeRank { get; set; }

        [NotMapped]
        public string AppTypeFriendlyName { get; set; }

        #endregion
    }
}
