using System.Data;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;
using CustomerManagement.Windows;
using CDB.Model;
using Moq;

namespace CustomerManagement.Test.ViewModel
{
    public class NewServiceViewModelTest
    {
        private NavigationStore testNavigationStore;
        private Mock<IServiceDataProvider> mockServiceDataProvider;
        private IServiceDataProvider mockServiceDataProviderObject;
        private NewServiceViewModel testNewServiceViewModel;
        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper messageBoxHelperObject;
        private ServicesViewModel testServicesViewModel;

        [SetUp]
        public void Setup()
        {
            this.testNavigationStore = new NavigationStore();
            this.mockServiceDataProvider = new Mock<IServiceDataProvider>();
            this.mockServiceDataProviderObject = this.mockServiceDataProvider.Object;
            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.messageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testServicesViewModel = new ServicesViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);
            NewServiceViewModel.ParentServicesViewModel = this.testServicesViewModel;

            this.testNewServiceViewModel = new NewServiceViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            this.testNewServiceViewModel = new NewServiceViewModel(this.testNavigationStore, this.mockServiceDataProviderObject, this.messageBoxHelperObject);

            Assert.That(this.testNewServiceViewModel.NavigateBackDelegateCommand, Is.Not.Null);
            Assert.That(this.testNewServiceViewModel.SaveServiceCommand, Is.Not.Null);
            Assert.That(this.testNewServiceViewModel.Name, Is.EqualTo(string.Empty));
            Assert.That(this.testNewServiceViewModel.Price, Is.EqualTo(0.0m));
            Assert.That(this.testNewServiceViewModel.PriceString, Is.EqualTo(string.Empty));
            Assert.That(this.testNewServiceViewModel.IsRecurring, Is.False);
        }

        [Test]
        public void TestName_ShouldTriggerValidation()
        {
            // Act.
            this.testNewServiceViewModel.Name = "";
            IEnumerable<string>? errors = this.testNewServiceViewModel.GetErrors(nameof(this.testNewServiceViewModel.Name)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Name of service cannot be blank"));
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestName_ShouldClearErrors()
        {
            // Act.
            this.testNewServiceViewModel.Name = "Test name value.";
            IEnumerable<string>? errors = this.testNewServiceViewModel.GetErrors(nameof(this.testNewServiceViewModel.Name)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestPrice()
        {
            // Act.
            this.testNewServiceViewModel.Price = 1.99m;

            // Assert.
            Assert.That(this.testNewServiceViewModel.Price, Is.EqualTo(1.99m));
        }

        [Test]
        public void TestPriceString_ShouldTriggerValidation_ZeroLengthString()
        {
            // Act.
            this.testNewServiceViewModel.PriceString = "";
            IEnumerable<string>? errors = this.testNewServiceViewModel.GetErrors(nameof(this.testNewServiceViewModel.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testNewServiceViewModel.Price, Is.EqualTo(0.0m));
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Price cannot be blank!"));
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestPriceString_ShouldTriggerValidation_NotValidDecimal()
        {
            // Act.
            this.testNewServiceViewModel.PriceString = "Not a number";
            IEnumerable<string>? errors = this.testNewServiceViewModel.GetErrors(nameof(this.testNewServiceViewModel.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Value must be a valid decimal."));
            Assert.That(this.testNewServiceViewModel.Price, Is.EqualTo(0.0m));
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        [TestCase("0.99", 0.99)]
        [TestCase("500", 500.00)]
        [TestCase(".1", 0.10)]
        public void TestPriceString_ShouldClearErrors(string testPriceString, decimal expectedPrice)
        {
            // Act.
            this.testNewServiceViewModel.PriceString = testPriceString;
            IEnumerable<string>? errors = this.testNewServiceViewModel.GetErrors(nameof(this.testNewServiceViewModel.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(0));
            Assert.That(this.testNewServiceViewModel.Price, Is.EqualTo(expectedPrice));
        }

        [Test]
        [TestCase("1.5", "£1.50", 0)]
        [TestCase("250", "£250.00", 0)]
        [TestCase("85.22", "£85.22", 0)]
        [TestCase("", "£0.00", 1)]
        [TestCase("Not a number", "£0.00", 1)]
        public void TestPriceFormatted(string inputPrice, string expectedFormattedPrice, int expectedNumberOfErrors)
        {
            // Act.
            this.testNewServiceViewModel.PriceString = inputPrice;
            IEnumerable<string>? errors = this.testNewServiceViewModel.GetErrors(nameof(this.testNewServiceViewModel.PriceString)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(expectedNumberOfErrors));
            Assert.That(this.testNewServiceViewModel.PriceFormatted, Is.EqualTo(expectedFormattedPrice));
        }

        [Test]
        public void TestRecurring_ShouldReturnTrue()
        {
            // Act.
            this.testNewServiceViewModel.IsRecurring = true;

            // Assert.
            Assert.That(this.testNewServiceViewModel.IsRecurring, Is.True);
        }

        [Test]
        public void TestIsRecurring_ShouldReturnFalse()
        {
            // Act.
            this.testNewServiceViewModel.IsRecurring = false;

            // Assert.
            Assert.That(this.testNewServiceViewModel.IsRecurring, Is.False);
        }

        [Test]
        public void TestNavigateBack()
        {
            // Act.
            this.testNewServiceViewModel.NavigateBack(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }

        [Test]
        public void TestSaveService_ShouldSuccessfullySaveService()
        {
            // Arrange.
            this.testNewServiceViewModel.Name = "Test service name";
            this.testNewServiceViewModel.PriceString = "1.25";
            this.testNewServiceViewModel.IsRecurring = true;
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.True);

            // Set up mock data provider.
            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.InsertNewService(It.IsAny<Service>())).Returns(1);

            // Act.
            // Save the service.
            this.testNewServiceViewModel.SaveService(new object());

            // Assert.
            // Customer should have been successfully added.
            Assert.That(NewServiceViewModel.ParentServicesViewModel, Is.Not.Null);
            Assert.That(NewServiceViewModel.ParentServicesViewModel.Services.Count, Is.EqualTo(1));
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }

        [Test]
        public void TestSaveService_ShouldCatchException()
        {
            // Arrange.
            this.testNewServiceViewModel.Name = "Test invalid Service name";
            this.testNewServiceViewModel.PriceString = "5.84";
            this.testNewServiceViewModel.IsRecurring = true;
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.True);

            // Set up mock data provider.
            DataException testException = new DataException("Test exception attempting to insert new service.");
            this.mockServiceDataProvider.Setup(dataProvider => dataProvider.InsertNewService(It.IsAny<Service>())).Throws(testException);

            // Act.
            this.testNewServiceViewModel.SaveService(new object());

            // Assert.
            // CustomersViewModel should still not contain any customers.
            Assert.That(NewServiceViewModel.ParentServicesViewModel, Is.Not.Null);
            Assert.That(NewServiceViewModel.ParentServicesViewModel.Services.Count, Is.EqualTo(0));
            Assert.That(this.testNavigationStore.SelectedViewModel is ServicesViewModel);
        }

        [Test]
        public void TestCanSaveService_NameIsEmpty_ShouldReturnFalse()
        {
            this.testNewServiceViewModel.Name = "";

            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_PriceStringIsEmpty_ShouldReturnFalse()
        {
            this.testNewServiceViewModel.PriceString = "";

            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_PriceStringNotValidDecimalValue_ShouldReturnFalse()
        {
            this.testNewServiceViewModel.PriceString = "Not a decimal value";

            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveService_ShouldReturnTrue()
        {
            // Act.
            this.testNewServiceViewModel.Name = "Test new service";
            this.testNewServiceViewModel.PriceString = "1.95";
            this.testNewServiceViewModel.IsRecurring = true;
            
            // Assert.
            // Should now be able to save the Service Record.
            Assert.That(this.testNewServiceViewModel.CanSaveService(new object()), Is.True);
        }
    }
}
