using CDB;
using CDB.Model;
using log4net;

namespace CustomerManagement.Data
{
    public interface ICustomerDataProvider
    {
        List<Customer> GetAll();
    }

    public class CustomerDataProvider : ICustomerDataProvider
    {
        private static readonly DataWrapper cdbDataWrapper = new DataWrapper();
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomerDataProvider));

        public CustomerDataProvider()
        {
        }

        public List<Customer> GetAll()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                customers = cdbDataWrapper.SelectAllCustomers();
                log.Info($"Customer records successfully retrieved from database. {customers.Count} customers returned.");
                
            }
            catch(Exception exception)
            {
                log.Error("Error occurred attempting to retrieve customers from the database.");
                log.Error(exception);
            }

            return customers;
        }
    }
}
