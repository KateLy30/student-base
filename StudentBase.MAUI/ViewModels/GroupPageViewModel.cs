using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class GroupPageViewModel : BaseViewModel 
    {
        private readonly IDataService _dataService;
        private readonly Func<object> _createNewGroupPage;
        private readonly Func<object> _createCardGroupPage;
        public ObservableCollection<GroupEntity> Groups { get; } = [];

        public GroupPageViewModel(IDataService dataService, Func<object> createNewGroupPage, Func<object> createCardGroupPage)
        {
            _dataService = dataService;
            _createNewGroupPage = createNewGroupPage;
            _createCardGroupPage = createCardGroupPage;

            LoadCommand = new AsyncCommand(LoadAsync);
            AddCommand = new AsyncCommand(AddAsync);
            EditCommand = new AsyncCommand(g => EditAsync(g as GroupEntity));
            DeleteCommand = new AsyncCommand(g => DeleteAsync(g as GroupEntity));
            OpenCardCommand = new AsyncCommand(g => OpenCardAsync(g as GroupEntity));
        }

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
        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText == value) return;
                _searchText = value;
                OnPropertyChanged();
                _ = LoadAsync();
            }
        }
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _dataService.Groups.GetAllAsync();
                if (list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Groups.Clear();
                foreach (var group in list)
                    Groups.Add(group);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public AsyncCommand OpenCardCommand { get; }
        public AsyncCommand LoadCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }
        public AsyncCommand EditCommand { get; }
        public async Task DeleteAsync(GroupEntity? g)
        {
            if (g is null) return;
            var ok = await Shell.Current.DisplayAlert("Подтверждение", $"Удалить {g.Name}?", "Да", "Нет");
            if (!ok) return;
            await _dataService.Groups.DeleteAsync(g.Id);
            await LoadAsync();
        }
        public async Task AddAsync()
        {
            var page = (Page)_createNewGroupPage();
            await Shell.Current.Navigation.PushAsync(page);
        }
        public async Task EditAsync(GroupEntity? g)
        {
            if (g is null) return;
            var page = (Page)_createNewGroupPage();
            if (page.BindingContext is NewGroupViewModel viewModel)
                viewModel.LoadFrom(g);
            await Shell.Current.Navigation.PushAsync(page);
        }
        public async Task OpenCardAsync(GroupEntity? g)
        {
            if (g is null) return;
            var page = (Page)_createCardGroupPage();
            if (page.BindingContext is GroupCardViewModel viewModel)
                viewModel.UploadData(g);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
