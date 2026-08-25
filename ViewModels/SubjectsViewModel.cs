using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudyFlowSA.Data;
using StudyFlowSA.Models;

namespace StudyFlowSA.ViewModels
{
    public partial class SubjectsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public SubjectsViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool isEmpty;

        public ObservableCollection<Subject> Subjects { get; } = new();

        [RelayCommand]
        private async Task LoadSubjectsAsync()
        {
            IsBusy = true;

            var subjects = await _databaseService.GetSubjectsAsync();

            Subjects.Clear();
            foreach (var subject in subjects)
                Subjects.Add(subject);

            IsEmpty = Subjects.Count == 0;
            IsBusy = false;
        }

        [RelayCommand]
        private async Task AddSubjectAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.AddEditSubjectPage));
        }

        [RelayCommand]
        private async Task EditSubjectAsync(Subject subject)
        {
            if (subject is null)
                return;

            var navParams = new Dictionary<string, object>
            {
                { "SubjectId", subject.Id }
            };

            await Shell.Current.GoToAsync(nameof(Views.AddEditSubjectPage), navParams);
        }

        [RelayCommand]
        private async Task DeleteSubjectAsync(Subject subject)
        {
            if (subject is null)
                return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Delete subject",
                $"Delete \"{subject.Name}\"? Tasks linked to this subject will remain but show no subject.",
                "Delete",
                "Cancel");

            if (!confirm)
                return;

            await _databaseService.DeleteSubjectAsync(subject);
            Subjects.Remove(subject);
            IsEmpty = Subjects.Count == 0;
        }
    }
}