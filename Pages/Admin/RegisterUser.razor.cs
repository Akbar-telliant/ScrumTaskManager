using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using ScrumMaster.Models;
using ScrumMaster.Services;
using static ScrumMaster.Models.UserDetails;

namespace ScrumMaster.Pages.Admin;

/// <summary>
/// Component for registering a new user.
/// </summary>
public partial class RegisterUser
{
    /// <summary>
    /// Service used for performing CRUD operations on user entities.
    /// </summary>
    [Inject]
    private EntityDataService<UserDetails> m_UserService { get; set; } = default!;

    /// <summary>
    /// Service for handling page navigation within the application.
    /// </summary>
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    /// <summary>
    /// Gets or sets the user details.
    /// </summary>
    public UserDetails UserDetails { get; set; } = new();

    /// <summary>
    /// Stores the last verified email to prevent redundant database checks.
    /// </summary>
    private string? m_LastCheckedEmail;

    /// <summary>
    /// Registers a new user if validation passes.
    /// </summary>
    public async Task RegisterUserAsync()
    {
        if (string.IsNullOrWhiteSpace(UserDetails?.Email))
        {
            SnackbarService.Add("Please enter an email address.", Severity.Warning);
            return;
        }

        try
        {
            if (await m_UserService.ExistsAsync(u => u.Email == UserDetails.Email).ConfigureAwait(false))
            {
                SnackbarService.Add("This email is already registered!", Severity.Warning);
                return;
            }

            UserDetails.Role = UserDetails.Role == 0 ? TUserRole.User : UserDetails.Role;

            var user = await m_UserService.AddAsync(UserDetails).ConfigureAwait(false);

            if (user is { Id: > 0 })
            {
                SnackbarService.Add("User registered successfully!", Severity.Success);
                Navigation.NavigateTo("/");
            }
            else
            {
                SnackbarService.Add("Something went wrong while creating the user.", Severity.Error);
            }
        }
        catch (DbUpdateException ex)
        {
            Console.Error.WriteLine($"[DB ERROR] {ex.GetType().Name}: {ex.Message}");
            SnackbarService.Add("Email already exists or database error occurred.", Severity.Error);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR] {ex.GetType().Name}: {ex.Message}");
            SnackbarService.Add("An unexpected error occurred.", Severity.Error);
        }
    }

    /// <summary>
    /// Validates whether the entered email already exists when the field loses focus.
    /// </summary>
    private async Task CheckEmailExistsAsync(FocusEventArgs args)
    {
        var email = UserDetails?.Email?.Trim();
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        if (string.Equals(email, m_LastCheckedEmail, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        m_LastCheckedEmail = email;

        try
        {
            if (await m_UserService.ExistsAsync(u => u.Email == email).ConfigureAwait(false))
            {
                SnackbarService.Add("This email is already registered!", Severity.Warning);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Email Check Error] {ex.GetType().Name}: {ex.Message}");
            SnackbarService.Add("Unable to verify email at this moment.", Severity.Error);
        }
    }
}
