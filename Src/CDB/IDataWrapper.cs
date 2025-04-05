using CDB.Model;

namespace CDB
{
    public interface IDataWrapper
    {
        int InsertNewCustomer(Customer customer);
        int UpdateCustomer(int id);
        List<Customer> SelectAllCustomers();
        List<Service> SelectAllServices();
        int InsertNewService(Service service);
        int UpdateService(int serviceId);
    }
}
