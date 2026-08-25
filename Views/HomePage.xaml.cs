using StudyFlowSA.ViewModels;

namespace StudyFlowSA.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDashboardCommand.ExecuteAsync(null);
    }

    private async void OnGoToTimerClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(StudySessionPage));
    }
}