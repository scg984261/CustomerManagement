using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using CDB.Model;
using System.Printing;

namespace CustomerManagement.ViewModel
{
    public class ServiceItemViewModel : ValidationViewModelBase
    {
        private Service service;
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceItemViewModel));

        public ServiceItemViewModel(Service service)
        {
            this.service = service;
            this.priceString = string.Empty;
        }

        public int Id
        {
            get
            {
                return this.service.Id;
            }
            set
            {
                this.service.Id = value;
            }
        }

        public string Name
        {
            get
            {
                return this.service.Name;
            }
            set
            {
                this.service.Name = value;
            }
        }

        private string priceString;

        public string PriceString
        {
            get
            {
                return this.priceString;
            }

            set
            {
                this.priceString = value;

                if (string.IsNullOrEmpty(this.priceString))
                {
                    const string errorMessage = "Price cannot be blank!";
                    this.AddError(errorMessage);
                    return;
                }
                else
                {
                    this.ClearErrors();
                }

                decimal price;
                if (Decimal.TryParse(value, out price))
                {
                    this.service.Price = price;
                    this.ClearErrors();
                }
                else
                {
                    const string errorMessage = $"Value must be a valid decimal.";
                    this.service.Price = 0m;
                    this.AddError(errorMessage);
                }

                this.NotifyPropertyChanged(nameof(PriceFormatted));
            }
        }

        public string PriceFormatted
        {
            get
            {
                string formattedString = $"£{this.service.Price.ToString("0.00")}";
                return formattedString;
            }
        }
    }
}
