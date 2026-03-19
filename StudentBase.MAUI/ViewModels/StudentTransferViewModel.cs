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
        private readonly Func<object> _openStudentCardPage;
        public ObservableCollection<StudentTransferEntity> TransferredStudents { get; } = [];
        public StudentTransferViewModel(IDataService dataService, Func<object> createNewTransferPage, Func<object> openStudentCardPage)
        {
            _dataService = dataService;
            _createNewTransferPage = createNewTransferPage;
            _openStudentCardPage = openStudentCardPage;

            OpenCardCommand = new AsyncCommand(s => OpenCardAsync(s as StudentEntity));
            TransferCommand = new AsyncCommand(TransferAsync);
        }
        public AsyncCommand OpenCardCommand { get; }
        public AsyncCommand TransferCommand { get; }

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
                var list = await _dataService.Transfers.GetAllAsync();
                if (list == null) return;
                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    list = [.. list.Where(e => (e.Student.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
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
        private async Task OpenCardAsync(StudentEntity? s)
        {
            if (s is null) return;
            var page = (Page)_openStudentCardPage();
            if (page.BindingContext is StudentCardViewModel viewModel)
                await viewModel.UploadData(s);
            await Shell.Current.Navigation.PushModalAsync(page);
        }
    }
}
