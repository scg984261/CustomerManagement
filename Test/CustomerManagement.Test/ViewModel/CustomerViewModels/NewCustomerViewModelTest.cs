using CDB.Model;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel.CustomerViewModels;
using CustomerManagement.Windows;
using Moq;

namespace CustomerManagement.Test.ViewModel.CustomerViewModels
{
    public class NewCustomerViewModelTest
    {
        private NavigationStore testNavigationStore;
        private NewCustomerViewModel testNewCustomerViewModel;
        private CustomersViewModel testCustomersViewModel;
        
        private Mock<ICustomerDataProvider> mockCustomerDataProvider;
        private ICustomerDataProvider mockCustomerDataProviderObject;
        
        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper mockMessageBoxHelperObject;
        
        [SetUp]
        public void Setup()
        {
            this.testNavigationStore = new NavigationStore();

            this.mockCustomerDataProvider = new Mock<ICustomerDataProvider>();
            this.mockCustomerDataProviderObject = this.mockCustomerDataProvider.Object;
            
            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.mockMessageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testCustomersViewModel = new CustomersViewModel(this.testNavigationStore, this.mockCustomerDataProviderObject);
            NewCustomerViewModel.ParentCustomersViewModel = this.testCustomersViewModel;

            this.testNewCustomerViewModel = new NewCustomerViewModel(this.testNavigationStore, this.mockCustomerDataProviderObject, this.mockMessageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            // Invoke the constructor.
            this.testNewCustomerViewModel = new NewCustomerViewModel(this.testNavigationStore, this.mockCustomerDataProviderObject, this.mockMessageBoxHelperObject);

            Assert.That(this.testNewCustomerViewModel.NavigateBackCommand, Is.Not.Null);
            Assert.That(this.testNewCustomerViewModel.SaveCustomerCommand, Is.Not.Null);
            Assert.That(this.testNewCustomerViewModel.CompanyName, Is.Null);
            Assert.That(this.testNewCustomerViewModel.BusinessContact, Is.Null);
            Assert.That(this.testNewCustomerViewModel.EmailAddress, Is.Null);
            Assert.That(this.testNewCustomerViewModel.ContactNumber, Is.Null);
        }

        [Test]
        public void TestCompanyName_ShouldTriggerValidation()
        {
            // Act.
            this.testNewCustomerViewModel.CompanyName = "";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.CompanyName)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Company name cannot be blank."));
            Assert.That(this.testNewCustomerViewModel.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestCompanyName_ShouldClearErrors()
        {
            // Act.
            this.testNewCustomerViewModel.CompanyName = "Test company name.";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.CompanyName)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testNewCustomerViewModel.CompanyName, Is.EqualTo("Test company name."));
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestBusinessContact_ShouldTriggerValidation()
        {
            // Act.
            this.testNewCustomerViewModel.BusinessContact = "";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.BusinessContact)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Business contact cannot be blank."));
            Assert.That(this.testNewCustomerViewModel.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestBusinessContact_ShouldClearErrors()
        {
            // Act.
            this.testNewCustomerViewModel.BusinessContact = "Test business contact.";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.BusinessContact)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testNewCustomerViewModel.BusinessContact, Is.EqualTo("Test business contact."));
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestContactNumber_ShouldTriggerValidation()
        {
            // Act.
            this.testNewCustomerViewModel.ContactNumber = "";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.ContactNumber)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Contact Number cannot be blank."));
            Assert.That(this.testNewCustomerViewModel.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestContactNumber_ShouldClearErrors()
        {
            // Act.
            this.testNewCustomerViewModel.ContactNumber = "Test contact number";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.ContactNumber)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testNewCustomerViewModel.ContactNumber, Is.EqualTo("Test contact number"));
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestEmailAddress_ShouldTriggerValidation()
        {
            // Act.
            this.testNewCustomerViewModel.EmailAddress = "";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.EmailAddress)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Email Address cannot be blank."));
            Assert.That(this.testNewCustomerViewModel.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestEmailAddress_ShouldClearErrors()
        {
            // Act.
            this.testNewCustomerViewModel.EmailAddress = "Test email address";
            IEnumerable<string>? errors = this.testNewCustomerViewModel.GetErrors(nameof(this.testNewCustomerViewModel.EmailAddress)) as IEnumerable<string>;

            // Assert.
            Assert.That(this.testNewCustomerViewModel.EmailAddress, Is.EqualTo("Test email address"));
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestNavigateBack()
        {
            // Act.
            this.testNewCustomerViewModel.NavigateBack(new object());

            // Assert.
            Assert.That(this.testNavigationStore.SelectedViewModel is CustomersViewModel);
        }

        [Test]
        public void TestSaveCustomer_ShouldSaveCustomer()
        {
            this.mockCustomerDataProvider.Setup(dataProvider => dataProvider.InsertNewCustomer(It.IsAny<Customer>())).Returns(1);

            this.testNewCustomerViewModel.CompanyName = "Test company name.";
            this.testNewCustomerViewModel.BusinessContact = "Test business contact.";
            this.testNewCustomerViewModel.EmailAddress = "Test email address.";
            this.testNewCustomerViewModel.ContactNumber = "Test contact number.";

            this.testNewCustomerViewModel.SaveCustomer(new object());

            Assert.That(this.testCustomersViewModel.Customers.Count(), Is.EqualTo(1));
            Assert.That(this.testNavigationStore.SelectedViewModel is CustomersViewModel);
        }

        [Test]
        public void TestSaveCustomer_ShouldCatchException()
        {
            Exception testException = new Exception("Test exception attempting to insert new customer");
            this.mockCustomerDataProvider.Setup(dataProvider => dataProvider.InsertNewCustomer(It.IsAny<Customer>())).Throws(testException);

            this.testNewCustomerViewModel.SaveCustomer(new object());

            Assert.That(this.testCustomersViewModel.Customers.Count(), Is.EqualTo(0));
            Assert.That(this.testNavigationStore.SelectedViewModel is CustomersViewModel);
        }

        [Test]
        public void TestCanSaveCustomer_ShouldReturnFalse()
        {
            this.testNewCustomerViewModel.CompanyName = "Test company name";
            this.testNewCustomerViewModel.BusinessContact = "Test business contact";
            this.testNewCustomerViewModel.ContactNumber = "Test contact number";
            this.testNewCustomerViewModel.CompanyName = "";

            Assert.That(this.testNewCustomerViewModel.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveCustomer_ShouldReturnTrue()
        {
            this.testNewCustomerViewModel.CompanyName = "Test company name";
            this.testNewCustomerViewModel.BusinessContact = "Test business contact";
            this.testNewCustomerViewModel.ContactNumber = "Test contact number";
            this.testNewCustomerViewModel.EmailAddress = "Email Address";

            Assert.That(this.testNewCustomerViewModel.CanSaveCustomer(new object()), Is.True);
        }
    }
}
