using System.ComponentModel.DataAnnotations;

namespace ScrumMaster.Models;

/// <summary>
/// User details for authentication and profile management.
/// </summary>
public class UserDetails
{
    /// <summary>
    /// User roles for the application.
    /// </summary>
    public enum TUserRole
    {
        Admin,
        User
    }

    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    [Key]
    public int UserId { get; set; }

    /// <summary>
    /// User Name.
    /// </summary>
    [Required(ErrorMessage = "User name is required")]
    [StringLength(250, ErrorMessage = "User name cannot exceed 250 characters")]
    public string? UserName { get; set; }

    /// <summary>
    /// Email.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address format")]
    public string? Email { get; set; }

    /// <summary>
    /// Role of the user.
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    public TUserRole Role { get; set; } = TUserRole.User;

    /// <summary>
    /// Date when the user was created.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public UserDetails()
    {
    }

    public UserDetails(string name, string email, TUserRole role)
    {
        UserName = name;
        Email = email;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}