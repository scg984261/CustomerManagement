using CDB.Model;

namespace CDB
{
    public interface IDataWrapper
    {
        void InsertNewCustomer(Customer customer);
        int UpdateCustomer(int id);
        List<Customer> SelectAllCustomers();
        List<Service> SelectAllServices();
        void InsertNewService(Service service);
        void UpdateService(int serviceId);
    }
}
