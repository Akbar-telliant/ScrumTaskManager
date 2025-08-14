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

}
