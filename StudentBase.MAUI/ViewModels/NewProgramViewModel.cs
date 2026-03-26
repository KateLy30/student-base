using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class NewProgramViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private ProgramEntity _program = new();

        // списки для Enums
        public ObservableCollection<TermsOfStudy> TermsOfStudyList { get; }
        public ObservableCollection<StatusPrograms> StatusList { get; }

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public NewProgramViewModel(IDataService dataService)
        {
            _dataService = dataService;

            SaveCommand = new AsyncCommand(SaveAsync, CanSave);
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopModalAsync());

            // заполнение списков из Enums
            StatusList = new ObservableCollection<StatusPrograms>(Enum.GetValues<StatusPrograms>().Cast<StatusPrograms>());
            TermsOfStudyList = new ObservableCollection<TermsOfStudy>(Enum.GetValues<TermsOfStudy>().Cast<TermsOfStudy>());
        }
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Specialty) && !string.IsNullOrWhiteSpace(Qualification) && Cost > 0;
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
                if (specialty == value) return;
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
                if (qualification == value) return;
                qualification = value; OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private TermsOfStudy selectedTermOfStudy;
        public TermsOfStudy SelectedTermOfStudy
        {
            get => selectedTermOfStudy;
            set
            {
                if (selectedTermOfStudy == value) return;
                selectedTermOfStudy = value; OnPropertyChanged();
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
                SaveCommand.RaiseCanExecuteChanged();
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
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите данные", "ОK");
                return;
            }
            _program.Specialty = Specialty;
            _program.Qualification = Qualification;
            _program.DurationTraining = SelectedTermOfStudy;
            _program.CostPerSemester = Cost;
            _program.Status = SelectedStatus;

            if (_program.Id == 0)
            {
                var result = await _dataService.ProgramService.CreateProgramAsync(_program);
                if (!result.Success) await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");

            }
            else
            {
                var result = await _dataService.ProgramService.UpdateProgramAsync(_program);
                if (!result.Success) await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
            }

            await Shell.Current.Navigation.PopModalAsync();
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
                SelectedTermOfStudy = _program.DurationTraining;
                Cost = _program.CostPerSemester;
                SelectedStatus = _program.Status;
            }
        }
    }
}
