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

        public Customer InsertNewCustomer(string? companyName, string? businessContact, string? emailAddress, string? contactNumber)
        {
            try
            {
                Customer newlyInsertedCustomer = cdbDataWrapper.InsertNewCustomer(companyName, businessContact, emailAddress, contactNumber);
                log.Info($"New Customer record successfully inserted into the database with ID {newlyInsertedCustomer.Id}.");
                return newlyInsertedCustomer;
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception occurred attempting to insert customer into the database. Company name: {companyName}, Business contact: {businessContact} Email address: {emailAddress}, Contact number: {contactNumber}.";
                log.Error(errorMessage);
                log.Error(exception);
                throw;
            }
        }

        public Customer UpdateCustomer(int? id, string? companyName, string? businessContact, string? emailAddress, string? contactNumber, bool isActive)
        {
            try
            {
                Customer updatedCustomer = cdbDataWrapper.UpdateCustomer(id, companyName, businessContact, emailAddress, contactNumber, isActive);
                return updatedCustomer;
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception of type {exception.GetType().FullName} occurred attempting to update customer with values: ID: {id}, Company name: {companyName}, Business contact: {businessContact} Email address: {emailAddress}, Contact number: {contactNumber}, Is Active: {isActive}.";
                log.Error(errorMessage);
                throw;
            }
        }
    }
}
