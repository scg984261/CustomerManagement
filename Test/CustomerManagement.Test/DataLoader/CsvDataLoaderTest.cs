using System.Data;
using CustomerManagement.DataLoader;
using CDB;
using CDB.Model;
using Moq;

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

            for (int i = 0; i < newlyLoadedCustomers.Count; i++)
            {
                Assert.That(newlyLoadedCustomers[i].Equals(mockCustomerList[i]));
            }
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
                "Katell Garcia,(011732) 81816,enim@icloud.co.uk,Vivamus Nibh Corp.",
                "Alyssa Valencia,07854 519297,eu.tempor@protonmail.co.uk,Risus Donec Institute"
            };

            Mock<CsvDataLoader> mockDataLoader = new Mock<CsvDataLoader>();

            Customer customer1 = new Customer
            {
                Id = 256,
                CompanyName = "Non Incorporated",
                BusinessContact = "Clayton Bowen",
                ContactNumber = "(01644) 29265",
                EmailAddress = "parturient.montes@google.org",
                IsActive = true,
                CreatedDateTime = new DateTime(2024, 5, 3, 21, 52, 57),
                LastUpdateDateTime = new DateTime(2024, 7, 22, 19, 41, 44)
            };

            Customer customer2 = new Customer
            {
                Id = 484,
                CompanyName = "Vivamus Nibh Corp.",
                BusinessContact = "Katell Garcia",
                ContactNumber = "(011732) 81816",
                EmailAddress = "enim@icloud.co.uk",
                IsActive = true,
                CreatedDateTime = new DateTime(2025, 2, 1, 11, 01, 23),
                LastUpdateDateTime = new DateTime(2025, 2, 1, 10, 33, 12)
            };

            Customer customer3 = new Customer
            {
                Id = 151,
                CompanyName = "Risus Donec Institute",
                BusinessContact = "Alyssa Valencia",
                ContactNumber = "07854 519297",
                EmailAddress = "eu.tempor@protonmail.co.uk",
                IsActive = true,
                CreatedDateTime = new DateTime(2025, 1, 30, 20, 59, 01),
                LastUpdateDateTime = new DateTime(2025, 1, 30, 20, 59, 00)
            };

            mockDataLoader.SetupSequence(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>()))
                .Returns(customer1)
                .Returns(customer2)
                .Returns(customer3);

            mockDataLoader.Setup(dataLoader => dataLoader.ConvertLinesToCustomers(It.IsAny<string[]>())).CallBase();

            CsvDataLoader testDataLoader = mockDataLoader.Object;

            // Act.
            List<Customer> testCustomers = testDataLoader.ConvertLinesToCustomers(lines);

            // Assert.
            Assert.That(testCustomers.Count, Is.EqualTo(3));

            Assert.That(testCustomers[0].Equals(customer1));
            Assert.That(testCustomers[1].Equals(customer2));
            Assert.That(testCustomers[2].Equals(customer3));
        }

        [Test]
        public void TestConvertLinesToCustomers_ShouldCatchException()
        {
            // Arrange
            string[] csvFileLines =
            {
                "Paul Everett,(01746) 433844,curabitur.egestas.nunc@aol.ca,Cursus A Incorporated",
                "Fitzgerald Rivas,(01337) 39353,id.libero@yahoo.edu,Et Rutrum Non LLP",
                "Keegan Briggs,07624 307025,mauris.rhoncus.id@hotmail.org,Libero Proin Sed Incorporated"
            };

            Mock<CsvDataLoader> mockDataLoader = new Mock<CsvDataLoader>();

            Customer customer1 = new Customer
            {
                Id = 456664,
                BusinessContact = "Paul Everett",
                ContactNumber = "(01746) 433844",
                EmailAddress = "curabitur.egestas.nunc@aol.ca",
                CompanyName = "Cursus A Incorporated",
                IsActive = true,
                CreatedDateTime = new DateTime(2024, 09, 27, 18, 47, 36),
                LastUpdateDateTime = new DateTime(2024, 06, 16, 15, 32, 20)
            };

            // Second attempt to insert customer should throw and catch exception.
            Exception testException = new Exception("Test Exception message.");

            Customer customer3 = new Customer
            {
                Id = 456665,
                BusinessContact = "Keegan Briggs",
                ContactNumber = "07624 307025",
                EmailAddress = "mauris.rhoncus.id@hotmail.org",
                CompanyName = "Libero Proin Sed Incorporated",
                IsActive = true,
                CreatedDateTime = new DateTime(2024, 07, 18, 19, 02, 42),
                LastUpdateDateTime = new DateTime(2024, 02, 14, 04, 41, 13)
            };

            mockDataLoader.SetupSequence(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>()))
                .Returns(customer1)
                .Throws(testException)
                .Returns(customer3);

            mockDataLoader.Setup(dataLoader => dataLoader.ConvertLinesToCustomers(It.IsAny<string[]>())).CallBase();

            CsvDataLoader testDataLoader = mockDataLoader.Object;

            // Act
            List<Customer> testCustomers = testDataLoader.ConvertLinesToCustomers(csvFileLines);

            // Assert
            Assert.That(testCustomers.Count, Is.EqualTo(2));

            Assert.That(testCustomers[0].Equals(customer1));
            Assert.That(testCustomers[1].Equals(customer3));
        }

        [Test]
        public void TestInsertCustomer_ShouldInsertCustomer()
        {
            // Arrange
            string line = "Ray Mckenzie,(016977) 6234,pretium.et@google.com,Nunc In At Inc.";

            Mock<CsvDataLoader> mockDataLoader = new Mock<CsvDataLoader>();

            Customer mockCustomer = new Customer
            {
                Id = 1,
                BusinessContact = "Ray Mckenzie",
                ContactNumber = "(016977) 6234",
                EmailAddress = "pretium.et@google.com",
                CompanyName = "Nunc In At Inc.",
                IsActive = true,
                CreatedDateTime = new DateTime(2024, 02, 11, 07, 24, 43),
                LastUpdateDateTime = new DateTime(2024, 10, 06, 15, 58, 35)
            };

            mockDataLoader.Setup(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>())).CallBase();
            mockDataLoader.Setup(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(mockCustomer);

            CsvDataLoader testDataLoader = mockDataLoader.Object;

            testDataLoader.businessContactColumn = 0;
            testDataLoader.contactNumberColumn = 1;
            testDataLoader.emailAddressColumn = 2;
            testDataLoader.companyNameColumn = 3;

            // Act
            Customer testCustomer = testDataLoader.InsertCustomer(line);

            // Assert
            Assert.That(testCustomer.Equals(testCustomer));
        }

        [Test]
        public void TestInsertCustomer_CustomerDataColumnsNotSet_ShouldThrowOutOfRangeException()
        {
            // Arrange.
            string line = "Wallace Padilla,(0111) 257 5655,diam.dictum @google.couk,Faucibus Company";

            Mock<CsvDataLoader> mockDataLoader = new Mock<CsvDataLoader>();
            mockDataLoader.Setup(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>())).CallBase();

            CsvDataLoader testDataLoader = mockDataLoader.Object;

            // Act & Assert.
            Assert.That(() => testDataLoader.InsertCustomer(line), Throws.Exception.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void TestInsertCustomer_ExceptionThrownInsertingCustomer_ShouldThrowException()
        {
            // Arrange.
            string line = "William James,0368 124 1829,purus.duis@yahoo.ca,Phasellus Ornare Consulting";

            Mock<CsvDataLoader> mockDataLoader = new Mock<CsvDataLoader>();
            mockDataLoader.Setup(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>())).CallBase();

            DataException testException = new DataException("Test exception message.");
            mockDataLoader.Setup(dataLoader => dataLoader.InsertCustomer(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(testException);

            CsvDataLoader testDataLoader = mockDataLoader.Object;

            testDataLoader.businessContactColumn = 0;
            testDataLoader.contactNumberColumn = 1;
            testDataLoader.emailAddressColumn = 2;
            testDataLoader.companyNameColumn = 3;

            // Act and assert.
            Assert.That(() => testDataLoader.InsertCustomer(line), Throws.Exception.TypeOf<DataException>());
        }

        [Test]
        public void TestInsertCustomer_ShouldInsertNewCustomer()
        {
            // Arrange.
            Mock<IDataWrapper> mockDatabaseWrapper = new Mock<IDataWrapper>();
            string companyName = "Capital Business Centre";
            string businessContact = "Kayla Clements";
            string emailAddress = "Kayla.clements@weroad.co.uk";
            string contactNumber = "0131 221 1234";
            DateTime createdDateTime = new DateTime(2024, 05, 16, 19, 58, 30);
            DateTime lastUpdateDateTime = new DateTime(2024, 06, 22, 02, 26, 45);

            Customer newCustomer = new Customer
            {
                Id = 584,
                CompanyName = companyName,
                BusinessContact = businessContact,
                EmailAddress = emailAddress,
                ContactNumber = contactNumber,
                IsActive = true,
                CreatedDateTime = createdDateTime,
                LastUpdateDateTime = lastUpdateDateTime
            };

            mockDatabaseWrapper.Setup(dataWrapper => dataWrapper.InsertNewCustomer(companyName, businessContact, emailAddress, contactNumber)).Returns(newCustomer);

            IDataWrapper testDataWrapper = mockDatabaseWrapper.Object;

            CsvDataLoader testDataLoader = new CsvDataLoader
            {
                dataWrapper = testDataWrapper
            };

            // Act.
            Customer newlyInsertedCustomer = testDataLoader.InsertCustomer(companyName, businessContact, emailAddress, contactNumber);

            // Assert.
            Assert.That(newlyInsertedCustomer.Equals(newCustomer));
        }

        [Test]
        public void TestInsertCustomer_ShouldThrowException()
        {
            // Arrange.
            Mock<IDataWrapper> mockDatabaseWrapper = new Mock<IDataWrapper>();

            string companyName = "Capital Business Centre";
            string businessContact = "Kayla Clements";
            string emailAddress = "Kayla.clements@weroad.co.uk";
            string contactNumber = "0131 221 1234";
            DateTime createdDateTime = new DateTime(2024, 05, 16, 19, 58, 30);
            DateTime lastUpdateDateTime = new DateTime(2024, 06, 22, 02, 26, 45);

            DataException dataException = new DataException("Test Data Exception");

            mockDatabaseWrapper.Setup(dataWrapper => dataWrapper.InsertNewCustomer(companyName, businessContact, emailAddress, contactNumber)).Throws(dataException);

            IDataWrapper testDataWrapper = mockDatabaseWrapper.Object;

            CsvDataLoader testDataLoader = new CsvDataLoader
            {
                dataWrapper = testDataWrapper
            };

            Assert.That(() => testDataLoader.InsertCustomer(companyName, businessContact, emailAddress, contactNumber), Throws.Exception.TypeOf<DataException>());
        }
    }
}
