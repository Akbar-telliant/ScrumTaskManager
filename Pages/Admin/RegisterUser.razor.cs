using Microsoft.AspNetCore.Components;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Pages.Admin;

public partial class RegisterUser
{
    /// <summary>
    /// Service for performing CRUD operations on user entities.
    /// </summary>
    [Inject]
    private EntityDataService<UserDetails> m_UserService { get; set; } = default!;

    public UserDetails UserDetails { get; set; } = new();

    public async Task HandleValidSubmitAsync()
    {
        var user = await m_UserService.AddAsync(UserDetails);
        if (user.Id > 0)
        {
            SnackbarService.Add("User registered successfully!", Severity.Success);
            Navigation.NavigateTo("/");
        }
        else
        {
            SnackbarService.Add("Something Went Wrong!", Severity.Error);
        }
    }

}

