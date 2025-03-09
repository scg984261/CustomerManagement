using CDB.Model;
using CustomerManagement.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManagement.ViewModel
{
    public class ServicesViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServicesViewModel));
        public ObservableCollection<Service> Services { get; } = new ObservableCollection<Service>();
        private readonly IServiceDataProvider serviceDataProvider;

        public ServicesViewModel(IServiceDataProvider serviceDataProvider)
        {
            this.serviceDataProvider = new ServiceDataProvider();
        }

        public override async Task LoadAsync()
        {
            var services = await this.serviceDataProvider.GetAllAsync();

            if (services != null)
            {
                foreach (Service service in services)
                {
                    this.Services.Add(service);
                }
            }
        }
    }
}
