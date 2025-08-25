using Microsoft.AspNetCore.Components;
using ScrumMaster.Dialog;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Pages.Admin;

/// <summary>
/// Page for managing client profiles and their associated projects.
/// </summary>
public partial class ClientProfileManagement : ComponentBase
{
    /// <summary>
    /// Holds the list of all clients loaded from the service.
    /// </summary>
    private List<ClientProfile> clients = new();

    /// <summary>
    /// Tracks which client IDs have their projects expanded in the UI.
    /// </summary>
    private HashSet<int> ExpandedClients = new();

    /// <summary>
    /// Service for performing CRUD operations on client profiles.
    /// </summary>
    [Inject]
    private EntityDataService<ClientProfile> ClientService { get; set; } = default!;

    /// <summary>
    /// Service for performing CRUD operations on project details.
    /// </summary>
    [Inject]
    private EntityDataService<ProjectDetail> ProjectService { get; set; } = default!;

    /// <summary>
    /// Provides dialog services for opening modals.
    /// </summary>
    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Provides snackbar notifications for user feedback.
    /// </summary>
    [Inject]
    private ISnackbar Snackbar { get; set; } = default!;

    /// <summary>
    /// Initializes the component by loading clients.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync() => await LoadClientsAsync();

    /// <summary>
    /// Fetches all clients and their associated projects from the services.
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

    /// <summary>
    /// Toggles the expanded state of a client's projects in the UI.
    /// </summary>
    /// <param name="clientId">The ID of the client whose projects are being toggled.</param>
    private void ToggleProjects(int clientId)
    {
        if (!ExpandedClients.Add(clientId))
            ExpandedClients.Remove(clientId);
    }

    /// <summary>
    /// Opens a dialog to add a new client profile.
    /// </summary>
    /// <returns>A task representing the asynchronous add operation.</returns>
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
    /// Opens a dialog to edit an existing client profile.
    /// </summary>
    /// <param name="client">The client profile to edit.</param>
    /// <returns>A task representing the asynchronous edit operation.</returns>
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
    /// Deletes the given client profile after user confirmation.
    /// </summary>
    /// <param name="client">The client profile to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
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
    /// Opens a dialog to add a new project for the given client.
    /// </summary>
    /// <param name="client">The client for whom the project will be added.</param>
    /// <returns>A task representing the asynchronous add operation.</returns>
    private async Task AddProjectAsync(ClientProfile client)
    {
        var newProject = new ProjectDetail
        {
            ClientId = client.Id,
            StartDate = DateTime.Now,
            DefaultTeamSize = 5 // Default TeamSize
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

    /// <summary>
    /// Opens a dialog to edit an existing project.
    /// </summary>
    /// <param name="project">The project to edit.</param>
    /// <returns>A task representing the asynchronous edit operation.</returns>
    private async Task EditProjectAsync(ProjectDetail project)
    {
        var editableCopy = new ProjectDetail
        {
            Id = project.Id,
            ClientId = project.ClientId,
            Name = project.Name,
            Description = project.Description,
            DefaultTeamSize = project.DefaultTeamSize,
            StartDate = project.StartDate,
            EndDate = project.EndDate
        };

        var parameters = new DialogParameters
        {
            { "ProjectDetail", editableCopy },
            { "DialogTitle", $"Edit Project - {project.Name}" }
        };

        var dialog = await DialogService.ShowAsync<ProjectDetailDialog>("Edit Project", parameters);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadClientsAsync();
            Snackbar.Add("Project updated successfully!", Severity.Info);
        }
    }

    /// <summary>
    /// Deletes the given project after user confirmation.
    /// </summary>
    /// <param name="project">The project to delete.</param>
    /// <returns>A task representing the asynchronous delete operation.</returns>
    private async Task DeleteProjectAsync(ProjectDetail project)
    {
        bool? confirmed = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete project '{project.Name}'?",
            yesText: "Delete",
            cancelText: "Cancel");

        if (confirmed == true)
        {
            await ProjectService.DeleteAsync(project.Id);
            await LoadClientsAsync();
            Snackbar.Add("Project deleted successfully!", Severity.Error);
        }
    }
}
