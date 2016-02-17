using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.ZldPartner.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.ZldPartner.Models
{
    public class StornoModel
    {
        public string DatensatzId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Reason)]
        public string GrundId { get; set; }

        [XmlIgnore]
        public List<StornoGrund> Gruende { get { return GetViewModel == null ? new List<StornoGrund>() : GetViewModel().Gruende.Where(g => g.Status == Status).ToList(); } }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        public bool SendingEnabled { get { return GetViewModel != null && GetViewModel().SendingEnabled; } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<ZldPartnerZulassungenViewModel> GetViewModel { get; set; }
    }
}
