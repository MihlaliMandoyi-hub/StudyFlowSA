using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public HomeViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string greeting = "Hello!";

        [ObservableProperty]
        private bool hasTodayTasks;

        [ObservableProperty]
        private TaskListItem? nextDeadline;

        [ObservableProperty]
        private bool hasNextDeadline;

        [ObservableProperty]
        private int completionRatePercent;

        [ObservableProperty]
        private int totalMinutesStudiedThisWeek;

        [ObservableProperty]
        private int tasksCompletedThisWeek;

        [ObservableProperty]
        private int tasksDueThisWeek;

        public ObservableCollection<TaskListItem> TodayTasks { get; } = new();

        [RelayCommand]
        private async Task LoadDashboardAsync()
        {
            IsBusy = true;

            var profile = await _databaseService.GetProfileAsync();
            var timeOfDay = DateTime.Now.Hour switch
            {
                < 12 => "Good morning",
                < 17 => "Good afternoon",
                _ => "Good evening"
            };
            var name = string.IsNullOrWhiteSpace(profile?.Name) ? "there" : profile!.Name;
            Greeting = $"{timeOfDay}, {name}";

            var allTasks = await _databaseService.GetTasksAsync();
            var allSubjects = await _databaseService.GetSubjectsAsync();

            // Today's tasks
            TodayTasks.Clear();
            var todays = allTasks
                .Where(t => !t.IsCompleted && t.DueDate.Date == DateTime.Today)
                .OrderBy(t => t.Priority);

            foreach (var task in todays)
            {
                var subject = allSubjects.FirstOrDefault(s => s.Id == task.SubjectId);
                TodayTasks.Add(new TaskListItem
                {
                    Task = task,
                    SubjectName = subject?.Name ?? "No subject",
                    SubjectColorHex = subject?.ColorHex ?? "#888780"
                });
            }
            HasTodayTasks = TodayTasks.Count > 0;

            // Next deadline (soonest incomplete task, today or later)
            var next = allTasks
                .Where(t => !t.IsCompleted && t.DueDate.Date >= DateTime.Today)
                .OrderBy(t => t.DueDate)
                .FirstOrDefault();

            if (next is not null)
            {
                var subject = allSubjects.FirstOrDefault(s => s.Id == next.SubjectId);
                NextDeadline = new TaskListItem
                {
                    Task = next,
                    SubjectName = subject?.Name ?? "No subject",
                    SubjectColorHex = subject?.ColorHex ?? "#888780"
                };
                HasNextDeadline = true;
            }
            else
            {
                HasNextDeadline = false;
            }

            // Weekly progress (Monday - Sunday of current week)
            var today = DateTime.Today;
            int diffFromMonday = ((int)today.DayOfWeek == 0) ? 6 : (int)today.DayOfWeek - 1;
            var weekStart = today.AddDays(-diffFromMonday);
            var weekEnd = weekStart.AddDays(6);

            var tasksThisWeek = allTasks.Where(t => t.DueDate.Date >= weekStart && t.DueDate.Date <= weekEnd).ToList();
            TasksDueThisWeek = tasksThisWeek.Count;
            TasksCompletedThisWeek = tasksThisWeek.Count(t => t.IsCompleted);
            CompletionRatePercent = TasksDueThisWeek == 0
                ? 0
                : (int)Math.Round(TasksCompletedThisWeek * 100.0 / TasksDueThisWeek);

            var sessionsThisWeek = await _databaseService.GetSessionsBetweenAsync(weekStart, weekEnd.AddDays(1).AddSeconds(-1));
            TotalMinutesStudiedThisWeek = sessionsThisWeek.Where(s => s.IsCompleted).Sum(s => s.DurationMinutes);

            IsBusy = false;
        }

        [RelayCommand]
        private async Task GoToAddTaskAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.AddEditTaskPage));
        }

        [RelayCommand]
        private async Task GoToSubjectsAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.SubjectsPage));
        }

        [RelayCommand]
        private async Task GoToSettingsAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.SettingsPage));
        }
    }
}