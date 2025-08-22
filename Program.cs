using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using ScrumMaster;
using ScrumMaster.Data;
using ScrumMaster.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Configure Services
// ----------------------

// EF Core with SQLite (single registration)
builder.Services.AddDbContext<ScrumMasterDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=scrummaster.db"));

// Entity data service
builder.Services.AddScoped(typeof(EntityDataService<>));

// MudBlazor
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.PreventDuplicates = true;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 3000;
    config.SnackbarConfiguration.HideTransitionDuration = 250;
    config.SnackbarConfiguration.ShowTransitionDuration = 250;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

// Blazor (server-side interactive components)
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

var app = builder.Build();

// ----------------------
// Database Init
// ----------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScrumMasterDbContext>();
    db.Database.EnsureCreated(); // ✅ create SQLite db + tables if not exist
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
