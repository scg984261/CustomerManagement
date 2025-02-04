using System;
using CDB.Model;
using Microsoft.EntityFrameworkCore;

namespace CDB;

public partial class CdbContext : DbContext
{
    public CdbContext()
    {
    }

    public CdbContext(DbContextOptions<CdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-3GIU7MF\\SQLEXPRESS;Initial Catalog=CDB;trustservercertificate=True;trusted_connection=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CustomerIdPrimaryKey");

            entity.ToTable("Customer", tb => tb.HasTrigger("CustomerAfterUpdateTrigger"));

            entity.Property(e => e.BusinessContact).HasMaxLength(256);
            entity.Property(e => e.CompanyName).HasMaxLength(256);
            entity.Property(e => e.ContactNumber).HasMaxLength(32);
            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(128);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastUpdateDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ServiceIdPrimaryKey");

            entity.ToTable("Service", tb => tb.HasTrigger("ServiceAfterUpdateTrigger"));

            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdateDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SubscriptionIdPrimaryKey");

            entity.ToTable("Subscription");

            entity.Property(e => e.CreatedDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SubscriptionCustomerIdForeignKey");

            entity.HasOne(d => d.Service).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SubscriptionServiceIdForeignKey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public virtual IQueryable<TEntity> RunSql<TEntity>(string sql, params object[] parameters) where TEntity : class
    {
        return this.Set<TEntity>().FromSqlRaw(sql, parameters);
    }
}
