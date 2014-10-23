using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    public class VersandBeauftragungSelektor : Store
    {
        [LocalizedDisplay(LocalizeConstants.Selection)]
        public string FilterVersandBeauftragungsTyp { get { return PropertyCacheGet(() => "ALL"); } set { PropertyCacheSet(value); } }

        [XmlIgnore]
        public string VersandBeauftragungsTypen
        {
            get
            {
                return string.Format("{0},{1};{2},{3};{4},{5}",
                                        "ALL", Localize.All,
                                        "OnlyZBII", Localize.OnlyZBII,
                                        "OnlyKeys", Localize.OnlyKeys);
            }
        }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool FilterAuchNeue { get { return PropertyCacheGet(() => true); } set { PropertyCacheSet(value);} }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool FilterFreigabeTreugeberOffen { get { return PropertyCacheGet(() => true); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public bool FilterVersandVorbereitungDAD { get { return PropertyCacheGet(() => true); } set { PropertyCacheSet(value); } }
    }
}
