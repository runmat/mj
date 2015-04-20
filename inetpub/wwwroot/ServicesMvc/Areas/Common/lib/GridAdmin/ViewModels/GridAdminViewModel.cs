// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CkgDomainLogic.General.Contracts;
using GeneralTools.Models;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class GridAdminViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IGridAdminDataService DataService { get { return CacheGet<IGridAdminDataService>(); } }

        public void DataInit()
        {
        }
    }
}
