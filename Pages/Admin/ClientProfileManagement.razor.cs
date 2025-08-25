using Microsoft.AspNetCore.Components;
using MudBlazor;
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
    /// Tracks expanded clients to show/hide project subgrid.
    /// </summary>
    private HashSet<int> ExpandedClients = new();

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
    /// Toggle display of project subgrid for a given client.
    /// </summary>
    /// <param name="clientId">ID of the client to toggle.</param>
    private void ToggleProjects(int clientId)
    {
        if (ExpandedClients.Contains(clientId))
            ExpandedClients.Remove(clientId);
        else
            ExpandedClients.Add(clientId);
    }

    /// <summary>
    /// Open dialog to add new client.
    /// </summary>
    private async Task AddClientAsync()
    {
        var parameters = new DialogParameters
        {
            { "ClientProfile", new ClientProfile() },
            { "DialogTitle", "Add Client Profile" }
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
        var editableCopy = new ClientProfile
        {
            Id = client.Id,
            Name = client.Name,
            Code = client.Code,
            IsActive = client.IsActive,
            Notes = client.Notes,
            Projects = client.Projects?.ToList() ?? new List<ProjectDetail>()
        };

        var parameters = new DialogParameters
        {
            { "ClientProfile", editableCopy },
            { "DialogTitle", "Edit Client Profile" }
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

    /// <summary>
    /// Open dialog to add new project for a client.
    /// </summary>
    private async Task AddProjectAsync(ClientProfile client)
    {
        var newProject = new ProjectDetail
        {
            ClientId = client.Id,
            StartDate = DateTime.Now
        };

        var parameters = new DialogParameters
        {
            { "ProjectDetail", newProject },
            { "DialogTitle", $"Add Project for {client.Name}" }
        };

        var dialog = await DialogService.ShowAsync<ProjectDetailDialog>("Add Project", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadClientsAsync();
            Snackbar.Add("Project added successfully!", Severity.Success);
        }
    }
}
