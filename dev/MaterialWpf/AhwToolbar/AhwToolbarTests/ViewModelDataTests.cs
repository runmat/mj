// ReSharper disable RedundantUsingDirective
using System;
using System.IO;
using System.Linq;
using AhwToolbar.Models;
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
            var fileName =_viewModel.PersistViewModelDataGetFilename();
            Assert.IsTrue(fileName.IsNotNullOrEmpty());

            if (!FileService.PathExistsAndWriteEnabled(fileName))
                Assert.Fail("File path 'ViewModelDataFileLocation' is not write enabled!");

            var savedFileName = "";
            if (File.Exists(fileName))
            {
                savedFileName = Path.GetFileNameWithoutExtension(fileName) + "-tmp." + Path.GetExtension(fileName);
                File.Copy(fileName, savedFileName);
                File.Delete(fileName);
            }

            _viewModel.PersistViewModelData();

            if (!File.Exists(fileName))
            {
                if (savedFileName.IsNotNullOrEmpty())
                    File.Copy(savedFileName, fileName);

                Assert.Fail("File 'ViewModelDataFileLocation' was not created!");
            }
        }
    }
}
