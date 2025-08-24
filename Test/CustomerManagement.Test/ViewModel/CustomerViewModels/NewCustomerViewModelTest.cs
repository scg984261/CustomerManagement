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

            this.testCustomersViewModel = new CustomersViewModel(this.testNavigationStore, this.mockCustomerDataProviderObject, this.mockMessageBoxHelperObject);
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
            Assert.That(this.testNewCustomerViewModel.CompanyName, Is.EqualTo(string.Empty));
            Assert.That(this.testNewCustomerViewModel.BusinessContact, Is.EqualTo(string.Empty));
            Assert.That(this.testNewCustomerViewModel.EmailAddress, Is.EqualTo(string.Empty));
            Assert.That(this.testNewCustomerViewModel.ContactNumber, Is.EqualTo(string.Empty));
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
            // Assert that the ViewModel has navigated back.
            Assert.That(this.testNavigationStore.SelectedViewModel is CustomersViewModel);
        }
    }
}
