using Microsoft.EntityFrameworkCore;
using CDB;
using CDB.Model;
using CustomerManagement.Data;
using Moq;

namespace CustomerManagement.Test.Data
{
    public class CustomerDataProviderTest
    {
        [Test]
        public void TestGetAll_ShouldGetAll()
        {
            Mock<IDataWrapper> mockDataWrapper = new Mock<IDataWrapper>();

            List<Customer> mockCustomerList = new List<Customer>
            {
                new Customer(1, "Test company Name 1", "test business contact 1", "test@emailaddrss.com", "012586942345", new DateTime(2025, 04, 5, 20, 42, 41), new DateTime(2025, 04, 5, 20, 43, 02)),
                new Customer(2, "Amet Luctus Company", "Wyatt Munoz", "purus.mauris@hotmail.co.uk", "(016977) 6174", new DateTime(2025, 04, 5, 20, 39, 22), new DateTime(2025, 04, 5, 20, 40, 33)),
                new Customer(3, "Cursus A Incorporated", "Paul Everett", "curabitur.egestas.nunc@aol.ca", "(01746) 433844", new DateTime(2025, 04, 5, 20, 42, 51), new DateTime(2025, 04, 5, 20, 43, 02))
            };

            mockDataWrapper.Setup(wrapper => wrapper.SelectAllCustomers()).Returns(mockCustomerList);

            IDataWrapper mockDataWrapperObject = mockDataWrapper.Object;

            CustomerDataProvider testCustomerDataProvider = new CustomerDataProvider(mockDataWrapperObject);

            List<Customer> testCustomers = testCustomerDataProvider.GetAll();

            Assert.That(testCustomers.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestGetAll_ShouldThrowException()
        {
            Mock<IDataWrapper> mockDataWrapper = new Mock<IDataWrapper>();

            DbUpdateException dbUpdateException = new DbUpdateException("Test database update exception");

            mockDataWrapper.Setup(wrapper => wrapper.SelectAllCustomers()).Throws(dbUpdateException);

            IDataWrapper mockDataWrapperObject = mockDataWrapper.Object;

            CustomerDataProvider testCustomerDataProvider = new CustomerDataProvider(mockDataWrapperObject);

            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => testCustomerDataProvider.GetAll());

            Assert.That(expectedException.Message, Is.EqualTo("Test database update exception"));
        }
    }
}
