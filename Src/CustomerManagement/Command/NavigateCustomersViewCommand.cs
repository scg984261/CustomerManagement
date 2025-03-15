using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;

namespace CustomerManagement.Command
{
    public class NavigateCustomersViewCommand : CommandBase
    {
        private NavigationStore navigationStore;

        public NavigateCustomersViewCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override async void Execute(object? parameter)
        {
            if (parameter is null) return;

            CustomersViewModel customersViewModel = (CustomersViewModel) parameter;

            this.navigationStore.SelectedViewModel = customersViewModel;

            if (this.navigationStore.SelectedViewModel != null)
            {
                await this.navigationStore.SelectedViewModel.LoadAsync();
            }
        }
    }
}
