using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Northwind.Application;
using Northwind.Data;

namespace Northwind.ViewModel.Tests
{
    [TestClass]
    public class CustomerDetailsViewModelTests
    {
        private Mock<IUIDataProvider> _uiDataProviderMock;
        const string ExpectedID = "EXPECTEDID";

        [TestInitialize]
        public void SetUp()
        {
            _uiDataProviderMock = new Mock<IUIDataProvider>();            
        }

        [TestMethod]
        public void Ctor_Always_CallsGetCustomer()
        {
            // Arrange
            _uiDataProviderMock.Setup(uidp => uidp.GetCustomer(ExpectedID)).Returns(new Customer());

            // Act
            var target = new CustomerDetailsViewModel(_uiDataProviderMock.Object, ExpectedID);

            // Assert
            _uiDataProviderMock.VerifyAll();
        }

        [TestMethod]
        public void Customer_Always_ReturnsCustomerFromGetCustomer()  // behavior test , object initialized only from one route
        {
            // Arrange
            var expectedCustomer = new Customer {CustomerID = ExpectedID};
            _uiDataProviderMock.Setup(uidp => uidp.GetCustomer(ExpectedID)).Returns(expectedCustomer);

            // Act 
            var target = new CustomerDetailsViewModel(_uiDataProviderMock.Object, ExpectedID);

            // Assert
            Assert.AreSame(expectedCustomer, target.Customer);
        }

        [TestMethod]
        public void DisplayName_Always_ReturnsCompanyName()
        {
            // Arrange
            const string expectedCompanyName = "EXPECTEDNAME";
            var customer = new Customer {CustomerID = ExpectedID, CompanyName = expectedCompanyName};

            _uiDataProviderMock.Setup(uidp => uidp.GetCustomer(ExpectedID)).Returns(customer);

            // Act
            var target = new CustomerDetailsViewModel(_uiDataProviderMock.Object, ExpectedID);

            // Assert
            var displayName = target.DisplayName;
            Assert.AreEqual(expectedCompanyName, displayName);
        }
    }
}
