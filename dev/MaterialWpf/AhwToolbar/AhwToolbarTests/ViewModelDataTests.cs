// ReSharper disable RedundantUsingDirective
using System;
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
        public void TestPersistViewModelData()
        {
            var fileName = _viewModel.PersistViewModelDataGetFilename;

            Assert.IsTrue(File.Exists(fileName));
        }
    }
}
