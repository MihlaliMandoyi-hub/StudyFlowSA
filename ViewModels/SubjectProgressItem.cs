namespace StudyFlowSA.ViewModels
{
    // Display-only: one subject's study time this week, for the Progress bar chart.
    public class SubjectProgressItem
    {
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectColorHex { get; set; } = "#888780";
        public int MinutesStudied { get; set; }
        public double BarWidthFraction { get; set; } // 0.0 - 1.0, relative to the top subject
    }
}