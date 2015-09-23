using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class HistoryAuftragSelector : Store
    {
        [LocalizedDisplay(LocalizeConstants.OverpassDate)]
        [RequiredAsGroup]
        public DateRange UeberfuehrungsDatumRange 
        { 
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months)); } 
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.OrderDate)]
        [RequiredAsGroup]
        public DateRange AuftragsDatumRange
        {
            get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months, true)); } 
            set { PropertyCacheSet(value); }
        }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        public string KundenNrUser { get; set; }

        #region AG

        private List<KundeAusHierarchie> _kundenAusHierarchie;

        [XmlIgnore]
        public List<KundeAusHierarchie> KundenAusHierarchie
        {
            get
            {
                if (GetKundenAusHierarchie == null)
                    return new List<KundeAusHierarchie>();

                if (_kundenAusHierarchie != null)
                    return _kundenAusHierarchie;

                _kundenAusHierarchie = GetKundenAusHierarchie().ToList();
                if (_kundenAusHierarchie.None())
                {
                    _kundenAusHierarchie = new List<KundeAusHierarchie> { new KundeAusHierarchie { KundenNr = KundenNrUser } };
                    AgKundenNr = KundenNrUser;
                }
                else if (_kundenAusHierarchie.Count() == 1)
                    AgKundenNr = _kundenAusHierarchie.First().KundenNr;

                return _kundenAusHierarchie;
            }
        }

        [LocalizedDisplay(LocalizeConstants.Principal)]
        public string AgKundenNr { get; set; }

        [XmlIgnore]
        public KundeAusHierarchie AgKunde { get { return KundenAusHierarchie.FirstOrDefault(k => k.KundenNr == AgKundenNr) ?? new KundeAusHierarchie(); } }

        [XmlIgnore]
        public Func<List<KundeAusHierarchie>> GetKundenAusHierarchie { get; set; }

        #endregion

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        [RequiredAsGroup]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReferenceNo)]
        [RequiredAsGroup]
        public string Referenz { get; set; }
        
        [LocalizedDisplay(LocalizeConstants._AlleOrganisationen)]
        public bool AlleOrganisationen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [RequiredAsGroup]
        [KennzeichenPartial]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants._KundenReferenz)]
        public string KundenReferenz { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftragsart)]
        public string AuftragsArt { get { return PropertyCacheGet(() => "A"); } set { PropertyCacheSet(value);} }

        [XmlIgnore]
        public List<SelectItem> AuftragsArtOptions
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("A", Localize.AllOrders),
                    new SelectItem("O", Localize.OpenOrders),
                    new SelectItem("D", Localize.FinishedOrders),
                    new SelectItem("N", Localize.OnlyItemsToClarify),
                };
            }
        }
    }
}