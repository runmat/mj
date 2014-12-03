using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.Fahrzeugbestand.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeugbestand.Models
{
    public class FahrzeugAkteBestandSelektor
    {
        private string _fin;

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string FIN
        {
            get { return _fin.NotNullOrEmpty().ToUpper(); }
            set { _fin = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.FactoryName)]
        public string FabrikName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.Buyer)]
        public string Kaeufer { get; set; }

        
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FahrzeugbestandViewModel> GetViewModel { get; set; }

        public List<Adresse> HalterForSelection { get { return GetViewModel().HalterForSelection; } }

        public List<Adresse> KaeuferForSelection { get { return GetViewModel().KaeuferForSelection; } }
    }
}
