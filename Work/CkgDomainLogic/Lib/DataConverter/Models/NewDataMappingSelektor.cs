using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DataConverter.Models
{
    public class NewDataMappingSelektor
    {
        [Required]
        [MaxLength(50)]
        [LocalizedDisplay(LocalizeConstants.Name)]
        public string MappingName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public int CustomerId { get; set; }

        [XmlIgnore]
        public List<Customer> CustomerList { get { return GetViewModel == null ? new List<Customer>() : GetViewModel().Kunden; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Process)]
        public string ProzessName { get; set; }

        [XmlIgnore]
        public List<string> ProzessList { get { return GetViewModel == null ? new List<string>() : GetViewModel().Prozesse; } }

        public bool CanUserSelectCustomer { get { return GetViewModel != null && GetViewModel().CanUserSelectCustomer; } }

        public SourceFile SourceFile { get; set; }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<DataConverterViewModel> GetViewModel { get; set; }
    }
}
