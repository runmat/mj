using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DataConverter.ViewModels;
using CkgDomainLogic.General.Database.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.DataConverter.Models
{
    public class DataMappingSelektor
    {
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public int CustomerId { get; set; }

        [XmlIgnore]
        public List<Customer> CustomerList { get { return GetViewModel == null ? new List<Customer>() : GetViewModel().Kunden; } }

        [LocalizedDisplay(LocalizeConstants.Process)]
        public string ProzessName { get; set; }

        [XmlIgnore]
        public List<string> ProzessList { get { return GetViewModel == null ? new List<string>() : GetViewModel().Prozesse; } }

        public bool CanUserSelectCustomer { get { return GetViewModel != null && GetViewModel().CanUserSelectCustomer; } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<DataConverterViewModel> GetViewModel { get; set; }
    }
}
