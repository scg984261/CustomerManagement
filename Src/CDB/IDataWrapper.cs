using CDB.Model;

namespace CDB
{
    public interface IDataWrapper
    {
        int InsertNewCustomer(Customer customer);
        int UpdateCustomer(int customerId);
        List<Customer> SelectAllCustomers();
        List<Service> SelectAllServices();
        int InsertNewService(Service service);
        int UpdateService(int serviceId);
        void LoadSubscriptions(int customerId);
    }
}
