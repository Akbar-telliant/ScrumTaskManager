using Microsoft.AspNetCore.Components;
using ScrumMaster.Dialog;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Pages.Admin;

/// <summary>
/// Page for managing client profiles.
/// </summary>
public partial class ClientProfileManagement : ComponentBase
{
    /// <summary>
    /// Local list of client profiles.
    /// </summary>
    private List<ClientProfile> clients = new();

    /// <summary>
    /// Service for performing CRUD operations on <see cref="ClientProfile"/> entities.
    /// </summary>
    [Inject]
    private EntityDataService<ClientProfile> ClientService { get; set; } = default!;

    /// <summary>
    /// Provides dialog functionality for displaying modal dialogs.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Service for displaying toast-style notification messages.
    /// </summary>
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// On page load → fetch all clients.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadClientsAsync();
    }

    /// <summary>
    /// Fetch clients from service.
    /// </summary>
    private async Task LoadClientsAsync()
    {
        clients = await ClientService.GetAllAsync();
    }

    /// <summary>
    /// Open dialog to add new client.
    /// </summary>
    private async Task AddClientAsync()
    {
        var parameters = new DialogParameters<ClientProfileManagementDialog>
        {
            { x => x.ClientProfile, new ClientProfile() },
            { x => x.DialogTitle, "Add Client Profile" }
        };

        var dialog = await DialogService.ShowAsync<ClientProfileManagementDialog>("Add Client", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadClientsAsync();
            Snackbar.Add("Client profile added successfully!", Severity.Success);
        }
    }

    /// <summary>
    /// Open dialog to edit an existing client.
    /// </summary>
    private async Task EditClientAsync(ClientProfile client)
    {
        // Create a copy to avoid editing list item before save
        var editableCopy = new ClientProfile
        {
            Id = client.Id,
            Name = client.Name,
            Code = client.Code,
            IsActive = client.IsActive,
            Notes = client.Notes
        };

        var parameters = new DialogParameters<ClientProfileManagementDialog>
        {
            { x => x.ClientProfile, editableCopy },
            { x => x.DialogTitle, "Edit Client Profile" }
        };

        var dialog = await DialogService.ShowAsync<ClientProfileManagementDialog>("Edit Client", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadClientsAsync();
            Snackbar.Add("Client profile updated successfully!", Severity.Info);
        }
    }

    /// <summary>
    /// Confirm and delete a client.
    /// </summary>
    private async Task DeleteClientAsync(ClientProfile client)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete client '{client.Name}'?",
            yesText: "Delete",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ClientService.DeleteAsync(client.Id);
            await LoadClientsAsync();
            Snackbar.Add("Client profile deleted successfully!", Severity.Error);
        }
    }
}
