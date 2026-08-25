using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    public partial class CalendarViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private List<StudyTask> _allTasks = new();
        private List<Subject> _allSubjects = new();

        public CalendarViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private DateTime currentMonth = new(DateTime.Today.Year, DateTime.Today.Month, 1);

        [ObservableProperty]
        private string monthLabel = string.Empty;

        [ObservableProperty]
        private DateTime selectedDate = DateTime.Today;

        [ObservableProperty]
        private bool isEmpty;

        [ObservableProperty]
        private bool hasUpcomingDeadline;

        [ObservableProperty]
        private string upcomingDeadlineText = string.Empty;

        public ObservableCollection<CalendarDayCell> DayCells { get; } = new();
        public ObservableCollection<TaskListItem> SelectedDayTasks { get; } = new();

        [RelayCommand]
        private async Task LoadCalendarAsync()
        {
            _allTasks = await _databaseService.GetTasksAsync();
            _allSubjects = await _databaseService.GetSubjectsAsync();

            BuildMonthGrid();
            BuildSelectedDayTasks();
            BuildUpcomingDeadlineCountdown();
        }

        private void BuildMonthGrid()
        {
            MonthLabel = CurrentMonth.ToString("MMMM yyyy");

            DayCells.Clear();

            var firstOfMonth = new DateTime(CurrentMonth.Year, CurrentMonth.Month, 1);
            int leadingBlanks = ((int)firstOfMonth.DayOfWeek == 0) ? 6 : (int)firstOfMonth.DayOfWeek - 1;
            var gridStart = firstOfMonth.AddDays(-leadingBlanks);

            for (int i = 0; i < 42; i++)
            {
                var date = gridStart.AddDays(i);
                bool hasTasks = _allTasks.Any(t => t.DueDate.Date == date.Date);

                DayCells.Add(new CalendarDayCell
                {
                    Date = date,
                    DayNumber = date.Day,
                    IsCurrentMonth = date.Month == CurrentMonth.Month,
                    IsToday = date.Date == DateTime.Today,
                    HasTasks = hasTasks
                });
            }
        }

        private void BuildSelectedDayTasks()
        {
            SelectedDayTasks.Clear();

            var dayTasks = _allTasks.Where(t => t.DueDate.Date == SelectedDate.Date).OrderBy(t => t.Priority);

            foreach (var task in dayTasks)
            {
                var subject = _allSubjects.FirstOrDefault(s => s.Id == task.SubjectId);
                SelectedDayTasks.Add(new TaskListItem
                {
                    Task = task,
                    SubjectName = subject?.Name ?? "No subject",
                    SubjectColorHex = subject?.ColorHex ?? "#888780"
                });
            }

            IsEmpty = SelectedDayTasks.Count == 0;
        }

        private void BuildUpcomingDeadlineCountdown()
        {
            var next = _allTasks
                .Where(t => !t.IsCompleted && t.DueDate.Date >= DateTime.Today)
                .OrderBy(t => t.DueDate)
                .FirstOrDefault();

            if (next is null)
            {
                HasUpcomingDeadline = false;
                UpcomingDeadlineText = string.Empty;
                return;
            }

            int daysUntil = (next.DueDate.Date - DateTime.Today).Days;

            string whenText = daysUntil switch
            {
                0 => "due today",
                1 => "due tomorrow",
                _ => $"due in {daysUntil} days"
            };

            UpcomingDeadlineText = $"{next.Title} — {whenText}";
            HasUpcomingDeadline = true;
        }

        [RelayCommand]
        private void SelectDay(CalendarDayCell cell)
        {
            if (cell is null)
                return;

            SelectedDate = cell.Date;
            BuildSelectedDayTasks();
        }

        [RelayCommand]
        private void PreviousMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(-1);
            BuildMonthGrid();
        }

        [RelayCommand]
        private void NextMonth()
        {
            CurrentMonth = CurrentMonth.AddMonths(1);
            BuildMonthGrid();
        }
    }
}