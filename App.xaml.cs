using StudyFlowSA.Data;
using StudyFlowSA.ViewModels;
using StudyFlowSA.Views;

namespace StudyFlowSA;

public partial class App : Application
{
    private readonly DatabaseService _databaseService;
    private readonly OnboardingViewModel _onboardingViewModel;

    public App(DatabaseService databaseService, OnboardingViewModel onboardingViewModel)
    {
        InitializeComponent();
        _databaseService = databaseService;
        _onboardingViewModel = onboardingViewModel;

        // Show a simple loading page first while we check the profile
        MainPage = new ContentPage { BackgroundColor = Color.FromArgb("#1B3358") };
        _ = InitializeStartupPageAsync();
    }

    private async Task InitializeStartupPageAsync()
    {
        var profile = await _databaseService.GetProfileAsync();

        if (profile is not null && profile.HasCompletedOnboarding)
        {
            // Restore saved theme preference
            Application.Current!.UserAppTheme = profile.IsDarkMode ? AppTheme.Dark : AppTheme.Light;
            MainPage = new AppShell();
        }
        else
        {
            MainPage = new OnboardingPage(_onboardingViewModel);
        }
    }
}