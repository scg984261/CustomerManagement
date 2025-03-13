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
    public class NavigateToServicesCommand : CommandBase
    {
        private readonly NavigationStore navigationStore;

        public NavigateToServicesCommand(NavigationStore navigationStore)
        {
            this.navigationStore = navigationStore;
        }

        public override async void Execute(object? parameter)
        {
            this.navigationStore.SelectedViewModel = new ServicesViewModel(new ServiceDataProvider());
            await this.navigationStore.SelectedViewModel.LoadAsync();
        }
    }
}
