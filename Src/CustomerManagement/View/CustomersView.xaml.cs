using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomerManagement.View
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : UserControl
    {
        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();

        public CustomersView()
        {
            InitializeComponent();

            // Placeholder data for the customers list.
            this.Customers.Add(new Customer("Scott", "Gillen"));
            this.Customers.Add(new Customer("Niall", "Gillen"));
            this.Customers.Add(new Customer("Kirsty", "Gillen"));
            this.Customers.Add(new Customer("Eethan", "Hawkins"));
            this.DataContext = this;
        }
    }

    public class Customer
    {
        public string FirstName { get; set;}
        public string LastName { get; set; }

        public Customer(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
