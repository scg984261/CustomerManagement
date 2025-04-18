using CDB.Model;

namespace CDB.Test.Model
{
    public class ServiceTest
    {
        [Test]
        public void TestServiceConstuctor_NoArgs()
        {
            Service testService = new Service
            {
                Id = 8,
                Name = "CALLSANSWERED",
                Price = 1.25m,
                CreatedDateTime = new DateTime(2026, 01, 17, 12, 02, 40),
                LastUpdateDateTime = new DateTime(2025, 05, 16, 14, 30, 22),
                Subscriptions = new List<Subscription>()
            };

            Assert.That(testService.Id, Is.EqualTo(8));
            Assert.That(testService.Name, Is.EqualTo("CALLSANSWERED"));
            Assert.That(testService.Price, Is.EqualTo(1.25m));
            Assert.That(testService.CreatedDateTime, Is.EqualTo(new DateTime(2026, 01, 17, 12, 02, 40)));
            Assert.That(testService.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 05, 16, 14, 30, 22)));
            Assert.That(testService.Subscriptions.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestServiceConstructor_WithArgs()
        {
            const string serviceName = "TEST_SERVICE_NAME";
            const decimal price = 1.99m;
            const bool isRecurring = true;

            Service testService = new Service(serviceName, price, isRecurring);

            Assert.That(testService.Name, Is.EqualTo(serviceName));
            Assert.That(testService.Price, Is.EqualTo(price));
            Assert.That(testService.IsRecurring, Is.True);
        }

        [Test]
        public void TestServiceConstructor_AllArgs()
        {
            const int id = 5;
            const string serviceName = "Test service name 2";
            const decimal price = 0.79m;
            const bool isRecurring = true;
            DateTime createdDateTime = new DateTime(2025, 04, 16, 15, 42, 53);
            DateTime lastUpdateDateTime = new DateTime(2025, 04, 16, 15, 42, 53);

            Service testService = new Service(id, serviceName, price, isRecurring, createdDateTime, lastUpdateDateTime);

            Assert.That(testService.Id, Is.EqualTo(5));
            Assert.That(testService.Name, Is.EqualTo("Test service name 2"));
            Assert.That(testService.Price, Is.EqualTo(0.79m));
            Assert.That(testService.IsRecurring, Is.True);
            Assert.That(testService.CreatedDateTime, Is.EqualTo(new DateTime(2025, 04, 16, 15, 42, 53)));
            Assert.That(testService.LastUpdateDateTime, Is.EqualTo(new DateTime(2025, 04, 16, 15, 42, 53)));
        }
    }
}
