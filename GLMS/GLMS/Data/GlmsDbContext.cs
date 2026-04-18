using GLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Data;

// bridges my models to sql server tables - inject this in controllers
public class GlmsDbContext : DbContext
{
    public GlmsDbContext(DbContextOptions<GlmsDbContext> options)
        : base(options)
    {
    }

    // each DbSet = a table more or less
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

    // fluent api = configure columns/keys here instead of [attributes] all over the models
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactDetails).HasMaxLength(500);
            entity.Property(e => e.Region).HasMaxLength(100);
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId);
            entity.Property(e => e.ServiceLevel).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SignedAgreementFileName).HasMaxLength(260);

            // indexes - helps when i do admin search/filter later
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.Status, e.EndDate });

            entity.HasOne(e => e.Client)
                .WithMany(e => e.Contracts)
                .HasForeignKey(e => e.ClientId)
                // restrict = dont auto delete contracts if someone deletes a client (safer for demo data)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.HasKey(e => e.ServiceRequestId);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
            // 18,2 is normal money precision for sql
            entity.Property(e => e.Cost).HasPrecision(18, 2);
            entity.HasIndex(e => e.ContractId);

            entity.HasOne(e => e.Contract)
                .WithMany(e => e.ServiceRequests)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
