using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class NewGroupViewModel(IDataService dataService) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private GroupEntity _group = new();

        [ObservableProperty]
        public partial ObservableCollection<StatusGroups> StatusList { get; set; } = new ObservableCollection<StatusGroups>(Enum.GetValues<StatusGroups>().Cast<StatusGroups>());

        [ObservableProperty]
        public partial ObservableCollection<ProgramEntity> Programs { get; set; } = new ObservableCollection<ProgramEntity>();

        [ObservableProperty]
        public partial bool ChangedProgram { get; set; } = true;

        [ObservableProperty]
        public partial int SelectedProgramId { get; set; }

        [ObservableProperty]
        public partial string Title { get; set; } = "Добавление группы";

        [ObservableProperty]
        public partial string Name { get; set; }

        [ObservableProperty]
        public partial DateTime DateOfCreation { get; set; }

        [ObservableProperty]
        public partial ProgramEntity SelectedProgram { get; set; }  

        [ObservableProperty]
        public partial StatusGroups SelectedStatus {  get; set; }


        [RelayCommand]
        private static async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        public async Task LoadProgramsAsync()
        {
            var programsFromDb = await _dataService.ProgramService.GetAllProgramsAsync();
            if (programsFromDb == null) return;
            Programs.Clear();
            foreach (var p in programsFromDb)
                Programs.Add(p);
        }


        [RelayCommand]
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
            {
                var result = await _dataService.GroupService.CreateGroupAsync(_group);
                if (!result.Success) await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
            }
            else
            {
                var result = await _dataService.GroupService.UpdateGroupAsync(_group);
                if (!result.Success) await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
            }

            await Shell.Current.Navigation.PopModalAsync();
        }
        public void LoadFrom(GroupEntity? g)
        {
            _group = g ?? new GroupEntity();
            if (g == null || g.Id == 0)
                Title = "Добавление группы";
            else
            {
                Title = "Изменение данных группы";

                ChangedProgram = false;

                Name = _group.Name;
                DateOfCreation = _group.DateOfCreation;
                SelectedProgram = Programs.FirstOrDefault(p => p.Id == _group.ProgramId);
                SelectedStatus = _group.Status;
            }
        }
    }
}
