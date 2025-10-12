using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ScrumMaster.Security;
using System.Threading.Tasks;

namespace ScrumMaster.Pages.Shared;

/// <summary>
/// Main layout component for the ScrumMaster app.
/// </summary>
public partial class MainLayout
{

    public bool m_DrawerOpen = false;

    void DrawerToggle()
    {
        m_DrawerOpen = !m_DrawerOpen;
    }

    public async Task LogOut()
    {
        await ((CustomAuthStateProvider)AuthStateProvider).LogoutAsync();
        Navigation.NavigateTo("/");
    }

}
