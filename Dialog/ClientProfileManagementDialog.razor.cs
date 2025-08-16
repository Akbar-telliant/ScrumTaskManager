using Microsoft.AspNetCore.Components;
using MudBlazor;
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
    /// 
    /// </summary>
    [Parameter]
    public ClientProfile Client { get; set; } = new ClientProfile();

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string DialogTitle { get; set; } = "Add Client";

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task OnValidSubmitAsync()
    {
        await Task.Yield();
    }

    /// <summary>
    /// Closes the dialog without saving.
    /// </summary>
    private void Cancel()
    {
        MudDialog?.Cancel();
    }
}
