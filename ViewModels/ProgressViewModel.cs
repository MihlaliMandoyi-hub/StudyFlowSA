using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;

namespace StudyFlowSA.ViewModels
{
    public partial class ProgressViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public ProgressViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool hasData;

        [ObservableProperty]
        private int totalMinutesThisWeek;

        [ObservableProperty]
        private int completionRatePercent;

        [ObservableProperty]
        private int tasksCompletedThisWeek;

        [ObservableProperty]
        private int tasksDueThisWeek;

        [ObservableProperty]
        private string weekRangeLabel = string.Empty;

        public ObservableCollection<SubjectProgressItem> SubjectBreakdown { get; } = new();

        [RelayCommand]
        private async Task LoadProgressAsync()
        {
            IsBusy = true;

            var today = DateTime.Today;
            int diffFromMonday = ((int)today.DayOfWeek == 0) ? 6 : (int)today.DayOfWeek - 1;
            var weekStart = today.AddDays(-diffFromMonday);
            var weekEnd = weekStart.AddDays(6);

            WeekRangeLabel = $"{weekStart:dd MMM} - {weekEnd:dd MMM}";

            var subjects = await _databaseService.GetSubjectsAsync();
            var sessions = await _databaseService.GetSessionsBetweenAsync(weekStart, weekEnd.AddDays(1).AddSeconds(-1));
            var completedSessions = sessions.Where(s => s.IsCompleted).ToList();

            var allTasks = await _databaseService.GetTasksAsync();
            var tasksThisWeek = allTasks.Where(t => t.DueDate.Date >= weekStart && t.DueDate.Date <= weekEnd).ToList();

            TasksDueThisWeek = tasksThisWeek.Count;
            TasksCompletedThisWeek = tasksThisWeek.Count(t => t.IsCompleted);
            CompletionRatePercent = TasksDueThisWeek == 0
                ? 0
                : (int)Math.Round(TasksCompletedThisWeek * 100.0 / TasksDueThisWeek);

            TotalMinutesThisWeek = completedSessions.Sum(s => s.DurationMinutes);

            var bySubject = completedSessions
                .GroupBy(s => s.SubjectId)
                .Select(g => new
                {
                    SubjectId = g.Key,
                    Minutes = g.Sum(s => s.DurationMinutes)
                })
                .OrderByDescending(x => x.Minutes)
                .ToList();

            SubjectBreakdown.Clear();

            int maxMinutes = bySubject.Count > 0 ? bySubject.Max(x => x.Minutes) : 0;

            foreach (var entry in bySubject)
            {
                var subject = subjects.FirstOrDefault(s => s.Id == entry.SubjectId);
                SubjectBreakdown.Add(new SubjectProgressItem
                {
                    SubjectName = subject?.Name ?? "No subject",
                    SubjectColorHex = subject?.ColorHex ?? "#888780",
                    MinutesStudied = entry.Minutes,
                    BarWidthFraction = maxMinutes == 0 ? 0 : (double)entry.Minutes / maxMinutes
                });
            }

            HasData = SubjectBreakdown.Count > 0;
            IsBusy = false;
        }
    }
}