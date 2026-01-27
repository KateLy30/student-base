using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class NewGroupViewModel : BaseViewModel 
    {
        private readonly IProgramRepository _programRepository;
        private readonly IGroupRepository _groupRepository;
        private GroupEntity _group = new();
        public ObservableCollection<StatusGroups> StatusList { get; }
        public ObservableCollection<ProgramEntity> Programs { get; } = [];
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public NewGroupViewModel(IProgramRepository programRepository, IGroupRepository groupRepository)
        {
            _programRepository = programRepository;
            _groupRepository = groupRepository;
            SaveCommand = new AsyncCommand(SaveAsync, () => !string.IsNullOrWhiteSpace(Name));
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopAsync());

            StatusList = new ObservableCollection<StatusGroups>(Enum.GetValues(typeof(StatusGroups)).Cast<StatusGroups>());
        }
        private string _title = "Добавление группы";
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
                if(name == value) return;
                name = value; OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private DateOnly dateOfCreation;
        public string DateOfCreation
        {
            get => dateOfCreation.ToString();
            set
            {
                if (DateOnly.TryParse(value, out DateOnly date))
                {
                    dateOfCreation = date; 
                    OnPropertyChanged();
                }
            }
        }
        private ProgramEntity selectedProgram;
        public ProgramEntity SelectedProgram
        {
            get => selectedProgram;
            set
            {
                if(selectedProgram ==  value) return;
                selectedProgram = value; OnPropertyChanged();
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
        private StatusGroups status;
        public StatusGroups Status
        {
            get => status;
            set
            {
                if(status == value) return;
                status = value; OnPropertyChanged();
            }
        }
        private StatusGroups selectedStatus;
        public StatusGroups SelectedStatus
        {
            get => selectedStatus;
            set
            {
                if(value == selectedStatus) return;
                selectedStatus = value; OnPropertyChanged();
            }
        }
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите название группы.", "Ок");
                return;
            }
            _group.Name = Name;
            _group.ProgramId = SelectedProgram.Id;
            _group.ProgramName = SelectedProgram.Specialty;
            if (DateOnly.TryParse(DateOfCreation, out var parsed))
                _group.DateOfCreation = parsed;
            else
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите корректную дату.", "Ок");

            _group.DurationOfTraining = SelectedProgram.DurationTraining;
            _group.Status = SelectedStatus;
            if (_group.Id == 0)
                await _groupRepository.CreateAsync(_group);
            else
                await _groupRepository.UpdateAsync(_group);

            await Shell.Current.Navigation.PopAsync();
            if (Shell.Current?.CurrentPage?.BindingContext is GroupPageViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }
        public async void LoadFrom(GroupEntity? g)
        {
            _group = g ?? new GroupEntity();
            if (g == null || g.Id == 0)
                Title = "Добавление группы";
            else
                Title = "Изменение данных группы";

            var program = await _programRepository.GetByIdAsync(g!.ProgramId);

            Name = _group.Name!;
            DateOfCreation = _group.DateOfCreation.ToString();
            SelectedProgram = program!;
            SelectedStatus = _group.Status;
        }
    }
}
