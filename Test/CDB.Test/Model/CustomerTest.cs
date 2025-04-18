using CDB.Model;

namespace CDB.Test.Model
{
    public class CustomerTest
    {
        [Test]
        public void TestDefaultNoArgsConstructor()
        {
            Customer testCustomer = new Customer();

            Assert.That(testCustomer.Id, Is.EqualTo(0));
            Assert.That(testCustomer.CompanyName, Is.EqualTo(string.Empty));
            Assert.That(testCustomer.BusinessContact, Is.EqualTo(string.Empty));
            Assert.That(testCustomer.EmailAddress, Is.EqualTo(string.Empty));
            Assert.That(testCustomer.ContactNumber, Is.EqualTo(string.Empty));
            Assert.That(testCustomer.IsActive, Is.False);
            Assert.That(testCustomer.CreatedDateTime, Is.EqualTo(new DateTime()));
            Assert.That(testCustomer.LastUpdateDateTime, Is.EqualTo(new DateTime()));
            Assert.That(testCustomer.Subscriptions?.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestConstructor_SomeArgsProvided()
        {
            Customer testCustomer = new Customer("test company name", "test business contact", "test@emailaddress.com", "01842387429");

            Assert.That(testCustomer.Id, Is.EqualTo(0));
            Assert.That(testCustomer.CompanyName, Is.EqualTo("test company name"));
            Assert.That(testCustomer.BusinessContact, Is.EqualTo("test business contact"));
            Assert.That(testCustomer.EmailAddress, Is.EqualTo("test@emailaddress.com"));
            Assert.That(testCustomer.ContactNumber, Is.EqualTo("01842387429"));
            Assert.That(testCustomer.IsActive, Is.True);
            Assert.That(testCustomer.CreatedDateTime, Is.EqualTo(new DateTime()));
            Assert.That(testCustomer.LastUpdateDateTime, Is.EqualTo(new DateTime()));
            Assert.That(testCustomer.Subscriptions?.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestConstructor() 
        {
            Customer testCustomer = new Customer(1842, "Natwest Group PLC", "Alison Rose", "alison.rose@natwestgroup.co.uk", "+44 20 984 1847 6", new DateTime(2025, 09, 02, 16, 37, 01), new DateTime(2025, 11, 04, 17, 43, 03));

            Assert.That(testCustomer.Id, Is.EqualTo(1842));
            Assert.That(testCustomer.CompanyName, Is.EqualTo("Natwest Group PLC"));
            Assert.That(testCustomer.BusinessContact, Is.EqualTo("Alison Rose"));
            Assert.That(testCustomer.EmailAddress, Is.EqualTo("alison.rose@natwestgroup.co.uk"));
            Assert.That(testCustomer.ContactNumber, Is.EqualTo("+44 20 984 1847 6"));
            Assert.That(testCustomer.IsActive, Is.True);
            Assert.That(testCustomer.CreatedDateTime, Is.EqualTo(new DateTime(2025, 09, 02, 16, 37, 01)));
            Assert.That(testCustomer.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 11, 04, 17, 43, 03)));
            Assert.That(testCustomer.Subscriptions?.Count, Is.EqualTo(0));
        }

        
        [Test]
        public void TestEquals_ShouldReturnTrue()
        {
            int Id = 584;
            string companyName = "Hymenaeos Mauris Consulting";
            string businessContact = "Justin Roach";
            string emailAddress = "j.roach@hotmail.com";
            string contactNumber = "(015468) 46553";
            DateTime createdDateTime = new DateTime();
            DateTime lastUpdateDateTme = new DateTime();

            Customer testCustomer = new Customer(Id, companyName, businessContact, emailAddress, contactNumber, createdDateTime, lastUpdateDateTme);

            Customer identicalCustomer = new Customer(Id, companyName, businessContact, emailAddress, contactNumber, createdDateTime, lastUpdateDateTme);

            Assert.That(testCustomer.Equals(identicalCustomer));
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_IdsNotEqual()
        {
            Customer customer1 = new Customer
            {
                Id = 58
            };

            Customer customer2 = new Customer()
            {
                Id = 22
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_CompanyNamesNotEqual()
        {
            Customer customer1 = new Customer
            {
                CompanyName = "test Company1"
            };

            Customer customer2 = new Customer()
            {
                CompanyName = "test Company2"
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_BusinessContactNotEqual()
        {
            Customer customer1 = new Customer
            {
                BusinessContact = "John Jones"
            };

            Customer customer2 = new Customer()
            {
                BusinessContact = "Jimmy Jones"
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_EmailAddressesNotEqual()
        {
            Customer customer1 = new Customer
            {
                EmailAddress = "j.jones@outlook.com"
            };

            Customer customer2 = new Customer()
            {
                EmailAddress = "j.jones@hotmail.com"
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_ContactNumbersNotEqual()
        {
            Customer customer1 = new Customer
            {
                ContactNumber = "+844384279"
            };

            Customer customer2 = new Customer()
            {
                ContactNumber = "+844384275"
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_IsActiveNotEqual()
        {
            Customer customer1 = new Customer()
            {
                IsActive = false
            };

            Customer customer2 = new Customer()
            {
                IsActive = true
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_CreatedDateTimeNotEqual()
        {
            Customer customer1 = new Customer()
            {
                CreatedDateTime = new DateTime(2025, 02, 04, 21, 47, 01)
            };

            Customer customer2 = new Customer()
            {
                CreatedDateTime = new DateTime(2025, 02, 04, 21, 47, 02)
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }

        [Test]
        public void TestEquals_ShouldReturnFalse_LastUpdateDateTimeNotEqual()
        {
            Customer customer1 = new Customer()
            {
                LastUpdateDateTime = new DateTime(2024, 11, 5, 05, 22, 51)
            };

            Customer customer2 = new Customer()
            {
                LastUpdateDateTime = new DateTime(2024, 11, 5, 05, 22, 52)
            };

            Assert.That(customer1.Equals(customer2), Is.False);
        }
    }
}
