using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class PaymentPageViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly Func<object> _createNewPaymentPage;
        public ObservableCollection<PaymentEntity> Payments { get; } = [];

        public AsyncCommand AddReceiptCommand { get; }
        public AsyncCommand FindReceiptCommand { get; } 

        public PaymentPageViewModel(IDataService dataService, Func<object> createNewPaymentPage)
        {
            _dataService = dataService;
            _createNewPaymentPage = createNewPaymentPage;

            AddReceiptCommand = new AsyncCommand(AddReceiptAsync);
            FindReceiptCommand = new AsyncCommand(FindReceiptAsync);
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
            }
        }

        public async Task AddReceiptAsync()
        {
            var page = (Page)_createNewPaymentPage();
            await Shell.Current.Navigation.PushModalAsync(page);
        }
        public async Task FindReceiptAsync()
        {
            if (SearchText == null) return;
            var student = await _dataService.Students.GetByNameAsync(SearchText);
            var payments = await _dataService.Receipts.GetAllBuStudentAsync(student.Id);
            Payments.Clear();
            foreach(var p in payments)
                Payments.Add(p);
        }
    }
}
