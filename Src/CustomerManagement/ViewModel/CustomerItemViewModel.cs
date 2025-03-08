using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDB.Model;
using CustomerManagement.Command;
using log4net;

namespace CustomerManagement.ViewModel
{
    public class CustomerItemViewModel : ValidationViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomerItemViewModel));
        private Customer customer;
        public DelegateCommand UpdateCustomerCommand { get; }
        public DelegateCommand AddCustomerCommand { get; }
        public bool AddCustomerOnClose { get; set; }

        public CustomerItemViewModel(Customer customer)
        {
            this.customer = customer;
            this.UpdateCustomerCommand = new DelegateCommand(this.UpdateCustomer, this.CanSaveCustomer);
            this.AddCustomerCommand = new DelegateCommand(this.InsertCustomer, this.CanSaveCustomer);
            this.AddCustomerOnClose = false;
        }

        public void UpdateCustomer(object? parameters)
        {
            // Update Customer in CDB
            log.Info($"Updating customer with ID {this.Id}");
        }

        public void InsertCustomer(object? parameter)
        {
            // Insert new Customer in CDB.
            log.Info("Inserting new Customer");
            this.AddCustomerOnClose = true;
        }

        public bool CanSaveCustomer(object? parameter)
        {
            if (this.HasErrors) return false;
            if (string.IsNullOrEmpty(this.CompanyName)) return false;
            if (string.IsNullOrEmpty(this.BusinessContact)) return false;
            if (string.IsNullOrEmpty(this.ContactNumber)) return false;
            if (string.IsNullOrEmpty(this.EmailAddress)) return false;

            return true;
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
                this.customer.CompanyName = value;
                this.NotifyPropertyChanged();

                if (string.IsNullOrEmpty(this.customer.CompanyName))
                {
                    const string errorMessage = "Company name cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.UpdateCustomerCommand.RaiseCanExecuteChanged();
                this.AddCustomerCommand.RaiseCanExecuteChanged();
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

                if (string.IsNullOrEmpty(this.customer.BusinessContact))
                {
                    const string errorMessage = "Business contact cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.UpdateCustomerCommand.RaiseCanExecuteChanged();
                this.AddCustomerCommand.RaiseCanExecuteChanged();
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

                if (string.IsNullOrEmpty(this.customer.EmailAddress))
                {
                    const string errorMessage = "Email address cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.UpdateCustomerCommand.RaiseCanExecuteChanged();
                this.AddCustomerCommand.RaiseCanExecuteChanged();
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

                if (string.IsNullOrEmpty(this.customer.ContactNumber))
                {
                    const string errorMessage = "Contact number cannot be blank";
                    this.AddError(errorMessage);
                }
                else
                {
                    this.ClearErrors();
                }

                this.UpdateCustomerCommand.RaiseCanExecuteChanged();
                this.AddCustomerCommand.RaiseCanExecuteChanged();
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
