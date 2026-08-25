namespace StudyFlowSA.ViewModels
{
    // Display-only wrapper for one day cell in the calendar grid.
    public class CalendarDayCell
    {
        public DateTime Date { get; set; }
        public int DayNumber { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public bool HasTasks { get; set; }
    }
}