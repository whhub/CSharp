using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestExample.UT
{
    [TestClass]
    public class DummyTestClass
    {
        [TestMethod]
        public void TestWithADummy()
        {
            var dependency = new DummyDependency();
            var dependentClass = new DependentClass(dependency);
            const string param = "abc";
            const int expectedResultOne = 1;

            var resultOne = dependentClass.GetValue(param);

            Assert.AreEqual(expectedResultOne, resultOne);
        }
    }

    public class DummyDependency : IDependency
    {
        #region Implementation of IDependency

        public int GetValue(string s)
        {
            return 1;
        }

        #endregion
    }
}
