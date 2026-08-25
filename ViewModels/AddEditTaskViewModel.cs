using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    [QueryProperty(nameof(TaskId), "TaskId")]
    public partial class AddEditTaskViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private int _existingId;

        public AddEditTaskViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string taskId = string.Empty;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private Subject? selectedSubject;

        [ObservableProperty]
        private string selectedCategory = "Assignment";

        [ObservableProperty]
        private DateTime dueDate = DateTime.Today;

        [ObservableProperty]
        private string selectedPriority = "Medium";

        [ObservableProperty]
        private string estimatedMinutesText = "30";

        [ObservableProperty]
        private string notes = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string pageTitle = "Add task";

        public ObservableCollection<Subject> Subjects { get; } = new();
        public List<string> CategoryOptions { get; } = Enum.GetNames(typeof(TaskCategory)).ToList();
        public List<string> PriorityOptions { get; } = Enum.GetNames(typeof(TaskPriority)).ToList();

        public async Task InitializeAsync()
        {
            var subjects = await _databaseService.GetSubjectsAsync();

            Subjects.Clear();
            foreach (var s in subjects)
                Subjects.Add(s);
        }

        partial void OnTaskIdChanged(string value)
        {
            _ = LoadIfEditingAsync(value);
        }

        private async Task LoadIfEditingAsync(string idValue)
        {
            await InitializeAsync();

            if (int.TryParse(idValue, out int id) && id > 0)
            {
                _existingId = id;
                PageTitle = "Edit task";

                var task = await _databaseService.GetTaskAsync(id);
                if (task is not null)
                {
                    Title = task.Title;
                    SelectedSubject = Subjects.FirstOrDefault(s => s.Id == task.SubjectId);
                    SelectedCategory = task.Category.ToString();
                    DueDate = task.DueDate;
                    SelectedPriority = task.Priority.ToString();
                    EstimatedMinutesText = task.EstimatedMinutes.ToString();
                    Notes = task.Notes;
                }
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Title))
            {
                ErrorMessage = "Please enter a task title.";
                return;
            }

            if (SelectedSubject is null)
            {
                ErrorMessage = "Please select a subject.";
                return;
            }

            if (!int.TryParse(EstimatedMinutesText, out int minutes) || minutes <= 0)
            {
                ErrorMessage = "Please enter a valid estimated study time in minutes.";
                return;
            }

            var task = new StudyTask
            {
                Id = _existingId,
                Title = Title.Trim(),
                SubjectId = SelectedSubject.Id,
                Category = Enum.Parse<TaskCategory>(SelectedCategory),
                DueDate = DueDate,
                Priority = Enum.Parse<TaskPriority>(SelectedPriority),
                EstimatedMinutes = minutes,
                Notes = Notes.Trim()
            };

            await _databaseService.SaveTaskAsync(task);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}