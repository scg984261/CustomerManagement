using CustomerManagement.ViewModel;
using CDB.Model;

namespace CustomerManagement.Test.ViewModel
{
    public class ServiceItemViewModelTest
    {
        private Service testService;
        private ServiceItemViewModel testServiceItemViewModel;

        [SetUp]
        public void Setup()
        {
            this.testService = new Service
            {
                Id = 52,
                Name = "test service 123",
                Price = 5842.3m,
                IsRecurring = true,
                LastUpdateDateTime = new DateTime(2025, 3, 9, 16, 40, 55),
                CreatedDateTime = new DateTime(2025, 3, 9, 16, 37, 17)
            };

            this.testServiceItemViewModel = new ServiceItemViewModel(this.testService);
        }

        [Test]
        public void TestConstructor_WithService()
        {
            // Arrange/Act..
            this.testServiceItemViewModel = new ServiceItemViewModel(this.testService);

            // Assert.
            Assert.That(this.testServiceItemViewModel.Id, Is.EqualTo(52));
            Assert.That(this.testServiceItemViewModel.Name, Is.EqualTo("test service 123"));
            Assert.That(this.testServiceItemViewModel.Price, Is.EqualTo(5842.3));
            Assert.That(this.testServiceItemViewModel.IsRecurring, Is.True);
            Assert.That(this.testServiceItemViewModel.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 3, 9, 16, 40, 55)));
            Assert.That(this.testServiceItemViewModel.CreatedDateTime, Is.EqualTo(new DateTime(2025, 3, 9, 16, 37, 17)));
        }

        [Test]
        public void TestConstructor_NoArgs()
        {
            // Act.
            ServiceItemViewModel testServiceItemViewModel = new ServiceItemViewModel();

            Assert.That(testServiceItemViewModel.Id, Is.EqualTo(0));
            Assert.That(testServiceItemViewModel.Name, Is.EqualTo(string.Empty));
            Assert.That(testServiceItemViewModel.Price, Is.EqualTo(0.0));
            Assert.That(testServiceItemViewModel.IsRecurring, Is.False);
        }

        [Test]
        public void TestId()
        {
            this.testServiceItemViewModel.Id = 18425;

            Assert.That(this.testServiceItemViewModel.Id, Is.EqualTo(18425));
        }

        [Test]
        public void TestName()
        {
            this.testServiceItemViewModel.Name = "Test new service name";

            Assert.That(this.testServiceItemViewModel.Name, Is.EqualTo("Test new service name"));
        }

        [Test]
        public void TestPrice()
        {
            this.testServiceItemViewModel.Price = 15025.55m;

            Assert.That(this.testServiceItemViewModel.Price, Is.EqualTo(15025.55m));
        }

        [Test]
        [TestCase(5842.3, "£5842.30")]
        [TestCase(0.99, "£0.99")]
        [TestCase(531.897, "£531.90")]
        [TestCase(284.312, "£284.31")]
        [TestCase(1, "£1.00")]
        public void TestPriceFormatted_ShouldReturnFormattedPrice(decimal newPrice, string expectedPriceFormatted)
        {
            // Act.
            this.testServiceItemViewModel.Price = newPrice;

            // Assert.
            Assert.That(this.testServiceItemViewModel.PriceFormatted, Is.EqualTo(expectedPriceFormatted));
        }

        [Test]
        public void TestIsRecurring_ShouldReturnTrue()
        {
            this.testServiceItemViewModel.IsRecurring = true;

            Assert.That(this.testServiceItemViewModel.IsRecurring, Is.True);
        }

        [Test]
        public void TestIsRecurring_ShouldReturnFalse()
        {
            this.testServiceItemViewModel.IsRecurring = false;

            Assert.That(this.testServiceItemViewModel.IsRecurring, Is.False);
        }

        [Test]
        public void TestCreatedDateTime()
        {
            Service testService = new Service
            {
                CreatedDateTime = new DateTime(2025, 04, 14, 20, 42, 21)
            };

            this.testServiceItemViewModel = new ServiceItemViewModel(testService);

            Assert.That(this.testServiceItemViewModel.CreatedDateTime, Is.EqualTo(new DateTime(2025, 04, 14, 20, 42, 21)));
        }

        [Test]
        public void TestLastUpdateDateTime()
        {
            Service testService = new Service
            {
                LastUpdateDateTime = new DateTime(2025, 04, 14, 20, 39, 19)
            };

            this.testServiceItemViewModel = new ServiceItemViewModel(testService);

            Assert.That(this.testServiceItemViewModel.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 04, 14, 20, 39, 19)));
        }
    }
}
