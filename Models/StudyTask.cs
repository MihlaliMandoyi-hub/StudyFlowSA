using SQLite;

namespace StudyFlowSA.Models
{
    public enum TaskCategory
    {
        Assignment,
        Test,
        Exam,
        Homework,
        Project,
        Other
    }

    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public class StudyTask
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty; // e.g. "Life Sciences assignment"

        [Indexed]
        public int SubjectId { get; set; } // links to Subject.Id

        public TaskCategory Category { get; set; } = TaskCategory.Assignment;

        public DateTime DueDate { get; set; } = DateTime.Now;

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public int EstimatedMinutes { get; set; } = 30; // estimated study time

        public string Notes { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public DateTime? CompletedDate { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
