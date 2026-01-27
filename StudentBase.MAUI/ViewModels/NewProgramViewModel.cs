using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class NewProgramViewModel : BaseViewModel
    {
        private readonly IProgramRepository _programRepository;
        private ProgramEntity _program = new();

        public ObservableCollection<FormsOfEducation> FormsOfEducationList { get; }
        public ObservableCollection<TermsOfStudy> TermsOfStudyList { get; }
        public ObservableCollection<LevelsOfEducation> LevelsOfEducationList { get; }
        public ObservableCollection<StatusPrograms> StatusList { get; }
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public NewProgramViewModel(IProgramRepository programRepository)
        {
            _programRepository = programRepository;
            SaveCommand = new AsyncCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(Specialty));
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopAsync());

            StatusList =  new ObservableCollection<StatusPrograms>(Enum.GetValues(typeof(StatusPrograms)).Cast<StatusPrograms>());
            LevelsOfEducationList = new ObservableCollection<LevelsOfEducation>(Enum.GetValues(typeof(LevelsOfEducation)).Cast<LevelsOfEducation>());
            TermsOfStudyList = new ObservableCollection<TermsOfStudy>(Enum.GetValues(typeof(TermsOfStudy)).Cast<TermsOfStudy>());
            FormsOfEducationList = new ObservableCollection<FormsOfEducation>(Enum.GetValues(typeof(FormsOfEducation)).Cast<FormsOfEducation>());
        }
        private string _title = "Добавление программы обучения";
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value; OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private string specialty;
        public string Specialty
        {
            get => specialty;
            set
            {
                if(specialty == value) return;
                specialty = value; OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private string qualification;
        public string Qualification
        {
            get => qualification;
            set
            {
                if(qualification == value) return;
                qualification = value; OnPropertyChanged();
            }
        }
        private FormsOfEducation formOfEducation;
        public FormsOfEducation FormOfEducation
        {
            get => formOfEducation;
            set
            {
                if(formOfEducation == value) return;
                formOfEducation = value; OnPropertyChanged();
            }
        }
        private FormsOfEducation selectedFormOfEducation;
        public FormsOfEducation SelectedFormOfEducation
        {
            get => selectedFormOfEducation;
            set
            {
                if(selectedFormOfEducation == value) return;
                selectedFormOfEducation = value; OnPropertyChanged();
            }
        }
        private TermsOfStudy durationTraining;
        public TermsOfStudy DurationTraining
        {
            get => durationTraining;
            set
            {
                if (durationTraining == value) return;
                durationTraining = value; OnPropertyChanged();
            }
        }
        private TermsOfStudy selectedTermOfStudy;
        public TermsOfStudy SelectedTermOfStudy
        {
            get => selectedTermOfStudy;
            set
            {
                if(selectedTermOfStudy == value) return;
                selectedTermOfStudy = value; OnPropertyChanged();
            }
        }
        private LevelsOfEducation educationLevel;
        public LevelsOfEducation EducationLevel
        {
            get => educationLevel;
            set
            {
                if(educationLevel == value) return;
                educationLevel = value; OnPropertyChanged();
            }
        }
        private LevelsOfEducation selectedLevelOfEducation;
        public LevelsOfEducation SelectedLevelOfEducation
        {
            get => selectedLevelOfEducation;
            set
            {
                if(selectedLevelOfEducation == value) return;
                selectedLevelOfEducation = value; OnPropertyChanged();
            }
        }
        private StatusPrograms status;
        public StatusPrograms Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value; OnPropertyChanged();
            }
        }
        private StatusPrograms selectedStatus;
        public StatusPrograms SelectedStatus
        {
            get => selectedStatus;
            set
            {
                if (selectedStatus == value) return;
                selectedStatus = value;
                OnPropertyChanged();
            }
        }
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Specialty))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите специальность.", "Ок");
                return;
            }
            _program.Specialty = Specialty;
            _program.Qualification = Qualification;
            _program.FormOfEducation = SelectedFormOfEducation;
            _program.DurationTraining = SelectedTermOfStudy;
            _program.EducationLevel = SelectedLevelOfEducation;
            _program.Status = SelectedStatus;
            if (_program.Id == 0)
                await _programRepository.CreateAsync(_program);
            else
                await _programRepository.UpdateAsync(_program);

            await Shell.Current.Navigation.PopAsync();
            if (Shell.Current?.CurrentPage?.BindingContext is ProgramPageViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }
        public void LoadFrom(ProgramEntity? p)
        {
            _program = p ?? new ProgramEntity();
            if (p == null || p.Id == 0)
                Title = "Добавление программы обучения";
            else
                Title = "Изменение данных программы обучение";

            Specialty = _program.Specialty!;
            Qualification = _program.Qualification!;
            SelectedFormOfEducation = _program.FormOfEducation;
            SelectedTermOfStudy = _program.DurationTraining;
            SelectedLevelOfEducation = _program.EducationLevel;
            SelectedStatus = _program.Status;
        }
    }
}
