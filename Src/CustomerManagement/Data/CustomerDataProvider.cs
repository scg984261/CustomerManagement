using CDB.Model;

namespace CustomerManagement.Data
{
    public interface ICustomerDataProvider
    {
        Task<IEnumerable<Customer>?> GetAllAsync();
    }

    public class CustomerDataProvider : ICustomerDataProvider
    {
        public async Task<IEnumerable<Customer>?> GetAllAsync()
        {
            await Task.Delay(100); // Simulate a bit of server work

            List<Customer>? customers = new List<Customer>();

            customers.Add(new Customer(1, "Hudson House", "Niall Gillen", "niallgillen@hudsonhouse.co.uk", "(01605) 641755", DateTime.Now, DateTime.Now));
            customers.Add(new Customer(2, "Curabitur Egestas Company", "Ronan Hull", "fermentum.vel@aol.net", "(01605) 64594597", DateTime.Now, DateTime.Now));
            customers.Add(new Customer(3, "Orci Luctus Et Consulting", "Craig Duke", "sit.amet.consectetuer@outlook.com", "0800 1111", DateTime.Now, DateTime.Now));
            customers.Add(new Customer(4, "Ac Turpis Limited", "Clementine Evans", "ligula.consectetuer@hotmail.com", "(01518) 259130", DateTime.Now, DateTime.Now));

            return customers;
        }
    }
}
