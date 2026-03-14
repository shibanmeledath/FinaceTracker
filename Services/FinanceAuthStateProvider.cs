using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace FinanceTracker.Services;

public class FinanceAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _localStorage;
    private const string AuthStorageKey = "auth_token";

    // Create an anonymous user state
    private ClaimsPrincipal Anonymous => new(new ClaimsIdentity());

    public FinanceAuthStateProvider(ProtectedLocalStorage localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionResult = await _localStorage.GetAsync<bool>(AuthStorageKey);
            
            if (userSessionResult.Success && userSessionResult.Value)
            {
                var claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "User")
                }, "FinanceAuth");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return new AuthenticationState(claimsPrincipal);
            }
        }
        catch (InvalidOperationException)
        {
            // Usually occurs during prerendering before interop is available
        }
        catch (Exception)
        {
            // Fallback for random storage errors
        }

        return new AuthenticationState(Anonymous);
    }

    public async Task LoginAsync()
    {
        await _localStorage.SetAsync(AuthStorageKey, true);
        
        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "User")
        }, "FinanceAuth");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task LogoutAsync()
    {
        await _localStorage.DeleteAsync(AuthStorageKey);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(Anonymous)));
    }
}
