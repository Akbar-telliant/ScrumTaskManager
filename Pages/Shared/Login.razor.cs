using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ScrumMaster.Models;
using ScrumMaster.Security;
using ScrumMaster.Services;

namespace ScrumMaster.Pages.Shared
{
    public partial class Login
    {
        /// <summary>
        /// Service for performing CRUD operations on users.
        /// </summary>
        [Inject]
        private EntityDataService<UserDetails> m_UserService { get; set; } = default!;

        /// <summary>
        /// UserDetails model bound to the login form.
        /// </summary>
        private UserDetails UserDetails { get; set; } = new();

        /// <summary>
        /// Login Method to authenticate user.
        /// </summary>
        /// <returns></returns>
        private async Task LoginAsync()
        {
            if (UserDetails != null)
            {
                //Get All Users
                var users = await m_UserService.GetAllAsync();

                //Check is User Available
                var isUser = users.Where(x => x.UserName == UserDetails.UserName && x.Password == UserDetails.Password).FirstOrDefault();
                
                // TODO: Replace with real authentication logic
                if (isUser != null)
                {

                    //Set User as Authenticated
                    var customAuth = (CustomAuthStateProvider)AuthStateProvider;
                    await customAuth.SetUserAsync(isUser);

                    // Example: redirect to home
                    Navigation.NavigateTo("/home");
                }
                else
                {
                    SnackbarService.Add("Invalid username or password!", Severity.Error);
                }
                await Task.CompletedTask;
            }
        }
    }
}
