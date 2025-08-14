using Microsoft.Extensions.Options;
using ScrumMaster.Dialog;
using ScrumMaster.Models;

namespace ScrumMaster.Pages.Admin
{
    public partial class UserManagement
    {

        public List<Users> users { get; set; } = new List<Users>();       

        protected override async Task OnInitializedAsync()
        {
            users.Add( new Users("Alice", "alice@xyz", Users.TUserRole.Admin));
            users.Add(new Users("Kumar", "kumar@xyz", Users.TUserRole.User));
            await Task.Yield();
        }

        public async Task AddUser()
        {
            var parameters = new DialogParameters<UserManagementDialog>
            {
                { x => x.Users, new Users() }
            };

            await DialogService.ShowAsync<UserManagementDialog>(string.Empty, parameters);            
        }

        public async Task EditUser(Users value)
        {
            var parameters = new DialogParameters<UserManagementDialog>
            {
                { x => x.Users, value }
            };

            await DialogService.ShowAsync<UserManagementDialog>(string.Empty, parameters);
        }

        public async Task DeleteUser(Users value)
        {
            users.Remove(value);
        }

    }
}
