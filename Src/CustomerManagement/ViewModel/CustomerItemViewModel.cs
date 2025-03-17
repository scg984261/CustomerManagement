using CDB.Model;

namespace CustomerManagement.ViewModel
{
    public class CustomerItemViewModel : ViewModelBase
    {
        private Customer customer;

       /// <summary>
       /// Whether the CustomersViewModel should add a new customer when this Window closes.
       /// </summary>
        public bool AddCustomerOnClose { get; set; }

        public CustomerItemViewModel(Customer customer)
        {
            this.customer = customer;
        }

        public CustomerItemViewModel()
        {
            this.customer = new Customer();
        }

        public int Id
        {
            get
            {
                return this.customer.Id;
            }
        }
        
        public string? CompanyName
        {
            get
            {
                return this.customer.CompanyName;
            }
            set
            {
                if (value is null)
                {
                    value = string.Empty;
                }

                this.customer.CompanyName = value;
                this.NotifyPropertyChanged();
            }
        }

        public string? BusinessContact
        {
            get
            {
                return this.customer.BusinessContact;
            }
            set
            {
                this.customer.BusinessContact = value;
                this.NotifyPropertyChanged();
            }
        }

        public string? EmailAddress
        {
            get
            {
                return this.customer.EmailAddress;
            }
            set
            {
                this.customer.EmailAddress = value;
                this.NotifyPropertyChanged();
            }
        }

        public string? ContactNumber
        {
            get
            {
                return this.customer.ContactNumber;
            }
            set
            {
                this.customer.ContactNumber = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool IsActive
        {
            get
            {
                return this.customer.IsActive;
            }
            set
            {
                this.customer.IsActive = value;
                this.NotifyPropertyChanged();
            }
        }

        public DateTime CreatedDateTime
        {
            get
            {
                return this.customer.CreatedDateTime;
            }
        }

        public DateTime LastUpdateDateTime
        {
            get
            {
                return this.customer.LastUpdateDateTime;
            }
            set
            {
                this.customer.LastUpdateDateTime = value;
                this.NotifyPropertyChanged();
            }
        }
    }
}
