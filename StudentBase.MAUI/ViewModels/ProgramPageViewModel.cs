using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class ProgramPageViewModel : BaseViewModel 
    {
        private readonly IProgramRepository _programRepository;
        private readonly Func<object> _createNewProgramPage;
        public ObservableCollection<ProgramEntity> Programs { get; } = [];
        public ProgramPageViewModel(IProgramRepository programRepository, Func<object> createNewProgramPage)
        {
            _programRepository = programRepository;
            _createNewProgramPage = createNewProgramPage;

            LoadCommand = new AsyncCommand(LoadAsync);
            AddCommand = new AsyncCommand(AddAsync);
            EditCommand = new AsyncCommand(p => EditAsync(p as ProgramEntity));
            DeleteCommand = new AsyncCommand(p => DeleteAsync(p as ProgramEntity));
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
                var list = await _programRepository.GetAllAsync();
                if (list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Specialty ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Programs.Clear();
                foreach (var program in list)
                    Programs.Add(program);
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
        public async Task DeleteAsync(ProgramEntity? p)
        {
            if (p is null) return;
            var ok = await Shell.Current.DisplayAlert("Подтверждение", $"Удалить {p.Specialty}?", "Да", "Нет");
            if (!ok) return;
            await _programRepository.DeleteAsync(p.Id);
            await LoadAsync();
        }
        public async Task AddAsync()
        {
            var page = (Page)_createNewProgramPage();
            await Shell.Current.Navigation.PushAsync(page);
        }
        public async Task EditAsync(ProgramEntity? p)
        {
            if (p is null) return;
            var page = (Page)_createNewProgramPage();
            if (page.BindingContext is NewProgramViewModel viewModel)
                viewModel.LoadFrom(p);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
