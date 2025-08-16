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
}
