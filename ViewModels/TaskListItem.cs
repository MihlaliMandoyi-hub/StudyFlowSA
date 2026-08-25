using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    // Display-only wrapper: combines a StudyTask with its Subject's name/color.
    // Never saved to the database directly.
    public class TaskListItem
    {
        public StudyTask Task { get; set; } = null!;

        public string SubjectName { get; set; } = "No subject";

        public string SubjectColorHex { get; set; } = "#888780";

        public string DueDateDisplay => Task.DueDate.ToString("ddd, dd MMM");

        public bool IsOverdue => !Task.IsCompleted && Task.DueDate.Date < DateTime.Today;
    }
}