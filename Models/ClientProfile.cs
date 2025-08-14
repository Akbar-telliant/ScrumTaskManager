using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrumMaster.Models;

/// <summary>
/// Client profile details.
/// </summary>
public class ClientProfile
{
    /// <summary>
    /// Auto-generated primary key.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Client's full name.
    /// </summary>
    [Required(ErrorMessage = "Client name is required")]
    [StringLength(100, ErrorMessage = "Max 100 characters")]
    public string? Name { get; set; }

    /// <summary>
    /// Unique short client code.
    /// </summary>
    [StringLength(20, ErrorMessage = "Max 20 characters")]
    [RegularExpression("^[A-Za-z0-9_-]*$", ErrorMessage = "Use letters, numbers, '-' or '_'")]
    public string? Code { get; set; }

    /// <summary>
    /// Indicates if client is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Default team size for the client.
    /// </summary>
    [Range(0, 1000, ErrorMessage = "0–1000 allowed")]
    public int? DefaultTeamSize { get; set; }

    /// <summary>
    /// Additional notes.
    /// </summary>
    [StringLength(500, ErrorMessage = "Max 500 characters")]
    public string? Notes { get; set; }
}
