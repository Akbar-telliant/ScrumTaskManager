using Microsoft.AspNetCore.Components;
using MudBlazor;
using ScrumMaster.Data;
using ScrumMaster.Models;

namespace ScrumMaster.Dialog;

/// <summary>
/// 
/// </summary>
public partial class ClientProfileManagementDialog
{
    /// <summary>
    /// 
    /// </summary>
    [CascadingParameter]
    IMudDialogInstance? MudDialog { get; set; }

    /// <summary>
    /// Client profile data model.
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
    /// <returns>A Task representing the async operation.</returns>
    public async Task OnValidSubmitAsync()
    {
        await SaveClientAsync(ClientProfile);
        MudDialog?.Close(DialogResult.Ok(ClientProfile));
    }

    /// <summary>
    /// Saves the client to the database.
    /// </summary>
    /// /// <returns>A Task representing the async operation.</returns>
    private async Task SaveClientAsync(ClientProfile client)
    {
        DbContext.ClientProfile.Add(client);
        await DbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Closes the dialog without saving.
    /// </summary>
    private void Cancel() => MudDialog?.Cancel();
}
