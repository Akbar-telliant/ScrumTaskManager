using Microsoft.AspNetCore.Components;
using ScrumMaster.Models;
using ScrumMaster.Services;
using System.ComponentModel;
using System.Text;

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
    /// Provides snackbar notifications for user feedback.
    /// </summary>
    [Inject]
    private ISnackbar m_Snackbar { get; set; } = default!;

    /// <summary>
    /// Load Users, Projects, and Scrum items including Project navigation property when page initializes.
    /// </summary>
    /// <returns>Task representing the async operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        Users = (await UserService.GetAllAsync()) ?? new();
        Projects = (await ProjectService.GetAllAsync()) ?? new();
        Items = (await ScrumService.GetAllAsync(s => s.Project, s => s.User)) ?? new();
    }

    /// <summary>
    /// Add a new Scrum row, saved immediately to DB and added to UI list.
    /// </summary>
    /// <returns>Task representing the async operation.</returns>
    private async Task AddRow()
    {
        var defaultUserId = Users.FirstOrDefault()?.Id ?? 0;
        var defaultProjectId = Projects.FirstOrDefault()?.Id ?? 0;

        var newItem = new ScrumDetails
        {
            Status = ScrumDetails.ScrumStatus.New,
            UserId = defaultUserId,
            ProjectId = defaultProjectId
        };

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
        try
        {
            if (item.Id == 0)
            {
                await ScrumService.AddAsync(item);
                m_Snackbar.Add("Scrum item added successfully!", Severity.Success);
            }
            else
            {
                await ScrumService.UpdateAsync(item);
                m_Snackbar.Add("Scrum item updated successfully!", Severity.Success);
            }
        }
        catch (Exception ex)
        {
            m_Snackbar.Add($"Error saving item: {ex.Message}", Severity.Error);
        }
    }

    /// <summary>
    /// Reset the table by clearing all Scrum items in the UI (does not affect DB).
    /// </summary>
    private void ResetData() => Items.Clear();

    /// <summary>
    /// Exports Scrum items to a text file and downloads it.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task ExportToText()
    {
        if (Items == null || Items.Count == 0)
        {
            m_Snackbar.Add("No data to export.", Severity.Warning);
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine("Scrum Items Export");
        sb.AppendLine("==================\n");

        foreach (var item in Items)
        {
            foreach (var prop in typeof(ScrumDetails).GetProperties())
            {
                // Skip navigation objects
                if (prop.PropertyType == typeof(UserDetails) || prop.PropertyType == typeof(ProjectDetails))
                    continue;

                // Skip primary Id
                if (prop.Name == nameof(ScrumDetails.Id))
                    continue;

                // Get display name if available
                var displayName = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                                      .Cast<DisplayNameAttribute>()
                                      .FirstOrDefault()?.DisplayName ?? prop.Name;

                object? value = prop.GetValue(item);

                // Replace IDs with friendly names
                if (prop.Name == nameof(ScrumDetails.UserId))
                    value = Users.FirstOrDefault(u => u.Id == item.UserId)?.UserName ?? "Unknown";

                if (prop.Name == nameof(ScrumDetails.ProjectId))
                    value = Projects.FirstOrDefault(p => p.Id == item.ProjectId)?.Name ?? "Unknown";

                sb.AppendLine($"{displayName} : {value}");
            }

            sb.AppendLine(new string('-', 40));
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        await JSRuntime.InvokeDownloadAsync("ScrumItems.txt", "text/plain", bytes);
    }
}
