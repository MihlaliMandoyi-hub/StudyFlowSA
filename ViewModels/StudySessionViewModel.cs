using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    public partial class StudySessionViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private IDispatcherTimer? _timer;
        private DateTime _startedAt;
        private int _elapsedSeconds;

        public StudySessionViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private Subject? selectedSubject;

        [ObservableProperty]
        private int selectedDurationMinutes = 25;

        [ObservableProperty]
        private string elapsedDisplay = "00:00";

        [ObservableProperty]
        private bool isRunning;

        [ObservableProperty]
        private bool isPaused;

        [ObservableProperty]
        private bool isIdle = true;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ObservableCollection<Subject> Subjects { get; } = new();
        public List<int> DurationOptions { get; } = new() { 15, 25, 30, 45, 60, 90 };

        public async Task InitializeAsync()
        {
            var subjects = await _databaseService.GetSubjectsAsync();

            Subjects.Clear();
            foreach (var s in subjects)
                Subjects.Add(s);
        }

        [RelayCommand]
        private void Start()
        {
            ErrorMessage = string.Empty;

            if (SelectedSubject is null)
            {
                ErrorMessage = "Please select a subject before starting.";
                return;
            }

            _elapsedSeconds = 0;
            _startedAt = DateTime.Now;

            _timer = Application.Current!.Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => Tick();
            _timer.Start();

            IsRunning = true;
            IsPaused = false;
            IsIdle = false;
        }

        private void Tick()
        {
            if (IsPaused) return;

            _elapsedSeconds++;
            var ts = TimeSpan.FromSeconds(_elapsedSeconds);
            ElapsedDisplay = ts.ToString(@"mm\:ss");
        }

        [RelayCommand]
        private void Pause()
        {
            IsPaused = !IsPaused;
        }

        [RelayCommand]
        private async Task FinishAsync()
        {
            _timer?.Stop();
            IsRunning = false;
            IsPaused = false;

            int minutesStudied = Math.Max(1, _elapsedSeconds / 60);

            var session = new StudySession
            {
                SubjectId = SelectedSubject!.Id,
                StartTime = _startedAt,
                EndTime = DateTime.Now,
                DurationMinutes = minutesStudied,
                IsCompleted = true
            };

            await _databaseService.SaveSessionAsync(session);

            IsIdle = true;
            ElapsedDisplay = "00:00";
            _elapsedSeconds = 0;

            await Shell.Current.DisplayAlert("Session saved", $"Great work! {minutesStudied} minute(s) logged for {SelectedSubject.Name}.", "OK");
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void Cancel()
        {
            _timer?.Stop();
            IsRunning = false;
            IsPaused = false;
            IsIdle = true;
            ElapsedDisplay = "00:00";
            _elapsedSeconds = 0;
        }
    }
}