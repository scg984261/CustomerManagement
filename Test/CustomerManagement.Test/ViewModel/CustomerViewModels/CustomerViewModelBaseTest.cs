using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel.CustomerViewModels;
using CustomerManagement.Windows;
using Moq;

namespace CustomerManagement.Test.ViewModel.CustomerViewModels
{
    public class CustomerViewModelBaseTest
    {
        private NavigationStore navigationStore;

        private Mock<ICustomerDataProvider> mockCustomerDataProvider;
        private ICustomerDataProvider mockCustomerDataProviderObject;

        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper mockMessageBoxHelperObject;

        private CustomersViewModel testCustomersViewModel;

        private CustomerViewModelBase testCustomerViewModelBase;

        [SetUp]
        public void SetUp()
        {
            this.navigationStore = new NavigationStore();

            // Create the Mock ICustomerDataProvider.
            this.mockCustomerDataProvider = new Mock<ICustomerDataProvider>();
            this.mockCustomerDataProviderObject = mockCustomerDataProvider.Object;

            // Mock the message box helper to prevent dialog boxes appearing during unit tests.
            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.mockMessageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testCustomersViewModel = new CustomersViewModel(this.navigationStore, this.mockCustomerDataProviderObject, this.mockMessageBoxHelperObject);

            CustomerViewModelBase.ParentCustomersViewModel = this.testCustomersViewModel;

            this.testCustomerViewModelBase = new CustomerViewModelBase(navigationStore, this.mockCustomerDataProviderObject, this.mockMessageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            Assert.That(this.testCustomerViewModelBase.CompanyName, Is.EqualTo(string.Empty));
            Assert.That(this.testCustomerViewModelBase.BusinessContact, Is.EqualTo(string.Empty));
            Assert.That(this.testCustomerViewModelBase.EmailAddress, Is.EqualTo(string.Empty));
            Assert.That(this.testCustomerViewModelBase.ContactNumber, Is.EqualTo(string.Empty));
            Assert.That(this.testCustomerViewModelBase.IsActive, Is.False);

            Assert.That(this.testCustomerViewModelBase.SaveCustomerCommand, Is.Not.Null);
            Assert.That(this.testCustomerViewModelBase.NavigateBackCommand, Is.Not.Null);
        }

        [Test]
        public void TestSetCompanyName_ShouldTriggerValidation()
        {
            // Act.
            this.testCustomerViewModelBase.CompanyName = "";
            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.CompanyName)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Company name cannot be blank."));
            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestSetCompanyName_ShouldClearErrors()
        {
            // Set company name to zero-length string to trigger validation.
            this.testCustomerViewModelBase.CompanyName = string.Empty;

            // Act.
            this.testCustomerViewModelBase.CompanyName = "Test string";

            // Assert.
            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.CompanyName)) as IEnumerable<string>;
            Assert.That(this.testCustomerViewModelBase.CompanyName, Is.EqualTo("Test string"));
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestSetBusinessContact_ShouldTriggerValidation()
        {
            // Act.
            this.testCustomerViewModelBase.BusinessContact = "";
            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.BusinessContact)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Business contact cannot be blank."));
            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestSetBusinessContact_ShouldClearErrors()
        {
            // Act.
            this.testCustomerViewModelBase.BusinessContact = "";

            this.testCustomerViewModelBase.BusinessContact = "Another test business contact.";

            // Assert.
            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.BusinessContact)) as IEnumerable<string>;
            Assert.That(this.testCustomerViewModelBase.BusinessContact, Is.EqualTo("Another test business contact."));
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestSetContactNumber_ShouldTriggerValidation()
        {
            this.testCustomerViewModelBase.ContactNumber = "";
            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.ContactNumber)) as IEnumerable<string>;

            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Contact Number cannot be blank."));
            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestSetContactNumber_ShouldClearErrors()
        {
            this.testCustomerViewModelBase.ContactNumber = "";
            this.testCustomerViewModelBase.ContactNumber = "Changed test contact number";

            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.ContactNumber)) as IEnumerable<string>;
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
            Assert.That(this.testCustomerViewModelBase.ContactNumber, Is.EqualTo("Changed test contact number"));
        }

        [Test]
        public void TestSetEmailAddress_ShouldTriggerValidation()
        {
            this.testCustomerViewModelBase.EmailAddress = "";

            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.EmailAddress)) as IEnumerable<string>;

            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Email Address cannot be blank."));
            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestSetEmailAddress_ShouldClearErrors()
        {
            this.testCustomerViewModelBase.EmailAddress = "";
            this.testCustomerViewModelBase.EmailAddress = "Changed email address";

            IEnumerable<string>? errors = this.testCustomerViewModelBase.GetErrors(nameof(this.testCustomerViewModelBase.EmailAddress)) as IEnumerable<string>;
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
            Assert.That(this.testCustomerViewModelBase.EmailAddress, Is.EqualTo("Changed email address"));
        }

        [Test]
        public void TestSetIsActive()
        {
            this.testCustomerViewModelBase.IsActive = true;
            Assert.That(this.testCustomerViewModelBase.IsActive, Is.True);
        }

        [Test]
        public void TestCanSaveCustomer_ShouldReturnFalse_CompanyNameIsEmpty()
        {
            this.testCustomerViewModelBase.CompanyName = "";
            this.testCustomerViewModelBase.BusinessContact = "Test business contact";
            this.testCustomerViewModelBase.ContactNumber = "Test contact number";
            this.testCustomerViewModelBase.EmailAddress = "Test email Address";

            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveCustomer_ShouldReturnFalse_BusinessContactIsEmpty()
        {
            this.testCustomerViewModelBase.CompanyName = "Test company name";
            this.testCustomerViewModelBase.BusinessContact = "";
            this.testCustomerViewModelBase.ContactNumber = "Test contact number";
            this.testCustomerViewModelBase.EmailAddress = "Test email Address";

            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveCustomer_ShouldReturnFalse_ContactNumberIsEmpty()
        {
            this.testCustomerViewModelBase.CompanyName = "Test company name";
            this.testCustomerViewModelBase.BusinessContact = "Test Business Contact";
            this.testCustomerViewModelBase.ContactNumber = "";
            this.testCustomerViewModelBase.EmailAddress = "Test Email Address";

            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveCustomer_ShouldReturnFalse_EmailAddressIsEmpty()
        {
            this.testCustomerViewModelBase.CompanyName = "Test company name";
            this.testCustomerViewModelBase.BusinessContact = "Test business contact";
            this.testCustomerViewModelBase.ContactNumber = "Test contact number";
            this.testCustomerViewModelBase.EmailAddress = "";

            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.False);
        }

        [Test]
        public void TestCanSaveCustomer_ShouldReturnTrue()
        {
            this.testCustomerViewModelBase.CompanyName = "Test company name";
            this.testCustomerViewModelBase.BusinessContact = "Test business contact";
            this.testCustomerViewModelBase.ContactNumber = "Test contact number";
            this.testCustomerViewModelBase.EmailAddress = "Test email address";

            Assert.That(this.testCustomerViewModelBase.CanSaveCustomer(new object()), Is.True);
        }

        [Test]
        public void TestNavigateBack()
        {
            // Act.
            this.testCustomerViewModelBase.NavigateBack(new object());

            // Assert.
            Assert.That(this.navigationStore.SelectedViewModel is CustomersViewModel);
        }
    }
}
