﻿// ReSharper disable RedundantUsingDirective
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
        private ViewModelData _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _viewModel = new ViewModelData();
        }

        [TestMethod]
        public void TestPersistViewModelDataExists()
        {
            Assert.IsTrue(File.Exists(_viewModel.PersistViewModelDataFilename));
        }

        [TestMethod]
        public void TestPersistViewModelDataIsDeserialize()
        {
            var vm = _viewModel.LoadViewModelData();
            Assert.IsNotNull(vm);
            Assert.IsTrue(vm.Tabs.AnyAndNotNull());
        }

        [TestMethod]
        public void TestPersistViewModelData()
        {
            var vm = new ViewModelData
            {
                Tabs = new List<TabData>
                {
                    new TabData {Header = "h1", UserControlType = "test 1"},
                    new TabData {Header = "h2", UserControlType = "test 2"},
                    new TabData {Header = "h3", UserControlType = "test 3"},
                }
            };

            vm.PersistViewModelData();
            var vm2 = _viewModel.LoadViewModelData();
//            vm2.Tabs.Add(new TabData());

            Assert.AreEqual(XmlService.XmlSerializeToString(vm), XmlService.XmlSerializeToString(vm2));
        }

        [TestMethod]
        public void TestSetViewModelDataTabs()
        {
            var vm = _viewModel.LoadViewModelData();

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
    }
}
