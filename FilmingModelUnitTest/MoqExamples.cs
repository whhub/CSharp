using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTestExample.UT
{
    [TestClass]
    public class MoqExamples
    {
        private Mock<ILongRunningLibrary> _longRunningLibrary;

        [TestInitialize]
        public void SetUp()
        {
            _longRunningLibrary = new Mock<ILongRunningLibrary>();

            _longRunningLibrary.Setup(lrl => lrl.RunForALongTime(It.IsAny<int>()))
                .Returns((int i) => string.Format("This method has been mocked! The input value was {0}", i));

            _longRunningLibrary.Setup(lrl => lrl.RunForALongTime(0))
                .Throws(new ArgumentException("0 is not a valid interval!"));
        }

        [TestMethod]
        public void TestLongRunningLibrary()
        {
           const int interval = 1000;
            var result = _longRunningLibrary.Object.RunForALongTime(interval);
            Debug.WriteLine(string.Format("Return from method was '{0}'", result));
        }
    }
}
