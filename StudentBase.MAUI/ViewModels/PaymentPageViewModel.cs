using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Extensions;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class PaymentPageViewModel(IDataService dataService, Func<object> createNewPaymentPage) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private readonly Func<object> _createNewPaymentPage = createNewPaymentPage;

        [ObservableProperty]
        public partial ObservableCollection<PaymentEntity> Payments { get; set; } = [];

        [ObservableProperty]
        public partial string TitleListPayments { get; set; } = "История платежей";

        [ObservableProperty]
        public partial string NumberOfEntries { get; set; }

        [ObservableProperty]
        public partial string? SearchText { get; set; }


        [RelayCommand]
        public async Task LoadAsync()
        {
            SearchText = null;
            try
            {
                IsBusy = true;
                var list = await _dataService.PaymentsService.GetAllPaymentsAsync();
                if (list == null) return;
                Payments.Clear();
                foreach (var payment in list)
                    Payments.Add(payment);

                NumberOfEntries = $"Записей: {Payments.Count}";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "Ок");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task AddReceiptAsync()
        {
            var page = (Page)_createNewPaymentPage();
            if (page.BindingContext is NewPaymentViewModel viewModel)
                await viewModel.LoadStudentsAsync();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        private async Task DeleteAsync(PaymentEntity? p)
        {
            if (p is null) return;
            var ok = await Shell.Current.DisplayAlert("Подтверждение", 
                $"Вы действительно хотите безвозвратно удалить квитанцию студента {p.Name} за {p.PaidSemester.ToSemesterDisplay(p.Student.DateOfReceipt)}.", "Да","Отмена");
            if (!ok) return;
            var result = await _dataService.PaymentsService.DeletePaymentAsync(p.Id);
            if (!result.Success)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "ОК");
                return;
            }
            else await Shell.Current.DisplayAlert("Успех", "Квитанция удалена", "OK");
            await LoadAsync();
        }

        [RelayCommand]
        public async Task FindReceiptAsync()
        {
            if (SearchText == null || SearchText == "") return;
            try
            {
                IsBusy = true;
                var studentsWithPayments = await _dataService.StudentService.GetAllStudentsWithPaymentsAsync();
                if(studentsWithPayments == null) return;

                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    studentsWithPayments = [.. studentsWithPayments.Where(p => (p.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Payments.Clear();
                foreach (var student in studentsWithPayments)
                {
                    foreach (var s in student.Payments)
                    {
                        Payments.Add(s);
                    }
                }

                NumberOfEntries = $"Записей: {Payments.Count}";
            }
            catch(Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "Ok");
            }
            finally 
            { 
                IsBusy = false; 
            }
        }
    }
}
