using CDB.Model;

namespace CDB.Test.Model
{
    public class TestService
    {
        [Test]
        public void TestServiceConstuctor()
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
    }
}
