using Microsoft.EntityFrameworkCore;
using CustomerManagement.Data;
using CDB;
using CDB.Model;
using Moq;

namespace CustomerManagement.Test.Data
{
    public class CustomerDataProviderTest
    {
        private Mock<IDataWrapper> mockDataWrapper;
        private IDataWrapper mockDataWrapperObject;
        private CustomerDataProvider testCustomerDataProvider;

        [SetUp]
        public void Setup()
        {
            this.mockDataWrapper = new Mock<IDataWrapper>();
            this.mockDataWrapperObject = this.mockDataWrapper.Object;
            this.testCustomerDataProvider = new CustomerDataProvider(this.mockDataWrapperObject);
        }

        [Test]
        public void TestGetAll_ShouldGetAll()
        {
            // Arrange.
            List<Customer> mockCustomerList = new List<Customer>
            {
                new Customer(1, "Test company Name 1", "test business contact 1", "test@emailaddrss.com", "012586942345", new DateTime(2025, 04, 5, 20, 42, 41), new DateTime(2025, 04, 5, 20, 43, 02)),
                new Customer(2, "Amet Luctus Company", "Wyatt Munoz", "purus.mauris@hotmail.co.uk", "(016977) 6174", new DateTime(2025, 04, 5, 20, 39, 22), new DateTime(2025, 04, 5, 20, 40, 33)),
                new Customer(3, "Cursus A Incorporated", "Paul Everett", "curabitur.egestas.nunc@aol.ca", "(01746) 433844", new DateTime(2025, 04, 5, 20, 42, 51), new DateTime(2025, 04, 5, 20, 43, 02))
            };

            this.mockDataWrapper.Setup(wrapper => wrapper.SelectAllCustomers()).Returns(mockCustomerList);

            // Act.
            List<Customer> testCustomers = this.testCustomerDataProvider.GetAll();

            // Assert.
            Assert.That(testCustomers.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestGetAll_ShouldThrowException()
        {
            // Arrange.
            DbUpdateException dbUpdateException = new DbUpdateException("Test database update exception");

            this.mockDataWrapper.Setup(wrapper => wrapper.SelectAllCustomers()).Throws(dbUpdateException);

            // Act.
            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => this.testCustomerDataProvider.GetAll());

            // Assert.
            Assert.That(expectedException.Message, Is.EqualTo("Test database update exception"));
        }

        [Test]
        public void TestInsertNewCustomer_ShouldInsertNewCustomer()
        {
            // Arrange.
            Customer testCustomer = new Customer
            {
                CompanyName = "Sociosqu Ad Litora Associates",
                BusinessContact = "Vladimir Solis",
                ContactNumber = "056 4025 5806",
                EmailAddress = "amet.metus.aliquam@yahoo.net",
            };
            this.mockDataWrapper.Setup(dataWrapper => dataWrapper.InsertNewCustomer(It.IsAny<Customer>())).Returns(1);

            // Act.
            int result = this.testCustomerDataProvider.InsertNewCustomer(testCustomer);

            // Assert.
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void TestInsertNewCustomer_ShouldThrowException()
        {
            // Arrange.
            Customer testCustomer = new Customer
            {
                CompanyName = null,
                BusinessContact = "Vladimir Solis",
                ContactNumber = "056 4025 5806",
                EmailAddress = "amet.metus.aliquam@yahoo.net",
            };

            DbUpdateException dbUpdateException = new DbUpdateException("Test database update exception");
            mockDataWrapper.Setup(wrapper => wrapper.InsertNewCustomer(It.IsAny<Customer>())).Throws(dbUpdateException);

            // Act.
            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => this.testCustomerDataProvider.InsertNewCustomer(testCustomer));
            
            // Assert.
            Assert.That(expectedException.Message, Is.EqualTo("Test database update exception"));
        }

        [Test]
        public void TestUpdateCustomer_ShouldUpdateCustomer()
        {
            // Arrange.
            Customer testCustomer = new Customer
            {
                Id = 2,
                CompanyName = "Sociosqu Ad Litora Associates",
                BusinessContact = "Vladimir Solis",
                ContactNumber = "056 4025 5806",
                EmailAddress = "amet.metus.aliquam@yahoo.net",
                CreatedDateTime = new DateTime(),
                LastUpdateDateTime = new DateTime()
            };

            this.mockDataWrapper.Setup(dataWrapper => dataWrapper.UpdateCustomer(2)).Returns(1);

            // Act.
            int result = this.testCustomerDataProvider.UpdateCustomer(testCustomer.Id);

            // Assert.
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void TestUpdateCustomer_ShouldThrowException()
        {
            // Arrange
            Customer testCustomer = new Customer
            {
                Id = 3,
                CompanyName = "Velit Pellentesque Ltd",
                BusinessContact = "Wendy Carrillo",
                ContactNumber = "0813 568 4084",
                EmailAddress = "praesent@aol.edu",
                CreatedDateTime = new DateTime(),
                LastUpdateDateTime = new DateTime()
            };

            DbUpdateException dbUpdateException = new DbUpdateException("Test database update exception attempting to update customer");

            this.mockDataWrapper.Setup(dataWrapper => dataWrapper.UpdateCustomer(3)).Throws(dbUpdateException);

            // Act.
            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => this.testCustomerDataProvider.UpdateCustomer(testCustomer.Id));

            // Assert.
            Assert.That(expectedException.Message, Is.EqualTo("Test database update exception attempting to update customer"));
        }

        [Test]
        public void TestLoadSubscriptions_ShouldLoadSubscriptions()
        {
            int customerId = 1;
            CustomerDataProvider testCustomerDataProvider = new CustomerDataProvider();
            Customer testCustomer = testCustomerDataProvider.GetAll().First(cust => cust.Id == customerId);
            testCustomerDataProvider.LoadSubscriptions(testCustomer.Id);

            Assert.That(testCustomer.Subscriptions.Count, Is.EqualTo(4));

            foreach (Subscription sub in testCustomer.Subscriptions)
            {
                Assert.That(sub.Service, Is.Not.Null);
            }
        }

        [Test]
        public void TestLoadSubscriptions_ShouldThrowException()
        {
            // Arrange.
            InvalidOperationException testException = new InvalidOperationException("Test Exception occurred attempting to load subscriptions.");
            this.mockDataWrapper.Setup(dataWrapper => dataWrapper.LoadSubscriptions(It.IsAny<int>())).Throws(testException);

            // Act.
            InvalidOperationException expectedException = Assert.Throws<InvalidOperationException>(() => this.testCustomerDataProvider.LoadSubscriptions(123));

            // Assert.
            Assert.That(expectedException.Message, Is.EqualTo("Test Exception occurred attempting to load subscriptions."));
        }
    }
}
