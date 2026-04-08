using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{   
    public partial class NewProgramViewModel(IDataService dataService) : ViewModelBase
    {
        /// <summary>
        /// ViewModel для формы редактирования сущности "Программа обучения"
        /// Если форма открывается для создания новой сущности, создается новый объект _program и заголовок по умолчанию
        /// Если форма открывается для редактирования существующей сущности, то при открытии форма заполняется данными сущности и меняется заголовок
        /// </summary>

        private readonly IDataService _dataService = dataService;
        private ProgramEntity _program = new();

        [ObservableProperty]
        public partial string? Title { get; set; } = "Добавление программы обучения";

        [ObservableProperty]
        public partial string Specialty { get; set; }

        [ObservableProperty]
        public partial string Qualification { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<TermsOfStudy> TermsOfStudyList { get; set; } = new ObservableCollection<TermsOfStudy>(Enum.GetValues<TermsOfStudy>().Cast<TermsOfStudy>());

        [ObservableProperty]
        public partial ObservableCollection<StatusPrograms> StatusList { get; set; } = new ObservableCollection<StatusPrograms>(Enum.GetValues<StatusPrograms>().Cast<StatusPrograms>());

        [ObservableProperty]
        public partial TermsOfStudy SelectedDurationAfter9thGrade { get; set; }

        [ObservableProperty]
        public partial TermsOfStudy SelectedDurationAfter11thGrade { get; set; }

        [ObservableProperty]
        public partial TermsOfStudy SelectedDurationOfCorrespondence { get; set; }

        [ObservableProperty]
        public partial StatusPrograms SelectedStatus { get; set; }

        [ObservableProperty]
        public partial decimal Cost { get; set; }


        // кнопка отмены
        [RelayCommand]
        private static async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }


        // кнопка сохранения
        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Specialty) || string.IsNullOrWhiteSpace(Qualification) || Cost == 0)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите данные", "ОK");
                return;
            }
            _program.Specialty = Specialty;
            _program.Qualification = Qualification;
            _program.DurationAfter9thGrade = SelectedDurationAfter9thGrade;
            _program.DurationAfter11thGrade = SelectedDurationAfter11thGrade;
            _program.DurationOfCorrespondence = SelectedDurationOfCorrespondence;
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
                SelectedDurationAfter9thGrade = _program.DurationAfter9thGrade;
                SelectedDurationAfter11thGrade = _program.DurationAfter11thGrade;
                SelectedDurationOfCorrespondence = _program.DurationOfCorrespondence;
                Cost = _program.CostPerSemester;
                SelectedStatus = _program.Status;
            }
        }
    }
}
