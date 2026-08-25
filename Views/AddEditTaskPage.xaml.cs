using StudyFlowSA.ViewModels;

namespace StudyFlowSA.Views;

public partial class AddEditTaskPage : ContentPage
{
    private readonly AddEditTaskViewModel _viewModel;

    public AddEditTaskPage(AddEditTaskViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // If adding a new task (no TaskId set), still need the Subjects list loaded
        if (string.IsNullOrEmpty(_viewModel.TaskId))
        {
            await _viewModel.InitializeAsync();
        }
    }
}