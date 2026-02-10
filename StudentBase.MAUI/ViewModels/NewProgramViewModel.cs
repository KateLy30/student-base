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

        // списки для Enums
        public ObservableCollection<FormsOfEducation> FormsOfEducationList { get; }
        public ObservableCollection<TermsOfStudy> TermsOfStudyList { get; }
        public ObservableCollection<StatusPrograms> StatusList { get; }

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public NewProgramViewModel(IProgramRepository programRepository)
        {
            _programRepository = programRepository;

            SaveCommand = new AsyncCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(Specialty));
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopAsync());

            // заполнение списков из Enums
            StatusList =  new ObservableCollection<StatusPrograms>(Enum.GetValues<StatusPrograms>().Cast<StatusPrograms>());
            TermsOfStudyList = new ObservableCollection<TermsOfStudy>(Enum.GetValues<TermsOfStudy>().Cast<TermsOfStudy>());
            FormsOfEducationList = new ObservableCollection<FormsOfEducation>(Enum.GetValues<FormsOfEducation>().Cast<FormsOfEducation>());
        }

        // заголовок окна
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

        // поля для ввода
        private string? specialty;
        public string? Specialty
        {
            get => specialty;
            set
            {
                if(specialty == value) return;
                specialty = value; OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private string? qualification;
        public string? Qualification
        {
            get => qualification;
            set
            {
                if(qualification == value) return;
                qualification = value; OnPropertyChanged();
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

        // кнопка сохранения
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Specialty) || string.IsNullOrWhiteSpace(Qualification))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите данные", "Ок");
                return;
            }
            _program.Specialty = Specialty;
            _program.Qualification = Qualification;
            _program.FormOfEducation = SelectedFormOfEducation;
            _program.DurationTraining = SelectedTermOfStudy;
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

        // заполнения формы ввода при редактировании
        // определение заголовка окна
        public void LoadFrom(ProgramEntity? p)
        {
            _program = p ?? new ProgramEntity();
            if (p == null || p.Id == 0)
                Title = "Добавление новой программы обучения";
            else
            {
                Title = "Изменение данных программы обучение";

                Specialty = _program.Specialty!;
                Qualification = _program.Qualification!;
                SelectedFormOfEducation = _program.FormOfEducation;
                SelectedTermOfStudy = _program.DurationTraining;
                SelectedStatus = _program.Status;
            }
        }
    }
}
