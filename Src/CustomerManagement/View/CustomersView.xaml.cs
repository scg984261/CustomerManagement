using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CDB.Model;
using CustomerManagement.Data;
using CustomerManagement.ViewModel;

namespace CustomerManagement.View
{
    public partial class CustomersView : UserControl
    {
        private readonly CustomersViewModel customersViewModel;

        public CustomersView()
        {
            InitializeComponent();
            this.customersViewModel = new CustomersViewModel(new CustomerDataProvider());
            this.DataContext = this.customersViewModel;
            this.Loaded += this.CustomersView_Loaded;
        }

        public async void CustomersView_Loaded(object sender, RoutedEventArgs args)
        {
            await this.customersViewModel.LoadAsync();
        }
    }
}
