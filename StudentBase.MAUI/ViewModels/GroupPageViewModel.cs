using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class GroupPageViewModel(IDataService dataService, Func<object> createNewGroupPage, Func<object> openCardGroupPage) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private readonly Func<object> _createNewGroupPage = createNewGroupPage;
        private readonly Func<object> _openCardGroupPage = openCardGroupPage;

        [ObservableProperty]
        public partial string? SearchText { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<GroupEntity> Groups { get; set; } = new ObservableCollection<GroupEntity>();

        [ObservableProperty]
        public partial GroupEntity SelectedGroup { get; set; }

        [ObservableProperty]
        public partial string NumberOfEntries { get; set; }

        partial void OnSearchTextChanged(string? value)
        {
            _ = LoadAsync();
        }


        [RelayCommand]
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _dataService.GroupService.GetAllGroupsAsync();
                if (list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Groups.Clear();
                foreach (var group in list)
                    Groups.Add(group);

                NumberOfEntries = $"Записей: {Groups.Count}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task DeleteAsync(GroupEntity? g)
        {
            if (g is null) return;
            if (g.Students?.Count != 0)
            {
                var ok = await Shell.Current.DisplayAlert("Подтверждение",
                    $"Количество студентов в группе: {g.Students?.Count}. Удалить группу {g.Name} со всеми студентами?", "Да", "Нет");
                if (!ok) return;
            }
            else if (g.Students.Count == 0)
            {
                var ok = await Shell.Current.DisplayAlert("Подтверждение",
                    $"Удалить группу {g.Name}. Студентов в этой группе нет.", "Да", "Нет");
                if (!ok) return;
            }
            var result = await _dataService.GroupService.DeleteGroupAsync(g.Id);
            if (!result.Success) 
            {
                await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
                return;
            }
            await LoadAsync();
        }

        [RelayCommand]
        public async Task AddAsync()
        {
            var page = (Page)_createNewGroupPage();
            if (page.BindingContext is NewGroupViewModel viewModel)
                await viewModel.LoadProgramsAsync();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        public async Task EditAsync(GroupEntity? g)
        {
            if (g is null) return;
            var page = (Page)_createNewGroupPage();
            if (page.BindingContext is NewGroupViewModel viewModel)
            {
                await viewModel.LoadProgramsAsync();
                viewModel.LoadFrom(g);
            }
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        public async Task OpenCardAsync(GroupEntity? g)
        {
            if (g is null) return;
            var page = (Page)_openCardGroupPage();
            if (page.BindingContext is GroupCardViewModel viewModel)
                viewModel.UploadData(g);
            await Shell.Current.Navigation.PushModalAsync(page);
        }
    }
}
