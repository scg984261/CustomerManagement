using CDB;
using CDB.Model;
using log4net;

namespace CustomerManagement.Data
{
    public interface IServiceDataProvider
    {
        List<Service> GetAll();
    }

    public class ServiceDataProvider : IServiceDataProvider
    {
        private static readonly DataWrapper cdbDataWrapper = new DataWrapper();
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceDataProvider));

        public List<Service> GetAll()
        {
            try
            {
                List<Service> services = cdbDataWrapper.SelectAllServices();
                log.Debug($"Services successfully obtained from CDB Data Context. {services.Count} records returned.");
                return services;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }

        public void InsertNewService(Service service)
        {
            try
            {
                cdbDataWrapper.InsertNewService(service);
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }

        public void UpdateService(int serviceId)
        {
            try
            {
                cdbDataWrapper.UpdateService(serviceId);
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }
    }
}
