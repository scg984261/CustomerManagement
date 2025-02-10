using CustomerManagement.View.Windows;

namespace CustomerManagement.Test
{
    [Apartment(ApartmentState.STA)]
    public class NewCustomerWindowTest
    {
        [Test]
        public void TestDefaultNoArgsConstructor()
        {
            NewCustomerWindow testNewCustomerWindow = new NewCustomerWindow();
        }
    }
}
