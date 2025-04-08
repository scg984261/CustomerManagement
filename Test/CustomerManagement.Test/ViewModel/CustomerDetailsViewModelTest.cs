using Moq;
using CDB.Model;
using CustomerManagement.ViewModel;
using CustomerManagement.Navigation;
using CustomerManagement.Data;

namespace CustomerManagement.Test.ViewModel
{
    public class CustomerDetailsViewModelTest
    {
        private Customer customer;
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;
        private Mock<ICustomerDataProvider> mockCustomerDataProvider;
        private ICustomerDataProvider testCustomerDataProvider;
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

            this.testCustomerDetailsViewModel = new CustomerDetailsViewModel(this.customerItemViewModel, this.navigationStore, this.testCustomerDataProvider);
        }

        [Test]
        public void TestConstructor()
        {
            // Arrange.
            ICustomerDataProvider testMockCustomerDataProvider = mockCustomerDataProvider.Object;

            // Act.
            CustomerDetailsViewModel testCustomerDetailsViewModel = new CustomerDetailsViewModel(customerItemViewModel, navigationStore, testMockCustomerDataProvider);

            // Assert.
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
        public void TestSetCompanyName_ShouldTriggerValidation()
        {
            // Act.
            testCustomerDetailsViewModel.CompanyName = "";
            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.CompanyName)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Company name cannot be blank"));
        }

        [Test]
        public void TestSetCompanyName_ShouldClearErrors()
        {
            // Set company name to zero-length string to trigger validation.
            testCustomerDetailsViewModel.CompanyName = string.Empty;

            // Act.
            testCustomerDetailsViewModel.CompanyName = "Test string";

            // Assert.
            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.CompanyName)) as IEnumerable<string>;
            Assert.That(testCustomerDetailsViewModel.CompanyName, Is.EqualTo("Test string"));
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestSetBusinessContact_ShouldTriggerValidation()
        {
            // Act.
            this.testCustomerDetailsViewModel.BusinessContact = "";
            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.BusinessContact)) as IEnumerable<string>;

            // Assert.
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Business contact cannot be blank"));
        }

        [Test]
        public void TestSetBusinessContact_ShouldClearErrors()
        {
            // Act.
            this.testCustomerDetailsViewModel.BusinessContact = "";

            this.testCustomerDetailsViewModel.BusinessContact = "Another test business contact.";

            // Assert.
            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.BusinessContact)) as IEnumerable<string>;
            Assert.That(testCustomerDetailsViewModel.BusinessContact, Is.EqualTo("Another test business contact."));
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestSetContactNumber_ShouldTriggerValidation()
        {
            this.testCustomerDetailsViewModel.ContactNumber = "";
            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.ContactNumber)) as IEnumerable<string>;

            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Contact number cannot be blank"));
        }

        [Test]
        public void TestSetContactNumber_ShouldClearErrors()
        {
            this.testCustomerDetailsViewModel.ContactNumber = "";
            this.testCustomerDetailsViewModel.ContactNumber = "Changed test contact number";

            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.ContactNumber)) as IEnumerable<string>;
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
            Assert.That(this.testCustomerDetailsViewModel.ContactNumber, Is.EqualTo("Changed test contact number"));
        }

        [Test]
        public void TestSetEmailAddress_ShouldTriggerValidation()
        {
            this.testCustomerDetailsViewModel.EmailAddress = "";

            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.EmailAddress)) as IEnumerable<string>;

            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(1));
            Assert.That(errors.ToList()[0], Is.EqualTo("Email address cannot be blank"));
        }

        [Test]
        public void TestSetEmailAddress_ShouldClearErrors()
        {
            this.testCustomerDetailsViewModel.EmailAddress = "";
            this.testCustomerDetailsViewModel.EmailAddress = "Changed email address";

            IEnumerable<string>? errors = testCustomerDetailsViewModel.GetErrors(nameof(testCustomerDetailsViewModel.EmailAddress)) as IEnumerable<string>;
            Assert.That(errors, Is.Not.Null);
            Assert.That(errors.Count, Is.EqualTo(0));
            Assert.That(this.testCustomerDetailsViewModel.EmailAddress, Is.EqualTo("Changed email address"));
        }

        [Test]
        public void TestSetIsActive()
        {
            this.testCustomerDetailsViewModel.IsActive = false;
            Assert.That(this.testCustomerDetailsViewModel.IsActive, Is.False);
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
                    Id = 101,
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
                    Id = 102,
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
                    Id = 103,
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
                    Id = 104,
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
                    Id = 105,
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

            CustomerDetailsViewModel testCustomerDetailsViewModel = new CustomerDetailsViewModel(new CustomerItemViewModel(testCustomer), this.navigationStore, this.testCustomerDataProvider);

            Assert.That(testCustomerDetailsViewModel.RecurringServices.Count, Is.EqualTo(3));
            Assert.That(testCustomerDetailsViewModel.NonRecurringServices.Count, Is.EqualTo(2));
        }
    }
}
