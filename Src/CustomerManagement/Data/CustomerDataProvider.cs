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
            try
            {
                List<Customer> customers = new List<Customer>();
                customers = cdbDataWrapper.SelectAllCustomers();
                log.Info($"Customer records successfully retrieved from database. {customers.Count} customers returned.");
                return customers;
            }
            catch(Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }

        public void InsertNewCustomer(Customer customer)
        {
            try
            {
                cdbDataWrapper.InsertNewCustomer(customer);
                log.Info($"New Customer record successfully inserted into the database with ID {customer.Id}.");
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception occurred attempting to insert customer into the database. Company name: {customer.CompanyName}, Business contact: {customer.BusinessContact} Email address: {customer.EmailAddress}, Contact number: {customer.ContactNumber}.";
                log.Error(errorMessage);
                log.Error(exception);
                throw;
            }
        }

        public void UpdateCustomer(int customerId)
        {
            try
            {
                int updateResult = cdbDataWrapper.UpdateCustomer(customerId);
                log.Info($"Customer with ID {customerId} updated. Result code was {updateResult}.");
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception of type {exception.GetType().FullName} occurred attempting to update customer with values: ID: {customerId}.";
                log.Error(errorMessage);
                log.Error(exception);
                throw;
            }
        }
    }
}
