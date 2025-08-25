using Microsoft.AspNetCore.Components;
using ScrumMaster.Models;
using static ScrumMaster.Models.Users;

namespace ScrumMaster.Dialog
{
    public partial class UserManagementDialog
    {
        [CascadingParameter]
        private IMudDialogInstance? MudDialog { get; set; }

        [Parameter]
        public Users? Users { get; set; } 

        public async Task OnValidSubmitAsync()
        {
            await Task.Yield();
        }

    }
}
