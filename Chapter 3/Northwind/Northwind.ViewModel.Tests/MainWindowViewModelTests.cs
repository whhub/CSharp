using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Northwind.Application;

namespace Northwind.ViewModel.Tests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        [TestMethod]
        public void Customers_Always_CallsGetCustomers() //Method_Scenario_ExpectedResult
        {
            // Create stub
            //var expected = new List<Customer>
            //{
            //    new Customer {CustomerID = "id", CompanyName = "company"}
            //};

            var uiDataProviderMock = new Mock<IUIDataProvider>();
            uiDataProviderMock.Setup(uidp => uidp.GetCustomers());

            // Inject stub
            var target = new MainWindowViewModel(uiDataProviderMock.Object);

            // Assert
            //CollectionAssert.AreEquivalent(expected, (List<Customer>) target.Customers);
            var customers = target.Customers;


            uiDataProviderMock.Verify(uidp => uidp.GetCustomers()); //测试函数是否被调用？
        }
    }
}