// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.General.ViewModels;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.CoC.ViewModels
{
    public class SendungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IZulassungDataService ZulassungDataService { get { return CacheGet<IZulassungDataService>(); } }
    }
}
