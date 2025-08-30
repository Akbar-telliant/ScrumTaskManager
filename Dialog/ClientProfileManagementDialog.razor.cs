using Microsoft.AspNetCore.Components;
using ScrumMaster.Models;
using ScrumMaster.Services;

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
    private IMudDialogInstance? m_MudDialog { get; set; }

    /// <summary>
    /// Client profile model (passed in from parent).
    /// </summary>
    [Parameter]
    public ClientProfile ClientProfile { get; set; } = new();

    /// <summary>
    /// Dialog header text.
    /// </summary>
    [Parameter]
    public string DialogTitle { get; set; } = "Add Client Profile";

    /// <summary>
    /// Entity data service for ClientProfile CRUD.
    /// </summary>
    [Inject]
    private EntityDataService<ClientProfile> m_ClientService { get; set; } = default!;

    /// <summary>
    /// Handles form submission.
    /// </summary>
    private async Task OnValidSubmitAsync()
    {
        if (ClientProfile.Id == 0)
        {
            // New entity → Add
            await m_ClientService.AddAsync(ClientProfile);
        }
        else
        {
            // Existing entity → Update
            await m_ClientService.UpdateAsync(ClientProfile);
        }

        m_MudDialog?.Close(DialogResult.Ok(ClientProfile));
    }

    /// <summary>
    /// Closes dialog without saving.
    /// </summary>
    private void Cancel() => m_MudDialog?.Cancel();
}
