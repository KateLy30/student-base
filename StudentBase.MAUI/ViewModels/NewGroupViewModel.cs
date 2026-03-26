using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class NewGroupViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private GroupEntity _group = new();
        public ObservableCollection<StatusGroups> StatusList { get; }
        public ObservableCollection<ProgramEntity> Programs { get; } = [];
        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public NewGroupViewModel(IDataService dataService)
        {
            _dataService = dataService;
            SaveCommand = new AsyncCommand(SaveAsync, CanSave);
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopModalAsync());

            StatusList = new ObservableCollection<StatusGroups>(Enum.GetValues<StatusGroups>().Cast<StatusGroups>());

            _ = LoadProgramsAsync();
        }
        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name) && SelectedProgram != null;
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
        private string? name;
        public string? Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value; OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        private DateTime dateOfCreation;
        public DateTime DateOfCreation
        {
            get => dateOfCreation;
            set
            {
                dateOfCreation = value;
                OnPropertyChanged();

            }
        }
        private ProgramEntity? selectedProgram;
        public ProgramEntity? SelectedProgram
        {
            get => selectedProgram;
            set
            {
                if (selectedProgram == value) return;
                selectedProgram = value; OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        public async Task LoadProgramsAsync()
        {
            var programsFromDb = await _dataService.ProgramService.GetAllProgramsAsync();
            if (programsFromDb == null) return;
            Programs.Clear();
            foreach (var p in programsFromDb)
                Programs.Add(p);
        }
        private StatusGroups selectedStatus;
        public StatusGroups SelectedStatus
        {
            get => selectedStatus;
            set
            {
                if (value == selectedStatus) return;
                selectedStatus = value; OnPropertyChanged();
            }
        }
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name) || SelectedProgram == null)
            {
                await Shell.Current.DisplayAlert("Ошибка", "Пожалуйста, введите данные.", "Ок");
                return;
            }
            _group.Name = Name;
            _group.DateOfCreation = DateOfCreation;
            _group.ProgramId = SelectedProgram.Id;
            _group.EducationalProgram = SelectedProgram;
            _group.Status = SelectedStatus;
            if (_group.Id == 0)
                await _dataService.GroupService.CreateGroupAsync(_group);
            else
                await _dataService.GroupService.UpdateGroupAsync(_group);

            await Shell.Current.Navigation.PopModalAsync();
            if (Shell.Current?.CurrentPage?.BindingContext is GroupPageViewModel viewModel)
            {
                await viewModel.LoadAsync();
            }
        }
        public void LoadFrom(GroupEntity? g)
        {
            _group = g ?? new GroupEntity();
            if (g == null || g.Id == 0)
                Title = "Добавление группы";
            else
            {
                Title = "Изменение данных группы";

                Name = _group.Name;
                DateOfCreation = _group.DateOfCreation;
                SelectedProgram = _group.EducationalProgram;
                SelectedStatus = _group.Status;
            }
        }
    }
}
