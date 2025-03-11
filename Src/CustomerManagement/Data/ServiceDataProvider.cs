using CDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace CustomerManagement.Data
{
    public interface IServiceDataProvider
    {
        Task<IEnumerable<Service>?> GetAllAsync();
    }

    public class ServiceDataProvider : IServiceDataProvider
    {
        public async Task<IEnumerable<Service>?> GetAllAsync()
        {
            await Task.Delay(10);
            List<Service> services = new List<Service>();
            services.Add(new Service(1, "Service 1", 10.0m));
            services.Add(new Service(2, "Service 2", 15.2m));
            services.Add(new Service(3, "Service 3", 25.7m));
            return services;
        }
    }
}
