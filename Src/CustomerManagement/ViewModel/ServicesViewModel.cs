using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using CustomerManagement.Command;
using CustomerManagement.Data;
using CustomerManagement.View.Windows;
using CDB.Model;

namespace CustomerManagement.ViewModel
{
    public class ServicesViewModel : ViewModelBase
    {
        public ObservableCollection<ServiceItemViewModel> Services { get; } = new ObservableCollection<ServiceItemViewModel>();
        private readonly IServiceDataProvider serviceDataProvider;
        public DelegateCommand AddServiceCommand { get; }

        public ServicesViewModel(IServiceDataProvider serviceDataProvider)
        {
            this.serviceDataProvider = new ServiceDataProvider();
            this.AddServiceCommand = new DelegateCommand(this.AddNewService);
        }

        public override async Task LoadAsync()
        {
            if (this.Services.Any())
            {
                return;
            }

            var services = await this.serviceDataProvider.GetAllAsync();

            if (services != null)
            {
                foreach (Service service in services)
                {
                    ServiceItemViewModel serviceItemViewModel = new ServiceItemViewModel(service);
                    this.Services.Add(serviceItemViewModel);
                }
            }
        }

        public void AddNewService(object? parameter)
        {
            NewServiceWindow newServiceWindow = new NewServiceWindow();
            newServiceWindow.ShowDialog();
        }
    }
}
