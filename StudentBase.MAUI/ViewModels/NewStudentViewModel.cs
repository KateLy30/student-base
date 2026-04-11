using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class NewStudentViewModel(IDataService dataService) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private StudentEntity _student = new();

        [ObservableProperty]
        public partial bool CanChangedGroup { get; set; } = true;

        [ObservableProperty]
        public partial string Title { get; set; } = "Добавление студента";

        [ObservableProperty]
        public partial string Name { get; set; }

        [ObservableProperty]
        public partial string Phone { get; set; }

        [ObservableProperty]
        public partial DateTime DateOfReceipt { get; set; }

        [ObservableProperty]
        public partial DateTime DateOfBirth { get; set; }

        [ObservableProperty]
        public partial GroupEntity SelectedGroup { get; set; }

        [ObservableProperty]
        public partial StatusStudents SelectedStatus { get; set; }

        [ObservableProperty]
        public partial LevelsOfEducation SelectedLevel { get; set; }

        [ObservableProperty]
        public partial FormsOfEducation SelectedForm { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<StatusStudents> StatusList { get; set; } = new ObservableCollection<StatusStudents>(Enum.GetValues<StatusStudents>().Cast<StatusStudents>());

        [ObservableProperty]
        public partial ObservableCollection<LevelsOfEducation> EducationLevelsList { get; set; } = new ObservableCollection<LevelsOfEducation>(Enum.GetValues<LevelsOfEducation>().Cast<LevelsOfEducation>());

        [ObservableProperty]
        public partial ObservableCollection<FormsOfEducation> FormsOfEducationList { get; set; } = new ObservableCollection<FormsOfEducation>(Enum.GetValues<FormsOfEducation>().Cast<FormsOfEducation>());

        [ObservableProperty]
        public partial ObservableCollection<GroupEntity> Groups { get; set; } = new ObservableCollection<GroupEntity>();


        public async Task LoadGroupsAsync()
        {
            var groupsFromDb = await _dataService.GroupService.GetAllGroupsAsync();
            if (groupsFromDb == null) return;
            Groups.Clear();
            foreach (var g in groupsFromDb)
                Groups.Add(g);
        }

        [RelayCommand]
        private static async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Phone)
                || SelectedGroup == null)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите данные.", "Ок");
                return;
            }

            var program = SelectedGroup.EducationalProgram;

            switch (SelectedForm)
            {
                case FormsOfEducation.FullTime:

                    if (SelectedLevel == LevelsOfEducation.BasicGeneralEducation)
                        _student.DurationTraining = program.DurationAfter9thGrade;
                    else if (SelectedLevel == LevelsOfEducation.SecondaryGeneralEducation)
                        _student.DurationTraining = program.DurationAfter11thGrade;

                    break;

                case FormsOfEducation.Correspondence:

                    if (SelectedLevel == LevelsOfEducation.BasicGeneralEducation)
                    {
                        await Shell.Current.DisplayAlert("Ошибка", "Студент не может обучаться на заочной форме после 9 класаа.", "ОК");
                        return;
                    }
                    else if (SelectedLevel == LevelsOfEducation.SecondaryGeneralEducation)
                        _student.DurationTraining = program.DurationOfCorrespondence;

                    break;
            }

            _student.Name = Name;
            _student.Phone = Phone;
            _student.DateOfBirth = DateOfBirth;
            _student.DateOfReceipt = DateOfReceipt;
            _student.CurrentGroupId = SelectedGroup.Id;
            _student.EducationalGroup = SelectedGroup;
            _student.EducationLevel = SelectedLevel;
            _student.Status = SelectedStatus;
            _student.FormOfEducation = SelectedForm;

            if (_student.Id == 0)
            {
                var result = await _dataService.StudentService.CreateStudentAsync(_student);
                if (!result.Success)
                    await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
            }
            else
            {
                var result = await _dataService.StudentService.UpdateStudentAsync(_student);
                if (!result.Success)
                    await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
            }

            await Shell.Current.Navigation.PopModalAsync();
        }
        public void LoadFrom(StudentEntity? s)
        {
            _student = s ?? new StudentEntity();
            if (s == null || s.Id == 0)
                Title = "Добавление сотрудника";
            else
            {
                Title = "Изменение данных студента";
                CanChangedGroup = false;
                Name = _student.Name;
                Phone = _student.Phone!;
                DateOfBirth = _student.DateOfBirth;
                DateOfReceipt = _student.DateOfReceipt;
                SelectedLevel = _student.EducationLevel;
                SelectedForm = _student.FormOfEducation;
                SelectedGroup = Groups.FirstOrDefault(g => g.Id == _student.CurrentGroupId);
                SelectedStatus = _student.Status;
            }
        }
    }
}
