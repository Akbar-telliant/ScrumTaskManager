using Microsoft.AspNetCore.Components;
using MudBlazor;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Dialog;

public partial class UserManagementDialog
{
    [CascadingParameter]
    private IMudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public UserDetails User { get; set; } = new();

    [Parameter]
    public string DialogTitle { get; set; } = "Add User";

    [Inject]
    private EntityDataService<UserDetails> UserService { get; set; } = default!;

    private async Task OnValidSubmitAsync()
    {
        if (User.UserId == 0)
        {
            // New → Add
            await UserService.AddAsync(User);
        }
        else
        {
            // Existing → Update
            await UserService.UpdateAsync(User);
        }

        MudDialog?.Close(DialogResult.Ok(User));
    }

    private void Cancel() => MudDialog?.Cancel();
}
