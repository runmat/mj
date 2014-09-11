// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using CkgDomainLogic.CoC.Contracts;
using CkgDomainLogic.CoC.Models;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.CoC.ViewModels
{
    public class SendungenViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public IZulassungDataService DataService { get { return CacheGet<IZulassungDataService>(); } }
        
        
        public SendungsAuftragSelektor SendungsAuftragSelektor
        {
            get { return PropertyCacheGet(() => new SendungsAuftragSelektor()); }
            set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> Sendungen
        {
            get { return PropertyCacheGet(() => new List<SendungsAuftrag>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<SendungsAuftrag> SendungenFiltered
        {
            get { return PropertyCacheGet(() => Sendungen); }
            private set { PropertyCacheSet(value); }
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.SendungenFiltered);
            PropertyCacheClear(this, m => m.SendungsAuftragSelektor);
        }

        public void LoadSendungen(SendungsAuftragSelektor model)
        {
            Sendungen = DataService.GetSendungsAuftraege(model);
            DataMarkForRefresh();
        }

        public void FilterSendungen(string filterValue, string filterProperties)
        {
            SendungenFiltered = Sendungen.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
