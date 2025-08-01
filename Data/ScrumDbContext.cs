using Microsoft.EntityFrameworkCore;
using ScrumMaster.Models;

namespace ScrumMaster.Data;

/// <summary>
/// Database context for the ScrumMaster app.
/// </summary>
public class ScrumDbContext : DbContext
{
    /// <summary>
    /// Initializes the database context with options.
    /// </summary>
    /// <param name="options">The options to configure the database context.</param>
    public ScrumDbContext(DbContextOptions<ScrumDbContext> options) : base(options) { }

    /// <summary>
    /// Represents the Projects table in the database.
    /// </summary>
    public DbSet<Project> Projects { get; set; }
}
