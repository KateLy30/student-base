using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class NewStudentTransferViewModel(IDataService dataService) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private StudentTransferEntity _transfer = new();

        [ObservableProperty]
        public partial ObservableCollection<StudentEntity> Students { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<GroupEntity> Groups { get; set; } = [];

        [ObservableProperty]
        public partial bool ChangedStudent { get; set; } = true;

        [ObservableProperty]
        public partial string Title { get; set; } = "Добавление перевода";

        [ObservableProperty]
        public partial StudentEntity? SelectedStudent { get; set; }

        [ObservableProperty]
        public partial GroupEntity? SelectedGroup { get; set; }

        [ObservableProperty]
        public partial DateTime TransferDate { get; set; } = DateTime.Now;

        [ObservableProperty]
        public partial bool IsStudentSelected { get; set; }

        [ObservableProperty]
        public partial bool IsGroupSelected { get; set; }


        // текст кнопки создания
        public string TransferButtonText
        {
            get
            {
                if (!IsStudentSelected) return "Выберите студента";
                if (!IsGroupSelected) return "Выберите группу";
                if(SelectedStudent != null && SelectedGroup != null)
                    if (SelectedStudent.CurrentGroupId == SelectedGroup.Id) return "Выберите другую группу";
                return "Перевести";
            }
        }

        // можно ли создать перевод
        public bool CanCreateTransfer
        {
            get
            {
                if (SelectedStudent == null || !IsStudentSelected) return false;
                if (SelectedGroup == null || !IsGroupSelected) return false;
                if (SelectedStudent.CurrentGroupId == SelectedGroup.Id) return false;
                return true;
            }
        }

        // при выборе студента
        partial void OnSelectedStudentChanged(StudentEntity? value)
        {
            IsStudentSelected = value != null;
            if (SelectedGroup != null && SelectedStudent != null)
            {
                if (SelectedGroup.Id == SelectedStudent.CurrentGroupId)
                {
                    Shell.Current.DisplayAlert("Ошибка", "Выбрана текущая группа студента. Выберите другую группу для перевода.", "OK");
                }
            }
            OnPropertyChanged(nameof(CanCreateTransfer));
            OnPropertyChanged(nameof(TransferButtonText));
        }

        // при выборе группы
        partial void OnSelectedGroupChanged(GroupEntity? value)
        {
            IsGroupSelected = value != null;
            if (SelectedGroup != null && SelectedStudent != null)
            {
                if (SelectedGroup.Id == SelectedStudent.CurrentGroupId)
                {
                    Shell.Current.DisplayAlert("Ошибка", "Выбрана текущая группа студента. Выберите другую группу для перевода.", "OK");
                }
            }
            OnPropertyChanged(nameof(CanCreateTransfer));
            OnPropertyChanged(nameof(TransferButtonText));
        }


        [RelayCommand]
        private static async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        public async Task LoadStudentsAsync()
        {
            try
            {
                IsBusy = true;
                var students = await _dataService.StudentService.GetAllStudentsAsync();
                if (students == null) return;
                Students.Clear();
                foreach (var student in students)
                    Students.Add(student);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка загрузки", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task LoadGroupsAsync()
        {
            try
            {
                IsBusy = true;
                var groupsFromDB = await _dataService.GroupService.GetAllGroupsAsync();
                if (groupsFromDB == null) return;
                Groups.Clear();
                foreach (var group in groupsFromDB)
                    Groups.Add(group);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка загрузки", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            try
            {
                IsBusy = true;
                if (!CanCreateTransfer) return;
                if (SelectedStudent == null || SelectedGroup == null)
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Не выбран студент или группа", "ОК");
                    return;
                }
                var transfer = new StudentTransferEntity
                {
                    StudentId = SelectedStudent.Id,
                    Student = SelectedStudent,
                    FromGroupId = SelectedStudent.CurrentGroupId,
                    FromGroup = SelectedStudent.EducationalGroup,
                    ToGroupId = SelectedGroup.Id,
                    ToGroup = SelectedGroup,
                    EnrollmentDate = TransferDate
                };
                var result = await _dataService.StudentTransferService.CreateStudentTransferAsync(transfer);
                if (!result.Success)
                {
                    await Shell.Current.DisplayAlert("Ошибка", result.ErrorMessage, "OK");
                    return;
                }
                else
                    await Shell.Current.DisplayAlert("", result.Message, "OK");
                await Shell.Current.Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка",ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void LoadFormStudentPage(StudentEntity? s)
        {
            if (s == null) return;
            ChangedStudent = false;
            SelectedStudent = Students.FirstOrDefault(st => st.Id == s.Id);
        }

        public async Task LoadFormTransferPage()
        {
            await LoadGroupsAsync();
            await LoadStudentsAsync();
        }
    }
}
