using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel.ServiceViewModels;
using CustomerManagement.Windows;
using Moq;

namespace CustomerManagement.Test.ViewModel.ServiceViewModels
{
    public class ServiceViewModelBaseTest
    {
        private NavigationStore testNavigationStore;
        private Mock<IServiceDataProvider> mockServiceDataProvider;
        private IServiceDataProvider mockServiceDataProviderObject;
        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper messageBoxHelperObject;

        private ServicesViewModel testServicesViewModel;

        private ServiceViewModelBase testServiceViewModelBase;

        [SetUp]
        public void SetUp()
        {
            this.testNavigationStore = new NavigationStore();
            this.mockServiceDataProvider = new Mock<IServiceDataProvider>();
            this.mockServiceDataProviderObject = this.mockServiceDataProvider.Object;
            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.messageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testServicesViewModel = new ServicesViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);
            ServiceViewModelBase.ParentServicesViewModel = this.testServicesViewModel;

            this.testServiceViewModelBase = new ServiceViewModelBase(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            Assert.That(this.testServiceViewModelBase.Name, Is.EqualTo(string.Empty));
            Assert.That(this.testServiceViewModelBase.Price, Is.EqualTo(0.0m));
            Assert.That(this.testServiceViewModelBase.PriceString, Is.EqualTo("0.0"));
            Assert.That(this.testServiceViewModelBase.PriceFormatted, Is.EqualTo("£0.00"));
            Assert.That(this.testServiceViewModelBase.IsRecurring, Is.False);
            Assert.That(this.testServiceViewModelBase.NavigateBackCommand, Is.Not.Null);
            Assert.That(this.testServiceViewModelBase.SaveServiceCommand, Is.Not.Null);
        }

        [Test]
        public void TestName_ShouldTriggerValidation()
        {
            // Arrange/act.
            this.testServiceViewModelBase.Name = "";
            IEnumerable<string>? errors = testServiceViewModelBase.GetErrors(nameof(testServiceViewModelBase.Name)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceViewModelBase.Name, Is.EqualTo(string.Empty));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Name of service cannot be blank"));
            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestName_ShouldClearErrors()
        {
            // Arrange/act.
            this.testServiceViewModelBase.Name = "Different test service name.";
            IEnumerable<string>? errors = testServiceViewModelBase.GetErrors(nameof(testServiceViewModelBase.Name)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceViewModelBase.Name, Is.EqualTo("Different test service name."));
            Assert.That(errors.Count, Is.EqualTo(0));
            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.True);
        }

        [Test]
        [TestCase(1.89, "1.89", "£1.89")]
        [TestCase(505.272, "505.272", "£505.27")]
        [TestCase(793.816, "793.816", "£793.82")]
        [TestCase(0.82, "0.82", "£0.82")]
        [TestCase(5, "5", "£5.00")]
        [TestCase(0.99,"0.99", "£0.99")]
        [TestCase(500.00, "500", "£500.00")]
        [TestCase(0.10, ".1", "£0.10")]
        [TestCase(225.423, "225.423", "£225.42")]
        [TestCase(3894.5276, "3894.5276", "£3894.53")]
        public void TestPrice(decimal price, string expectedPriceString, string expectedFormattedPrice)
        {
            // Arrange/Act.
            this.testServiceViewModelBase.Price = price;

            Assert.That(this.testServiceViewModelBase.Price, Is.EqualTo(price));
            Assert.That(this.testServiceViewModelBase.PriceFormatted, Is.EqualTo(expectedFormattedPrice));
        }

        [Test]
        public void TestPriceString_PriceIsEmptyString_ShouldTriggerValidation()
        {
            // Arrange/act.
            this.testServiceViewModelBase.PriceString = "";
            IEnumerable<string>? errors = testServiceViewModelBase.GetErrors(nameof(testServiceViewModelBase.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceViewModelBase.PriceString, Is.EqualTo(string.Empty));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Price cannot be blank!"));
            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestPriceString_PriceIsNotValidDecimal_ShouldTriggerValidation()
        {
            // Arrange/Act.
            this.testServiceViewModelBase.PriceString = "Not valid decimal value.";
            IEnumerable<string>? errors = testServiceViewModelBase.GetErrors(nameof(testServiceViewModelBase.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceViewModelBase.PriceString, Is.EqualTo("Not valid decimal value."));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Value must be a valid decimal."));
            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.False);
        }

        [Test]
        [TestCase("0.5", 0.5, "£0.50")]
        [TestCase("2842587.38429", 2842587.38429, "£2842587.38")]
        [TestCase("1.889", 1.889, "£1.89")]
        [TestCase("55", 55.0, "£55.00")]
        [TestCase(".22", 0.22, "£0.22")]
        public void TestPriceString_ShouldClearErrors(string inputPriceString, decimal expectedPrice, string expectedPriceFormatted)
        {
            // Arrange/Act.
            this.testServiceViewModelBase.PriceString = inputPriceString;
            IEnumerable<string>? errors = testServiceViewModelBase.GetErrors(nameof(testServiceViewModelBase.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testServiceViewModelBase.PriceString, Is.EqualTo(inputPriceString));
            Assert.That(this.testServiceViewModelBase.Price, Is.EqualTo(expectedPrice));
            Assert.That(this.testServiceViewModelBase.PriceFormatted, Is.EqualTo(expectedPriceFormatted));
        }

        [Test]
        public void TestPriceFormatted()
        {
            // Arrange/Act.
            // Set the price to some valid value.
            this.testServiceViewModelBase.PriceString = ".89";

            Assert.That(this.testServiceViewModelBase.PriceFormatted, Is.EqualTo("£0.89"));
        }

        [Test]
        public void TestIsRecurring_ShouldReturnTrue()
        {
            this.testServiceViewModelBase.IsRecurring = true;

            Assert.That(this.testServiceViewModelBase.IsRecurring, Is.True);
        }

        [Test]
        public void TestIsRecurring_ShouldReturnFalse()
        {
            this.testServiceViewModelBase.IsRecurring = false;

            Assert.That(this.testServiceViewModelBase.IsRecurring, Is.False);
        }

        [Test]
        public void TestCanSaveService_NameIsEmpty_ShouldReturnFalse()
        {
            this.testServiceViewModelBase.Name = "";

            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_PriceStringIsEmpty_ShouldReturnFalse()
        {
            this.testServiceViewModelBase.PriceString = "";

            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_PriceStringNotValidDecimalValue_ShouldReturnFalse()
        {
            this.testServiceViewModelBase.PriceString = "Not a decimal value";

            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_ShouldReturnTrue()
        {
            // Act.
            this.testServiceViewModelBase.Name = "Test new service";
            this.testServiceViewModelBase.PriceString = "1.95";
            this.testServiceViewModelBase.IsRecurring = true;

            // Assert.
            // Should now be able to save the Service Record.
            Assert.That(this.testServiceViewModelBase.CanSaveService(new object()), Is.True);
        }

        [Test]
        public void TestCancel()
        {
            // Arrange.
            // Set the properties - simulate entering some input.
            this.testServiceViewModelBase.Name = "Different name - should be reset after cancellation.";
            this.testServiceViewModelBase.PriceString = "2.29";
            this.testServiceViewModelBase.IsRecurring = false;

            // Act.
            this.testServiceViewModelBase.Cancel(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }

        [Test]
        public void TestNavigateBack()
        {
            // Act.
            this.testServiceViewModelBase.NavigateBack(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }
    }
}
