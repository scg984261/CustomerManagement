using CDB.Model;

namespace CDB.Test.Model
{
    public class TestCustomer
    {
        [Test]
        public void TestCustomerConstructor()
        {
            Customer testCustomer = new Customer
            {
                Id = 1824,
                CompanyName = "Natwest Group PLC",
                BusinessContact = "Alison Rose",
                EmailAddress = "alison.rose@natwestgroup.co.uk",
                ContactNumber = "+44 20 984 1847 6",
                IsActive = true,
                CreatedDateTime = new DateTime(2025, 09, 02, 16, 37, 01),
                LastUpdateDateTime = new DateTime(2025, 11, 04, 17, 43, 03),
                Subscriptions = new List<Subscription>()
            };

            Assert.That(testCustomer.Id, Is.EqualTo(1824));
            Assert.That(testCustomer.CompanyName, Is.EqualTo("Natwest Group PLC"));
            Assert.That(testCustomer.BusinessContact, Is.EqualTo("Alison Rose"));
            Assert.That(testCustomer.EmailAddress, Is.EqualTo("alison.rose@natwestgroup.co.uk"));
            Assert.That(testCustomer.ContactNumber, Is.EqualTo("+44 20 984 1847 6"));
            Assert.That(testCustomer.IsActive, Is.True);
            Assert.That(testCustomer.CreatedDateTime, Is.EqualTo(new DateTime(2025, 09, 02, 16, 37, 01)));
            Assert.That(testCustomer.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 11, 04, 17, 43, 03)));
            Assert.That(testCustomer.Subscriptions.Count, Is.EqualTo(0));
        }
    }
}
