using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestExample.UT
{
    [TestClass]
    public class StubTestClass
    {
        [TestMethod]
        public void TestWithAStub()
        {
            var dependency = new StubDependency();
            var dependentClass = new DependentClass(dependency);
            const string param1 = "abc";
            const string param2 = "xyz";
            const int expectedResultOne = 1;
            const int expectedResultTwo = 2;

            var resultOne = dependentClass.GetValue(param1);
            var resultTwo = dependentClass.GetValue(param2);
            Assert.AreEqual(expectedResultOne, resultOne);
            Assert.AreEqual(expectedResultTwo, resultTwo);
        }
   }

    public class StubDependency : IDependency
    {
        #region Implementation of IDependency

        public int GetValue(string s)
        {
            if (s == "abc")
                return 1;
            if (s == "xyz")
                return 2;
            return 0;
        }

        #endregion
    }
}
