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
    private List<ClientProfile> clients = new();
    private HashSet<int> ExpandedClients = new();

    [Inject] private EntityDataService<ClientProfile> ClientService { get; set; } = default!;
    [Inject] private EntityDataService<ProjectDetail> ProjectService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    protected override async Task OnInitializedAsync() => await LoadClientsAsync();

    /// <summary>
    /// Fetch clients and their projects from service.
    /// </summary>
    private async Task LoadClientsAsync()
    {
        clients = await ClientService.GetAllAsync();

        foreach (var client in clients)
        {
            var projects = await ProjectService.GetByConditionAsync(p => p.ClientId == client.Id);
            client.Projects = projects ?? new List<ProjectDetail>();
        }
    }

    private void ToggleProjects(int clientId)
    {
        if (!ExpandedClients.Add(clientId))
            ExpandedClients.Remove(clientId);
    }

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
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <returns></returns>
    private async Task AddProjectAsync(ClientProfile client)
    {
        var newProject = new ProjectDetail
        {
            ClientId = client.Id,
            StartDate = DateTime.Now,
            DefaultTeamSize = 5 // ✅ default TeamSize (adjust as needed)
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