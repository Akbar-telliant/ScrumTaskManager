using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Pages.Admin;

/// <summary>
/// Component for registering a new user.
/// </summary>
public partial class RegisterUser
{
    /// <summary>
    /// Service for performing CRUD operations on user entities.
    /// </summary>
    [Inject]
    private EntityDataService<UserDetails> m_UserService { get; set; } = default!;

    /// <summary>
    /// Model containing user registration details.
    /// </summary>
    public UserDetails UserDetails { get; set; } = new();

    /// <summary>
    /// Handles form submission and registers the user if validation passes.
    /// </summary>
    /// <returns> Task representing the asynchronous operation. </returns>
    public async Task HandleValidSubmitAsync()
    {
        if (string.IsNullOrWhiteSpace(UserDetails?.Email))
        {
            SnackbarService.Add("Please enter an email address.", Severity.Warning);
            return;
        }

        // Single efficient DB query — does not fetch all rows
        bool emailExists = await m_UserService.ExistsAsync(u => u.Email == UserDetails.Email);

        if (emailExists)
        {
            SnackbarService.Add("This email is already registered!", Severity.Warning);
            return;
        }

        try
        {
            var user = await m_UserService.AddAsync(UserDetails);
            if (user.Id > 0)
            {
                SnackbarService.Add("User registered successfully!", Severity.Success);
                Navigation.NavigateTo("/");
            }
            else
            {
                SnackbarService.Add("Something went wrong while creating the user.", Severity.Error);
            }
        }
        catch (DbUpdateException dbEx)
        {
            // In case of race condition or DB unique constraint violation
            // (unique index on Email), catch and display a friendly message.
            SnackbarService.Add("Email already exists or database error occurred.", Severity.Error);
            // Optionally log dbEx
        }
        catch (Exception ex)
        {
            SnackbarService.Add("An unexpected error occurred.", Severity.Error);
            // Optionally log ex
        }
    }

    /// <summary>
    /// Checks whether the entered email address already exists when the field loses focus.
    /// </summary>
    /// <param name="args">Focus event arguments triggered when the email input loses focus.</param>
    /// <returns> A task representing the asynchronous email existence check operation. </returns>
    private async Task CheckEmailExistsAsync(FocusEventArgs args)
    {
        if (string.IsNullOrWhiteSpace(UserDetails?.Email))
            return;

        bool exists = await m_UserService.ExistsAsync(u => u.Email == UserDetails.Email);
        if (exists)
            SnackbarService.Add("This email is already registered!", Severity.Warning);
    }
}

