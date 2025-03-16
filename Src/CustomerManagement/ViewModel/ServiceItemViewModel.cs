using CDB.Model;

namespace CustomerManagement.ViewModel
{
    public class ServiceItemViewModel : ViewModelBase
    {
        private Service service;

        public ServiceItemViewModel(Service service)
        {
            this.service = service;
            this.priceString = string.Empty;
        }

        public ServiceItemViewModel()
        {
            this.service = new Service();
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

        public DateTime CreatedDateTime
        {
            get
            {
                return this.service.CreatedDateTime;
            }
        }

        public DateTime LastUpdateDateTime
        {
            get
            {
                return this.service.LastUpdateDateTime;
            }
        }
    }
}
