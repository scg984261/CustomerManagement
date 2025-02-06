using CDB;
using CDB.Model;
using CustomerManagement.View.UserControls;
using Moq;
using System.Windows.Controls;

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

                },
                new Customer
                {

                },
                new Customer
                {

                }
            };

            mockWrapper.Setup(wrapper => wrapper.SelectAllCustomers()).Returns(mockCustomers);

            dataGrid.wrapper = mockWrapper.Object;

            // Act.
            dataGrid.Window_Loaded(this, new EventArgs());

            // Assert.
            Assert.That(dataGrid.Customers?.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestCustomerTableRowEditEnding()
        {
            DataGridRow row = new DataGridRow
            {
                Item = new Customer
                {
                    BusinessContact = "Hello, World"
                }
            };

            DataGridRowEditEndingEventArgs args = new DataGridRowEditEndingEventArgs(row, DataGridEditAction.Commit);

            // Arrange.
            CustomerDataGrid dataGrid = dataGrid = new CustomerDataGrid();

            dataGrid.CustomerTable_RowEditEnding(this, args);
        }
    }
}
