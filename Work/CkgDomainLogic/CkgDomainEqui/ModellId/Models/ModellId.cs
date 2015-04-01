using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class ModellId
    {
        [LocalizedDisplay(LocalizeConstants.Model)]
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string Bezeichnung { get; set; }


        [GridHidden, NotMapped]
        public bool InsertModeTmp { get; set; }

        public ModellId SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }
    }
}
