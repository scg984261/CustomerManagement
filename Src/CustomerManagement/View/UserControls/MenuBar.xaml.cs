using System.Windows;
using System.Windows.Controls;

namespace CustomerManagement.View.UserControls
{
    public partial class MenuBar : UserControl
    {
        public MenuBar()
        {
            InitializeComponent();
        }

        private void MenuItemOpen_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void MenuItemExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to close?", "Close", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void NewCustomerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
