using CDB.Model;

namespace CustomerManagement.ViewModel.ServiceViewModels
{
    public class ServiceItemViewModel : ViewModelBase
    {
        private Service service;

        public ServiceItemViewModel(Service service)
        {
            this.service = service;
        }

        public ServiceItemViewModel()
        {
            this.service = new Service();
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
                string formattedString = $"£{this.service.Price.ToString("0.00")}";
                return formattedString;
            }
        }

        public bool IsRecurring
        {
            get
            {
                return this.service.IsRecurring;
            }
            set
            {
                this.service.IsRecurring = value;
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
