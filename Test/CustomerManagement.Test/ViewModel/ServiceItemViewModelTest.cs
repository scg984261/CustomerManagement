using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManagement.ViewModel;
using CDB.Model;


namespace CustomerManagement.Test.ViewModel
{
    public class ServiceItemViewModelTest
    {
        [Test]
        public void TestPriceFormatted_ShouldReturnFormattedPrice()
        {
            // Arrange.
            Service service = new Service()
            {
                Id = 52,
                Name = "test service 123",
                Price = 5842.3m,
                LastUpdateDateTime = new DateTime(2025, 3, 9, 16, 40, 55),
                CreatedDateTime = new DateTime(2025, 3, 9, 16, 37, 17)
            };

            ServiceItemViewModel testViewModel = new ServiceItemViewModel(service);

            // Act.
            string formattedPriceString = testViewModel.PriceFormatted;

            // Assert.
            Assert.That(formattedPriceString, Is.EqualTo("£5842.30"));
        }
    }
}
