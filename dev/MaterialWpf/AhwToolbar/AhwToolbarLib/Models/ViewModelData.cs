using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using GeneralTools.Models;

namespace AhwToolbar.Models
{
    public class ViewModelData
    {
        private static readonly List<TabData> DefaultTabs = new List<TabData>
        {
            new TabData {Header = "Walter", UserControlType = "AhwToolbar.UserControls.UcContent1", IsSelected = false},
            new TabData {Header = "Zabel", UserControlType = "AhwToolbar.UserControls.UcContent2", IsSelected = true},
        };

        public List<TabData> Tabs { get; set; } = DefaultTabs;


        public string PersistViewModelDataGetFilename => @"AhwToolbarData.xml";
        

        public void PersistViewModelData()
        {
            var fileName = PersistViewModelDataGetFilename;
        }
    }
}