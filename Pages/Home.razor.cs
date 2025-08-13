using Microsoft.AspNetCore.Components;

namespace ScrumMaster.Pages;

/// <summary>
/// Defines logic for the home page.
/// </summary>
public partial class Home : ComponentBase
{

    public string Text { get; set; } = string.Empty;


    public async Task OnClickAsync()
    {       
        Text = "Hello, world!";
        StateHasChanged(); // Notify UI to update
    }
}
