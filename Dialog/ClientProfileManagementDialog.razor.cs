using Microsoft.AspNetCore.Components;
using MudBlazor;
using ScrumMaster.Data;
using ScrumMaster.Models;

namespace ScrumMaster.Dialog;

/// <summary>
/// Dialog for adding or editing client profiles.
/// </summary>
public partial class ClientProfileManagementDialog
{
    /// <summary>
    /// Dialog instance reference.
    /// </summary>
    [CascadingParameter]
    IMudDialogInstance? MudDialog { get; set; }

    /// <summary>
    /// Client profile data model (passed in from parent).
    /// </summary>
    [Parameter]
    public ClientProfile ClientProfile { get; set; } = new ClientProfile();

    /// <summary>
    /// Dialog header text.
    /// </summary>
    [Parameter]
    public string DialogTitle { get; set; } = "Add Client Profile";

    /// <summary>
    /// Injected DbContext for data access.
    /// </summary>
    [Inject]
    private ScrumMasterDbContext DbContext { get; set; } = default!;

    /// <summary>
    /// Handles form submission when the form is valid.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OnValidSubmitAsync()
    {
        await SaveClientAsync(ClientProfile);
        MudDialog?.Close(DialogResult.Ok(ClientProfile));
    }

    /// <summary>
    /// Saves or updates the client profile in the database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task SaveClientAsync(ClientProfile client)
    {
        var existing = await DbContext.ClientProfile.FindAsync(client.Id);

        if (existing is null)
        {
            // New entity → Add
            DbContext.ClientProfile.Add(client);
        }
        else
        {
            // Existing entity → Update values without attaching duplicate
            DbContext.Entry(existing).CurrentValues.SetValues(client);
        }

        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Closes the dialog without saving.
    /// </summary>
    private void Cancel() => MudDialog?.Cancel();
}
