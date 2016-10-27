using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestExample.UT
{
    [TestClass]
    public class MockTestClass
    {
        [TestMethod]
        public void TestWithAMock()
        {
            var dependency = new MockDependency();
            var dependentClass = new DependentClass(dependency);
            const string param1 = "abc";
            const string param2 = "xyz";
            const int expectedResultOne = 1;
            const int expectedResultTwo = 2;

            dependentClass.CallMeFirst();
            var resultOne = dependentClass.CallMeTwice(param1);
            var resultTwo = dependentClass.CallMeTwice(param2);
            dependentClass.CallMeLast();

            Assert.AreEqual(expectedResultOne, resultOne);
            Assert.AreEqual(expectedResultTwo, resultTwo);
 
        }
   }

    public class MockDependency : IDependency
    {
        private bool _callMeLastCalled;
        private int _callMeTwiceCalled;
        private bool _callMeFirstCalled;

        #region Implementation of IDependency

        public int GetValue(string s)
        {
           if (s == "abc")
                return 1;
            if (s == "xyz")
                return 2;
            return 0;
        }

        public void CallMeFirst()
        {
            if ((_callMeTwiceCalled > 0) || _callMeLastCalled)
                throw new AssertFailedException("CallMeFirst not first method called");
            _callMeFirstCalled = true;
        }

        public int CallMeTwice(string s)
        {
            if(!_callMeFirstCalled)
                throw new AssertFailedException("CallMeTwice called before CallMeFirst");
            if (_callMeLastCalled)
                throw new AssertFailedException("CallMeTwice called after CallMeLast");
            if(_callMeTwiceCalled >= 2)
                throw new AssertFailedException("CallMeTwice called more than twice");
            _callMeTwiceCalled++;
            return GetValue(s);
        }

        public void CallMeLast()
        {
            if(!_callMeFirstCalled)
                throw new AssertFailedException("CallMeLast called before CallMeFirst");
            if(_callMeTwiceCalled != 2)
                throw new AssertFailedException(string.Format("CallMeTwice not called {0} times", _callMeTwiceCalled));
            _callMeLastCalled = true;
        }

        #endregion
    }
}
