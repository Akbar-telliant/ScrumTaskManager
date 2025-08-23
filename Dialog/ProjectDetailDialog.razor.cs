using Microsoft.AspNetCore.Components;
using ScrumMaster.Models;
using ScrumMaster.Services;

namespace ScrumMaster.Dialog;

/// <summary>
/// Dialog for adding or editing project details.
/// </summary>
public partial class ProjectDetailDialog
{
    /// <summary>
    /// Dialog instance reference.
    /// </summary>
    [CascadingParameter]
    private IMudDialogInstance? MudDialog { get; set; }

    /// <summary>
    /// Project detail model (passed in from parent).
    /// </summary>
    [Parameter]
    public ProjectDetail ProjectDetail { get; set; } = new();

    /// <summary>
    /// Dialog header text.
    /// </summary>
    [Parameter]
    public string DialogTitle { get; set; } = "Add Project";

    /// <summary>
    /// Entity data service for ProjectDetail CRUD.
    /// </summary>
    [Inject]
    private EntityDataService<ProjectDetail> ProjectService { get; set; } = default!;

    /// <summary>
    /// Handles form submission.
    /// </summary>
    private async Task OnValidSubmitAsync()
    {
        if (ProjectDetail.Id == 0)
        {
            await ProjectService.AddAsync(ProjectDetail); // New entity → Add
        }
        else
        {
            await ProjectService.UpdateAsync(ProjectDetail); // Existing entity → Update
        }

        MudDialog?.Close(DialogResult.Ok(ProjectDetail));
    }

    /// <summary>
    /// Closes dialog without saving.
    /// </summary>
    private void Cancel() => MudDialog?.Cancel();
}
