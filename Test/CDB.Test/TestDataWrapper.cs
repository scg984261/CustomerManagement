using CDB.Model;
using Microsoft.EntityFrameworkCore;
using Moq;

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

        }
    }
}
