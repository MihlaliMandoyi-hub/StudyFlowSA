using StudyFlowSA.ViewModels;

namespace StudyFlowSA.Views;

public partial class StudySessionPage : ContentPage
{
    private readonly StudySessionViewModel _viewModel;

    public StudySessionPage(StudySessionViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}