namespace CDB.Model
{
    public partial class Service
    {
        public Service()
        {
            this.Id = 0;
            this.Name = string.Empty;
            this.Price = 0.0m;
            this.IsRecurring = false;
            this.LastUpdateDateTime = new DateTime();
            this.CreatedDateTime = new DateTime();
        }

        public Service(string? name, decimal price, bool isRecurring)
        {
            if (name != null)
            {
                this.Name = name;
            }
            this.Price = price;
            this.IsRecurring = isRecurring;
            this.LastUpdateDateTime = new DateTime();
            this.CreatedDateTime = new DateTime();
        }

        public Service(int id, string? name, decimal price, bool isRecurring, DateTime createdDateTime, DateTime lastUpdateDateTime) : this(name, price, isRecurring)
        {
            this.Id = id;
            this.CreatedDateTime = createdDateTime;
            this.LastUpdateDateTime = lastUpdateDateTime;
        }
    }
}
