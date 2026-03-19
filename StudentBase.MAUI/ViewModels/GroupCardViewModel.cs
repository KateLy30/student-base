using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;

namespace StudentBase.MAUI.ViewModels
{
    public class GroupCardViewModel : BaseViewModel
    {
        public AsyncCommand ExitCommand { get; }

        public GroupCardViewModel() =>
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
        private string? name;
        public string? Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private int programId;
        public int ProgramId
        {
            get => programId;
            set
            {
                programId = value;
                OnPropertyChanged();
            }
        }
        private string? programSpecialty;
        public string? ProgramSpecialty
        {
            get => programSpecialty;
            set
            {
                programSpecialty = value;
                OnPropertyChanged();
            }
        }
        private string? programQualification;
        public string? ProgramQualification
        {
            get => programQualification;
            set
            {
                programQualification = value;
                OnPropertyChanged();
            }
        }
        private DateTime dateOfCreation;
        public DateTime DateOfCreation
        {
            get => dateOfCreation;
            set
            {
                dateOfCreation = value;
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
        private StatusGroups status;
        public StatusGroups Status
        {
            get => status;
            set
            {
                status = value;
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
        public void UploadData(GroupEntity g)
        {
            Id = g.Id;
            Name = g.Name;
            DateOfCreation = g.DateOfCreation;
            ProgramId = g.ProgramId;
            ProgramSpecialty = g.EducationalProgram.Specialty;
            ProgramQualification = g.EducationalProgram.Qualification;
            DurationTraining = g.EducationalProgram.DurationTraining;
            Status = g.Status;
            StudentsCount = g.Students.Count;
        }
    }
}
