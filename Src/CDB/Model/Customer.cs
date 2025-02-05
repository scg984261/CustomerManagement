namespace CDB.Model;

public partial class Customer : IEquatable<Customer>
{
    public int Id { get; set; }
    public string? SageRef { get; set; }
    public string CompanyName { get; set; } = null!;
    public string? BusinessContact { get; set; }
    public string? EmailAddress { get; set; }
    public string? ContactNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime LastUpdateDateTime { get; set; }
    public virtual ICollection<Subscription>? Subscriptions { get; set; }
}
