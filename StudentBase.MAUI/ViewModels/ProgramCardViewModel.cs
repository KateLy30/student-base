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

        // заполнение полей
        public void UploadData(ProgramEntity p)
        {
            Id = p.Id;
            Specialty = p.Specialty;
            Qualification = p.Qualification;
            FormOfEducation = p.FormOfEducation;
            DurationTraining = p.DurationTraining;
            Status = p.Status;
        }
    }
}
