using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using GeneralTools.Models;
using GeneralTools.Services;

namespace AhwToolbar.Models
{
    public class ViewModelData
    {
        public List<TabData> Tabs { get; set; }

        public string PersistViewModelDataFilename => @"AhwToolbarData.xml";
       

        public void PersistViewModelData()
        {
            if (Tabs.AnyAndNotNull())
                XmlService.XmlSerializeToFile(this, PersistViewModelDataFilename);
        }

        public ViewModelData LoadViewModelData()
        {
            return XmlService.XmlDeserializeFromFile<ViewModelData>(PersistViewModelDataFilename);
        }

        public void SetTabs(IEnumerable<string> tabHeaders, string selectedTabHeader)
        {
            var tabs = new List<TabData>();
            tabHeaders.ToList().ForEach(th =>
            {
                var tab = Tabs.First(t => t.Header == th);
//                if (!tab.IsSelected)
                    tab.IsSelected = tab.Header == selectedTabHeader;
                tabs.Add(tab);
            });
            Tabs = tabs;
        }
    }
}