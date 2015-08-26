using System.Collections.Generic;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using System.Linq;

namespace CkgDomainLogic.WFM.Models
{
    public enum SelektionsModus
    {
        Abmeldevorgaenge,
        KlaerfallWorkplace,
        Durchlauf
    }

    public class WfmAuftragSelektor : Store 
    {
        public List<SelectItem> AlleAbmeldearten
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                {
                    new SelectItem("1", Localize.Standard),
                    new SelectItem("2", Localize.ClarificationCases),
                    new SelectItem("3", Localize.WithoutOrder),
                });
            }
        }

        public List<SelectItem> AlleAbmeldestatus
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                {
                    new SelectItem("0", Localize.Outstanding),
                    new SelectItem("1", Localize.WorkInProgress),
                    new SelectItem("2", Localize.Deregistered),
                    new SelectItem("3", Localize.Cancelled),
                });
            }
        }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationType)]
        public List<string> Abmeldearten { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationStatus)]
        public List<string> Abmeldestatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection1)]
        public bool Selektionsfeld1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection2)]
        public bool Selektionsfeld2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Selection3)]
        public bool Selektionsfeld3 { get; set; }

        public string Selektionsfeld1Name { get; set; }
        public string Selektionsfeld2Name { get; set; }
        public string Selektionsfeld3Name { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerOrderId)]
        public string KundenAuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference3)]
        public string Referenz3 { get; set; }

        public string Referenz1Name { get; set; }
        public string Referenz2Name { get; set; }
        public string Referenz3Name { get; set; }

        public SelektionsModus Modus { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateRange SolldatumVonBis { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateRange AnlageDatumVonBis { get { return PropertyCacheGet(() => new DateRange(DateRangeType.LastMonth)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.FinishDate)]
        public DateRange ErledigtDatumVonBis { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last3Months, true)); } set { PropertyCacheSet(value); } }

        public List<SelectItem> AlleToDoWer
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                {
                    new SelectItem("", Localize.All),
                    new SelectItem("DAD", "DAD"),
                    new SelectItem("KUNDE", Localize.Customer)
                });
            }
        }

        [LocalizedDisplay(LocalizeConstants.TimeInWorkingDays)]
        public bool DurchlaufzeitInTagen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ToDoWho)]
        public string ToDoWer { get; set; }

        public List<SelectItem> AlleAbmeldeartenDurchlauf
        {
            get
            {
                return PropertyCacheGet(() => new List<SelectItem>
                {
                    new SelectItem("Alle", Localize.All),
                    new SelectItem("Klär", Localize.ClarificationCase),
                    new SelectItem("Std", Localize.Standard),
                });
            }
        }

        public string GetAlleAbmeldeartenDurchlaufNextKeyFor(string key)
        {
            var item = AlleAbmeldeartenDurchlauf.First(d => d.Key == key);
            var index = AlleAbmeldeartenDurchlauf.IndexOf(item);
            index = (index + 1) % AlleAbmeldeartenDurchlauf.Count;

            return AlleAbmeldeartenDurchlauf[index].Key;
        }

        [LocalizedDisplay(LocalizeConstants.DeRegistrationType)]
        public string AbmeldeartDurchlauf { get { return PropertyCacheGet(() => "Alle"); } set { PropertyCacheSet(value); } }
    }
}
