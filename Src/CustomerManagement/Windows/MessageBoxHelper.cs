using System.Windows;

namespace CustomerManagement.Windows
{
    public interface IMessageBoxHelper
    {
        void ShowInfoDialog(string message, string title);
        void ShowErrorDialog(string message, string title);
        void ShowErrorDialog(Exception exception, string title);
    }

    public class MessageBoxHelper : IMessageBoxHelper
    {
        public void ShowInfoDialog(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowErrorDialog(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowErrorDialog(Exception exception, string title)
        {
            MessageBox.Show($"{exception.GetType().FullName} ({exception.HResult}) - {exception.Message}", title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
