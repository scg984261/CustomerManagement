using CustomerManagement.View.Windows;
using System.Windows.Controls;
using System.Windows;

namespace CustomerManagement.Test
{
    [Apartment(ApartmentState.STA)]
    public class NewCustomerWindowTest
    {
        [Test]
        public void TestDefaultNoArgsConstructor()
        {
            if (Application.Current == null)
            {
                App application = new App();

                Style textBlockStyle = new Style(typeof(TextBlock));
                application.Resources.Add("FormTextBlock", textBlockStyle);

                Style textBoxStyle = new Style(typeof(TextBox));
                application.Resources.Add("InputFormTextBox", textBoxStyle);
            }

            NewCustomerWindow testNewCustomerWindow = new NewCustomerWindow();
        }
    }
}
