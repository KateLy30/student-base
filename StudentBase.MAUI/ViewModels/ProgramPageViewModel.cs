using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Application.Interfaces;
using StudentBase.Domain.Entities;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class ProgramPageViewModel(IDataService dataService, Func<object> createNewProgramPage, Func<object> openCardProgram) : ViewModelBase
    {
        private readonly IDataService _dataService = dataService;
        private readonly Func<object> _createNewProgramPage = createNewProgramPage;
        private readonly Func<object> _openProgramCardPage = openCardProgram;

        [ObservableProperty]
        public partial string? SearchText { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<ProgramEntity> Programs { get; set; } = new ObservableCollection<ProgramEntity>();

        [ObservableProperty]
        public partial ProgramEntity? SelectedProgram { get; set; }

        [ObservableProperty]
        public partial string? NumberOfEntries { get; set; }


        // поиск
        [RelayCommand]
        public async Task FindProgramAsync()
        {
            if (SearchText == null || SearchText == "") return;
            try
            {
                IsBusy = true;
                var programs = await _dataService.ProgramService.GetAllProgramsAsync();
                if (programs == null) return;

                var filter = (SearchText ?? string.Empty).Trim();
                if (filter.Length > 0)
                {
                    programs = [.. programs.Where(p => (p.Specialty ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase) 
                                                    || (p.Qualification ?? "").Contains(filter, StringComparison.OrdinalIgnoreCase))];
                }
                Programs.Clear();
                foreach (var program in programs)
                    Programs.Add(program);

                NumberOfEntries = $"Записей: {Programs.Count}";
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


        // загрузка списка программ
        [RelayCommand]
        public async Task LoadAsync()
        {
            SearchText = null;
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _dataService.ProgramService.GetAllProgramsAsync(); 
                if (list == null) return;
                Programs.Clear();
                foreach (var program in list)
                    Programs.Add(program);

                NumberOfEntries = $"Записей: {Programs.Count}";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка загрузки", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }



        [RelayCommand]
        public async Task DeleteAsync(ProgramEntity? p)
        {
            if (p is null) return;
            if (p.EducationalGroups?.Count != 0)
            {
                var deleteProgramWithGroups = await Shell.Current.DisplayAlert("Предупреждение",
                    $"Количество групп, обучающихся по этой программе: {p.EducationalGroups?.Count}. Удалить программу вместе с группами?",
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
            var result = await _dataService.ProgramService.DeleteProgramAsync(p.Id);
            if (!result.Success) await Shell.Current.DisplayAlert("Ошибка", $"{result.ErrorMessage}", "OK");
            await LoadAsync();
        }

        [RelayCommand]
        public async Task AddAsync()
        {
            var page = (Page)_createNewProgramPage();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        public async Task EditAsync(ProgramEntity? p)
        {
            if (p is null) return;
            var page = (Page)_createNewProgramPage();
            if (page.BindingContext is NewProgramViewModel viewModel)
                viewModel.LoadFrom(p);
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        [RelayCommand]
        public async Task OpenProgramCardAsync(ProgramEntity? p)
        {
            if(p is null) return;
            var page = (Page)_openProgramCardPage();
            if (page.BindingContext is ProgramCardViewModel viewModel)
                viewModel.UploadData(p);
            await Shell.Current.Navigation.PushModalAsync(page);
        }
    }
}
