using GLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Data;

// this class is how i talk to sql server - Program.cs wires up the connection string then i inject this wherever i need db access
public class GlmsDbContext : DbContext
{
    // options has the connection string + provider stuff - i pass it to base() so EF knows which database to hit
    public GlmsDbContext(DbContextOptions<GlmsDbContext> options)
        : base(options)
    {
    }

    // DbSet = basically "give me the Clients table as a queryable collection" - i use _context.Clients in controllers etc
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<ServiceRequest> ServiceRequests => Set<ServiceRequest>();

    // fluent api = i configure the database shape here instead of putting [Key] [MaxLength] on every property in the model files
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ----- Client table -----
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ContactDetails).HasMaxLength(500);
            entity.Property(e => e.Region).HasMaxLength(100);
        });

        // ----- Contract table -----
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId);

            // from brief - sla / package name, must have a value
            entity.Property(e => e.ServiceLevel).IsRequired().HasMaxLength(100);

            // optional - only filled after upload; 260 is just a safe max for a filename
            entity.Property(e => e.SignedAgreementFileName).HasMaxLength(260);

            // indexes - helps when i do admin search/filter later
            entity.HasIndex(e => e.ClientId);
            entity.HasIndex(e => new { e.Status, e.EndDate });

            // contract belongs to one client, client has many contracts - ClientId column is the fk
            entity.HasOne(e => e.Client)
                .WithMany(e => e.Contracts)
                .HasForeignKey(e => e.ClientId)
                // restrict = dont auto delete contracts if someone deletes a client (safer for demo data)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ----- ServiceRequest table -----
        modelBuilder.Entity<ServiceRequest>(entity =>
        {
            entity.HasKey(e => e.ServiceRequestId);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);

            // 18,2 is normal money precision for sql (zar amounts)
            entity.Property(e => e.Cost).HasPrecision(18, 2);
            entity.HasIndex(e => e.ContractId);

            // each request belongs to one contract, contract has a list of requests
            entity.HasOne(e => e.Contract)
                .WithMany(e => e.ServiceRequests)
                .HasForeignKey(e => e.ContractId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

// References (accessed 18 Apr 2026):
// - EF Core model building: https://learn.microsoft.com/en-us/ef/core/modeling/
// - EF Core relationships: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
// - EF Core indexes: https://learn.microsoft.com/en-us/ef/core/modeling/indexes
// - SQL Server value precision in EF Core: https://learn.microsoft.com/en-us/ef/core/modeling/entity-properties
