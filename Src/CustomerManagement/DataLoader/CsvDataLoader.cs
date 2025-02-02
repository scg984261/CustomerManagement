using System.IO;
using CDB;
using CDB.Model;
using log4net;

namespace CustomerManagement.DataLoader
{
    public class CsvDataLoader
    {
        private readonly ILog log = LogManager.GetLogger(typeof(CsvDataLoader));

        public IDataWrapper dataWrapper = new DataWrapper();

        // Integers to represent the index positions of the
        // columns with respective data.
        public int companyNameColumn = -1;
        public int businessContactColumn = -1;
        public int emailAddressColumn = -1;
        public int contactNumberColumn = -1;

        public List<Customer> LoadCustomersFromFile(string filePath)
        {
            log.Info($"Starting customer data load from file {filePath}.");

            DetermineHeaderColumns(filePath);

            string[] lines = File.ReadAllLines(filePath).Skip(1).ToArray();
            List<Customer> customerList = this.ConvertLinesToCustomers(lines);

            return customerList;
        }

        public void DetermineHeaderColumns(string file)
        {
            log.Info("Determining file headers.");

            string headerLine = File.ReadAllLines(file).First();
            log.Debug($"Header line read from file: {headerLine}.");

            string[] fileHeaders = headerLine.Split(',');
            log.Debug($"File headers parsed. {fileHeaders.Length} headers found.");

            this.FormatFileHeaders(fileHeaders);
            this.SetColumnIndices(fileHeaders);
        }


        public void FormatFileHeaders(string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                string header = headers[i];
                header = header.Replace(" ", string.Empty).ToUpper();
                headers[i] = header;
            }
        }

        public void SetColumnIndices(string[] headers)
        {
            this.companyNameColumn = Array.IndexOf(headers, "COMPANYNAME");
            log.Debug($"Company name column determined as {this.companyNameColumn}");

            this.businessContactColumn = Array.IndexOf(headers, "BUSINESSCONTACT");
            log.Debug($"Business contact  column determined as {this.businessContactColumn}");

            this.emailAddressColumn = Array.IndexOf(headers, "EMAILADDRESS");
            log.Debug($"Email address column determined as {this.emailAddressColumn}");

            this.contactNumberColumn = Array.IndexOf(headers, "CONTACTNUMBER");
            log.Debug($"Contact number column determined as {this.contactNumberColumn}");
        }

        public virtual List<Customer> ConvertLinesToCustomers(string[] lines)
        {
            List<Customer> customers = new List<Customer>();

            foreach (string line in lines)
            {
                try
                {
                    Customer customer = this.InsertCustomer(line);
                    customers.Add(customer);
                }
                catch (Exception exception)
                {
                    string errorMessage = $"Exception of type {exception.GetType().FullName} occurred attemtping to insert customer into database from line {line}.\r\nError message: {exception.Message}.";
                    log.Error(errorMessage);
                }
            }

            log.Debug($"Customer data load complete. {customers.Count} rows inserted");
            return customers;
        }

        public virtual Customer InsertCustomer(string line)
        {
            try
            {
                string[] columns = line.Split(',');
                string companyName = columns[this.companyNameColumn];
                string businessContact = columns[this.businessContactColumn];
                string emailAddress = columns[this.emailAddressColumn];
                string contactNumber = columns[this.contactNumberColumn];
                return this.InsertCustomer(companyName, businessContact, emailAddress, contactNumber);
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception of type {exception.GetType().FullName} occurred attempting to insert customer from line {line} into the database.\r\nException message {exception.Message}.";
                log.Error(errorMessage);
                log.Error(exception);
                throw;
            }
        }

        public virtual Customer InsertCustomer(string companyName, string businessContact, string emailAddress, string contactNumber)
        {
            try
            {
                Customer customer = dataWrapper.InsertNewCustomer(companyName, businessContact, emailAddress, contactNumber);
                log.Debug($"New customer with ID {customer.Id} inserted into the database.");
                return customer;
            }
            catch (Exception exception)
            {
                string errorMessage = $"Exception of type {exception.GetType().FullName} occurred attempting to insert customer with company name {companyName}, business contact {businessContact}, email address {emailAddress}, and contact number {contactNumber} into the database\r\n.";
                errorMessage += $"Exception message: {exception.Message}.";
                log.Error(errorMessage);
                log.Error(exception);
                throw;
            }
        }
    }
}
