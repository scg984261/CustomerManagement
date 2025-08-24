using Moq;
using CDB.Model;
using CustomerManagement.Navigation;
using CustomerManagement.Data;
using CustomerManagement.Windows;
using CustomerManagement.ViewModel.CustomerViewModels;

namespace CustomerManagement.Test.ViewModel.CustomerViewModels
{
    public class CustomerDetailsViewModelTest
    {
        private Customer customer;
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;

        private Mock<ICustomerDataProvider> mockCustomerDataProvider;
        private ICustomerDataProvider testCustomerDataProvider;

        private Mock<IMessageBoxHelper> mockMessageBoxHelper;
        private IMessageBoxHelper mockMessageBoxHelperObject;

        private CustomerDetailsViewModel testCustomerDetailsViewModel;

        [SetUp]
        public void Setup()
        {
            // Arrange.
            this.customer = new Customer
            {
                Id = 52,
                CompanyName = "Test company name",
                BusinessContact = "Test business contact",
                EmailAddress = "test.email@hotmail.com",
                ContactNumber = "01425987635",
                IsActive = true,
                CreatedDateTime = new DateTime(2025, 03, 25, 09, 29, 27),
                LastUpdateDateTime = new DateTime(2025, 03, 25, 09, 31, 58)
            };

            // Create the customer Item View Model.
            this.customerItemViewModel = new CustomerItemViewModel(this.customer);

            // Create the navigation Store.
            this.navigationStore = new NavigationStore();

            // Create the Mock ICustomerDataProvider.
            this.mockCustomerDataProvider = new Mock<ICustomerDataProvider>();
            this.testCustomerDataProvider = mockCustomerDataProvider.Object;

            // Mock the message box helper to prevent dialog boxes appearing during unit tests.
            this.mockMessageBoxHelper = new Mock<IMessageBoxHelper>();
            this.mockMessageBoxHelperObject = this.mockMessageBoxHelper.Object;

            this.testCustomerDetailsViewModel = new CustomerDetailsViewModel(this.customerItemViewModel, this.navigationStore, this.testCustomerDataProvider, this.mockMessageBoxHelperObject);

            // Set the Parent view model to allow backwards navigation.
            CustomerDetailsViewModel.ParentCustomersViewModel = new CustomersViewModel(this.navigationStore, this.testCustomerDataProvider, this.mockMessageBoxHelperObject);
        }

        [Test]
        public void TestConstructor()
        {
            // Arrange.
            ICustomerDataProvider testMockCustomerDataProvider = mockCustomerDataProvider.Object;

            // Act.
            CustomerDetailsViewModel testCustomerDetailsViewModel = new CustomerDetailsViewModel(customerItemViewModel, navigationStore, testMockCustomerDataProvider, this.mockMessageBoxHelperObject);

            // Assert.
            // Public properties should have been overridden with values from the CustomerItemViewModel,
            // Which is populated by the underlying Customer record from the Model.
            Assert.That(testCustomerDetailsViewModel.CompanyName, Is.EqualTo("Test company name"));
            Assert.That(testCustomerDetailsViewModel.BusinessContact, Is.EqualTo("Test business contact"));
            Assert.That(testCustomerDetailsViewModel.EmailAddress, Is.EqualTo("test.email@hotmail.com"));
            Assert.That(testCustomerDetailsViewModel.ContactNumber, Is.EqualTo("01425987635"));
            Assert.That(testCustomerDetailsViewModel.IsActive, Is.True);
            Assert.That(testCustomerDetailsViewModel.CreatedDateTime, Is.EqualTo("25-Mar-2025 09:29:27"));
            Assert.That(testCustomerDetailsViewModel.LastUpdateDateTimeFormatted, Is.EqualTo("25-Mar-2025 09:31:58"));
            Assert.That(testCustomerDetailsViewModel.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 03, 25, 09, 31, 58)));
        }

        [Test]
        public void TestGetCreatedDateTimeFormatted()
        {
            this.testCustomerDetailsViewModel.LastUpdateDateTime = new DateTime(2025, 04, 08, 21, 01, 58);
            Assert.That(this.testCustomerDetailsViewModel.LastUpdateDateTimeFormatted, Is.EqualTo("08-Apr-2025 21:01:58"));
        }

        [Test]
        public void TestPopulateServices()
        {
            Customer testCustomer = new Customer
            {
                Id = 52,
                CompanyName = "Test company name",
                BusinessContact = "Test business contact",
                EmailAddress = "test.email@hotmail.com",
                ContactNumber = "01425987635",
                IsActive = true,
                CreatedDateTime = new DateTime(2025, 03, 25, 09, 29, 27),
                LastUpdateDateTime = new DateTime(2025, 03, 25, 09, 31, 58),
            };

            testCustomer.Subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    CustomerId = 52,
                    ServiceId = 1,
                    CreatedDateTime = new DateTime(),
                    Customer = this.customer,
                    Service = new Service
                    {
                        Id = 1,
                        Name = "TESTRECURRINGSERVICE1",
                        Price = 0.99m,
                        IsRecurring = true,
                        CreatedDateTime = new DateTime(),
                        LastUpdateDateTime = new DateTime()
                    }
                },
                new Subscription
                {
                    CustomerId = 52,
                    ServiceId = 2,
                    CreatedDateTime = new DateTime(),
                    Customer = this.customer,
                    Service = new Service
                    {
                        Id = 1,
                        Name = "TESTRECURRINGSERVICE2",
                        Price = 1.99m,
                        IsRecurring = true,
                        CreatedDateTime = new DateTime(),
                        LastUpdateDateTime = new DateTime()
                    }
                },
                new Subscription
                {
                    CustomerId = 52,
                    ServiceId = 3,
                    CreatedDateTime = new DateTime(),
                    Customer = this.customer,
                    Service = new Service
                    {
                        Id = 1,
                        Name = "TESTRECURRINGSERVICE3",
                        Price = 1.49m,
                        IsRecurring = true,
                        CreatedDateTime = new DateTime(),
                        LastUpdateDateTime = new DateTime()
                    }
                },
                new Subscription
                {
                    CustomerId = 52,
                    ServiceId = 4,
                    CreatedDateTime = new DateTime(),
                    Customer = this.customer,
                    Service = new Service
                    {
                        Id = 1,
                        Name = "TESTSERVICE1",
                        Price = 700.52m,
                        IsRecurring = false,
                        CreatedDateTime = new DateTime(),
                        LastUpdateDateTime = new DateTime()
                    }
                },
                new Subscription
                {
                    CustomerId = 52,
                    ServiceId = 8,
                    CreatedDateTime = new DateTime(),
                    Customer = this.customer,
                    Service = new Service
                    {
                        Id = 1,
                        Name = "TESTSERVICE1",
                        Price = 599.99m,
                        IsRecurring = false,
                        CreatedDateTime = new DateTime(),
                        LastUpdateDateTime = new DateTime()
                    }
                },
            };

            CustomerDetailsViewModel testCustomerDetailsViewModel = new CustomerDetailsViewModel(new CustomerItemViewModel(testCustomer), this.navigationStore, this.testCustomerDataProvider, this.mockMessageBoxHelperObject);

            Assert.That(testCustomerDetailsViewModel.RecurringServices.Count, Is.EqualTo(3));
            Assert.That(testCustomerDetailsViewModel.NonRecurringServices.Count, Is.EqualTo(2));
        }

        [Test]
        public void TestCancel()
        {
            // Set the values of the fields.
            this.testCustomerDetailsViewModel.CompanyName = "Different company name";
            this.testCustomerDetailsViewModel.BusinessContact = "Different business contact";
            this.testCustomerDetailsViewModel.EmailAddress = "Different Email address";
            this.testCustomerDetailsViewModel.ContactNumber = "New contact number";

            this.testCustomerDetailsViewModel.Cancel();

            // Check that the fields have been reverted to their original values.
            Assert.That(this.customerItemViewModel.CompanyName, Is.EqualTo("Test company name"));
            Assert.That(this.customerItemViewModel.BusinessContact, Is.EqualTo("Test business contact"));
            Assert.That(this.customerItemViewModel.EmailAddress, Is.EqualTo("test.email@hotmail.com"));
            Assert.That(this.customerItemViewModel.ContactNumber, Is.EqualTo("01425987635"));

            // Check that navigation has succeeded.
            Assert.That(this.navigationStore.SelectedViewModel is CustomersViewModel);
        }

        [Test]
        public void TestSaveCustomer_ShouldSucceed()
        {
            this.mockCustomerDataProvider.Setup(dataProvider => dataProvider.UpdateCustomer(52)).Returns(1);

            // Act.
            this.testCustomerDetailsViewModel.CompanyName = "Another different company name";
            this.testCustomerDetailsViewModel.SaveCustomer(new object());

            // Assert that the change has propagated to the customerItemViewModel.
            Assert.That(this.customerItemViewModel.CompanyName, Is.EqualTo("Another different company name"));

            // Assert that navigation has succeeded.
            Assert.That(this.navigationStore.SelectedViewModel is CustomersViewModel);
        }

        [Test]
        public void TestSaveCustomer_ShouldCatchExceptionAndCancel()
        {
            // Arrange.
            this.mockCustomerDataProvider.Setup(dataProvider => dataProvider.UpdateCustomer(52)).Throws(new Exception("Test exception attempting to update customer details."));

            // Act.
            // Edit the customer fields.
            this.testCustomerDetailsViewModel.CompanyName = "Another Different company name";
            this.testCustomerDetailsViewModel.BusinessContact = "Another Different business contact";
            this.testCustomerDetailsViewModel.EmailAddress = "Another Different Email address";
            this.testCustomerDetailsViewModel.ContactNumber = "Another New contact number";
            this.testCustomerDetailsViewModel.SaveCustomer(new object());

            // Assert.
            // Check that the fields have been reset.
            Assert.That(this.customerItemViewModel.CompanyName, Is.EqualTo("Test company name"));
            Assert.That(this.customerItemViewModel.BusinessContact, Is.EqualTo("Test business contact"));
            Assert.That(this.customerItemViewModel.EmailAddress, Is.EqualTo("test.email@hotmail.com"));
            Assert.That(this.customerItemViewModel.ContactNumber, Is.EqualTo("01425987635"));
            
            // Assert that navigation has succeeded.
            Assert.That(this.navigationStore.SelectedViewModel is CustomersViewModel);
        }
    }
}
