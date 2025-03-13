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
    public class NavigateCustomerCommand : CommandBase
    {
        private readonly NavigationStore navigationStore;
        private readonly Func<object?, bool>? canExecute;

        public NavigateCustomerCommand(NavigationStore navigationStore, Func<object?, bool>? canExecute = null)
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
            ViewModelBase? viewModel = parameter as ViewModelBase;
            this.navigationStore.SelectedViewModel = viewModel;

            if (this.navigationStore.SelectedViewModel != null)
            {
                await this.navigationStore.SelectedViewModel.LoadAsync();
            }
        }
    }
}
