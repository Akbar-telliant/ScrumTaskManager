using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ScrumMaster;
using ScrumMaster.Data;
using ScrumMaster.Security;
using ScrumMaster.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Configure Services
// ----------------------

// EF Core with SQLite (single registration)
builder.Services.AddDbContext<ScrumMasterDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=scrummaster.db"));

builder.Services.AddAuthenticationCore();
builder.Services.AddAuthorizationCore();

// Entity data service
builder.Services.AddScoped(typeof(EntityDataService<>));
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(); 
builder.Services.AddScoped<ProtectedSessionStorage>();

// MudBlazor
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 250;
    config.SnackbarConfiguration.ShowTransitionDuration = 250;
});

// Blazor (server-side interactive components)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

var app = builder.Build();

// ----------------------
// Database Init
// ----------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScrumMasterDbContext>();
    //db.Database.Migrate(); // ✅ applies migrations (creates/updates schema)
}

// ----------------------
// Configure Middleware
// ----------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Map root Razor component
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
