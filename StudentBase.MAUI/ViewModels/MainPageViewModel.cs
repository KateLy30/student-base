using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.MAUI.Mvvm;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private readonly IDataService _dataService;
        private readonly Func<object> _createNewTransferPage;
        public ObservableCollection<StudentEntity> Debtors { get; } = [];
        public ObservableCollection<StudentEntity> Translations { get; } = [];
        public AsyncCommand ArchiveCommand { get; }
        public AsyncCommand TransferStudentCommand { get; }
        public AsyncCommand CreateReceiptCommand { get; }
        public MainPageViewModel(IDataService dataService, Func<object> createNewTransferPage)
        {
            _dataService = dataService;
            _createNewTransferPage = createNewTransferPage;

            ArchiveCommand = new AsyncCommand(OpenArchiveAsync);
            TransferStudentCommand = new AsyncCommand(TransferStudentAsync);
            CreateReceiptCommand = new AsyncCommand(CreateReceiptAsync);
        }

        private int numberOfStudents;
        public int NumberOfStudents
        {
            get => numberOfStudents; 
            set
            {
                numberOfStudents = value;
                OnPropertyChanged();
            }
        }
        private int numberOfGroups;
        public int NumberOfGroups
        {
            get => numberOfGroups;
            set
            {
                numberOfGroups = value;
                OnPropertyChanged();
            }
        }
        private int numberOfPrograms;
        public int NumberOfPrograms
        {
            get => numberOfPrograms; 
            set
            {
                numberOfPrograms = value;
                OnPropertyChanged();
            }
        }
        private int numberOfOverduePayments;
        public int NumberOfOverduePayments
        {
            get => numberOfOverduePayments;
            set
            {
                numberOfOverduePayments = value;
                OnPropertyChanged();
            }
        }


        private StudentEntity? selectedDebtors;
        public StudentEntity? SelectedDebtors
        {
            get => selectedDebtors;
            set
            {
                selectedDebtors = value;
                OnPropertyChanged();
            }
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
        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _dataService.Students.GetAllAsync();
                if (list == null) return;
                Debtors.Clear();
                foreach (var student in list)
                    Debtors.Add(student);

                var list2 = await _dataService.Students.GetAllTransferredStudentsAsync();
                if (list2 == null) return;
                Translations.Clear();
                foreach (var student in list2)
                    Translations.Add(student);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async Task LoadSummaries()
        {
            NumberOfStudents = await _dataService.Students.GetStudentsCountAsync();
            NumberOfGroups = await _dataService.Groups.GetGroupsCountAsync();
            NumberOfPrograms = await _dataService.Programs.GetProgramsCountAsync();
            NumberOfOverduePayments = await _dataService.Students.GetStudentsCountAsync();
        }
        private async Task OpenArchiveAsync() { }  // #TODO
        private async Task TransferStudentAsync()
        {
            var page = (Page)_createNewTransferPage();
            await Shell.Current.Navigation.PushAsync(page);
        }

        private async Task CreateReceiptAsync() { } // #TODO




    }
}
