namespace CDB.Model
{
    public partial class Customer : IEquatable<Customer>
    {
        public Customer()
        {
            this.Id = 0;
            this.CompanyName = string.Empty;
            this.BusinessContact = string.Empty;
            this.EmailAddress = string.Empty;
            this.ContactNumber = string.Empty;
            this.IsActive = false;
            this.CreatedDateTime = new DateTime();
            this.LastUpdateDateTime = new DateTime();
            this.Subscriptions = new List<Subscription>();
        }

        public Customer(int id, string companyName, string businessContact, string emailAddress, string contactNumber, DateTime createdDateTime, DateTime lastUpdateDateTime)
        {
            this.Id = id;
            this.CompanyName = companyName;
            this.BusinessContact = businessContact;
            this.EmailAddress = emailAddress;
            this.ContactNumber = contactNumber;
            this.IsActive = true;
            this.CreatedDateTime = createdDateTime;
            this.LastUpdateDateTime = lastUpdateDateTime;
            this.Subscriptions = new List<Subscription>();
        }

        public Customer(string? companyName, string? businessContact, string? emailAddress, string? contactNumber)
        {
            if (companyName != null)
            {
                this.CompanyName = companyName;
            }
            
            this.BusinessContact = businessContact;
            this.EmailAddress = emailAddress;
            this.ContactNumber = contactNumber;
            this.IsActive = true;
        }

        public bool Equals(Customer? customer)
        {
            if (this.Id != customer?.Id)
            {
                return false;
            }


            if (this.CompanyName != customer.CompanyName)
            {
                return false;
            }

            if (this.BusinessContact != customer.BusinessContact)
            {
                return false;
            }

            if (this.EmailAddress != customer.EmailAddress) return false;
            if (this.ContactNumber != customer.ContactNumber) return false;
            if (this.IsActive != customer.IsActive) return false;
            if (this.CreatedDateTime != customer.CreatedDateTime) return false;
            if (this.LastUpdateDateTime != customer.LastUpdateDateTime) return false;
            return true;
        }
    }
}
