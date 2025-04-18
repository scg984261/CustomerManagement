namespace CDB.Model;

public partial class Subscription
{
    public int CustomerId { get; set; }

    public int ServiceId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
