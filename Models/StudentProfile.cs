using SQLite;

namespace StudyFlowSA.Models
{
    public class StudentProfile
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Grade { get; set; } = string.Empty; // e.g. "Grade 11" or "1st Year"

        public bool IsDarkMode { get; set; } = false;

        public bool NotificationsEnabled { get; set; } = true;

        public bool HasCompletedOnboarding { get; set; } = false;
    }
}