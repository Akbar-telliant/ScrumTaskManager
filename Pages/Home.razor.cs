using Microsoft.AspNetCore.Components;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Pages;

/// <summary>
/// Home page for managing Scrum tasks with CRUD operations using EntityDataService.
/// </summary>
public partial class Home : ComponentBase
{
    /// <summary>
    /// Service for CRUD operations on ScrumDetails.
    /// </summary>
    [Inject]
    private EntityDataService<ScrumDetails> ScrumService { get; set; } = default!;

    /// <summary>
    /// Service for CRUD operations on UserDetails.
    /// </summary>
    [Inject]
    private EntityDataService<UserDetails> UserService { get; set; } = default!;

    /// <summary>
    /// Service for CRUD operations on ProjectDetails.
    /// </summary>
    [Inject]
    private EntityDataService<ProjectDetails> ProjectService { get; set; } = default!;

    /// <summary>
    /// List of users for UI dropdowns.
    /// </summary>
    private List<UserDetails> Users { get; set; } = new();

    /// <summary>
    /// List of projects for UI dropdowns.
    /// </summary>
    private List<ProjectDetails> Projects { get; set; } = new();

    /// <summary>
    /// List of Scrum items displayed in the table.
    /// </summary>
    private List<ScrumDetails> Items { get; set; } = new();

    /// <summary>
    /// Load Users, Projects, and Scrum items including Project navigation property when page initializes.
    /// </summary>
    /// <returns>Task representing the async operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        Users = (await UserService.GetAllAsync()) ?? new();
        Projects = (await ProjectService.GetAllAsync()) ?? new();
        Items = (await ScrumService.GetAllAsync(s => s.Project)) ?? new();
    }

    /// <summary>
    /// Add a new Scrum row, saved immediately to DB and added to UI list.
    /// </summary>
    /// <returns>Task representing the async operation.</returns>
    private async Task AddRow()
    {
        var newItem = new ScrumDetails { Status = ScrumDetails.ScrumStatus.New };
        await ScrumService.AddAsync(newItem);
        Items.Add(newItem);
    }

    /// <summary>
    /// Remove a Scrum row from DB (if it exists) and from UI list.
    /// </summary>
    /// <param name="item">ScrumDetails item to remove.</param>
    /// <returns>Task representing the async operation.</returns>
    private async Task Remove(ScrumDetails item)
    {
        if (item.Id != 0) await ScrumService.DeleteAsync(item.Id);
        Items.Remove(item);
    }

    /// <summary>
    /// Save changes for a Scrum row; adds new if Id is 0 or updates existing row.
    /// </summary>
    /// <param name="item">ScrumDetails item to save or update.</param>
    /// <returns>Task representing the async operation.</returns>
    private async Task SaveChanges(ScrumDetails item)
    {
        if (item.Id == 0) await ScrumService.AddAsync(item);
        else await ScrumService.UpdateAsync(item);
    }

    /// <summary>
    /// Reset the table by clearing all Scrum items in the UI (does not affect DB).
    /// </summary>
    private void ResetData() => Items.Clear();
}
