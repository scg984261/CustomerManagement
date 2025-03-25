using CDB.Model;

namespace CDB
{
    public interface IDataWrapper
    {
        Customer InsertNewCustomer(Customer customer);
        List<Customer> SelectAllCustomers();
    }
}
