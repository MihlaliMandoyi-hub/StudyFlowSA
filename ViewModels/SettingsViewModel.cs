using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private StudentProfile? _profile;

        public SettingsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string learnerName = string.Empty;

        [ObservableProperty]
        private bool isDarkMode;

        [ObservableProperty]
        private bool notificationsEnabled;

        [ObservableProperty]
        private string statusMessage = string.Empty;

        [RelayCommand]
        private async Task LoadSettingsAsync()
        {
            _profile = await _databaseService.GetProfileAsync();

            if (_profile is not null)
            {
                LearnerName = _profile.Name;
                IsDarkMode = _profile.IsDarkMode;
                NotificationsEnabled = _profile.NotificationsEnabled;
            }
        }

        partial void OnIsDarkModeChanged(bool value)
        {
            Application.Current!.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
            _ = SaveProfileAsync();
        }

        partial void OnNotificationsEnabledChanged(bool value)
        {
            _ = SaveProfileAsync();
        }

        private async Task SaveProfileAsync()
        {
            if (_profile is null)
                return;

            _profile.IsDarkMode = IsDarkMode;
            _profile.NotificationsEnabled = NotificationsEnabled;

            await _databaseService.SaveProfileAsync(_profile);
        }

        [RelayCommand]
        private async Task ClearAllDataAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Clear all data",
                "This will permanently delete all your subjects, tasks, and study sessions. This cannot be undone. Are you sure?",
                "Delete everything",
                "Cancel");

            if (!confirm)
                return;

            var subjects = await _databaseService.GetSubjectsAsync();
            foreach (var s in subjects)
                await _databaseService.DeleteSubjectAsync(s);

            var tasks = await _databaseService.GetTasksAsync();
            foreach (var t in tasks)
                await _databaseService.DeleteTaskAsync(t);

            var sessions = await _databaseService.GetSessionsAsync();
            foreach (var s in sessions)
                await _databaseService.DeleteSessionAsync(s);

            StatusMessage = "All data cleared.";
        }
    }
}