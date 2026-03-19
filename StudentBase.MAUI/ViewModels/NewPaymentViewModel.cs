using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class NewPaymentViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private PaymentEntity _payment = new();
        public ObservableCollection<StudentEntity> Students { get; } = [];

        public AsyncCommand SaveCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public NewPaymentViewModel(IDataService dataService)
        {
            _dataService = dataService;

            SaveCommand = new AsyncCommand(SaveAsync, CanSave);
            CancelCommand = new AsyncCommand(() => Shell.Current.Navigation.PopModalAsync());
            _ = LoadStudentsAsync();
        }
        private bool CanSave()
        {
            return SelectedStudent != null && Semester != 0 && Amount > 0;
        }

        private StudentEntity? selectedStudent;
        public StudentEntity? SelectedStudent
        {
            get => selectedStudent;
            set
            {
                selectedStudent = value;    
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private int semester;
        public int Semester
        {
            get => semester;
            set
            {
                semester = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private decimal amount;
        public decimal Amount
        {
            get => amount;
            set
            {
                amount = value;
                OnPropertyChanged();
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime paymentDate;
        public DateTime PaymentDate
        {
            get => paymentDate;
            set
            {
                paymentDate = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadStudentsAsync()
        {
            var list = await _dataService.Students.GetAllAsync();
            Students.Clear();
            foreach(var s in list)
                Students.Add(s);
        }

        private async Task SaveAsync()
        {
            _payment.StudentId = SelectedStudent.Id;
            _payment.Student = SelectedStudent;
            _payment.PaidSemester = Semester;
            _payment.Amount = Amount;
            _payment.PaymentDate = PaymentDate;

            await _dataService.Receipts.CreateAsync(_payment);

            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
