using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    public partial class TasksViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private List<StudyTask> _allTasks = new();
        private List<Subject> _allSubjects = new();

        public TasksViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isEmpty;

        [ObservableProperty]
        private string selectedFilter = "Today";

        public List<string> FilterOptions { get; } = new() { "Today", "Upcoming", "Overdue", "Completed" };

        public ObservableCollection<TaskListItem> Tasks { get; } = new();

        partial void OnSelectedFilterChanged(string value)
        {
            ApplyFilter();
        }

        [RelayCommand]
        private async Task LoadTasksAsync()
        {
            IsBusy = true;

            _allTasks = await _databaseService.GetTasksAsync();
            _allSubjects = await _databaseService.GetSubjectsAsync();

            ApplyFilter();
            IsBusy = false;
        }

        private void ApplyFilter()
        {
            IEnumerable<StudyTask> filtered = SelectedFilter switch
            {
                "Today" => _allTasks.Where(t => !t.IsCompleted && t.DueDate.Date == DateTime.Today),
                "Upcoming" => _allTasks.Where(t => !t.IsCompleted && t.DueDate.Date > DateTime.Today),
                "Overdue" => _allTasks.Where(t => !t.IsCompleted && t.DueDate.Date < DateTime.Today),
                "Completed" => _allTasks.Where(t => t.IsCompleted),
                _ => _allTasks
            };

            Tasks.Clear();
            foreach (var task in filtered.OrderBy(t => t.DueDate))
            {
                var subject = _allSubjects.FirstOrDefault(s => s.Id == task.SubjectId);
                Tasks.Add(new TaskListItem
                {
                    Task = task,
                    SubjectName = subject?.Name ?? "No subject",
                    SubjectColorHex = subject?.ColorHex ?? "#888780"
                });
            }

            IsEmpty = Tasks.Count == 0;
        }

        [RelayCommand]
        private async Task AddTaskAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.AddEditTaskPage));
        }

        [RelayCommand]
        private async Task EditTaskAsync(TaskListItem item)
        {
            if (item is null)
                return;

            var navParams = new Dictionary<string, object> { { "TaskId", item.Task.Id } };
            await Shell.Current.GoToAsync(nameof(Views.AddEditTaskPage), navParams);
        }

        [RelayCommand]
        private async Task ToggleCompleteAsync(TaskListItem item)
        {
            if (item is null)
                return;

            item.Task.IsCompleted = !item.Task.IsCompleted;
            item.Task.CompletedDate = item.Task.IsCompleted ? DateTime.Now : null;

            await _databaseService.SaveTaskAsync(item.Task);
            await LoadTasksAsync();
        }

        [RelayCommand]
        private async Task DeleteTaskAsync(TaskListItem item)
        {
            if (item is null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete task", $"Delete \"{item.Task.Title}\"?", "Delete", "Cancel");

            if (!confirm)
                return;

            await _databaseService.DeleteTaskAsync(item.Task);
            Tasks.Remove(item);
            IsEmpty = Tasks.Count == 0;
        }
    }
}