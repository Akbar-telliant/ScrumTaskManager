using Microsoft.AspNetCore.Components;

namespace ScrumMaster.Pages;

/// <summary>
/// Defines logic for the home page.
/// </summary>
public partial class Home : ComponentBase
{
    // --- Demo lookups ---
    private readonly List<string> Names = new() { "Roophan", "Akbar", "Pushpalatha", "Aishwarya", "Sanjay" };
    private readonly List<string> Projects = new() { "Content-Server", "Mobile", "Capture", "IPTA" };
    private readonly List<string> Statuses = new() { "New", "In Progress", "Closed", "On Hold", "Dropped" };

    // --- Data model ---
    public class WorkItem
    {
        public string? Name { get; set; }
        public string? Project { get; set; }
        public string Id { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Status { get; set; } = "New";
    }

    // --- Backing collection ---
    private List<WorkItem> items = new()
    {
        new() { Name = "Roophan", Project = "Milner", Id = "T-1001", Description = "Initial setup", Status = "In Progress" },
        new() { Name = "Akbar", Project = "Milner", Id = "T-1002", Description = "API skeleton", Status = "New" },
        new() { Name = "Pushpalatha", Project = "Milner", Id = "T-1003", Description = "DB schema", Status = "Closed" },
        new() { Name = "Aishwarya", Project = "Milner", Id = "T-1003", Description = "DB schema", Status = "On Hold" },
        new() { Name = "Sanjay", Project = "Milner", Id = "T-1003", Description = "DB schema", Status = "Dropped" }

};

    // --- Commands ---
    private void AddRow()
    {
        items.Add(new WorkItem { Status = "New" });
    }

    private void Remove(WorkItem item)
    {
        items.Remove(item);
    }

    private bool IsFirst(WorkItem item) => ReferenceEquals(item, items.FirstOrDefault());
    private bool IsLast(WorkItem item) => ReferenceEquals(item, items.LastOrDefault());

    private void MoveUp(WorkItem item)
    {
        var idx = items.IndexOf(item);
        if (idx > 0)
        {
            (items[idx - 1], items[idx]) = (items[idx], items[idx - 1]);
        }
    }

    private void MoveDown(WorkItem item)
    {
        var idx = items.IndexOf(item);
        if (idx >= 0 && idx < items.Count - 1)
        {
            (items[idx + 1], items[idx]) = (items[idx], items[idx + 1]);
        }
    }

    private void ResetData()
    {
        items = new()
        {
             new() { Name = "Roophan", Project = "Milner", Id = "T-1001", Description = "Initial setup", Status = "In Progress" },
        new() { Name = "Akbar", Project = "Milner", Id = "T-1002", Description = "API skeleton", Status = "New" },
        new() { Name = "Pushpalatha", Project = "Milner", Id = "T-1003", Description = "DB schema", Status = "Closed" },
        new() { Name = "Aishwarya", Project = "Milner", Id = "T-1003", Description = "DB schema", Status = "On Hold" },
        new() { Name = "Sanjay", Project = "Milner", Id = "T-1003", Description = "DB schema", Status = "Dropped" }
        };
    }

}
