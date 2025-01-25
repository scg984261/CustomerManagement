using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Configuration;
using CDB.Model;

namespace CDB
{
    public class DataWrapper
    {
        public CdbContext context;
        public static string DatabaseConnectionString = string.Empty;

        static DataWrapper()
        {
            GetConnectionStringFromConfig();

            if (string.IsNullOrEmpty(DatabaseConnectionString))
            {
                throw new ConfigurationErrorsException("Unable to determine database connection string from configuration file!");
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
                List<Customer> customerList = context.Customers.FromSqlRaw("SelectAllCustomers").ToList();
                return customerList;
            }
            catch (Exception exception)
            {
                // throw exception;
                return new List<Customer>();
            }
        }

        public void InsertNewCustomer()
        {
            string companyName = "Test company name";
            string businessContact = "test bus contact (jefferey rogers)";
            string emailAddress = "test.email@hotmail.com";
            string contactNumber = "012345678";

            FormattableString sql = $"InsertCustomer {companyName}, {businessContact}, {emailAddress}, {contactNumber}";

            var result = context.Customers.FromSql(sql);
        }
    }
}
