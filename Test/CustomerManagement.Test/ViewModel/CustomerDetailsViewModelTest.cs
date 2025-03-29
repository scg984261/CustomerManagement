using Moq;
using CDB.Model;
using CustomerManagement.ViewModel;
using CustomerManagement.Navigation;
using CustomerManagement.Data;

namespace CustomerManagement.Test.ViewModel
{
    public class CustomerDetailsViewModelTest
    {
        [Test]
        public void TestConstructor()
        {
            // Arrange.
            Customer customer = new Customer
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
            CustomerItemViewModel customerItemViewModel = new CustomerItemViewModel(customer);

            // Create the navigation Store.
            NavigationStore navigationStore = new NavigationStore();

            // Create the Mock ICustomerDataProvider.
            Mock<ICustomerDataProvider> mockCustomerDataProvider = new Mock<ICustomerDataProvider>();
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
    }
}
