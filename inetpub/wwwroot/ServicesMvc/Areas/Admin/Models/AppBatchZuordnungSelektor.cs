using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Admin.ViewModels;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Admin.Models
{
    public class AppBatchZuordnungSelektor : Store
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.Application)]
        public int SelectedAppId { get; set; }

        [XmlIgnore]
        public static List<Application> AppList
        {
            get
            {
                var liste = (GetViewModel == null ? new List<Application>() : GetViewModel().Applications);
                return liste.CopyAndInsertAtTop(new Application { AppID = -1, AppFriendlyName = Localize.DropdownDefaultOptionPleaseChoose });
            }
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<AppBatchZuordnungViewModel> GetViewModel { get; set; }
    }
}
