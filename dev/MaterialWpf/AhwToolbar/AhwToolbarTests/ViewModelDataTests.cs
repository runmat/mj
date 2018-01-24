// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AhwToolbar.Models;
using AhwToolbar.ViewModels;
using GeneralTools.Models;
using GeneralTools.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AhwToolbarTests
{
    [TestClass]
    public class ViewModelDataTests
    {
        private ViewModelLocator _vmLocator;
        private ViewModelData _vmData;

        [TestInitialize]
        public void Setup()
        {
            _vmData = new ViewModelData();
            _vmLocator = new ViewModelLocator();
        }

        [TestMethod]
        public void TestPersistViewModelDataExists()
        {
            Assert.IsTrue(File.Exists(_vmData.PersistViewModelDataFilename));
        }

        [TestMethod]
        public void TestPersistViewModelDataIsDeserialize()
        {
            var vm = _vmData.LoadViewModelData();
            Assert.IsNotNull(vm);
            Assert.IsTrue(vm.Tabs.AnyAndNotNull());
        }

        [TestMethod]
        public void TestPersistViewModelData()
        {
            var vmOrg = _vmData.LoadViewModelData();

            var vm1 = new ViewModelData
            {
                Tabs = new List<TabData>
                {
                    new TabData {Header = "h1", UserControlType = "test 1"},
                    new TabData {Header = "h2", UserControlType = "test 2"},
                    new TabData {Header = "h3", UserControlType = "test 3"},
                }
            };

            vm1.PersistViewModelData();
            var vm2 = _vmData.LoadViewModelData();
            //vm2.Tabs.Add(new TabData());

            vmOrg.PersistViewModelData();

            Assert.IsTrue(vm1.Tabs.Count == 3);
            Assert.IsTrue(vm2.Tabs.Count == 3);
            Assert.AreEqual(vm1.Tabs.Count, vm2.Tabs.Count);
            Assert.AreEqual(XmlService.XmlSerializeToString(vm1), XmlService.XmlSerializeToString(vm2));
        }

        [TestMethod]
        public void TestSetViewModelDataTabs()
        {
            var vm = _vmData.LoadViewModelData();

            var vmOld = XmlService.XmlDeserializeFromString<ViewModelData>(XmlService.XmlSerializeToString(vm));
            var oldTabs = vmOld.Tabs;

            var tabHeaders = oldTabs.Select(t => t.Header).Reverse();
            var unSelectedTabHeader = oldTabs.First(t => !t.IsSelected).Header;

            vm.SetTabs(tabHeaders, unSelectedTabHeader);

            var newTabs = vm.Tabs;

            // count test 
            Assert.IsTrue(newTabs.Count == oldTabs.Count);
            
            // reorder test
            Assert.AreEqual(newTabs.First().Header, oldTabs.Last().Header);

            // selection tests
            Assert.IsTrue(newTabs.Count(t => t.IsSelected) == 1);
            Assert.AreNotEqual(newTabs.First(t => t.IsSelected).Header, oldTabs.First(t => t.IsSelected).Header);
        }

        [TestMethod]
        public void TestCreationUiViewModels()
        {
            var vmMain = _vmLocator.Main;
            var vmSub1 = _vmLocator.Content1ViewModel;
            var vmSub2 = _vmLocator.Content2ViewModel;
        }
    }
}
