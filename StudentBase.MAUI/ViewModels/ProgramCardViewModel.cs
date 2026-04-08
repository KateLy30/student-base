using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Domain;
using StudentBase.Domain.Entities;

namespace StudentBase.MAUI.ViewModels
{
    public partial class ProgramCardViewModel : ViewModelBase
    {
        [ObservableProperty]
        public partial int Id { get; set; }

        [ObservableProperty]
        public partial string Specialty {  get; set; }

        [ObservableProperty]
        public partial string Qualification {  get; set; }

        [ObservableProperty]
        public partial FormsOfEducation FormOfEducation {  get; set; }

        [ObservableProperty]
        public partial TermsOfStudy DurationAfter9thGrade {  get; set; }

        [ObservableProperty]
        public partial TermsOfStudy DurationAfter11thGrade { get; set; }

        [ObservableProperty]
        public partial TermsOfStudy DurationOfCorrespondence { get; set; }

        [ObservableProperty]
        public partial decimal Cost { get; set; }

        [ObservableProperty]
        public partial StatusPrograms Status { get; set; }

        [ObservableProperty]
        public partial int GroupsCount { get; set; }

        [ObservableProperty]
        public partial int StudentsCount { get; set; }


        [RelayCommand]
        public static async Task ExitAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        // заполнение полей
        public void UploadData(ProgramEntity p)
        {
            Id = p.Id;
            Specialty = p.Specialty;
            Qualification = p.Qualification;
            DurationAfter9thGrade = p.DurationAfter9thGrade;
            DurationAfter11thGrade = p.DurationAfter11thGrade;
            DurationOfCorrespondence = p.DurationOfCorrespondence;
            Cost = p.CostPerSemester;
            Status = p.Status;
            GroupsCount = p.EducationalGroups.Count;
            StudentsCount = p.EducationalGroups.Sum(g => g.Students.Count);
        }
    }
}
