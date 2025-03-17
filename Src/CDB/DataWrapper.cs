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
                List<Customer> customerList = context.RunSql<Customer>("SelectAllCustomers").ToList();
                log.Info($"Customers successfully queried from CDB database. {customerList.Count} results returned.");
                return customerList;
            }
            catch (Exception exception)
            {
                log.Error($"Exception of type {exception.GetType().FullName} occurred attempting to select customers from database.\r\nException message: {exception.Message}.");
                return new List<Customer>();
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
        public Customer InsertNewCustomer(string? companyName, string? businessContact, string? emailAddress, string? contactNumber)
        {
            FormattableString sql = $"InsertCustomer {companyName}, {businessContact}, {emailAddress}, {contactNumber}";

            try
            {
                Customer customerInsertResult = context.RunSql<Customer>(sql).AsEnumerable().First();
                log.Info($"Customer with ID {customerInsertResult.Id} successfully inserted.");
                return customerInsertResult;
            } catch (Exception exception) {
                string errorMessage = $"Exception of type: {exception.GetType().FullName} occurred attempting to insert new customer record into CDB database.\r\n";
                errorMessage += $"Exception message: {exception.Message}.";
                log.Error(errorMessage);
                log.Error(exception);
                throw;
            }
        }

        public Customer UpdateCustomer(int? id, string? companyName, string? businessContact, string? emailAddress, string? contactNumber, bool isActive)
        {
            FormattableString sql = $"UpdateCustomer {id}, {companyName}, {businessContact}, {emailAddress}, {contactNumber}, {isActive}";
            FormattableString sql2 = $"dbo.SelectCustomerById {id}";

            try
            {
                Customer customerUpdateResult = context.RunSql<Customer>(sql).AsEnumerable().First();
                context.Customers.Attach(customerUpdateResult);
                var entry = context.Entry(customerUpdateResult);
                entry.State = EntityState.Modified;

                Customer selectCustomerResult = context.RunSql<Customer>(sql2).AsEnumerable().First();
                return customerUpdateResult;
            }
            catch(Exception exception)
            {
                log.Error(exception);
                throw;
            }
        }
    }
}
