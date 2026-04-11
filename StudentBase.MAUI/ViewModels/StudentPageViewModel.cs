using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class StudentPageViewModel(IDataService dataService, Func<object> createNewStudentPage,
                                              Func<object> openStudentCardPage, Func<object> createNewTransferPage,
                                              Func<object> createNewPaymentPage) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private readonly Func<object> _createNewStudentPage = createNewStudentPage;
        private readonly Func<object> _openStudentCardPage = openStudentCardPage;
        private readonly Func<object> _createNewTransferPage = createNewTransferPage;
        private readonly Func<object> _createNewPaymentPage = createNewPaymentPage;

        [ObservableProperty]
        public partial ObservableCollection<StudentEntity> Students { get; set; } = new ObservableCollection<StudentEntity>();

        [ObservableProperty]
        public partial StudentEntity? SelectedStudent { get; set; }

        [ObservableProperty]
        public partial string? SearchText { get; set; }

        [ObservableProperty]
        public partial string NumberOfEntries { get; set; }

        [ObservableProperty]
        public partial GroupEntity SelectedGroupsFilter { get; set; }

        [ObservableProperty]
        public partial ProgramEntity SelectedProgramFilter { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<GroupEntity> ListGroupsFilter { get; set; } = new ObservableCollection<GroupEntity>();

        [ObservableProperty]
        public partial ObservableCollection<ProgramEntity> ListProgramsFilter { get; set; } = new ObservableCollection<ProgramEntity>();


        // поиск
        [RelayCommand]
        public async Task FindStudentAsync()
        {
            if (SearchText == null || SearchText == "") return;
            try
            {
                IsBusy = true;
                var students = await _dataService.StudentService.GetAllStudentsAsync();
                if (students == null) return;

                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    students = [.. students.Where(p => (p.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Students.Clear();
                foreach (var student in students)
                    Students.Add(student);

                NumberOfEntries = $"Записей: {Students.Count}";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        partial void OnSelectedGroupsFilterChanged(GroupEntity value)
        {
            if (value == null || value.Id == -1) return;
            _ = ApplyFilterAsync();
        }
        partial void OnSelectedProgramFilterChanged(ProgramEntity value)
        {
            if (value == null || value.Id == -1) return;
            _ = ApplyFilterAsync();
        }
        private async Task ApplyFilterAsync()
        {
            if (SelectedGroupsFilter.Id == -1 && SelectedProgramFilter.Id == -1)
            {
                await LoadAsync();
                return;
            }
            if (SelectedProgramFilter.Id != -1)
            {
                var list = await _dataService.StudentService.GetAllStudentsByProgramIdAsync(SelectedProgramFilter.Id);
                if (list == null) return;
                Students.Clear();
                foreach (var student in list)
                    Students.Add(student);

                NumberOfEntries = $"Записей: {Students.Count}";
            }
            if (SelectedGroupsFilter.Id != -1)
            {
                var list = await _dataService.StudentService.GetAllStudentsByGroupIdAsync(SelectedGroupsFilter.Id);
                if (list == null) return;
                Students.Clear();
                foreach (var student in list)
                    Students.Add(student);

                NumberOfEntries = $"Записей: {Students.Count}";
            }
        }

        //private async Task ApplyFilterGroups()
        //{
        //    if (SelectedGroupsFilter.Id == -1) await LoadAsync();
        //    var list = await _dataService.StudentService.GetAllStudentsByGroupIdAsync(SelectedGroupsFilter.Id);
        //    if (list == null) return;
        //    Students.Clear();
        //    foreach (var student in list)
        //    {
        //        student.GroupName = student.EducationalGroup.Name;
        //        student.ProgramSpecialty = student.EducationalGroup.EducationalProgram.Specialty;
        //        student.ProgramQualification = student.EducationalGroup.EducationalProgram.Qualification;
        //        Students.Add(student);
        //    }

        //    NumberOfEntries = $"Записей: {Students.Count}";
        //}
        //private async Task ApplyFilterPrograms()
        //{
        //    if (SelectedProgramFilter.Id == -1) await LoadAsync();
        //    var list = await _dataService.StudentService.GetAllStudentsByProgramIdAsync(SelectedProgramFilter.Id);
        //    if (list == null) return;
        //    Students.Clear();
        //    foreach (var student in list)
        //    {
        //        student.GroupName = student.EducationalGroup.Name;
        //        student.ProgramSpecialty = student.EducationalGroup.EducationalProgram.Specialty;
        //        student.ProgramQualification = student.EducationalGroup.EducationalProgram.Qualification;
        //        Students.Add(student);
        //    }

        //    NumberOfEntries = $"Записей: {Students.Count}";

        //}

        public async Task LoadPickerFilterAsync()
        {
            var groupsFromDb = await _dataService.GroupService.GetAllGroupsAsync();
            var programsFromDb = await _dataService.ProgramService.GetAllProgramsAsync();
            if (groupsFromDb != null)
            {
                var allGroupsItem = new GroupEntity
                {
                    Id = -1,
                    Name = "Все группы"
                };
                ListGroupsFilter.Clear();
                ListGroupsFilter.Add(allGroupsItem);
                foreach (var g in groupsFromDb)
                    ListGroupsFilter.Add(g);
                SelectedGroupsFilter = allGroupsItem;
            }
            if (programsFromDb != null)
            {
                var allProgramsItem = new ProgramEntity
                {
                    Id = -1,
                    Specialty = "Все программы"
                };
                ListProgramsFilter.Clear();
                ListProgramsFilter.Add(allProgramsItem);
                foreach (var p in programsFromDb)
                    ListProgramsFilter.Add(p);
                SelectedProgramFilter = allProgramsItem;
            }
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            SearchText = null;
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _dataService.StudentService.GetAllStudentsAsync();
                if (list == null) return;
                Students.Clear();
                foreach (var student in list)
                    Students.Add(student);

                NumberOfEntries = $"Записей: {Students.Count}";
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        public async Task OpenCardAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_openStudentCardPage();
            if (page.BindingContext is StudentCardViewModel viewModel)
                await viewModel.UploadData(s);
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        public async Task DeleteAsync(StudentEntity s)
        {
            if (s is null) return;
            var ok = await Shell.Current.DisplayAlert("Подтверждение", $"Удалить {s.Name}?", "Да", "Нет");
            if (!ok) return;
            var result = await _dataService.StudentService.DeleteStudentAsync(s.Id);
            if (!result.Success)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "ОК");
                return;
            }
            await LoadAsync();
        }

        [RelayCommand]
        public async Task AddAsync()
        {
            var page = (Page)_createNewStudentPage();
            if (page.BindingContext is NewStudentViewModel viewModel)
                await viewModel.LoadGroupsAsync();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        public async Task EditAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_createNewStudentPage();
            if (page.BindingContext is NewStudentViewModel viewModel)
            {
                await viewModel.LoadGroupsAsync();
                viewModel.LoadFrom(s);
            }
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        private async Task TransferAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_createNewTransferPage();
            if (page.BindingContext is NewStudentTransferViewModel viewModel)
            {
                await viewModel.LoadStudentsAsync();
                await viewModel.LoadGroupsAsync();
                viewModel.LoadFormStudentPage(s);
            }
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        private async Task PaymentAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_createNewPaymentPage();
            if (page.BindingContext is NewPaymentViewModel viewModel)
            {
                await viewModel.LoadStudentsAsync();
                viewModel.LoadForm(s);
            }
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        public async Task ImportListAsync()
        {
            // TODO
        }
    }
}
