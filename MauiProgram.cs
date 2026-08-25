using Microsoft.Extensions.Logging;
using StudyFlowSA.Data;
using StudyFlowSA.ViewModels;
using StudyFlowSA.Views;

namespace StudyFlowSA
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();

            builder.Services.AddTransient<OnboardingViewModel>();
            builder.Services.AddTransient<OnboardingPage>();

            builder.Services.AddTransient<SubjectsViewModel>();
            builder.Services.AddTransient<SubjectsPage>();

            builder.Services.AddTransient<AddEditSubjectViewModel>();
            builder.Services.AddTransient<AddEditSubjectPage>();

            builder.Services.AddTransient<TasksViewModel>();
            builder.Services.AddTransient<TasksPage>();

            builder.Services.AddTransient<AddEditTaskViewModel>();
            builder.Services.AddTransient<AddEditTaskPage>();

            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();

            builder.Services.AddTransient<StudySessionViewModel>();
            builder.Services.AddTransient<StudySessionPage>();

            builder.Services.AddTransient<CalendarViewModel>();
            builder.Services.AddTransient<CalendarPage>();

            builder.Services.AddTransient<ProgressViewModel>();
            builder.Services.AddTransient<ProgressPage>();

            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}