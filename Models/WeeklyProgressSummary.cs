namespace StudyFlowSA.Models
{
    // Note: this model is NOT stored directly in SQLite.
    // It's calculated on the fly from StudyTask + StudySession data,
    // so it does not need a [PrimaryKey] or table attributes.
    public class WeeklyProgressSummary
    {
        public DateTime WeekStartDate { get; set; }

        public DateTime WeekEndDate { get; set; }

        public int TotalTasksCompleted { get; set; }

        public int TotalTasksDue { get; set; }

        public double CompletionRatePercent { get; set; } // TasksCompleted / TasksDue * 100

        public int TotalMinutesStudied { get; set; }

        public Dictionary<int, int> MinutesStudiedBySubjectId { get; set; } = new();
    }
}
