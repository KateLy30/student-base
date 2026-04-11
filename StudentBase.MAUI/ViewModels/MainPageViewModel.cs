using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class MainPageViewModel(IDataService dataService, Func<object> createNewTransferPage, Func<object> createNewPaymentPage) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private readonly Func<object> _createNewTransferPage = createNewTransferPage;
        private readonly Func<object> _createNewPaymentPage = createNewPaymentPage;

        [ObservableProperty]
        public partial ObservableCollection<StudentEntity> Debtors { get; set; } = [];

        [ObservableProperty]
        public partial ObservableCollection<StudentTransferEntity> Translations { get; set; } = [];

        [ObservableProperty]
        public partial int NumberOfStudents { get; set; }

        [ObservableProperty]
        public partial int NumberOfGroups { get; set; }

        [ObservableProperty]
        public partial int NumberOfPrograms { get; set; }

        [ObservableProperty]
        public partial int NumberOfOverduePayments { get; set; }

        [ObservableProperty]
        public partial StudentEntity SelectedDebtors   { get; set; }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                //var list = await _dataService.StudentService.GetAllStudentsAsync();
                //if (list == null) return;
                //Debtors.Clear();
                //foreach (var student in list)
                //    Debtors.Add(student);

                var list2 = await _dataService.StudentTransferService.GetAllStudentTransfersAsync();
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
            NumberOfStudents = (await _dataService.StudentService.GetStudentsCountAsync()).Count;
            NumberOfGroups = (await _dataService.GroupService.GetGroupsCountAsync()).Count;
            NumberOfPrograms = (await _dataService.ProgramService.GetProgramsCountAsync()).Count;
        }


        [RelayCommand]
        private async Task OpenArchiveAsync() { }  // TODO


        [RelayCommand]
        private async Task TransferStudentAsync()
        {
            var page = (Page)_createNewTransferPage();
            if (page.BindingContext is NewStudentTransferViewModel viewModel)
                await viewModel.LoadFormTransferPage();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        private async Task CreateReceiptAsync()
        {
            var page = (Page)_createNewPaymentPage();
            if (page.BindingContext is NewPaymentViewModel viewModel)
                await viewModel.LoadStudentsAsync();
            await Shell.Current.Navigation.PushModalAsync(page);
        }
    }
}
