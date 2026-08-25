using StudyFlowSA.ViewModels;

namespace StudyFlowSA.Views;

public partial class AddEditSubjectPage : ContentPage
{
    public AddEditSubjectPage(AddEditSubjectViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}