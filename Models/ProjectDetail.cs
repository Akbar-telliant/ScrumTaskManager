using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrumMaster.Models;

/// <summary>
/// Project details for a client.
/// </summary>
public class ProjectDetail
{
    /// <summary>
    /// Auto-generated primary key.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key to the client this project belongs to.
    /// </summary>
    [ForeignKey("ClientProfile")]
    public int ClientId { get; set; }

    /// <summary>
    /// Navigation property for the client.
    /// </summary>
    public ClientProfile? Client { get; set; }

    /// <summary>
    /// Project name.
    /// </summary>
    [Required(ErrorMessage = "Project name is required")]
    [StringLength(150, ErrorMessage = "Max 150 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Short description or notes about the project.
    /// </summary>
    [StringLength(1000, ErrorMessage = "Max 1000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Project start date.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Project end date (optional).
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Indicates if project is active.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
