using Microsoft.AspNetCore.Components;
using ScrumMaster.Dialog;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Pages.Admin;

/// <summary>
/// Page for managing application users.
/// </summary>
public partial class UserManagement : ComponentBase
{
    /// <summary>
    /// List of users currently loaded.
    /// </summary>
    private List<UserDetails> users = new();

    /// <summary>
    /// Service for performing CRUD operations on users.
    /// </summary>
    [Inject] 
    private EntityDataService<UserDetails> UserService { get; set; } = default!;

    /// <summary>
    /// Dialog service for showing modals.
    /// </summary>
    [Inject] 
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Snackbar service for showing notifications.
    /// </summary>
    [Inject] 
    private ISnackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// Initializes the component by loading users.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync() => await LoadUsersAsync();

    /// <summary>
    /// Fetches all users from the service.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task LoadUsersAsync()
    {
        users = await UserService.GetAllAsync();
    }

    /// <summary>
    /// Opens a dialog to add a new user.
    /// </summary>
    /// <returns>A task representing the asynchronous add operation.</returns>
    private async Task AddUserAsync()
    {
        var parameters = new DialogParameters
        {
            { "UserDetail", new UserDetails() },
            { "DialogTitle", "Add User" }
        };

        var dialog = await DialogService.ShowAsync<UserManagementDialog>("Add User", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadUsersAsync();
            Snackbar.Add("User added successfully!", Severity.Success);
        }
    }

    /// <summary>
    /// Opens a dialog to edit an existing user.
    /// </summary>
    /// <param name="user">The user to edit.</param>
    /// <returns>A task representing the asynchronous edit operation.</returns>
    private async Task EditUserAsync(UserDetails user)
    {
        var editableCopy = new UserDetails
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt
        };

        var parameters = new DialogParameters
        {
            { "UserDetail", editableCopy },
            { "DialogTitle", "Edit User" }
        };

        var dialog = await DialogService.ShowAsync<UserManagementDialog>("Edit User", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadUsersAsync();
            Snackbar.Add("User updated successfully!", Severity.Info);
        }
    }

    /// <summary>
    /// Deletes the given user after confirmation.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    private async Task DeleteUserAsync(UserDetails user)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete user '{user.UserName}'?",
            yesText: "Delete",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await UserService.DeleteAsync(user.UserId);
            await LoadUsersAsync();
            Snackbar.Add("User deleted successfully!", Severity.Error);
        }
    }
}
