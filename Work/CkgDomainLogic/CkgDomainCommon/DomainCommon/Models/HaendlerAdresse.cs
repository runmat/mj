using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class HaendlerAdresse
    {
        public string ID { get { return HaendlerNr + "-" + LaenderCode; } }

        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CountryCode)]
        public string LaenderCode { get; set; }


        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<HaendlerAdressenViewModel> GetViewModel { get; set; }

        [GridHidden, NotMapped]
        public bool InsertModeTmp { get; set; }

        public HaendlerAdresse SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }

        [XmlIgnore, GridHidden, NotMapped]
        public List<SelectItem> LaenderList
        {
            get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().LaenderList; }
        }


        [XmlIgnore, NotMapped, GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }
    }
}
