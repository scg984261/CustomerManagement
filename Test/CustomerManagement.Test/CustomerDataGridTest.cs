using CDB;
using CDB.Model;
using CustomerManagement.View.UserControls;
using Moq;

namespace CustomerManagement.Test
{
    [Apartment(ApartmentState.STA)]
    public class CustomerDataGridTest
    {
        [Test]
        public void TestConstructor()
        {
            CustomerDataGrid dataGrid = dataGrid = new CustomerDataGrid();
            Assert.That(dataGrid.wrapper, Is.Not.Null);
            Assert.That(dataGrid.DataContext is CustomerDataGrid, Is.True);
        }

        [Test]
        public void TestWindowLoaded_ShouldLoadWindow()
        {
            // Arrange.
            CustomerDataGrid dataGrid = dataGrid = new CustomerDataGrid();

            Mock<IDataWrapper> mockWrapper = new Mock<IDataWrapper>();

            List<Customer> mockCustomers = new List<Customer>()
            {
                new Customer
                {
                    Id = 584,
                    CompanyName = "Vel Turpis Foundation",
                    BusinessContact = "Yardley Hensley",
                    ContactNumber = "0800 741350",
                    EmailAddress = "ac.ipsum@outlook.ca",
                    CreatedDateTime = new DateTime(2025, 02, 07, 12, 15, 52),
                    LastUpdateDateTime = new DateTime(2025, 02, 07, 16, 29, 12),

                },
                new Customer
                {
                    Id = 612,
                    CompanyName = "Semper Cursus Industries",
                    BusinessContact = "Steel Castaneda",
                    ContactNumber = "0845 46 42",
                    EmailAddress = "felis.ullamcorper.viverra@aol.ca",
                    CreatedDateTime = new DateTime(2025, 02, 07, 15, 22, 29),
                    LastUpdateDateTime = new DateTime(2025, 02, 07, 16, 29, 15),
                },
                new Customer
                {
                    Id = 655,
                    CompanyName = "Orci Luctus Et Consulting",
                    BusinessContact = "Craig Duke",
                    ContactNumber = "0800 1111",
                    EmailAddress = "sit.amet.consectetuer@outlook.com",
                    CreatedDateTime = new DateTime(2025, 01, 31, 10, 58, 41),
                    LastUpdateDateTime = new DateTime(2025, 01, 31, 17, 01, 13),
                }
            };

            mockWrapper.Setup(wrapper => wrapper.SelectAllCustomers()).Returns(mockCustomers);

            dataGrid.wrapper = mockWrapper.Object;

            // Act.
            dataGrid.Window_Loaded(this, new EventArgs());

            // Assert.
            Assert.That(dataGrid.Customers?.Count, Is.EqualTo(3));
            Assert.That(dataGrid.Customers[0].Equals(mockCustomers[0]));
            Assert.That(dataGrid.Customers[1].Equals(mockCustomers[1]));
            Assert.That(dataGrid.Customers[2].Equals(mockCustomers[2]));
        }
    }
}
