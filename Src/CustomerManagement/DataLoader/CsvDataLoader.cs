using System.IO;
using CDB.Model;
using log4net;

namespace CustomerManagement.DataLoader
{
    public class CsvDataLoader
    {
        private readonly ILog log = LogManager.GetLogger(typeof(CsvDataLoader));

        // Integers to represent the index positions of the
        // columns with respective data.
        public int companyNameColumn = -1;
        public int businessContactColumn = -1;
        public int emailAddressColumn = -1;
        public int contactNumberColumn = -1;

        // TODO: could potentially use a dictionay to make this a bit more elegant.

        public List<Customer> LoadCustomersFromFile(string filePath, bool hasHeaders = false)
        {
            log.Info($"Starting customer data load from file {filePath}.");

            if (hasHeaders)
            {
                log.Info("Loading data from file with headers.");
                DetermineHeaderColumns(filePath);
            }
            else
            {
                log.Info("Headers will be ignord when loading ");
            }

            return new List<Customer>();
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
    }
}
