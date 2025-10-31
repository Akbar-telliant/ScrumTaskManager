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
    public int Id { get; set; }

    /// <summary>
    /// Name of the user used for login or identification.
    /// </summary>
    [Required(ErrorMessage = "User name is required")]
    [StringLength(250, ErrorMessage = "User name cannot exceed 250 characters")]
    public string? UserName { get; set; }

    /// <summary>
    /// Password associated with the user account.
    /// </summary>
    [StringLength(250, ErrorMessage = "Password cannot exceed 250 characters")]
    public string? Password { get; set; }

    /// <summary>
    /// Registered email address of the user.
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(250)]
    public string? Email { get; set; }

    /// <summary>
    /// Assigned role or permission level of the user.
    /// </summary>
    [Required(ErrorMessage = "Role is required")]
    public TUserRole Role { get; set; } = TUserRole.User;

    /// <summary>
    /// Date when the user was created.
    /// </summary>
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDetails"/> class.
    /// </summary>
    public UserDetails()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDetails"/> class with specified values.
    /// </summary>
    /// <param name="name">The user's name.</param>
    /// <param name="email">The user's email address.</param>
    /// <param name="role">The user's role.</param>
    public UserDetails(string name, string email, TUserRole role)
    {
        UserName = name;
        Email = email;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }
}