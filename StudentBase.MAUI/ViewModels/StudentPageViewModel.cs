using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;
using StudentBase.Infrastructure.EntityFramework.Repositories;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace StudentBase.MAUI.ViewModels
{
    public class StudentPageViewModel : BaseViewModel 
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly Func<object> _createNewStudentPage;
        public ObservableCollection<StudentEntity> Students { get; } = [];
        public ObservableCollection<GroupEntity> Filters { get; } = [];

        public StudentPageViewModel(IStudentRepository studentRepository,IGroupRepository groupRepository, Func<object> createNewStudentPage)
        {
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
            _createNewStudentPage = createNewStudentPage;

            LoadCommand = new AsyncCommand(LoadAsync);
            AddCommand = new AsyncCommand(AddAsync);
            EditCommand = new AsyncCommand(s => EditAsync(s as StudentEntity));
            DeleteCommand = new AsyncCommand(s => DeleteAsync(s as StudentEntity));
        }
        private GroupEntity selectedFilter;
        public GroupEntity SelectedFilter
        {
            get => selectedFilter;
            set
            {
                if(selectedFilter != value)
                {
                    selectedFilter = value;
                    OnPropertyChanged();
                    ApplyFilter();
                }
            }
        }
        private async Task ApplyFilter()
        {
            if(SelectedFilter == null) return;
            var list = await _studentRepository.GetAllByGroupIdAsync(SelectedFilter.Id);
            if(list == null) return;
            Students.Clear();
            foreach(var student in list) 
                Students.Add(student);
        }
        public async Task LoadGroupsAsync()
        {
            var groupsFromDb = await _groupRepository.GetAllAsync();
            if (groupsFromDb == null) return;
            Filters.Clear();
            foreach (var g in groupsFromDb)
                Filters.Add(g);
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
                var list = await _studentRepository.GetAllAsync();
                if(list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Students.Clear();
                foreach (var student in list)
                    Students.Add(student);
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

        public async Task DeleteAsync(StudentEntity? s)
        {
            if (s is null) return;
            var ok = await Shell.Current.DisplayAlert("Подтверждение", $"Удалить {s.Name}?", "Да", "Нет");
            if (!ok) return;
            await _studentRepository.DeleteAsync(s.Id);
            await LoadAsync();
        }
        public async Task AddAsync()
        {
            var page = (Page)_createNewStudentPage();
            await Shell.Current.Navigation.PushAsync(page);
        }
        public async Task EditAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_createNewStudentPage();
            if (page.BindingContext is NewStudentViewModel viewModel)
                viewModel.LoadFrom(s);
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
