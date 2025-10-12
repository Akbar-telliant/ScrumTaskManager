using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using ScrumMaster.Models;
using System.Security.Claims;

namespace ScrumMaster.Security;


public partial class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Try to get the user details from session storage
            var storedUser = await _sessionStorage.GetAsync<UserDetails>("UserSession");

            // If user details are found, create a ClaimsPrincipal, otherwise use an empty one
            var user = storedUser.Success && storedUser.Value != null
                ? CreateClaimsPrincipal(storedUser.Value)
                : claimsPrincipal;

            // Return the authentication state
            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(claimsPrincipal);
        }
    }

    public async Task SetUserAsync(UserDetails user)
    {
        // Store user details in session storage
        await _sessionStorage.SetAsync("UserSession", user);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(CreateClaimsPrincipal(user))));
    }

    public async Task LogoutAsync()
    {
        // Clear the session storage
        await _sessionStorage.DeleteAsync("UserSession");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    private ClaimsPrincipal CreateClaimsPrincipal(UserDetails user)
    {

        // Create claims based on user information
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        // Create a ClaimsIdentity with the claims and authentication type
        var identity = new ClaimsIdentity(claims, "CustomAuth");
        return new ClaimsPrincipal(identity);
    }
}

