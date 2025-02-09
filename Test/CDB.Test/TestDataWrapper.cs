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
            string companyName = "Faucibus Company";
            string businessContact = "Wallace Padilla";
            string emailAddress = "diam.dictum@google.co.uk";
            string contactNumber = "(0111) 257 5655";

            Customer testCustomer = new Customer
            {
                Id = 582,
                CompanyName = companyName,
                BusinessContact = businessContact,
                EmailAddress = emailAddress,
                ContactNumber = contactNumber,
                IsActive = true,
                CreatedDateTime = new DateTime(2025, 02, 05, 22, 04, 57),
                LastUpdateDateTime = new DateTime(2025, 02, 05, 22, 04, 57),
            };

            List<Customer> customerResultSet = new List<Customer>
            {
                testCustomer
            };

            IQueryable<Customer> queryableResults = customerResultSet.AsQueryable();

            Mock<CdbContext> mockDbContext = new Mock<CdbContext>();

            mockDbContext.Setup(context => context.RunSql<Customer>(It.IsAny<FormattableString>())).Returns(queryableResults);

            CdbContext mockDbContextObject = mockDbContext.Object;

            DataWrapper testWrapper = new DataWrapper
            {
                context = mockDbContextObject
            };

            // Act.
            Customer testNewlyInsertedCustomer = testWrapper.InsertNewCustomer(companyName, businessContact, emailAddress, contactNumber);

            Assert.That(testCustomer.Equals(testNewlyInsertedCustomer));
        }

        [Test]
        public void TestInsertNewCustomer_ShouldThrowException()
        {
            string companyName = "Faucibus Company";
            string businessContact = "Wallace Padilla";
            string emailAddress = "diam.dictum@google.co.uk";
            string contactNumber = "(0111) 257 5655";

            Mock<CdbContext> mockDbContext = new Mock<CdbContext>();

            DataException dataException = new DataException("Test exception occurred attempting to insert new customer record!");

            mockDbContext.Setup(context => context.RunSql<Customer>(It.IsAny<FormattableString>())).Throws(dataException);

            CdbContext mockDbContextObject = mockDbContext.Object;

            DataWrapper testWrapper = new DataWrapper
            {
                context = mockDbContextObject
            };

            Assert.That(() => testWrapper.InsertNewCustomer(companyName, businessContact, emailAddress, contactNumber), Throws.Exception.TypeOf<DataException>());
        }
    }
}
