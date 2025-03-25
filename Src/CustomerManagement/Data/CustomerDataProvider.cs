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

        public Customer InsertNewCustomer(Customer customer)
        {
            try
            {
                Customer newlyInsertedCustomer = cdbDataWrapper.InsertNewCustomer(customer);
                log.Info($"New Customer record successfully inserted into the database with ID {newlyInsertedCustomer.Id}.");
                return newlyInsertedCustomer;
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception occurred attempting to insert customer into the database. Company name: {customer.CompanyName}, Business contact: {customer.BusinessContact} Email address: {customer.EmailAddress}, Contact number: {customer.ContactNumber}.";
                log.Error(errorMessage);
                log.Error(exception);
                throw;
            }
        }

        public Customer? UpdateCustomer(int? id)
        {
            try
            {
                Customer? updatedCustomer = cdbDataWrapper.UpdateCustomer(id);
                return updatedCustomer;
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception of type {exception.GetType().FullName} occurred attempting to update customer with values: ID: {id}.";
                log.Error(errorMessage);
                throw;
            }
        }
    }
}
