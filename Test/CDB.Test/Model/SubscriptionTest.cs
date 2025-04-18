using CDB.Model;

namespace CDB.Test.Model
{
    public class SubscriptionTest
    {
        [Test]
        public void TestSubscriptionConstructor()
        {
            Subscription testSubscription = new Subscription
            {
                CustomerId = 58421,
                ServiceId = 55,
                CreatedDateTime = new DateTime(2025, 01, 22, 17, 45, 15),
                Customer = new Customer(),
                Service = new Service()
            };

            Assert.That(testSubscription.CustomerId, Is.EqualTo(58421));
            Assert.That(testSubscription.ServiceId, Is.EqualTo(55));
            Assert.That(testSubscription.Customer, Is.Not.Null);
            Assert.That(testSubscription.Service, Is.Not.Null);
        }
    }
}
