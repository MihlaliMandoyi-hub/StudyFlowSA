using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    [QueryProperty(nameof(SubjectId), "SubjectId")]
    public partial class AddEditSubjectViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private int _existingId;

        public AddEditSubjectViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private string subjectId = string.Empty; // received as string from query, parsed below

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string selectedColor = "#1B3358";

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string pageTitle = "Add subject";

        public List<string> ColorOptions { get; } = new()
        {
            "#1B3358", // deep blue
            "#D9A441", // warm gold
            "#3B6D11", // sage green
            "#993C1D", // coral
            "#534AB7", // purple
            "#185FA5"  // sky blue
        };

        partial void OnSubjectIdChanged(string value)
        {
            _ = LoadIfEditingAsync(value);
        }

        private async Task LoadIfEditingAsync(string idValue)
        {
            if (int.TryParse(idValue, out int id) && id > 0)
            {
                _existingId = id;
                PageTitle = "Edit subject";

                var subject = await _databaseService.GetSubjectAsync(id);
                if (subject is not null)
                {
                    Name = subject.Name;
                    SelectedColor = subject.ColorHex;
                }
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Please enter a subject name.";
                return;
            }

            var subject = new Subject
            {
                Id = _existingId,
                Name = Name.Trim(),
                ColorHex = SelectedColor
            };

            await _databaseService.SaveSubjectAsync(subject);

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}