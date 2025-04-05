using Microsoft.EntityFrameworkCore;
using CDB.Model;
using Moq;
using System.Data;

namespace CDB.Test
{
    public class TestDataWrapper
    {
        [Test]
        public void TestGetConnectionStringFromConfig_ShouldGetConnectionString()
        {
            DataWrapper.GetConnectionStringFromConfig();
            Assert.That(DataWrapper.DatabaseConnectionString, Is.EqualTo("Server=INVALIDSERVERNAME\\SQLEXPRESS;Initial Catalog=TEST_DB;trustservercertificate=True;trusted_connection=true"));
        }

        [Test]
        public void TestSelectAllCustomers_ShouldReturnListOfCustomers()
        {
            Mock<CdbContext> mockDbContext = new Mock<CdbContext>();

            Customer testCustomer = new Customer()
            {
                Id = 4,
                CompanyName = "Nunc Est LLC",
                BusinessContact = "Cruz Pugh",
                EmailAddress = "sed.nec@aol.edu",
                ContactNumber = "(028) 7723 3562",
                IsActive = true,
                CreatedDateTime = new DateTime(),
                LastUpdateDateTime = new DateTime()
            };

            List<Customer> customers = new List<Customer>()
            {
                testCustomer
            };

            IQueryable<Customer> customerQueryable = customers.AsQueryable();

            mockDbContext.Setup(context => context.RunSql<Customer>("SelectAllCustomers")).Returns(customerQueryable);
            CdbContext mockDbContextObject = mockDbContext.Object;

            DataWrapper testWrapper = new DataWrapper
            {
                context = mockDbContextObject
            };

            List<Customer> testCustomers = testWrapper.SelectAllCustomers();

            Assert.That(testCustomers[0].Equals(customerQueryable.First()));
        }

        [Test]
        public void TestSelectAllCustomers_ShouldCatchException()
        {
            Mock<CdbContext> mockDbContext = new Mock<CdbContext>();

            DataException dataException = new DataException("Test data exception created in test.");

            mockDbContext.Setup(context => context.RunSql<Customer>("SelectAllCustomers")).Throws(dataException);

            CdbContext mockDbContextObject = mockDbContext.Object;

            DataWrapper testWrapper = new DataWrapper
            {
                context = mockDbContextObject
            };

            List<Customer> testCustomers = testWrapper.SelectAllCustomers();

            Assert.That(testCustomers.Count, Is.EqualTo(0));
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

            DataWrapper testWrapper = new DataWrapper();

            int numberOfCustomers = testWrapper.context.Customers.Count();

            // Act.
            testWrapper.InsertNewCustomer(testCustomer);
            Customer newlyInsertedCustomer = testWrapper.context.Customers.OrderByDescending(cust => cust.Id).First();

            int newNumberOfCustomers = testWrapper.context.Customers.Count();

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

            int numberOfCustomers = testWrapper.context.Customers.Count();

            Assert.That(() => testWrapper.InsertNewCustomer(testCustomer), Throws.Exception.TypeOf<DbUpdateException>());
        }
    }
}
