using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class NewStudentViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private StudentEntity _student = new();
        public ObservableCollection<StatusStudents> StatusList { get; }
        public ObservableCollection<LevelsOfEducation> EducationLevelsList { get; }
        public ObservableCollection<FormsOfEducation> FormsOfEducationList { get; }
        public ObservableCollection<GroupEntity> Groups { get; } = [];

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }

        public NewStudentViewModel(IDataService dataService)
        {
            _dataService = dataService;

            SaveCommand = new AsyncCommand(SaveAsync, CanSave);
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopAsync());

            StatusList = new ObservableCollection<StatusStudents>(Enum.GetValues<StatusStudents>().Cast<StatusStudents>());
            EducationLevelsList = new ObservableCollection<LevelsOfEducation>(Enum.GetValues<LevelsOfEducation>().Cast<LevelsOfEducation>());
            FormsOfEducationList = new ObservableCollection<FormsOfEducation>(Enum.GetValues<FormsOfEducation>().Cast<FormsOfEducation>());

            _ = LoadGroupsAsync();
        }
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) && Phone != null && SelectedGroup != null;
        }

        private string _title = "Добавление студента";
        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged();
            }
        }
        private string? name;
        public string? Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private string? phone;
        public string? Phone
        {
            get => phone;
            set
            {
                if (phone == value) return;
                phone = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private DateTime dateOfReceipt;
        public DateTime DateOfReceipt
        {
            get => dateOfReceipt;
            set
            {
                if (dateOfReceipt == value) return;
                dateOfReceipt = value;
                OnPropertyChanged();
            }
        }
        private DateTime dateOfBirth;
        public DateTime DateOfBirth
        {
            get => dateOfBirth;
            set
            {
                if (dateOfBirth == value) return;
                dateOfBirth = value;
                OnPropertyChanged();
            }
        }
        private bool isChecked;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                isChecked = value;
                OnPropertyChanged();
            }
        }

        GroupEntity? selectedGroup;
        public GroupEntity? SelectedGroup
        {
            get => selectedGroup;
            set
            {
                if (selectedGroup == value) return;
                selectedGroup = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public async Task LoadGroupsAsync()
        {
            var groupsFromDb = await _dataService.Groups.GetAllAsync();
            if (groupsFromDb == null) return;
            Groups.Clear();
            foreach (var g in groupsFromDb)
                Groups.Add(g);
        }

        private StatusStudents selectedStatus;
        public StatusStudents SelectedStatus
        {
            get => selectedStatus;
            set
            {
                if (selectedStatus == value) return;
                selectedStatus = value;
                OnPropertyChanged();
            }
        }

        private LevelsOfEducation selectedLevel;
        public LevelsOfEducation SelectedLevel
        {
            get => selectedLevel;
            set
            {
                if (selectedLevel == value) return;
                selectedLevel = value;
                OnPropertyChanged();
            }
        }
        private FormsOfEducation selectedForm;
        public FormsOfEducation SelectedForm
        {
            get => selectedForm;
            set
            {
                if (selectedForm == value) return;
                selectedForm = value;
                OnPropertyChanged();
            }
        }
        private async Task SaveAsync()
        {
            await CreateStudent();

            await Shell.Current.Navigation.PopAsync();
            if (Shell.Current?.CurrentPage?.BindingContext is StudentPageViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }
        public void LoadFrom(StudentEntity? s)
        {
            _student = s ?? new StudentEntity();
            if (s == null || s.Id == 0)
                Title = "Добавление сотрудника";
            else
                Title = "Изменение данных студента";

            Name = _student.Name;
            Phone = _student.Phone!;
            DateOfBirth = _student.DateOfBirth;
            DateOfReceipt = _student.DateOfReceipt;
            SelectedLevel = _student.EducationLevel;
            SelectedForm = _student.FormOfEducation;
            SelectedGroup = _student.EducationalGroup;
            SelectedStatus = _student.Status;
        }

        private async Task CreateStudent()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите имя.", "Ок");
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
            _student.GroupName = SelectedGroup.Name;
            _student.ProgramSpecialty = SelectedGroup.EducationalProgram.Specialty;
            _student.ProgramQualification = SelectedGroup.EducationalProgram.Qualification;

            if (_student.Id == 0)
                await _dataService.Students.CreateAsync(_student);
            else
                await _dataService.Students.UpdateAsync(_student);
        }

    }
}
