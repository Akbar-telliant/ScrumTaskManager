using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrumMaster.Models;

/// <summary>
/// Represents a Scrum work item (User Story, Task, Bug, etc.).
/// </summary>
public class ScrumDetails
{
    /// <summary>
    /// Defines the type of a Scrum work item.
    /// </summary>
    public enum ScrumItemType
    {
        /// <summary>
        /// A user story representing a business requirement or feature.
        /// </summary>
        Story = 1,

        /// <summary>
        /// A technical or development task needed to complete a story or project work.
        /// </summary>
        Task = 2,

        /// <summary>
        /// A defect or issue reported that needs fixing.
        /// </summary>
        Bug = 3,

        /// <summary>
        /// A large body of work that can be broken down into multiple stories.
        /// </summary>
        Epic = 4,

        /// <summary>
        /// A research or exploration task to gather information or test feasibility.
        /// </summary>
        Spike = 5
    }

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
        /// Work has been completed and is awaiting review or testing.
        /// </summary>
        Review = 3,

        /// <summary>
        /// The item has been completed successfully.
        /// </summary>
        Done = 4,

        /// <summary>
        /// Work is temporarily stopped due to blockers or dependencies.
        /// </summary>
        Blocked = 5
    }

    /// <summary>
    /// Defines the priority level of a Scrum work item.
    /// </summary>
    public enum ScrumPriority
    {
        /// <summary>
        /// Low urgency; can be addressed later.
        /// </summary>
        Low = 1,

        /// <summary>
        /// Normal importance; should be completed in due course.
        /// </summary>
        Medium = 2,

        /// <summary>
        /// High importance; should be prioritized for delivery.
        /// </summary>
        High = 3,

        /// <summary>
        /// Critical urgency; requires immediate attention.
        /// </summary>
        Critical = 4
    }

    /// <summary>
    /// Auto-generated primary key.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Business identifier for the work item (e.g., US-1001, T-2002, BUG-3003).
    /// </summary>
    [Required(ErrorMessage = "Item ID is required")]
    [StringLength(50, ErrorMessage = "Max 50 characters")]
    public string ItemId { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the project this item belongs to.
    /// </summary>
    [ForeignKey("ProjectDetails")]
    public int ProjectId { get; set; }

    /// <summary>
    /// Navigation property for the project.
    /// </summary>
    public ProjectDetails? Project { get; set; }

    /// <summary>
    /// Title or short name of the Scrum item.
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Max 200 characters")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the Scrum item.
    /// </summary>
    [StringLength(2000, ErrorMessage = "Max 2000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Type of Scrum item (Story, Task, Bug, Epic, Spike).
    /// </summary>
    [Required]
    public ScrumItemType ItemType { get; set; } = ScrumItemType.Story;

    /// <summary>
    /// Status of the item (New, InProgress, Review, Done, Blocked).
    /// </summary>
    [Required]
    public ScrumStatus Status { get; set; } = ScrumStatus.New;

    /// <summary>
    /// Priority (Low, Medium, High, Critical).
    /// </summary>
    [Required]
    public ScrumPriority Priority { get; set; } = ScrumPriority.Medium;

    /// <summary>
    /// Story points estimation (Agile metric).
    /// </summary>
    [Range(0, 100, ErrorMessage = "Story points must be between 0 and 100")]
    public int StoryPoints { get; set; }

    /// <summary>
    /// Sprint number this item is part of.
    /// </summary>
    public int? SprintNumber { get; set; }

    /// <summary>
    /// Person assigned to the item.
    /// </summary>
    [StringLength(100)]
    public string? AssignedTo { get; set; }

    /// <summary>
    /// Date when item was created.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Due date for completing the item.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Flag indicating if item is active or archived.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
