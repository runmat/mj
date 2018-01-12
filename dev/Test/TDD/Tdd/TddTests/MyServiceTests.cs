// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TddLib;

namespace TddTests
{
    public class MyServiceTests
    {
        private MyService _myService;

        [SetUp]
        public void SetUp()
        {
            _myService = new MyService();
        }

        [TestCase(1, Result = "1")]
        [TestCase(2, Result = "2")]
        public string TestConvert(int number)
        {
            return _myService.Convert(number);
        }
    }
}
