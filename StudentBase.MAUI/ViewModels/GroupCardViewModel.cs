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
        private DateOnly dateOfCreation;
        public DateOnly DateOfCreation
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


        // заполнение полей
        public void UploadData(GroupEntity g)
        {
            Id = g.Id;
            Name = g.Name;
            ProgramId = g.ProgramId;
            ProgramSpecialty = g.ProgramSpecialty;
            ProgramQualification = g.ProgramQualification;
            DateOfCreation = g.DateOfCreation;
            DurationTraining = g.DurationOfTraining;
            Status = g.Status;

        }
    }
}
