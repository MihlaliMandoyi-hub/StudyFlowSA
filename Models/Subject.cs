using SQLite;

namespace StudyFlowSA.Models
{
    public class Subject
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty; // e.g. "Mathematics"

        public string ColorHex { get; set; } = "#1B3358"; // for subject cards/badges

        public string IconName { get; set; } = string.Empty; // optional icon identifier

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}