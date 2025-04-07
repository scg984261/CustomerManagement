using CDB;
using CDB.Model;
using log4net;

namespace CustomerManagement.Data
{
    public interface IServiceDataProvider
    {
        List<Service> GetAll();
        int InsertNewService(Service service);
        int UpdateService(int serviceId);
    }

    public class ServiceDataProvider : IServiceDataProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceDataProvider));
        private readonly IDataWrapper cdbDataWrapper;

        public ServiceDataProvider()
        {
            this.cdbDataWrapper = new DataWrapper();
        }

        public ServiceDataProvider(IDataWrapper dataWrapper)
        {
            this.cdbDataWrapper = dataWrapper;
        }

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

        public int InsertNewService(Service service)
        {
            try
            {
                int insertResult = cdbDataWrapper.InsertNewService(service);
                return insertResult;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }

        public int UpdateService(int serviceId)
        {
            try
            {
                int updateResult = cdbDataWrapper.UpdateService(serviceId);
                return updateResult;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }
    }
}
