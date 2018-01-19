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
    }
}