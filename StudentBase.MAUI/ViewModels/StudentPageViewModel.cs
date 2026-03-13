using Microsoft.EntityFrameworkCore;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class StudentPageViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly Func<object> _createNewStudentPage;
        private readonly Func<object> _openStudentCardPage;
        public ObservableCollection<StudentEntity> Students { get; } = [];
        public ObservableCollection<GroupEntity> ListGroupsFilter { get; } = [];
        public ObservableCollection<ProgramEntity> ListProgramsFilter { get; } = [];

        public StudentPageViewModel(IDataService dataService, Func<object> createNewStudentPage, Func<object> openStudentCardPage)
        {
            _dataService = dataService;
            _createNewStudentPage = createNewStudentPage;
            _openStudentCardPage = openStudentCardPage;

            LoadCommand = new AsyncCommand(LoadAsync);
            AddCommand = new AsyncCommand(AddAsync);
            EditCommand = new AsyncCommand(s => EditAsync(s as StudentEntity));
            DeleteCommand = new AsyncCommand(s => DeleteAsync());
            OpenCardCommand = new AsyncCommand(s => OpenCardAsync(s as StudentEntity));
        }

        // поле для вывода кол-ва записей
        private string? numberOfEntries;
        public string? NumberOfEntries
        {
            get => numberOfEntries;
            set
            {
                numberOfEntries = value;
                OnPropertyChanged();
            }
        }
        // индикатор загрузки
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }
        // поле поиска
        private string? searchText;
        public string? SearchText
        {
            get => searchText;
            set
            {
                if (searchText == value) return;
                searchText = value;
                OnPropertyChanged();
                _ = LoadAsync();
            }
        }
        // выбранный фильтр группы
        private GroupEntity? selectedGroupsFilter;
        public GroupEntity? SelectedGroupsFilter
        {
            get => selectedGroupsFilter;
            set
            {
                if (selectedGroupsFilter != value)
                {
                    selectedGroupsFilter = value;
                    OnPropertyChanged();

                    // Проверяем, выбран ли элемент "Все" (по ID = -1)
                    if (selectedGroupsFilter?.Id == -1)
                    {
                        // Показать все записи (без фильтрации по группе)
                        _ = LoadAsync();
                    }
                    else
                    {
                        // Применить фильтр по выбранной группе
                        _ = ApplyFilterGroups();
                    }
                }
            }
        }
        // выбранный фильтр программы
        private ProgramEntity? selectedProgramFilter;
        public ProgramEntity? SelectedProgramFilter
        {
            get => selectedProgramFilter;
            set
            {
                selectedProgramFilter = value;
                OnPropertyChanged();
                if(selectedProgramFilter?.Id == -1)
                    _ = LoadAsync();
                else 
                    _ = ApplyFilterPrograms();
            }
        }
        // выбранный из списка студент
        private StudentEntity? selectedStudent;
        public StudentEntity? SelectedStudent
        {
            get => selectedStudent;
            set
            {
                if (selectedStudent == value) return;
                selectedStudent = value; OnPropertyChanged();
            }
        }
        private async Task ApplyFilterGroups()
        {
            if (SelectedGroupsFilter == null) return;
            var list = await _dataService.Students.GetAllByGroupIdAsync(SelectedGroupsFilter.Id);
            if (list == null) return;
            Students.Clear();
            foreach (var student in list)
            {
                student.GroupName = student.EducationalGroup.Name;
                student.ProgramSpecialty = student.EducationalGroup.EducationalProgram.Specialty;
                student.ProgramQualification = student.EducationalGroup.EducationalProgram.Qualification;
                Students.Add(student);
            }

            NumberOfEntries = $"Записей: {Students.Count}";
        }
        private async Task ApplyFilterPrograms()
        {
            if (SelectedProgramFilter == null) return;
            var list = await _dataService.Students.GetAllByProgramIdAsync(SelectedProgramFilter.Id);
            if(list == null) return;
            Students.Clear();
            foreach (var student in list)
            {
                student.GroupName = student.EducationalGroup.Name;
                student.ProgramSpecialty = student.EducationalGroup.EducationalProgram.Specialty;
                student.ProgramQualification = student.EducationalGroup.EducationalProgram.Qualification;
                Students.Add(student);
            }

            NumberOfEntries = $"Записей: {Students.Count}";

        }
        public async Task LoadPickerFilterAsync()
        {
            var groupsFromDb = await _dataService.Groups.GetAllAsync();
            var programsFromDb = await _dataService.Programs.GetAllAsync();
            if (groupsFromDb != null)
            {
                ListGroupsFilter.Clear();
                var allGroupsItem = new GroupEntity
                {
                    Id = -1,  
                    Name = "Все группы"  
                };
                ListGroupsFilter.Add(allGroupsItem);
                foreach (var g in groupsFromDb)
                    ListGroupsFilter.Add(g);
                SelectedGroupsFilter = allGroupsItem;
            }
            if (programsFromDb != null)
            {
                ListProgramsFilter.Clear();
                var allProgramsItem = new ProgramEntity
                {
                    Id = -1,    
                    Specialty = "Все программы"
                };
                ListProgramsFilter.Add(allProgramsItem);
                foreach(var p in  programsFromDb)
                    ListProgramsFilter.Add(p);
                SelectedProgramFilter = allProgramsItem;
            }
        }
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _dataService.Students.GetAllAsync();
                if (list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Students.Clear();
                foreach (var student in list)
                {
                    student.GroupName = student.EducationalGroup.Name;
                    student.ProgramSpecialty = student.EducationalGroup.EducationalProgram.Specialty;
                    student.ProgramQualification = student.EducationalGroup.EducationalProgram.Qualification;
                    Students.Add(student);
                }

                NumberOfEntries = $"Записей: {Students.Count}";
            }
            finally
            {
                IsBusy = false;
            }
        }
        public AsyncCommand LoadCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }
        public AsyncCommand EditCommand { get; }
        public AsyncCommand OpenCardCommand { get; }

        public async Task OpenCardAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_openStudentCardPage();
            if (page.BindingContext is StudentCardViewModel viewModel)
                viewModel.UploadData(s);
            await Shell.Current.Navigation.PushAsync(page);
        }

        public async Task DeleteAsync()
        {
            if (SelectedStudent is null) return;
            var ok = await Shell.Current.DisplayAlert("Подтверждение", $"Удалить {SelectedStudent.Name}?", "Да", "Нет");
            if (!ok) return;
            await _dataService.Students.DeleteAsync(SelectedStudent.Id);
            await LoadAsync();
        }
        public async Task AddAsync()
        {
            var page = (Page)_createNewStudentPage();
            await Shell.Current.Navigation.PushAsync(page);
        }
        public async Task EditAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_createNewStudentPage();
            if (page.BindingContext is NewStudentViewModel viewModel)
                viewModel.LoadFrom(s);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
