namespace ScrumMaster.Pages;

/// <summary>
/// Defines logic for the home page.
/// </summary>
public partial class Home
{

    public string Text { get; set; } = string.Empty;


    public async Task OnClickAsync()
    {       
        Text = "Hello, world!";
    }
}
