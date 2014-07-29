// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.ViewModels;
using System.Web.Mvc;
using CkgDomainLogic.Uebfuehrg.Contracts;
using CkgDomainLogic.Uebfuehrg.Models;
using CkgDomainLogic.Uebfuehrg.Models;
using GeneralTools.Models;
using System.IO;
using GeneralTools.Services;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Uebfuehrg.ViewModels
{
    public class UebfuehrgViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IUebfuehrgDataService DataService { get { return CacheGet<IUebfuehrgDataService>(); } }

        public void DataInit()
        {
            DataMarkForRefresh();
        }

        public void DataMarkForRefresh()
        {
        }

        public void Validate(Action<string, string> addModelError)
        {
        }
    }
}
