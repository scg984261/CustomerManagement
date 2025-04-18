using System.Data;
using Microsoft.EntityFrameworkCore;
using CDB.Model;

namespace CDB.Test
{
    public class TestDataWrapper
    {
        private DataWrapper testDataWrapper;

        [SetUp]
        public void Setup()
        {
            this.testDataWrapper = new DataWrapper();
        }

        [Test]
        public void TestGetConnectionStringFromConfig_ShouldGetConnectionString()
        {
            DataWrapper.GetConnectionStringFromConfig();
            Assert.That(DataWrapper.DatabaseConnectionString, Is.EqualTo("Server=INVALIDSERVERNAME\\SQLEXPRESS;Initial Catalog=TEST_DB;trustservercertificate=True;trusted_connection=true"));
        }

        [Test]
        public void TestSelectAllCustomers_ShouldReturnListOfCustomers()
        {
            List<Customer> customers = this.testDataWrapper.SelectAllCustomers();

            Assert.That(customers, Is.Not.Null);
            Assert.That(customers.Count, Is.GreaterThan(0));
        }

        [Test]
        public void TestInsertNewCustomer_ShouldInsertNewCustomer()
        {
            // Arrange
            string companyName = "Faucibus Company";
            string businessContact = "Wallace Padilla";
            string emailAddress = "diam.dictum@google.co.uk";
            string contactNumber = "(0111) 257 5655";

            Customer testCustomer = new Customer(companyName, businessContact, emailAddress, contactNumber);

            int numberOfCustomers = this.testDataWrapper.context.Customers.Count();

            // Act.
            this.testDataWrapper.InsertNewCustomer(testCustomer);
            Customer newlyInsertedCustomer = this.testDataWrapper.context.Customers.OrderByDescending(cust => cust.Id).First();

            int newNumberOfCustomers = this.testDataWrapper.context.Customers.Count();

            // Assert
            Assert.That(newNumberOfCustomers, Is.EqualTo(numberOfCustomers + 1));
            Assert.That(testCustomer.Id, Is.Not.EqualTo(0));
            Assert.That(testCustomer.Equals(newlyInsertedCustomer));
        }

        [Test]
        public void TestInsertNewCustomer_ShouldThrowException()
        {
            string? companyName = null;
            string businessContact = "Wallace Padilla";
            string emailAddress = "diam.dictum@google.co.uk";
            string contactNumber = "(0111) 257 5655";

            Customer testCustomer = new Customer(companyName, businessContact, emailAddress, contactNumber);

            // Test customer should throw an exception when inserted into the database,
            // As it should violate NOT NULL constraint.

            DataWrapper testWrapper = new DataWrapper();

            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => testWrapper.InsertNewCustomer(testCustomer));
            const string expectedExceptionMessage = "An error occurred while saving the entity changes. See the inner exception for details.";
            Assert.That(expectedException.Message, Is.EqualTo(expectedExceptionMessage));
        }

        [Test]
        public void TestUpdateCustomer_ShouldUpdateCustomer()
        {
            // Arrange.
            Customer customerToUpdate = this.testDataWrapper.SelectAllCustomers().First(customer => customer.Id == 1);

            DateTime lastUpdateDateTime = customerToUpdate.LastUpdateDateTime;

            customerToUpdate.BusinessContact = "Test new value";

            // Act.
            int updateResult = this.testDataWrapper.UpdateCustomer(1);

            // Assert.
            // 1 Row should have been updated, therefore Entity Framework should return 1.
            Assert.That(updateResult, Is.EqualTo(1));

            // New value for last update date time should now be greater that what it was before.
            Assert.That(customerToUpdate.LastUpdateDateTime, Is.GreaterThan(lastUpdateDateTime));
        }

        [Test]
        public void TestUpdateCustomer_ShouldThrowException()
        {
            DataWrapper testWrapper = new DataWrapper();

            Customer customerToUpdate = testWrapper.SelectAllCustomers().First(customer => customer.Id == 1);

            customerToUpdate.CompanyName = null;

            // Act.
            // Assert that an exception is thrown.
            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => testWrapper.UpdateCustomer(1));
            const string expectedExceptionMessage = "An error occurred while saving the entity changes. See the inner exception for details.";
            Assert.That(expectedException.Message, Is.EqualTo(expectedExceptionMessage));
        }

        [Test]
        public void TestLoadSubscriptions_ShouldLoadSubscriptions()
        {
            // Arrange.
            Customer testCustomer = this.testDataWrapper.SelectAllCustomers().First(customer => customer.Id == 1);

            // Act.
            this.testDataWrapper.LoadSubscriptions(testCustomer.Id);

            // Assert.
            const int expectedNumberOfServices = 4;
            Assert.That(testCustomer.Subscriptions.Count, Is.EqualTo(expectedNumberOfServices));

            foreach (Subscription sub in testCustomer.Subscriptions)
            {
                Assert.That(sub.Service, Is.Not.Null);
            }
        }

        [Test]
        public void TestLoadSubscriptions_ShouldThrowException()
        {
            // Arrange.
            const int invalidCustomerId = -1;

            // Act.
            InvalidOperationException expectedException = Assert.Throws<InvalidOperationException>(() => this.testDataWrapper.LoadSubscriptions(invalidCustomerId));

            // Cannot have a customer with a negative Customer ID, so this will throw an exception.
            const string expectedExceptionMessage = "Sequence contains no elements";
            Assert.That(expectedException.Message, Is.EqualTo(expectedExceptionMessage));
        }

        [Test]
        public void TestSelectAllServices_ShouldSelectAllServices()
        {
            List<Service> services = this.testDataWrapper.SelectAllServices();

            Assert.That(services, Is.Not.Null);
            Assert.That(services.Count, Is.GreaterThan(0));
        }

        [Test]
        public void TestInsertNewService_ShouldInsertNewService()
        {
            // Arrange.
            int numberOfServices = this.testDataWrapper.context.Services.Count();
            const string name = "Test new Service";
            const decimal price = 18425.91m;
            const bool isRecurring = true;

            Service testService = new Service(name, price, isRecurring);

            // Act.
            int result = this.testDataWrapper.InsertNewService(testService);

            // Assert.
            const int expectedResult = 1;
            Assert.That(result, Is.EqualTo(expectedResult));
            Assert.That(testService.Id, Is.Not.EqualTo(0));
            Assert.That(this.testDataWrapper.context.Services.Count(), Is.EqualTo(numberOfServices + 1));
        }

        [Test]
        public void TestInsertNewService_ShouldThrowException()
        {
            // Arrange.
            DataWrapper testWrapper = new DataWrapper();

            const string name = "Another test new service";
            const decimal price = 100.00m;
            const bool isRecurring = false;

            Service testService = new Service(name, price, isRecurring);
            // Insert now should fail as IDENTITY_INSERT is OFF and does not allow explicit declaration of Primary Keys.
            testService.Id = 123;

            // Act.
            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => testWrapper.InsertNewService(testService));

            // Assert.
            const string expectedExceptionMessage = "An error occurred while saving the entity changes. See the inner exception for details.";
            Assert.That(expectedException.Message, Is.EqualTo(expectedExceptionMessage));
        }

        [Test]
        public void TestUpdateService_ShouldUpdateService()
        {
            Service serviceToUpdate = this.testDataWrapper.SelectAllServices().Where(service => service.Id == 1).First();
            serviceToUpdate.Price = 1.99m;
            DateTime lastUpdateDateTime = serviceToUpdate.LastUpdateDateTime;

            int updateServiceResult = this.testDataWrapper.UpdateService(1);

            Assert.That(updateServiceResult, Is.EqualTo(1));
            Assert.That(serviceToUpdate.LastUpdateDateTime, Is.GreaterThan(lastUpdateDateTime));
        }

        [Test]
        public void TestUpdateService_ShouldThrowException()
        {
            DataWrapper testWrapper = new DataWrapper();

            Service serviceToUpdate = testWrapper.SelectAllServices().First(service => service.Id == 1);

            serviceToUpdate.Name = null;

            // Act.
            DbUpdateException expectedException = Assert.Throws<DbUpdateException>(() => testWrapper.UpdateService(1));

            // Assert.
            const string expectedExceptionMessage = "An error occurred while saving the entity changes. See the inner exception for details.";
            Assert.That(expectedException.Message, Is.EqualTo(expectedExceptionMessage));
        }

        [TearDown]
        public void TearDown()
        {
            Customer customerToReset = this.testDataWrapper.context.Customers.Where(cust => cust.Id == 1).First();
            customerToReset.BusinessContact = "Test Business contact";
            this.testDataWrapper.UpdateCustomer(1);

            Service serviceToReset = this.testDataWrapper.SelectAllServices().Where(service => service.Id == 1).First();
            serviceToReset.Price = 0.99m;
            this.testDataWrapper.UpdateService(1);
        }
    }
}
