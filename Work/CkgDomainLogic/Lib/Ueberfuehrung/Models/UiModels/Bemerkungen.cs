using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class Bemerkungen : UiModel
    {
        [LocalizedDisplay(LocalizeConstants._IhreHinweiseZumAuftrag)]
        public string Bemerkung { get; set; }

        public string FahrtIndex { get; set; }

        public override string ViewName
        {
            get { return "Partial/BemerkungenEdit"; }
        }

        public override bool IsEmpty
        {
            get { return string.IsNullOrEmpty(Bemerkung); }
        }

        public override GeneralEntity SummaryItem
        {
            get
            {
                return new GeneralEntity
                {
                    Title = HeaderShort,
                    Body = string.Format("<i>{0}</i>", Bemerkung.Crop(70)),
                };
            }
        }

        public List<KurzBemerkung> BemerkungAsKurzBemerkungen { get { return ConvertToShortTextList(FahrtIndex, "0018", 40); } }

        public List<KurzBemerkung> ConvertToShortTextList(string groupName, string textID, int shortTextLen)
        {
            var s = Bemerkung;
            if (s.IsNullOrEmpty())
                return new List<KurzBemerkung>();

            // MJE, 21.02.2014:
            // deactivated, note: this will only take complete occurrences of length (i.e. 3 sets of 40 char strings in a string that is 125 characters long).
            //var shortTextList = Enumerable.Range(0, s.Length/shortTextLen).Select(i => s.Substring(i*shortTextLen, shortTextLen));

            // MJE, 21.02.2014:
            // prefer classic code:
            var index = 0;
            var shortTextList = new List<string>();
            while (index < s.Length)
            {
                shortTextList.Add(s.SubstringTry(index, shortTextLen));
                index += shortTextLen;
            }

            return shortTextList.Select(shortText => new KurzBemerkung
                                                        {
                                                            ID = textID,
                                                            GroupName = groupName,
                                                            Bemerkung = shortText,
                                                        }).ToList();
        }
    }
}
