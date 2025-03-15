using CustomerManagement.Navigation;
using CustomerManagement.ViewModel;
using CustomerManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace CustomerManagement.Command
{
    public class NavigateDetailsCommand : CommandBase
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<object?, bool>? canExecute;

        public NavigateDetailsCommand(NavigationStore navigationStore, Func<object?, bool>? canExecute = null)
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

            CustomerItemViewModel customerItemViewModel = (CustomerItemViewModel) parameter;

            CustomerDetailsViewModel customerDetailsViewModel = new CustomerDetailsViewModel(customerItemViewModel, navigationStore);
            this.navigationStore.SelectedViewModel = customerDetailsViewModel;

            if (this.navigationStore.SelectedViewModel != null)
            {
                await this.navigationStore.SelectedViewModel.LoadAsync();
            }
        }
    }
}
