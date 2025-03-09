using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDB.Model;

namespace CustomerManagement.ViewModel
{
    public class ServiceItemViewModel : ViewModelBase
    {
        private Service service;

        public ServiceItemViewModel(Service service)
        {
            this.service = service;
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

        public decimal Price
        {
            get
            {
                return this.service.Price;
            }
            set
            {
                this.service.Price = value;
            }
        }

        public string PriceFormatted
        {
            get
            {
                string formattedString = $"£{this.Price.ToString("0.00")}";
                return formattedString;
            }
        }
    }
}
