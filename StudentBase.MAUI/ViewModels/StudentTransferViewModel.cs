using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class StudentTransferViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly Func<object> _createNewTransferPage;
        public ObservableCollection<StudentEntity> TransferredStudents { get; } = [];
        public StudentTransferViewModel(IDataService dataService, Func<object> createNewTransferPage)
        {
            _dataService = dataService;

            OpenCardCommand = new AsyncCommand(OpenCardAsync);
            TransferCommand = new AsyncCommand(TransferAsync);
            _createNewTransferPage = createNewTransferPage;
        }
        AsyncCommand OpenCardCommand { get; }
        AsyncCommand TransferCommand { get; }

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
                var list = await _dataService.Students.GetAllTransferredStudentsAsync();
                if (list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                TransferredStudents.Clear();
                foreach (var student in list)
                    TransferredStudents.Add(student);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task TransferAsync()
        {
            var page = (Page)_createNewTransferPage();
            await Shell.Current.Navigation.PushModalAsync(page);
        }
        private async Task OpenCardAsync()
        {
            // окно карты
        }
    }
}
