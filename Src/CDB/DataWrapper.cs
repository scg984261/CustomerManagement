using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Configuration;
using CDB.Model;
using log4net;

namespace CDB
{
    public class DataWrapper : IDataWrapper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DataWrapper));
        public CdbContext context;
        public static string DatabaseConnectionString = string.Empty;

        static DataWrapper()
        {
            GetConnectionStringFromConfig();

            if (string.IsNullOrEmpty(DatabaseConnectionString))
            {
                throw new ConfigurationErrorsException("Unable to determine database connection string from configuration file!");
            }
            else
            {
                log.Debug($"Database connection string successfully initialised as {DatabaseConnectionString}.");
            }
        }

        public DataWrapper()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = DatabaseConnectionString;
            DbContextOptions<CdbContext> options = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<CdbContext>(), DatabaseConnectionString).Options;
            this.context = new CdbContext(options);
        }

        public static void GetConnectionStringFromConfig()
        {
            var connectionStringCollection = ConfigurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings connectionStringSettings in connectionStringCollection)
            {
                if (connectionStringSettings.Name == "CDBConnectionString")
                {
                    DatabaseConnectionString = connectionStringSettings.ConnectionString;
                }
            }
        }

        public List<Customer> SelectAllCustomers()
        {
            try
            {
                List<Customer> customerList = context.Customers.ToList();
                log.Info($"Customers successfully queried from CDB database. {customerList.Count} results returned.");
                return customerList;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }

        /// <summary>
        /// Inserts a new customer into the database by invoking a stored procedure.
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="businessContact"></param>
        /// <param name="emailAddress"></param>
        /// <param name="contactNumber"></param>
        /// <returns></returns>
        public Customer InsertNewCustomer(Customer customer)
        {
            try
            {
                context.Customers.Add(customer);
                int dbSaveResult = context.SaveChanges();
                Customer newlyInsertedCustomer = this.context.Customers.OrderByDescending(cust => cust.Id).First();
                log.Debug($"Status code {dbSaveResult} returned attempting to insert customer. New customer ID is {newlyInsertedCustomer.Id}");
                return newlyInsertedCustomer;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }

        public Customer UpdateCustomer(int? id)
        {
            Customer customerToUpdate = this.context.Customers.Where(customer => customer.Id == id).First();

            try
            {
                int dbUpdateResult = context.SaveChanges();
                this.context.Entry(customerToUpdate).Reload();
                log.Debug($"Status code {dbUpdateResult} returned. Attempting to update customer with ID {id}.");
                return customerToUpdate;
            }
            catch (Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }
    }
}
