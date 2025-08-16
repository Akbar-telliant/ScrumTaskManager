using Microsoft.AspNetCore.Components;
using MudBlazor;
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
    /// Initializes the client list with sample data.
    /// </summary>
    protected override void OnInitialized()
    {
        // TODO: Replace with real service call
        clients = new List<ClientProfile>
        {
            new ClientProfile { Id = 1, Name = "Milnet Technologies", Code = "MTI", IsActive = true, DefaultTeamSize = 15, Notes = "Main client" },
            new ClientProfile { Id = 2, Name = "Telliant Systems", Code = "TS", IsActive = false, DefaultTeamSize = 8, Notes = "Inactive account" }
        };
    }

    /// <summary>
    /// Opens a dialog to add a new client profile.
    /// </summary>
    private async void AddClient()
    {
        var parameters = new DialogParameters<ClientProfileManagementDialog>
            {
                { x => x.ClientProfile, new ClientProfile() }
            };

        await DialogService.ShowAsync<ClientProfileManagementDialog>(string.Empty, parameters);
    }

    /// <summary>
    /// Opens a dialog to edit the specified client profile.
    /// </summary>
    /// <param name="client">The client profile to edit.</param>
    private void EditClient(ClientProfile client)
    {
        // TODO: Open dialog to edit ClientProfile
    }

    /// <summary>
    /// Performs an action on the specified client profile.
    /// </summary>
    /// <param name="client">The target client profile.</param>
    private void DeleteClient(ClientProfile client)
    {
        // TODO: Show confirmation and delete logic
    }
}
