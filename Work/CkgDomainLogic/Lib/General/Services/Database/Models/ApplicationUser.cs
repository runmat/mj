using System;
using System.Collections.Generic;
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

        public string NewLevel { get; set; }

        public Dictionary<string, string> BerechtigungsLevel
        {
            get
            {
                var liste = new Dictionary<string, string>();

                if (!String.IsNullOrEmpty(NewLevel))
                {
                    // Feld "NewLevel" enthält Berechtigungslevel und dazugehörige Autorisierungseinstellungen
                    var strLevel = NewLevel.Split('|')[0];
                    var strAut = NewLevel.Split('|')[1];
                    var levels = strLevel.Split(',');
                    var levelsAut = strAut.Split(',');

                    for (int i = 0; i < levels.Length; i++)
                    {
                        if (!liste.ContainsKey(levels[i]))
                            liste.Add(levels[i], levelsAut[i]);
                    }
                }

                return liste;
            }
        }

        
        #region Menu Group

        public string AppType { get; set; }

        [NotMapped]
        public int AppTypeRank { get; set; }

        [NotMapped]
        public string AppTypeFriendlyName { get; set; }

        #endregion


        public bool AppIsMvcFavorite { get; set; }
    }
}
