using CDB.Model;

namespace CustomerManagement.Data
{
    public interface IServiceDataProvider
    {
        IEnumerable<Service> GetAll();
    }

    public class ServiceDataProvider : IServiceDataProvider
    {
        public IEnumerable<Service> GetAll()
        {
            List<Service> services = new List<Service>();
            services.Add(new Service(1, "Service 1", 10.0m));
            services.Add(new Service(2, "Service 2", 15.2m));
            services.Add(new Service(3, "Service 3", 25.7m));
            return services;
        }
    }
}
