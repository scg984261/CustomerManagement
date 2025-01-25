using CustomerManagement;

namespace CustomerManagement.Test
{
    public class CalculatorTest
    {
        [Test]
        public void TestAdd()
        {
            // int sum = CustomerManagement.Calculator;
            int sum = Calculator.Add(1, 5);
            Assert.That(sum, Is.EqualTo(6));
        }
    }
}
