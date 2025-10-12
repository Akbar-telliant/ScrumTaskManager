namespace ScrumMaster.Pages.Shared;

/// <summary>
/// Represents the navigation menu component.
/// </summary>
public partial class NavMenu
{
   private bool IsAdmin;

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        IsAdmin = user.IsInRole("Admin");
    }

}
