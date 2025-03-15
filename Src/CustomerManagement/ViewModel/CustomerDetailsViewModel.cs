using CustomerManagement.Command;
using CustomerManagement.Navigation;

namespace CustomerManagement.ViewModel
{
    public class CustomerDetailsViewModel : ViewModelBase
    {
        public static CustomersViewModel? ParentCustomersViewModel { get; set; }
        private CustomerItemViewModel customerItemViewModel;
        private NavigationStore navigationStore;
        public NavigateCustomersViewCommand NavigateBackCommand { get; }

        public CustomerDetailsViewModel(CustomerItemViewModel customerItemViewModel, NavigationStore navigationStore)
        {
            this.customerItemViewModel = customerItemViewModel;
            this.navigationStore = navigationStore;
            this.NavigateBackCommand = new NavigateCustomersViewCommand(this.navigationStore);
        }

        public string? CompanyName
        {
            get
            {
                return this.customerItemViewModel.CompanyName;
            }

            set
            {
                this.customerItemViewModel.CompanyName = value;
                this.NotifyPropertyChanged();
            }
        }
    }
}
