using CustomerManagement.DataLoader;

namespace CustomerManagement.Test.DataLoader
{
    public class DataLoaderTest
    {
        [Test]
        public void TestLoadCustomersFromFile()
        {
            // Arrange
            CsvDataLoader testLoader = new CsvDataLoader();


        }

        [Test]
        public void TestDetermineHeaderColumns()
        {
            // Arrange.
            string testCustomerDataFile = @$"{Environment.CurrentDirectory}\DataLoader\Resources\CustomerData.csv";

            CsvDataLoader testLoader = new CsvDataLoader();

            // Act.
            testLoader.DetermineHeaderColumns(testCustomerDataFile);

            Assert.That(testLoader.businessContactColumn, Is.EqualTo(0));
            Assert.That(testLoader.contactNumberColumn, Is.EqualTo(1));
            Assert.That(testLoader.emailAddressColumn, Is.EqualTo(2));
            Assert.That(testLoader.companyNameColumn, Is.EqualTo(3));
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
        public void TestSetColumnIndices()
        {
            // Arrange
            CsvDataLoader testLoader = new CsvDataLoader();

            string[] headers =
            {
                "COMPANYNAME",
                "EMAILADDRESS",
                "CONTACTNUMBER",
                "BUSINESSCONTACT",
            };

            // Act.
            testLoader.SetColumnIndices(headers);

            // Assert.
            Assert.That(testLoader.companyNameColumn, Is.EqualTo(0));
            Assert.That(testLoader.businessContactColumn, Is.EqualTo(3));
            Assert.That(testLoader.emailAddressColumn, Is.EqualTo(1));
            Assert.That(testLoader.contactNumberColumn, Is.EqualTo(2));
        }
    }
}
