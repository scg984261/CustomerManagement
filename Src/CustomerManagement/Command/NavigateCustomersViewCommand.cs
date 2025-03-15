using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;

namespace CustomerManagement.Command
{
    public class NavigateCustomersViewCommand : CommandBase
    {
        private NavigationStore navigationStore;
        private readonly Func<object?, bool>? canExecute;

        public NavigateCustomersViewCommand(NavigationStore navigationStore, Func<object?, bool>? canExecute = null)
        {
            this.navigationStore = navigationStore;
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object? parameter)
        {
            if (this.canExecute is null)
            {
                return true;
            }

            return this.canExecute(parameter);
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
