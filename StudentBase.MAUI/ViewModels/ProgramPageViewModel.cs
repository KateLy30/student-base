using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class ProgramPageViewModel : BaseViewModel 
    {
        private readonly IProgramService _programService;
        private readonly Func<object> _createNewProgramPage;
        private readonly Func<object> _openProgramCardPage;
        public ObservableCollection<ProgramEntity> Programs { get; } = [];

        public AsyncCommand OpenProgramCardCommand { get; }
        public AsyncCommand LoadCommand { get; }
        public AsyncCommand AddCommand { get; }
        public AsyncCommand DeleteCommand { get; }
        public AsyncCommand EditCommand { get; }

        public ProgramPageViewModel(IProgramService programService, Func<object> createNewProgramPage, Func<object> openCardProgram)
        {
            _programService = programService;
            _createNewProgramPage = createNewProgramPage;
            _openProgramCardPage = openCardProgram;

            LoadCommand = new AsyncCommand(LoadAsync);
            AddCommand = new AsyncCommand(AddAsync);
            EditCommand = new AsyncCommand(p => EditAsync(p as ProgramEntity));
            DeleteCommand = new AsyncCommand(p => DeleteAsync(p as ProgramEntity));
            OpenProgramCardCommand = new AsyncCommand(p => OpenProgramCardAsync(p as ProgramEntity));
        }
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value) return;
                _isBusy = value; OnPropertyChanged();
            }
        }
        private string? numberOfEntries;
        public string? NumberOfEntries
        {
            get => numberOfEntries;
            set
            {
                numberOfEntries = value;
                OnPropertyChanged();
            }
        }

        // поле поиска
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

        // загрузка списка программ
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _programService.GetAllProgramsAsync(); 
                if (list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Specialty ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Programs.Clear();
                foreach (var program in list)
                    Programs.Add(program);

                NumberOfEntries = $"Записей: {Programs.Count}";
            }
            finally
            {
                IsBusy = false;
            }
        }
       
        public async Task DeleteAsync(ProgramEntity? p)
        {
            if (p is null) return;
            if (p.EducationalGroups.Count != 0)
            {
                var deleteProgramWithGroups = await Shell.Current.DisplayAlert("Предупреждение",
                    $"Количество групп, обучающихся по этой программе: {p.EducationalGroups.Count}. Удалить программу вместе с группами?",
                    "Да", "Нет");
                if (!deleteProgramWithGroups) return;
            }
            else if (p.EducationalGroups.Count == 0)
            {
                var ok = await Shell.Current.DisplayAlert("Подтверждение", 
                    $"Удалить специальность: {p.Specialty}, с квалификацией {p.Qualification}? Групп, обучающихся по этой программе нет.", 
                    "Да", "Нет");
                if (!ok) return;
            }
            var result = await _programService.DeleteProgramAsync(p.Id);
            if (!result.Success) await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
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
        public async Task OpenProgramCardAsync(ProgramEntity? p)
        {
            if(p is null) return;
            var page = (Page)_openProgramCardPage();
            if (page.BindingContext is ProgramCardViewModel viewModel)
                viewModel.UploadData(p);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
