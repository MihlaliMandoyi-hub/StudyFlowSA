using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    public partial class OnboardingViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public OnboardingViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string learnerName = string.Empty;

        [ObservableProperty]
        private string grade = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        public List<string> GradeOptions { get; } = new()
        {
            "Grade 10", "Grade 11", "Grade 12",
            "1st Year", "2nd Year", "3rd Year", "4th Year", "Postgraduate"
        };

        [RelayCommand]
        private async Task GetStartedAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(LearnerName))
            {
                ErrorMessage = "Please enter your name.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Grade))
            {
                ErrorMessage = "Please select your grade or year.";
                return;
            }

            IsBusy = true;

            var profile = new StudentProfile
            {
                Name = LearnerName.Trim(),
                Grade = Grade,
                HasCompletedOnboarding = true
            };

            await _databaseService.SaveProfileAsync(profile);

            IsBusy = false;

            Application.Current!.MainPage = new AppShell();
        }

        [RelayCommand]
        private async Task SkipAsync()
        {
            var profile = new StudentProfile
            {
                Name = "Student",
                Grade = string.Empty,
                HasCompletedOnboarding = true
            };

            await _databaseService.SaveProfileAsync(profile);

            Application.Current!.MainPage = new AppShell();
        }
    }
}