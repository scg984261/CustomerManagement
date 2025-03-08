using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDB.Model;

namespace CustomerManagement.ViewModel
{
    public class CustomerItemViewModel : ViewModelBase
    {
        private Customer customer;

        public CustomerItemViewModel(Customer customer)
        {
            this.customer = customer;
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
        }

        public string? BusinessContact
        {
            get
            {
                return this.customer.BusinessContact;
            }
        }

        public string? EmailAddress
        {
            get
            {
                return this.customer.EmailAddress;
            }
        }

        public string? ContactNumber
        {
            get
            {
                return this.customer.ContactNumber;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.customer.IsActive;
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
        }
    }
}
