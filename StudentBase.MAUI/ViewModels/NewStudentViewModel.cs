using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class NewStudentViewModel : BaseViewModel 
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IProgramRepository _programRepository;
        private StudentEntity _student = new();
        public ObservableCollection<StatusStudents> StatusList { get; }
        public ObservableCollection<ProgramEntity> Programs { get; } = [];
        public ObservableCollection<GroupEntity> Groups { get; } = [];

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }

        public NewStudentViewModel(IStudentRepository studentRepository,
                                   IGroupRepository groupRepository,
                                   IProgramRepository programRepository)
        {
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
            SaveCommand = new AsyncCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(Name));
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopAsync());
            _programRepository = programRepository;

            StatusList = new ObservableCollection<StatusStudents>(Enum.GetValues(typeof(StatusStudents)).Cast<StatusStudents>());
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
        private string name;
        public string Name
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
        private string phone;
        public string Phone
        {
            get => phone;
            set
            {
                if (phone == value) return;
                phone = value;
                OnPropertyChanged();
            }
        }
        private string email;
        public string Email
        {
            get => email;
            set
            {
                if(email == value) return;
                email = value;
                OnPropertyChanged();
            }
        }
        private DateOnly dateOfBirth;
        public string DateOfBirth
        {
            get => dateOfBirth.ToString();
            set
           {
                if (DateOnly.TryParse(value, out var date))
                {
                    dateOfBirth = date;
                    OnPropertyChanged();
                }
            }
        }
        private DateOnly dateOfReceipt;
        public string DateOfReceipt
        {
            get => dateOfReceipt.ToString();
            set
            {
                if (DateOnly.TryParse(value, out var date))
                {
                    dateOfReceipt = date;
                    OnPropertyChanged();
                }
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
                OnPropertyChanged(nameof(SelectedGroup));
            }
        }
        public async Task LoadGroupsAsync()
        {
            var groupsFromDb = await _groupRepository.GetAllAsync();
            if(groupsFromDb == null) return;
            Groups.Clear();
            foreach (var g in groupsFromDb)
                Groups.Add(g);
        }


        private int? groupId;
        public int? GroupId
        {
            get => groupId;
            set
            {
                if (groupId == value) return;
                groupId = value;
                OnPropertyChanged();
            }
        }
        private string? groupName;
        public string? GroupName
        {
            get => groupName;
            set
            {
                if (groupName ==  value) return;
                groupName = value; 
                OnPropertyChanged();
            }
        }
        private int? programId;
        public int? ProgramId
        {
            get => programId;
            set
            {
                if (programId == value) return;
                programId = value;
                OnPropertyChanged();
            }
        }
        ProgramEntity? selectedProgram;
        public ProgramEntity? SelectedProgram
        {
            get => selectedProgram;
            set
            {
                if(selectedProgram == value) return;
                selectedProgram = value;
                OnPropertyChanged();
            }
        }
        public async Task LoadProgramsAsync()
        {
            var programsFromDb = await _programRepository.GetAllAsync();
            if (programsFromDb == null) return;
            Programs.Clear();
            foreach (var p in programsFromDb)
                Programs.Add(p);
        }

        private string? programSpecialty;
        public string? ProgramSpecialty
        {
            get => programSpecialty;
            set
            {
                if (programSpecialty  == value) return;
                programSpecialty = value;
                OnPropertyChanged();
            }
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
        private StatusStudents status;
        public StatusStudents Status
        {
            get => status;
            set
            {
                if(status == value) return;
                status = value;
                OnPropertyChanged();
            }
        }
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите имя.", "Ок");
                return;
            }
            _student.Name = Name;
            _student.Phone = Phone;
            _student.Email = Email;

            if (DateOnly.TryParse(DateOfBirth, out var dateBirth) && DateOnly.TryParse(DateOfReceipt, out var dateReciept))
            {
                _student.DateOfBirth = dateBirth;
                _student.DateOfReceipt = dateReciept;
            }
            else
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите корректную дату.", "Ок");

            _student.GroupId = SelectedGroup?.Id;
            _student.GroupName = SelectedGroup?.Name;
            _student.ProgramId = SelectedProgram?.Id;
            _student.ProgramSpecialty = SelectedProgram?.Specialty;
            _student.Status = SelectedStatus;
            if (_student.Id == 0)
                await _studentRepository.CreateAsync(_student);
            else
                await _studentRepository.UpdateAsync(_student);

            await Shell.Current.Navigation.PopAsync();
            if (Shell.Current?.CurrentPage?.BindingContext is StudentPageViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }
        public async void LoadFrom(StudentEntity? s)
        {
            _student = s ?? new StudentEntity();
            if (s == null || s.Id == 0)
                Title = "Добавление сотрудника";
            else
                Title = "Изменение данных студента";

            var group = await _groupRepository.GetByIdAsync((int)_student.GroupId!);
            var program = await _programRepository.GetByIdAsync((int)_student.ProgramId!);

            Name = _student.Name!;
            Phone = _student.Phone!;
            Email = _student.Email!;
            DateOfBirth = _student.DateOfBirth.ToString();
            DateOfReceipt = _student.DateOfReceipt.ToString();
            SelectedGroup = group;
            SelectedProgram = program;
            SelectedStatus = _student.Status;
        }

    }
}
