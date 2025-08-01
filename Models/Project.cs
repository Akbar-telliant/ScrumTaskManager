using System.ComponentModel.DataAnnotations;

namespace ScrumMaster.Models;

/// <summary>
/// Represents a project entity.
/// </summary>
public class Project
{
    /// <summary>
    /// Unique identifier for the project.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the project.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
