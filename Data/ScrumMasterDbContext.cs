using Microsoft.EntityFrameworkCore;
using ScrumMaster.Models;

namespace ScrumMaster.Data;

/// <summary>
/// EF Core DbContext for ScrumMaster application entities.
/// </summary>
public class ScrumMasterDbContext : DbContext
{
    /// <summary>
    /// Creates a new ScrumMasterDbContext.
    /// </summary>
    /// <param name="options">DbContext configuration options.</param>
    public ScrumMasterDbContext(DbContextOptions<ScrumMasterDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Table for client profiles.
    /// </summary>
    public DbSet<ClientProfile> ClientProfile { get; set; }

    /// <summary>
    /// Table for project detailes.
    /// </summary>
    public DbSet<ProjectDetail> ProjectDetail { get; set; } = null!;

    /// <summary>
    /// Configure entity relationships.
    /// </summary>
    /// <param name="modelBuilder"> EF model builder. </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Client ↔ Projects (One-to-Many)
        modelBuilder.Entity<ClientProfile>()
            .HasMany(c => c.Projects)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Cascade); // delete projects when client is deleted
    }
}
