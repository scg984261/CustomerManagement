using CDB.Model;
using CustomerManagement.ViewModel.CustomerViewModels;

namespace CustomerManagement.Test.ViewModel.CustomerViewModels
{
    public class CustomerItemViewModelTest
    {
        private Customer testCustomer;
        private CustomerItemViewModel testCustomerItemViewModel;

        [SetUp]
        public void Setup()
        {
            this.testCustomer = new Customer();
            this.testCustomer.Id = 188;
            this.testCustomerItemViewModel = new CustomerItemViewModel(this.testCustomer);
        }

        [Test]
        public void TestConstructor_WithCustomer()
        {
            Customer testCustomer = new Customer()
            {
                Id = 84,
                CompanyName = "Vitae Erat Ltd",
                BusinessContact = "Cole Hyde",
                ContactNumber = "0824 812 9718",
                IsActive = true,
                EmailAddress = "vitae.risus@protonmail.edu",
                CreatedDateTime = new DateTime(2025, 04, 10, 15, 22, 58),
                LastUpdateDateTime = new DateTime(2025, 04, 10, 16, 12, 19)
            };

            CustomerItemViewModel testCustomerItemViewModel = new CustomerItemViewModel(testCustomer);

            Assert.That(testCustomerItemViewModel.Id, Is.EqualTo(84));
            Assert.That(testCustomerItemViewModel.CompanyName, Is.EqualTo("Vitae Erat Ltd"));
            Assert.That(testCustomerItemViewModel.BusinessContact, Is.EqualTo("Cole Hyde"));
            Assert.That(testCustomerItemViewModel.EmailAddress, Is.EqualTo("vitae.risus@protonmail.edu"));
            Assert.That(testCustomerItemViewModel.ContactNumber, Is.EqualTo("0824 812 9718"));
            Assert.That(testCustomerItemViewModel.IsActive, Is.True);
            Assert.That(testCustomerItemViewModel.CreatedDateTime, Is.EqualTo(new DateTime(2025, 04, 10, 15, 22, 58)));
            Assert.That(testCustomerItemViewModel.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 04, 10, 16, 12, 19)));
        }

        [Test]
        public void TestConstructor_DefaultNoArgs()
        {
            CustomerItemViewModel testViewModel = new CustomerItemViewModel();

            Assert.That(testViewModel.Id, Is.EqualTo(0));
            Assert.That(testViewModel.CompanyName, Is.EqualTo(string.Empty));
            Assert.That(testViewModel.BusinessContact, Is.EqualTo(string.Empty));
            Assert.That(testViewModel.EmailAddress, Is.EqualTo(string.Empty));
            Assert.That(testViewModel.ContactNumber, Is.EqualTo(string.Empty));
            Assert.That(testViewModel.IsActive, Is.False);
            Assert.That(testViewModel.CreatedDateTime, Is.EqualTo(new DateTime()));
            Assert.That(testViewModel.LastUpdateDateTime, Is.EqualTo(new DateTime()));
        }

        [Test]
        [TestCase("Test company name.", "Test company name.")]
        [TestCase(null, "")]
        public void TestCompanyName(string? companyName, string expectedCompanyName)
        {
            this.testCustomerItemViewModel.CompanyName = companyName;
            Assert.That(this.testCustomerItemViewModel.CompanyName, Is.EqualTo(expectedCompanyName));
            Assert.That(this.testCustomer.CompanyName, Is.EqualTo(expectedCompanyName));
        }

        [Test]
        [TestCase("Test business contact.", "Test business contact.")]
        [TestCase(null, "")]
        public void TestBusinessContact(string? businessContact, string expectedBusinessContact)
        {
            this.testCustomerItemViewModel.BusinessContact = businessContact;
            Assert.That(this.testCustomerItemViewModel.BusinessContact, Is.EqualTo(expectedBusinessContact));
            Assert.That(this.testCustomer.BusinessContact, Is.EqualTo(expectedBusinessContact));
        }

        [Test]
        [TestCase("Test email address.", "Test email address.")]
        [TestCase(null, "")]
        public void TestEmailAddress(string? emailAddress, string expectedEmailAddress)
        {
            this.testCustomerItemViewModel.EmailAddress = emailAddress;
            Assert.That(this.testCustomerItemViewModel.EmailAddress, Is.EqualTo(expectedEmailAddress));
            Assert.That(this.testCustomer.EmailAddress, Is.EqualTo(expectedEmailAddress));
        }

        [Test]
        [TestCase("Test contact number.", "Test contact number.")]
        [TestCase(null, "")]
        public void TestContactNumber(string? contactNumber, string expectedContactNumber)
        {
            this.testCustomerItemViewModel.ContactNumber = contactNumber;
            Assert.That(this.testCustomerItemViewModel.ContactNumber, Is.EqualTo(expectedContactNumber));
            Assert.That(this.testCustomer.ContactNumber, Is.EqualTo(expectedContactNumber));
        }

        [Test]
        public void TestIsActive()
        {
            this.testCustomerItemViewModel.IsActive = true;
            Assert.That(this.testCustomerItemViewModel.IsActive, Is.True);
            Assert.That(this.testCustomer.IsActive, Is.True);
        }

        [Test]
        public void TestSubscriptions()
        {
            // Arrange.
            // Set up test list of subscription records.
            List<Subscription> subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    CustomerId = this.testCustomer.Id,
                    Customer = this.testCustomer,
                    ServiceId = 2,
                    Service = new Service
                    {
                        Id = 2,
                        Name = "Test service 1",
                        Price = 58421m,
                        IsRecurring = false
                    }
                },
                new Subscription
                {
                    CustomerId = this.testCustomer.Id,
                    Customer = this.testCustomer,
                    ServiceId = 3,
                    Service = new Service
                    {
                        Id = 3,
                        Name = "Test service 2",
                        Price = 98427m,
                        IsRecurring = false
                    }
                },
                new Subscription
                {
                    CustomerId = this.testCustomer.Id,
                    Customer = this.testCustomer,
                    ServiceId = 4,
                    Service = new Service
                    {
                        Id = 4,
                        Name = "Test service 3",
                        Price = 0.99m,
                        IsRecurring = true
                    },
                },
                new Subscription
                {
                    CustomerId = this.testCustomer.Id,
                    Customer = this.testCustomer,
                    ServiceId = 5,
                    Service = new Service
                    {
                        Id = 5,
                        Name = "Test service 4",
                        Price = 1.89m,
                        IsRecurring = true
                    },
                },
            };

            this.testCustomerItemViewModel.Subscriptions = subscriptions;
            Assert.That(this.testCustomerItemViewModel.Subscriptions.Count, Is.EqualTo(4));
        }
    }
}
