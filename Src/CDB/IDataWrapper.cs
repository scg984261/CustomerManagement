using CDB.Model;

namespace CDB
{
    public interface IDataWrapper
    {
        Customer InsertNewCustomer(string companyName, string businessContact, string emailAddress, string contactNumber);
        List<Customer> SelectAllCustomers();
    }
}
