namespace CDB.Model;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsRecurring { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime LastUpdateDateTime { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
