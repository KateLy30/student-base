using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;

namespace StudentBase.MAUI.ViewModels
{
    public class ProgramCardViewModel : BaseViewModel
    {
        public AsyncCommand ExitCommand { get; }

        public ProgramCardViewModel() =>
            ExitCommand = new AsyncCommand(ExitAsync);

        public async Task ExitAsync() =>
            await Shell.Current.Navigation.PopModalAsync();

        // поля для вывода
        private int id;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        private string? specialty;
        public string? Specialty
        {
            get => specialty;
            set
            {
                specialty = value;
                OnPropertyChanged();
            }
        }
        private string? qualification;
        public string? Qualification
        {
            get => qualification;
            set
            {
                qualification = value;
                OnPropertyChanged();
            }
        }
        private FormsOfEducation formOfEducation;
        public FormsOfEducation FormOfEducation
        {
            get => formOfEducation;
            set
            {
                formOfEducation = value;
                OnPropertyChanged();
            }
        }
        private TermsOfStudy durationTraining;
        public TermsOfStudy DurationTraining
        {
            get => durationTraining;
            set
            {
                durationTraining = value;
                OnPropertyChanged();
            }
        }
        private decimal cost;
        public decimal Cost
        {
            get => cost;
            set
            {
                cost = value;
                OnPropertyChanged();
            }
        }
        private StatusPrograms status;
        public StatusPrograms Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }
        private int groupsCount;
        public int GroupsCount
        {
            get => groupsCount;
            set
            {
                groupsCount = value;
                OnPropertyChanged();
            }
        }
        private int studentsCount;
        public int StudentsCount
        {
            get => studentsCount;
            set
            {
                studentsCount = value;
                OnPropertyChanged();
            }
        }

        // заполнение полей
        public void UploadData(ProgramEntity p)
        {
            Id = p.Id;
            Specialty = p.Specialty;
            Qualification = p.Qualification;
            DurationTraining = p.DurationTraining;
            Cost = p.CostPerSemester;
            Status = p.Status;
            GroupsCount = p.EducationalGroups.Count;
            StudentsCount = p.EducationalGroups.Sum(g => g.Students.Count);
        }
    }
}
