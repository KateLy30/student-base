using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class StudentTransferViewModel(IDataService dataService, Func<object> createNewTransferPage, Func<object> openStudentCardPage) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private readonly Func<object> _createNewTransferPage = createNewTransferPage;
        private readonly Func<object> _openStudentCardPage = openStudentCardPage;

        [ObservableProperty]
        public partial ObservableCollection<StudentTransferEntity> TransferredStudents { get; set; } = [];

        [ObservableProperty]
        public partial string? SearchText { get; set; }

        [ObservableProperty]
        public partial string NumberOfEntries { get; set; }


        // поиск
        [RelayCommand]
        public async Task FindTransferAsync()
        {
            if (SearchText == null || SearchText == "") return;
            try
            {
                IsBusy = true;
                var transfers = await _dataService.StudentTransferService.GetAllStudentTransfersAsync();
                if (transfers == null) return;

                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    transfers = [.. transfers.Where(p => (p.Name ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                TransferredStudents.Clear();
                foreach (var transfer in transfers)
                    TransferredStudents.Add(transfer);

                NumberOfEntries = $"Записей: {TransferredStudents.Count}";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        public async Task LoadAsync()
        {
            SearchText = null;
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _dataService.StudentTransferService.GetAllStudentTransfersAsync();
                if (list == null) return;
                TransferredStudents.Clear();
                foreach (var student in list)
                    TransferredStudents.Add(student);

                NumberOfEntries = $"Записей: {TransferredStudents.Count}";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка загрузки: {ex.Message}", "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task TransferAsync()
        {
            var page = (Page)_createNewTransferPage();
            if (page.BindingContext is NewStudentTransferViewModel viewModel)
                await viewModel.LoadFormTransferPage();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        private async Task OpenCardAsync(StudentTransferEntity? s)
        {
            if (s is null) return;
            var result = await _dataService.StudentService.GetStudentByIdAsync(s.StudentId);
            if (!result.Success || result.Student == null) return;
            var page = (Page)_openStudentCardPage();
            if (page.BindingContext is StudentCardViewModel viewModel)
                await viewModel.UploadData(result.Student);
            await Shell.Current.Navigation.PushModalAsync(page);

            // TODO  fix bags
        }

        [RelayCommand]
        private async Task DeleteAsync(StudentTransferEntity st)
        {
            if (st is null) return;
            var ok = await Shell.Current.DisplayAlert("Подтверждение", $"Удалить {st.Name}?", "Да", "Нет");
            if (!ok) return;
            var result = await _dataService.StudentTransferService.DeleteStudentTransferAsync(st.Id);
            if (!result.Success)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "ОК");
                return;
            }
            await LoadAsync();
        }
    }
}
