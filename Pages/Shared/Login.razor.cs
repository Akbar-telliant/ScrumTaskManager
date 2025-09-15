using Microsoft.AspNetCore.Components;

namespace ScrumMaster.Pages.Shared
{
    public partial class Login
    {
        [Inject] 
        public NavigationManager Navigation { get; set; } = default!;
        
        [Inject] 
        
        public ISnackbar Snackbar { get; set; } = default!;
        
        private string? UserName;
        
        private string? Password;
        
        private bool RememberMe;

        private async Task LoginAsync()
        {
            // TODO: Replace with real authentication logic
            if (UserName == "admin" && Password == "password")
            {
                // Example: redirect to home
                Navigation.NavigateTo("/home");
            }
            else
            {
                Snackbar.Add("Invalid username or password!", Severity.Error);
            }
            await Task.CompletedTask;
        }
    }
}
