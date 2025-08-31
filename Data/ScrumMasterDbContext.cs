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
    public DbSet<ClientProfile> m_ClientProfile { get; set; } = null!;

    /// <summary>
    /// Table for project details.
    /// </summary>
    public DbSet<ProjectDetails> m_ProjectDetail { get; set; } = null!;

    /// <summary>
    /// Table for user details.
    /// </summary>
    public DbSet<UserDetails> m_UserDetail { get; set; } = null!;

    /// <summary>
    /// Table for scrum details.
    /// </summary>
    public DbSet<ScrumDetails> m_ScrumDetails { get; set; } = null!;

    /// <summary>
    /// Configure entity relationships.
    /// </summary>
    /// <param name="modelBuilder">EF model builder.</param>
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
