using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ScrumMaster.Dialog;
using ScrumMaster.Models;

namespace ScrumMaster.Pages.Admin;

/// <summary>
/// Component for client profile operations.
/// </summary>
public partial class ClientProfileManagement : ComponentBase
{
    /// <summary>
    /// Stores client profile data.
    /// </summary>
    private List<ClientProfile> clients = new();

    /// <summary>
    /// Lifecycle method: called when the component is initialized.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        await LoadClientsAsync();
    }

    /// <summary>
    /// Load client profiles from database.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task LoadClientsAsync()
    {
        clients = await DbContext.ClientProfile.ToListAsync();
    }

    /// <summary>
    /// Opens a dialog to add a new client profile asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AddClientAsync()
    {
        var parameters = new DialogParameters<ClientProfileManagementDialog>
    {
        { x => x.ClientProfile, new ClientProfile() },
        { x => x.DialogTitle, "Add Client Profile" }
    };

        var dialog = await DialogService.ShowAsync<ClientProfileManagementDialog>("Add Client", parameters);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            // Refresh grid after saving
            await LoadClientsAsync();
            StateHasChanged();

            // Show success snackbar
            Snackbar.Add("Client profile added successfully!", Severity.Success);
        }
    }

    /// <summary>
    /// Opens a dialog to edit the specified client profile.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task EditClientAsync(ClientProfile client)
    {
        var parameters = new DialogParameters<ClientProfileManagementDialog>
    {
        { x => x.ClientProfile, client },
        { x => x.DialogTitle, "Edit Client Profile" }
    };

        var dialog = await DialogService.ShowAsync<ClientProfileManagementDialog>("Edit Client", parameters);
        var result = await dialog.Result;

        if (result != null && !result.Canceled)
        {
            await LoadClientsAsync();
            StateHasChanged();
            Snackbar.Add("Client profile updated successfully!", Severity.Info);
        }
    }

    /// <summary>
    /// Confirms and deletes the specified client profile.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task DeleteClientAsync(ClientProfile client)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete client '{client.Name}'?",
            yesText: "Delete",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            var existing = await DbContext.ClientProfile.FindAsync(client.Id);
            if (existing is not null)
            {
                DbContext.ClientProfile.Remove(existing);
                await DbContext.SaveChangesAsync();
            }

            await LoadClientsAsync();
            StateHasChanged();
            Snackbar.Add("Client profile deleted successfully!", Severity.Error);
        }
    }
}
