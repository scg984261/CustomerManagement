using CDB.Model;
using CustomerManagement.Data;
using CustomerManagement.Navigation;
using CustomerManagement.ViewModel.CustomerViewModels;
using Moq;

namespace CustomerManagement.Test.ViewModel.CustomerViewModels
{
    public class CustomersViewModelTest
    {
        private CustomersViewModel testCustomersViewModel;
        private Mock<ICustomerDataProvider> mockCustomerDataProvider;
        private ICustomerDataProvider mockCustomerDataProviderObject;
        private NavigationStore testNavigationStore;
        private Customer testCustomer;
        private CustomerItemViewModel testCustomerItemViewModel;
        private List<Customer> testCustomers;

        [SetUp]
        public void Setup()
        {
            this.testCustomers = this.GenerateTestCustomers();

            this.mockCustomerDataProvider = new Mock<ICustomerDataProvider>();
            this.mockCustomerDataProvider.Setup(dataProvider => dataProvider.GetAll()).Returns(this.testCustomers);

            this.mockCustomerDataProviderObject = this.mockCustomerDataProvider.Object;

            this.testNavigationStore = new NavigationStore();
            this.testCustomersViewModel = new CustomersViewModel(this.testNavigationStore, this.mockCustomerDataProviderObject);

            this.testCustomer = new Customer
            {
                Id = 99013,
                BusinessContact = "Felix Patrick",
                CompanyName = "Tincidunt Congue Limited",
                ContactNumber = "(01648) 55473",
                EmailAddress = "porttitor@protonmail.org",
                CreatedDateTime = new DateTime(2025, 4, 11, 09, 42, 45),
                LastUpdateDateTime = new DateTime(2025, 4, 11, 11, 33, 51)
            };
            this.testCustomerItemViewModel = new CustomerItemViewModel(this.testCustomer);
        }

        private List<Customer> GenerateTestCustomers()
        {
            return new List<Customer>
            {
                new Customer
                {
                    Id = 1,
                    BusinessContact = "Shaine Mills",
                    CompanyName = "A Facilisis PC",
                    EmailAddress = "diam.dictum@google.couk",
                    ContactNumber = "(0111) 257 5655",
                    CreatedDateTime = new DateTime(2025, 4, 10, 08, 27, 11),
                    LastUpdateDateTime = new DateTime(2025, 4, 11, 09, 57, 02),
                },
                new Customer
                {
                    Id = 2,
                    BusinessContact = "Wallace Padilla",
                    CompanyName = "Faucibus Company",
                    EmailAddress = "nunc.ut.erat@hotmail.co.uk",
                    ContactNumber = "0832 741 8112",
                    CreatedDateTime = new DateTime(2025, 4, 10, 08, 27, 11),
                    LastUpdateDateTime = new DateTime(2025, 4, 11, 09, 57, 02),
                },
                new Customer
                {
                    Id = 3,
                    BusinessContact = "Ruth Cruz",
                    CompanyName = "Ipsum Sodales Purus Industries",
                    EmailAddress = "a.sollicitudin@google.edu",
                    ContactNumber = "056 4132 6936",
                    CreatedDateTime = new DateTime(2025, 4, 10, 08, 27, 11),
                    LastUpdateDateTime = new DateTime(2025, 4, 11, 09, 57, 02),
                },
                new Customer
                {
                    Id = 4,
                    BusinessContact = "Blythe Cameron",
                    CompanyName = "Erat Neque Corporation",
                    EmailAddress = "cras@protonmail.ca",
                    ContactNumber = "0800 207 3735",
                    CreatedDateTime = new DateTime(2025, 4, 10, 08, 27, 11),
                    LastUpdateDateTime = new DateTime(2025, 4, 11, 09, 57, 02),
                },
                new Customer
                {
                    Id = 5,
                    BusinessContact = "Mannix Brady",
                    CompanyName = "Ac Ipsum Inc.",
                    EmailAddress = "in.ornare.sagittis@yahoo.ca",
                    ContactNumber = "0800 1111",
                    CreatedDateTime = new DateTime(2025, 4, 10, 08, 27, 11),
                    LastUpdateDateTime = new DateTime(2025, 4, 11, 09, 57, 02),
                },
            };
        }

        [Test]
        public void TestConstructor()
        {
            // Act.
            this.testCustomersViewModel = new CustomersViewModel(this.testNavigationStore, this.mockCustomerDataProviderObject);

            // Assert.
            Assert.That(this.testCustomersViewModel.Customers.Count, Is.EqualTo(0));
            Assert.That(this.testCustomersViewModel.NavigateDetailsCommand, Is.Not.Null);
            Assert.That(this.testCustomersViewModel.NavigateNewCustomerCommand, Is.Not.Null);
            Assert.That(this.testCustomersViewModel.SelectedCustomer, Is.Null);

            Assert.That(this.testCustomersViewModel.IsCustomerSelected(new object()), Is.False);
        }

        [Test]
        public void TestSelectedCustomer()
        {
            // Arrange
            this.testCustomersViewModel.SelectedCustomer = this.testCustomerItemViewModel;

            // Assert.
            Assert.That(this.testCustomersViewModel.IsCustomerSelected(new object()), Is.True);

            Assert.That(this.testCustomersViewModel.SelectedCustomer.Id, Is.EqualTo(99013));
            Assert.That(this.testCustomersViewModel.SelectedCustomer.BusinessContact, Is.EqualTo("Felix Patrick"));
            Assert.That(this.testCustomersViewModel.SelectedCustomer.CompanyName, Is.EqualTo("Tincidunt Congue Limited"));
            Assert.That(this.testCustomersViewModel.SelectedCustomer.ContactNumber, Is.EqualTo("(01648) 55473"));
            Assert.That(this.testCustomersViewModel.SelectedCustomer.EmailAddress, Is.EqualTo("porttitor@protonmail.org"));
            Assert.That(this.testCustomersViewModel.SelectedCustomer.CreatedDateTime, Is.EqualTo(new DateTime(2025, 4, 11, 09, 42, 45)));
            Assert.That(this.testCustomersViewModel.SelectedCustomer.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 4, 11, 11, 33, 51)));
        }

        [Test]
        public void TestLoad_CustomersPopulated_ShouldNotLoad()
        {
            this.testCustomersViewModel.Customers.Add(this.testCustomerItemViewModel);
            this.testCustomersViewModel.Load();

            // Assert that the customers collection still only contains one customer.
            Assert.That(this.testCustomersViewModel.Customers.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestLoad_CustomersNotPopulated_ShouldLoadCustomers()
        {
            this.testCustomersViewModel.Load();

            // Assert that Data provider has loaded the test customers.
            Assert.That(this.testCustomersViewModel.Customers.Count, Is.EqualTo(5));
        }

        [Test]
        public void TestIsCustomerSelected_ShouldReturnFalse()
        {
            // Act.
            this.testCustomersViewModel.SelectedCustomer = null;

            // Assert.
            Assert.That(this.testCustomersViewModel.IsCustomerSelected(new object()), Is.False);
        }

        [Test]
        public void TestIsCustomerSelected_ShouldReturnTrue()
        {
            // Act.
            // Select a customer
            this.testCustomersViewModel.SelectedCustomer = new CustomerItemViewModel(new Customer());

            // Assert.
            Assert.That(this.testCustomersViewModel.IsCustomerSelected(new object()), Is.True);
        }

        [Test]
        public void TestNavigateToCustomerDetails()
        {
            // Arrange.
            // Select a customer
            this.testCustomersViewModel.SelectedCustomer = new CustomerItemViewModel(new Customer());

            // Act.
            // Navigate to the details screen.
            this.testCustomersViewModel.NavigateToCustomerDetails(new object());

            // Assert.
            // The selected view model should now be a custoemrs view model.
            Assert.That(this.testNavigationStore.SelectedViewModel is CustomerDetailsViewModel);
        }

        [Test]
        public void TestNavigateToNewCustomerScreen()
        {
            // Act.
            // Navigate to the new customer screen.
            this.testCustomersViewModel.NavigateToNewCustomer(new object());

            Assert.That(this.testNavigationStore.SelectedViewModel is NewCustomerViewModel);
        }
    }
}
