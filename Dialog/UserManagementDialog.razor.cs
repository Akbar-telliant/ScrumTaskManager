using Microsoft.AspNetCore.Components;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Dialog;

/// <summary>
/// Dialog component for adding or editing a user.
/// </summary>
public partial class UserManagementDialog
{
    /// <summary>
    /// Reference to the current MudBlazor dialog instance.
    /// </summary>
    [CascadingParameter]
    private IMudDialogInstance? MudDialog { get; set; }

    /// <summary>
    /// The user entity being created or updated.
    /// </summary>
    [Parameter]
    public UserDetails User { get; set; } = new();

    /// <summary>
    /// The title text displayed in the dialog header.
    /// </summary>
    [Parameter]
    public string DialogTitle { get; set; } = "Add User";

    /// <summary>
    /// Service for performing CRUD operations on user entities.
    /// </summary>
    [Inject]
    private EntityDataService<UserDetails> UserService { get; set; } = default!;

    /// <summary>
    /// Handles form submission and saves the user.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task OnValidSubmitAsync()
    {
        if (User.Id == 0)
        {
            // New → Add
            await UserService.AddAsync(User);
        }
        else
        {
            // Existing → Update
            await UserService.UpdateAsync(User);
        }

        MudDialog?.Close(DialogResult.Ok(User));
    }

    /// <summary>
    /// Closes the dialog without saving changes.
    /// </summary>
    private void Cancel() => MudDialog?.Cancel();
}
