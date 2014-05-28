using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Services;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class Summary : UiModel
    {
        private const int SummaryDescriptionsRowCount = 5;

        [XmlIgnore]
        public int StepMax { get; set; }

        private string _summaryItemsAsString;
        [XmlIgnore]
        public string SummaryItemsAsString
        {
            get { return _summaryItemsAsString; }
            set
            {
                _summaryItemsAsString = value;
                SummaryItems = XmlService.XmlDeserializeFromString<List<SummaryItem>>(SummaryItemsAsString);
            }
        }

        [XmlIgnore]
        public List<SummaryItem> SummaryItems { get; set; }

        [XmlIgnore]
        public override string ViewName
        {
            get { return "Partial/Summary"; }
        }

        public void InitSummaryItems(Func<int, List<GeneralEntity>> getSummaryDescriptions, Func<int, string> getHeader)
        {
            SummaryItems = new List<SummaryItem>();
            for (var i = 1; i < StepMax; i++)
                SummaryItems.Add(new SummaryItem
                                     {
                                         Step = i,
                                         Header = getHeader(i),
                                         Descriptions = PreparePaddingDescriptions(getSummaryDescriptions(i)),
                                     });

            SummaryItemsAsString = XmlService.XmlSerializeToString(SummaryItems);
        }

        static List<GeneralEntity> PreparePaddingDescriptions(List<GeneralEntity> src)
        {
            var descriptions = src;

            var dict = new List<GeneralEntity>();
            var i = 0;
            foreach (var dictEntry in descriptions)
            {
                if (i++ == 0 && descriptions.Count <= SummaryDescriptionsRowCount - 2)
                    // add 1 empty item at the top to center all items vertically appropriately
                    dict.Add(EmptyHtmlDescription);

                dict.Add(new GeneralEntity { Title = dictEntry.Title, Body = dictEntry.Body });
            }
            // append empty items to the end only for UI formatting purpose (right layout border)
            // ==> ensure there are exact 5 items (<=> SummaryDescriptionsRowCount)
            for (i = dict.Count; i < SummaryDescriptionsRowCount; i++)
                dict.Add(EmptyHtmlDescription);

            return dict;
        }

        [XmlIgnore]
        private static GeneralEntity EmptyHtmlDescription
        {
            get { return new GeneralEntity { Title = "&nbsp;", Body = "&nbsp;" }; }
        }
    }
}
