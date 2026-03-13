
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public  class NewStudentTransferViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private StudentTransferEntity _transfer = new();

        public ObservableCollection<StudentEntity> Students { get; } = [];
        public ObservableCollection<GroupEntity> Groups { get; } = [];
        public NewStudentTransferViewModel(IDataService dataService)
        {
            _dataService = dataService;

            SaveCommand = new AsyncCommand(SaveAsync, () => SelectedStudent != null && SelectedGroup != null);
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopAsync());
            _ = LoadGroupsAsync();
            _ = LoadStudentsAsync();
        }
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }

        private string _title = "Добавление перевода";
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
        private StudentEntity? selectedStudent;
        public StudentEntity? SelectedStudent
        {
            get => selectedStudent;
            set
            {
                if(selectedStudent == value) return;    
                selectedStudent = value;
                OnPropertyChanged();   
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private GroupEntity? selectedGroup;
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
        private DateTime transferDate;
        public DateTime TransferDate
        {
            get =>  transferDate;
            set
            {
                if (transferDate == value) return;
                transferDate = value;
                OnPropertyChanged();
            }
        }
        public async Task LoadStudentsAsync()
        {
            var studentsFromDb = await _dataService.Students.GetAllAsync();
            if (studentsFromDb == null) return;
            Students.Clear();
            foreach (var student in studentsFromDb) 
                Students.Add(student);
        }
        public async Task LoadGroupsAsync()
        {
            var groupsFromDB = await _dataService.Groups.GetAllAsync();
            if (groupsFromDB == null) return;
            Groups.Clear();
            foreach(var group in groupsFromDB)
                Groups.Add(group);
        }

        private async Task SaveAsync()
        {
            _transfer.StudentId = SelectedStudent.Id;
            _transfer.Student = SelectedStudent;
            _transfer.FromGroupId = SelectedStudent.CurrentGroupId;
            _transfer.FromGroup = SelectedStudent.EducationalGroup;
            _transfer.ToGroupId = SelectedGroup.Id;
            _transfer.ToGroup = SelectedGroup;
            _transfer.EnrollmentDate = TransferDate;


            await Shell.Current.Navigation.PopAsync();
            if (Shell.Current?.CurrentPage?.BindingContext is StudentTransferViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }
    }
}
