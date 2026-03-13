

using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;

namespace StudentBase.MAUI.ViewModels
{
    public class StudentCardViewModel : BaseViewModel
    {
        public AsyncCommand ExitCommand { get; }

        public StudentCardViewModel() =>
            ExitCommand = new AsyncCommand(ExitAsync);

        public async Task ExitAsync() =>
            await Shell.Current.Navigation.PopAsync();

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

        private string? phone;
        public string? Phone
        {
            get => phone;
            set
            {
                phone = value;
                OnPropertyChanged();
            }
        }

        private DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        private DateTime dateOfReceipt;
        public DateTime DateOfReceipt
        {
            get => dateOfReceipt;
            set
            {
                dateOfReceipt = value;
                OnPropertyChanged();
            }
        }

        private LevelsOfEducation educationLevel;
        public LevelsOfEducation EducationLevel
        {
            get => educationLevel;
            set 
            {
                educationLevel = value;
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

        private int groupId;
        public int GroupId
        {
            get => groupId;
            set
            {
                groupId = value;
                OnPropertyChanged();
            }
        }

        private string? groupName;
        public string? GroupName
        {
            get => groupName;
            set
            {
                groupName = value;
                OnPropertyChanged();
            }
        }

        private int programId;
        public int ProgramId
        {
            get => programId; set
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

        private StatusStudents status;
        public StatusStudents Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged();
            }
        }


        // заполнение полей
        public void UploadData(StudentEntity s)
        {
            Id = s.Id;
            Name = s.Name;
            Phone = s.Phone;
            DateOfBirth = s.DateOfBirth;
            DateOfReceipt = s.DateOfReceipt;
            EducationLevel = s.EducationLevel;
            FormOfEducation = s.FormOfEducation;
            GroupId = s.CurrentGroupId;
            GroupName = s.EducationalGroup.Name;
            ProgramId = s.EducationalGroup.ProgramId;
            ProgramSpecialty = s.EducationalGroup.EducationalProgram.Specialty;
            ProgramQualification = s.EducationalGroup.EducationalProgram.Qualification;
            Status = s.Status;
        }

    }
}
