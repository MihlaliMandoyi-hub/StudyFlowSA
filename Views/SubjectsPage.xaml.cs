using StudyFlowSA.ViewModels;

namespace StudyFlowSA.Views;

public partial class SubjectsPage : ContentPage
{
    private readonly SubjectsViewModel _viewModel;

    public SubjectsPage(SubjectsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSubjectsCommand.ExecuteAsync(null);
    }
}