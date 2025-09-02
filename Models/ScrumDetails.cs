using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrumMaster.Models;

/// <summary>
/// Represents a Scrum work item (User Story, Task, Bug, etc.).
/// </summary>
public class ScrumDetails
{
    /// <summary>
    /// Represents the status of a Scrum work item.
    /// </summary>
    public enum ScrumStatus
    {
        /// <summary>
        /// The work item is newly created and not yet started.
        /// </summary>
        New = 1,

        /// <summary>
        /// Work on the item is currently in progress.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// The item has been completed successfully.
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Work is temporarily stopped due to blockers or dependencies.
        /// </summary>
        Blocker = 4
    }

    /// <summary>
    /// Auto-generated primary key.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Scrum entry date. Defaults to today's date.
    /// </summary>
    [DisplayName("Date")]
    [DataType(DataType.Date)]
    public DateTime ScrumDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Foreign key referencing the assigned user.
    /// </summary>
    [Required]
    [DisplayName("Assigned To")]
    public int UserId { get; set; }

    /// <summary>
    /// Navigation property for the assigned user.
    /// </summary>
    public UserDetails? User { get; set; }

    /// <summary>
    /// Foreign key referencing the project.
    /// </summary>
    [Required]
    [DisplayName("Project")]
    public int ProjectId { get; set; }

    /// <summary>
    /// Navigation property for the project.
    /// </summary>
    public ProjectDetails? Project { get; set; }

    /// <summary>
    /// Unique identifier of the work item.
    /// </summary>
    [Required, StringLength(50)]
    [DisplayName("Item Id")]
    public string ItemId { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the work item.
    /// </summary>
    [Required, StringLength(500)]
    [DisplayName("Description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the work item.
    /// </summary>
    [Required]
    [DisplayName("Status")]
    public ScrumStatus Status { get; set; }
}
