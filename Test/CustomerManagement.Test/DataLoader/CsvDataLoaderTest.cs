using Moq;
using CDB.Model;
using CustomerManagement.DataLoader;

namespace CustomerManagement.Test.DataLoader
{
    public class CsvDataLoaderTest
    {
        [Test]
        public void TestLoadCustomersFromFile()
        {
            // Arrange
            Mock<CsvDataLoader> mockDataLoader = new Mock<CsvDataLoader>();

            // Setup mock return data.
            List<Customer> mockCustomerList = new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    CompanyName = "Test company name",
                    BusinessContact = "Jeffery Rogers",
                    EmailAddress = "test.email@hotmail.com",
                    ContactNumber = "012345678",
                    IsActive = true,
                    CreatedDateTime = new DateTime(2025, 2, 1, 19, 15, 22),
                    LastUpdateDateTime = new DateTime(2025, 2, 1, 19, 15, 25),
                    Subscriptions = new List<Subscription>()
                },
                new Customer
                {
                    Id = 2,
                    CompanyName = "Albany House Business Centres",
                    BusinessContact = "Niall Gillen",
                    EmailAddress = "niallgillen@hudsonhouse.co.uk",
                    ContactNumber = "07919367388",
                    IsActive = true,
                    CreatedDateTime = new DateTime(2025, 2, 1, 19, 15, 10),
                    LastUpdateDateTime = new DateTime(2025, 2, 1, 19, 15, 08),
                    Subscriptions = new List<Subscription>()
                },
                new Customer
                {
                    Id = 3,
                    CompanyName = "Thistle Court",
                    BusinessContact = "Christopher Wilkinson",
                    EmailAddress = "Chris.Wilkinson@hotmail.com",
                    ContactNumber = "+447990199712",
                    IsActive = true,
                    CreatedDateTime = new DateTime(2025, 2, 1, 19, 14, 57),
                    LastUpdateDateTime = new DateTime(2025, 2, 1, 19, 14, 59),
                    Subscriptions = new List<Subscription>()
                },
                new Customer
                {
                    Id = 4,
                    CompanyName = "Information Solutions",
                    BusinessContact = "Joe Smith",
                    EmailAddress = "joe.smith@outlook.com",
                    ContactNumber = "1598426984256",
                    IsActive = true,
                    CreatedDateTime = new DateTime(2025, 2, 1, 19, 14, 55),
                    LastUpdateDateTime = new DateTime(2025, 2, 1, 19, 14, 51),
                    Subscriptions = new List<Subscription>()
                }
            };

            mockDataLoader.Setup(loader => loader.ConvertLinesToCustomers(It.IsAny<string[]>())).Returns(mockCustomerList);

            string testCustomerDataFile = @$"{Environment.CurrentDirectory}\DataLoader\Resources\CustomerData2.csv";

            CsvDataLoader testLoader = mockDataLoader.Object;

            // Act.
            List<Customer> newlyLoadedCustomers = testLoader.LoadCustomersFromFile(testCustomerDataFile);

            // Assert.
            Assert.That(newlyLoadedCustomers.Count, Is.EqualTo(4));

            this.ValidateCustomer(newlyLoadedCustomers[0], 1, "Test company name", "Jeffery Rogers", "test.email@hotmail.com", "012345678", true, new DateTime(2025, 2, 1, 19, 15, 22), new DateTime(2025, 2, 1, 19, 15, 25));
            this.ValidateCustomer(newlyLoadedCustomers[1], 2, "Albany House Business Centres", "Niall Gillen", "niallgillen@hudsonhouse.co.uk", "07919367388", true, new DateTime(2025, 2, 1, 19, 15, 10), new DateTime(2025, 2, 1, 19, 15, 08));
            this.ValidateCustomer(newlyLoadedCustomers[2], 3, "Thistle Court", "Christopher Wilkinson", "Chris.Wilkinson@hotmail.com", "+447990199712", true, new DateTime(2025, 2, 1, 19, 14, 57), new DateTime(2025, 2, 1, 19, 14, 59));
            this.ValidateCustomer(newlyLoadedCustomers[3], 4, "Information Solutions", "Joe Smith", "joe.smith@outlook.com", "1598426984256", true, new DateTime(2025, 2, 1, 19, 14, 55), new DateTime(2025, 2, 1, 19, 14, 51));
        }

        private void ValidateCustomer(Customer customer, int Id, string companyName, string businessContact, string emailAddress, string contactNumber, bool isActive, DateTime createdDateTime, DateTime lastUpdateDateTime)
        {
            Assert.That(customer.Id, Is.EqualTo(Id));
            Assert.That(customer.CompanyName, Is.EqualTo(companyName));
            Assert.That(customer.BusinessContact, Is.EqualTo(businessContact));
            Assert.That(customer.EmailAddress, Is.EqualTo(emailAddress));
            Assert.That(customer.ContactNumber, Is.EqualTo(contactNumber));
            Assert.That(customer.IsActive, Is.True);
            Assert.That(customer.CreatedDateTime, Is.EqualTo(createdDateTime));
            Assert.That(customer.LastUpdateDateTime, Is.EqualTo(lastUpdateDateTime));
        }

        [Test]
        [TestCase("CustomerData.csv", 0, 1, 2, 3)]
        [TestCase("CustomerData2.csv", 1, 3, 0, 2)]
        [TestCase("CustomerData3.csv", 3, 2, 0, 1)]
        public void TestDetermineHeaderColumns(string fileName, int businessContactColumn, int contactNumberColumn, int emailAddressColumn, int companyNameColumn)
        {
            // Arrange.
            string testCustomerDataFile = @$"{Environment.CurrentDirectory}\DataLoader\Resources\{fileName}";

            CsvDataLoader testLoader = new CsvDataLoader();

            // Act.
            testLoader.DetermineHeaderColumns(testCustomerDataFile);

            Assert.That(testLoader.businessContactColumn, Is.EqualTo(businessContactColumn));
            Assert.That(testLoader.contactNumberColumn, Is.EqualTo(contactNumberColumn));
            Assert.That(testLoader.emailAddressColumn, Is.EqualTo(emailAddressColumn));
            Assert.That(testLoader.companyNameColumn, Is.EqualTo(companyNameColumn));
        }

        [Test]
        public void TestFormatFileHeaders_HeadersContainSpaces()
        {
            // Arrange.
            string[] headers =
            {
                "company name",
                "business contact",
                "email address",
                "post code",
            };

            CsvDataLoader testLoader = new CsvDataLoader();

            // Act.
            testLoader.FormatFileHeaders(headers);

            // Assert.
            Assert.That(headers.Length, Is.EqualTo(4));

            string[] expectedArray =
            {
                "COMPANYNAME",
                "BUSINESSCONTACT",
                "EMAILADDRESS",
                "POSTCODE",
            };

            Assert.That(headers, Is.EqualTo(expectedArray));
        }

        [Test]
        public void TestFormatFileHeaders_NoSpaces()
        {
            // Arrange.
            string[] headers =
            {
                "postcode",
                "addressline1",
                "companyname",
                "sageref",
            };

            CsvDataLoader testLoader = new CsvDataLoader();

            // Act.
            testLoader.FormatFileHeaders(headers);

            // Assert.
            Assert.That(headers.Length, Is.EqualTo(4));

            string[] expectedFormattedHeaders =
            {
                "POSTCODE",
                "ADDRESSLINE1",
                "COMPANYNAME",
                "SAGEREF"
            };

            Assert.That(headers, Is.EqualTo(expectedFormattedHeaders));
        }

        [Test]
        public void TestFormatFileHeaders_NoChangesRequired()
        {
            // Arrange.
            string[] headers =
            {
                "EMAILADDRESS",
                "COMPANYNAME",
                "CONTACTNUMBER",
                "CITY",
            };

            CsvDataLoader testLoader = new CsvDataLoader();

            // Act.
            testLoader.FormatFileHeaders(headers);

            string[] headersNoChanges =
            {
                "EMAILADDRESS",
                "COMPANYNAME",
                "CONTACTNUMBER",
                "CITY",
            };

            // Assert.
            // Assert no changes to the array of strings.
            Assert.That(headers, Is.EqualTo(headersNoChanges));
        }

        [Test]
        [TestCase(new string[] { "COMPANYNAME", "EMAILADDRESS", "CONTACTNUMBER", "BUSINESSCONTACT" }, 0, 3, 1, 2)]
        [TestCase(new string[] { "EMAILADDRESS", "COMPANYNAME", "BUSINESSCONTACT", "CONTACTNUMBER" }, 1, 2, 0, 3)]
        [TestCase(new string[] { "CONTACTNUMBER", "BUSINESSCONTACT", "EMAILADDRESS", "COMPANYNAME" }, 3, 1, 2, 0)]
        [TestCase(new string[] { "COMPANYNAME", "CONTACTNUMBER", "EMAILADDRESS", "BUSINESSCONTACT" }, 0, 3, 2, 1)]
        [TestCase(new string[] { "EMAILADDRESS", "CONTACTNUMBER", "COMPANYNAME", "BUSINESSCONTACT" }, 2, 3, 0, 1)]
        public void TestSetColumnIndices(string[] headers, int companyName, int businessContact, int emailAddress, int contactNumber)
        {
            // Arrange
            CsvDataLoader testLoader = new CsvDataLoader();

            // Act.
            testLoader.SetColumnIndices(headers);

            // Assert.
            Assert.That(testLoader.companyNameColumn, Is.EqualTo(companyName));
            Assert.That(testLoader.businessContactColumn, Is.EqualTo(businessContact));
            Assert.That(testLoader.emailAddressColumn, Is.EqualTo(emailAddress));
            Assert.That(testLoader.contactNumberColumn, Is.EqualTo(contactNumber));
        }

        [Test]
        public void TestConvertLinesToCustomers_ShouldConvertAllLinesToCustomers()
        {
            // Arrange.
            string[] lines =
            {
                "Clayton Bowen,(01644) 29265,parturient.montes@google.org,Non Incorporated",
                "Katell Garcia,(011732) 81816,enim@icloud.couk,Vivamus Nibh Corp.",
                "Alyssa Valencia,07854 519297,eu.tempor@protonmail.couk,Risus Donec Institute"
            };

            Mock<CsvDataLoader> mockDataLoader = new Mock<CsvDataLoader>();

            mockDataLoader.Setup(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>())).Returns(new Customer());
            mockDataLoader.Setup(dataLoader => dataLoader.ConvertLinesToCustomers(It.IsAny<string[]>())).CallBase();

            CsvDataLoader testDataLoader = mockDataLoader.Object;

            // Act.
            List<Customer> testCustomers = testDataLoader.ConvertLinesToCustomers(lines);

            // Assert.

        }
    }
}
