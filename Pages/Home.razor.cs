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
    /// Currently selected date, defaults to today.
    /// </summary>
    private DateTime? m_SelectedDate = DateTime.Today;

    /// <summary>
    /// List of Scrum items filtered based on the selected date or criteria.
    /// </summary>
    private List<ScrumDetails> filteredItems = new();

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

        if (Users.Count == 0 || Projects.Count == 0)
        {
            m_Snackbar.Add("Cannot create Scrum rows: Users or Projects are missing.", Severity.Warning);
            return;
        }

        Items = (await ScrumService.GetAllAsync(s => s.Project, s => s.User)) ?? new();
        m_SelectedDate = DateTime.Today;
        await EnsureRowsForAllUsers();
    }

    /// <summary>
    /// Add a new Scrum row, saved immediately to DB and added to UI list.
    /// </summary>
    /// <returns>Task representing the async operation.</returns>
    private async Task AddRow()
    {
        if (Users == null || Users.Count == 0)
        {
            m_Snackbar.Add("No users found. Cannot add row.", Severity.Warning);
            return;
        }

        if (Projects == null || Projects.Count == 0)
        {
            m_Snackbar.Add("No projects found. Cannot add row.", Severity.Warning);
            return;
        }

        var defaultUserId = Users.First().Id;      // guaranteed valid
        var defaultProjectId = Projects.First().Id; // guaranteed valid

        var newItem = new ScrumDetails
        {
            Status = ScrumDetails.ScrumStatus.New,
            UserId = defaultUserId,
            ProjectId = defaultProjectId,
            ScrumDate = m_SelectedDate ?? DateTime.Today
        };

        try
        {
            await ScrumService.AddAsync(newItem);
            Items.Add(newItem);
        }
        catch (Exception ex)
        {
            m_Snackbar.Add($"Failed to add Scrum row: {ex.Message}", Severity.Error);
        }
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
            if (item.ScrumDate == default)
                item.ScrumDate = m_SelectedDate ?? DateTime.Today;

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

            // Reload items from DB to keep Items fresh
            Items = await ScrumService.GetAllAsync();

            // Re-apply filter
            FilterByDate();
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

    /// <summary>
    /// Filters Scrum items based on the selected date, or returns all if no date is selected.
    /// </summary>
    private void FilterByDate()
    {
        if (m_SelectedDate.HasValue)
        {
            filteredItems = Items
                .Where(i => i.ScrumDate.Date == m_SelectedDate.Value.Date)
                .ToList();
        }
        else
        {
            filteredItems = Items.ToList();
        }

        StateHasChanged(); // force UI update
    }

    /// <summary>
    /// Ensures each user has at least one Scrum row for the selected date, safely checking for valid Users and Projects.
    /// </summary>
    private async Task EnsureRowsForAllUsers()
    {
        if (Users == null || Users.Count == 0)
        {
            m_Snackbar.Add("No users found. Cannot create Scrum rows.", Severity.Warning);
            return;
        }

        if (Projects == null || Projects.Count == 0)
        {
            m_Snackbar.Add("No projects found. Cannot create Scrum rows.", Severity.Warning);
            return;
        }

        var defaultProjectId = Projects.First().Id; // safe since Projects.Count > 0
        var date = m_SelectedDate ?? DateTime.Today;

        foreach (var user in Users)
        {
            // Only create a row if the user exists and project is valid
            if (!Items.Any(i => i.UserId == user.Id && i.ScrumDate.Date == date.Date))
            {
                var newItem = new ScrumDetails
                {
                    Status = ScrumDetails.ScrumStatus.New,
                    UserId = user.Id,
                    ProjectId = defaultProjectId,
                    ScrumDate = date
                };

                try
                {
                    await ScrumService.AddAsync(newItem);
                    Items.Add(newItem);
                }
                catch (Exception ex)
                {
                    m_Snackbar.Add($"Failed to add Scrum row for user {user.UserName}: {ex.Message}", Severity.Error);
                }
            }
        }

        FilterByDate();
    }
}
