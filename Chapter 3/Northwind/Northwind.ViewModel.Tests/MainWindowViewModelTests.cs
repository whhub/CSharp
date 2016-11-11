using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Northwind.Application;
using Northwind.Data;

namespace Northwind.ViewModel.Tests
{
    [TestClass]
    public class MainWindowViewModelTests
    {
        private const string ExpectedCustomerID = "EXPECTEDID";

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


            uiDataProviderMock.VerifyAll(); //测试函数是否被调用？
        }

        [ExpectedException(typeof (InvalidOperationException))]
        [TestMethod]
        public void ShowCustomerDetails_SelectedCustomerIDIsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            var target = new MainWindowViewModel(null);

            // Act 
            target.ShowCustomerDetails();
        }

        [TestMethod]
        public void ShowCustomerDetails_ToolNotFound_AddNewTool()
        {
            // Arrange
            var target = GetShowCustomerDetailsTarget(new Customer {CustomerID = ExpectedCustomerID});
        
            // Act
            target.ShowCustomerDetails();

            // Assert
            var actual = target.Tools.Cast<CustomerDetailsViewModel>()
                .FirstOrDefault(
                    vm => vm.Customer.CustomerID == ExpectedCustomerID);

            Assert.IsNotNull(actual);

        }

        [TestMethod]
        public void ShowCustomerDetails_Always_ToolIsSetToCurrent()
        {
            // Arrange
            var customer = new Customer {CustomerID = ExpectedCustomerID};
            var target = GetShowCustomerDetailsTarget(customer);

            // Act 
            target.ShowCustomerDetails();

            //Assert 
            var actual = CollectionViewSource.GetDefaultView(target.Tools).CurrentItem as CustomerDetailsViewModel;

            Assert.AreSame(customer, actual.Customer);
        }

        private MainWindowViewModel GetShowCustomerDetailsTarget(Customer customer)
        {
            var uiDataProviderMock = new Mock<IUIDataProvider>();
            uiDataProviderMock.Setup(uidp => uidp.GetCustomer(customer.CustomerID)).Returns(customer);

            var target = new MainWindowViewModel(uiDataProviderMock.Object);



            target.SelectedCustomerID = customer.CustomerID;

            return target;
        }
    }
}