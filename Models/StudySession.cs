using SQLite;

namespace StudyFlowSA.Models
{
    public class StudySession
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int SubjectId { get; set; } // links to Subject.Id

        public int? StudyTaskId { get; set; } // optional link to a specific task

        public DateTime StartTime { get; set; } = DateTime.Now;

        public DateTime? EndTime { get; set; }

        public int DurationMinutes { get; set; } // planned or actual duration

        public bool IsCompleted { get; set; } = false;

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}