using System.Data;
using CustomerManagement.Data;
using CDB;
using CDB.Model;
using Moq;

namespace CustomerManagement.Test.Data
{
    public class ServiceDataProviderTest
    {
        private Mock<IDataWrapper> mockDataWrapper;
        private IDataWrapper mockDataWrapperObject;
        private ServiceDataProvider testServiceDataProvider;

        [SetUp]
        public void Setup()
        {
            this.mockDataWrapper = new Mock<IDataWrapper>();
            this.mockDataWrapperObject = this.mockDataWrapper.Object;
            this.testServiceDataProvider = new ServiceDataProvider(this.mockDataWrapperObject);
        }

        [Test]
        public void TestGetAll_ShouldReturnServices()
        {
            // Arrange.
            List<Service> testServices = new List<Service>
            {
                new Service
                {
                    Id = 1,
                    Name = "CALLSANSWERED",
                    Price = 1.85m,
                    IsRecurring = true,
                    CreatedDateTime = new DateTime(),
                    LastUpdateDateTime = new DateTime()
                },
                new Service
                {
                    Id = 2,
                    Name = "CALLSANSWEREDREGULAR",
                    Price = 2.10m,
                    IsRecurring = true,
                    CreatedDateTime = new DateTime(),
                    LastUpdateDateTime = new DateTime()
                },
                new Service
                {
                    Id = 3,
                    Name = "TESTSERVICENON_RECURRING",
                    Price = 220.00m,
                    IsRecurring = false,
                    CreatedDateTime = new DateTime(),
                    LastUpdateDateTime = new DateTime()
                },
                new Service
                {
                    Id = 4,
                    Name = "TESTSERVICENON_RECURRING2",
                    Price = 240.00m,
                    IsRecurring = false,
                    CreatedDateTime = new DateTime(),
                    LastUpdateDateTime = new DateTime()
                }
            };

            this.mockDataWrapper.Setup(dataWrapper => dataWrapper.SelectAllServices()).Returns(testServices);

            // Act.
            List<Service> services = this.testServiceDataProvider.GetAll();

            // Assert.
            Assert.That(services.Count, Is.EqualTo(4));
        }

        [Test]
        public void TestGetAll_ShouldThrowException()
        {
            // Arrange.
            DataException testException = new DataException("Test SQL Exception attempting to get services.");
            this.mockDataWrapper.Setup(dataWrapper => dataWrapper.SelectAllServices()).Throws(testException);

            // Act.
            DataException expectedException = Assert.Throws<DataException>(() => this.testServiceDataProvider.GetAll());

            // Assert.
            Assert.That(expectedException.Message, Is.EqualTo("Test SQL Exception attempting to get services."));
        }

        [Test]
        public void TestInsertNewService_ShouldInsertNewService()
        {
            // Arrange.
            Service service = new Service
            {
                Id = 101284,
                Name = "Random new Service Name.",
                Price = 5896124.25m
            };

            this.mockDataWrapper.Setup(dataWrapper => dataWrapper.InsertNewService(service)).Returns(1);

            // Act.
            int result = this.testServiceDataProvider.InsertNewService(service);

            // Assert.
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void TestInsertNewService_ShouldThrowException()
        {
            // Arrange.
            Service testService = new Service
            {
                Id = 55,
                Name = "New service that should throw an exception.",
                Price = 0.99m
            };

            const string testExceptionMessage = "Test SQL Exception attempting to Insert new service.";
            DataException testException = new DataException(testExceptionMessage);
            mockDataWrapper.Setup(dataWrapper => dataWrapper.InsertNewService(testService)).Throws(testException);

            // Act.
            DataException expectedException = Assert.Throws<DataException>(() => this.testServiceDataProvider.InsertNewService(testService));

            // Assert
            Assert.That(expectedException.Message, Is.EqualTo(testExceptionMessage));
        }

        [Test]
        public void TestUpdateService_ShouldUpdateService()
        {
            // Arrange.
            const int testServiceId = 105;
            mockDataWrapper.Setup(dataWrapper => dataWrapper.UpdateService(testServiceId)).Returns(1);

            // Act.
            int result = this.testServiceDataProvider.UpdateService(105);

            // Assert.
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void TestUpdateService_ShouldThrowException()
        {
            // Arrange.
            const string testExceptionMessage = "Test SQL Exception attempting to update Service.";
            DataException testException = new DataException(testExceptionMessage);

            mockDataWrapper.Setup(dataWrapper => dataWrapper.UpdateService(106)).Throws(testException);

            // Act.
            DataException expectedException = Assert.Throws<DataException>(() => testServiceDataProvider.UpdateService(106));

            // Assert.
            Assert.That(expectedException.Message, Is.EqualTo(testExceptionMessage));
        }
    }
}
