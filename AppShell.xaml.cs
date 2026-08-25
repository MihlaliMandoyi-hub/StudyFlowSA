namespace StudyFlowSA;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Routes not in the tab bar - reached via Shell.Current.GoToAsync(...)
        Routing.RegisterRoute(nameof(Views.SubjectsPage), typeof(Views.SubjectsPage));
        Routing.RegisterRoute(nameof(Views.AddEditSubjectPage), typeof(Views.AddEditSubjectPage));
        Routing.RegisterRoute(nameof(Views.SettingsPage), typeof(Views.SettingsPage));
        Routing.RegisterRoute(nameof(Views.AddEditTaskPage), typeof(Views.AddEditTaskPage));
        Routing.RegisterRoute(nameof(Views.StudySessionPage), typeof(Views.StudySessionPage));
    }
}