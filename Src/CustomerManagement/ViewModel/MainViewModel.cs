using CustomerManagement.Command;
using CustomerManagement.Navigation;

namespace CustomerManagement.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ServicesViewModel ServicesViewModel { get; }
        public CustomersViewModel CustomersViewModel { get; }
        private NavigationStore navigationStore { get; }
        public DelegateCommand SelectViewModelCommand { get; }
        
        public ViewModelBase? SelectedViewModel
        {
            get
            {
                // Get the current, selected ViewModel
                // from the navigation store.
                return this.navigationStore.SelectedViewModel;
            }
            set
            {
                this.navigationStore.SelectedViewModel = value;
                this.NotifyPropertyChanged();
            }
        }

        public MainViewModel(NavigationStore navigationStore, CustomersViewModel customersViewModel, ServicesViewModel servicesViewModel)
        {
            this.navigationStore = navigationStore;
            this.CustomersViewModel = customersViewModel;
            this.ServicesViewModel = servicesViewModel;
            this.navigationStore.SelectedViewModelChanged += this.NotifySelectedViewModelChanged;
            this.SelectViewModelCommand = new DelegateCommand(SelectViewModel);
        }

        public void NotifySelectedViewModelChanged()
        {
            this.NotifyPropertyChanged(nameof(this.SelectedViewModel));
        }

        public override void Load()
        {
            if (navigationStore.SelectedViewModel != null)
            {
                this.navigationStore.SelectedViewModel.Load();
            }
        }

        public void SelectViewModel(object? parameter)
        {
            this.navigationStore.SelectedViewModel = parameter as ViewModelBase;
            this.Load();
        }
    }
}
