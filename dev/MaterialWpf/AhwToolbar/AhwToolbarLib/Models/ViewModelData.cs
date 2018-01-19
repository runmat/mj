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


        public string PersistViewModelDataGetFilename()
        {
            var persistFileName = ConfigurationManager.AppSettings["ViewModelDataFileLocation"];
            if (persistFileName.IsNullOrEmpty())
                throw new Exception("appSettings Key 'ViewModelDataFileLocation'does not exist in App.Config");

            if (!persistFileName.Contains("[LocalApplicationData]"))
                throw new Exception("appSettings Value 'ViewModelDataFileLocation' must contain a [LocalApplicationData] string part");

            return persistFileName.Replace("[LocalApplicationData]", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        }

        public void PersistViewModelData()
        {
            var fileName = PersistViewModelDataGetFilename();

            if (!File.Exists(fileName))
                File.CreateText(fileName);
        }
    }
}