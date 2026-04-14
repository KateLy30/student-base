using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Domain.Entities.Dynamic;
using StudentBase.Domain.Repositories;
using System.Collections.ObjectModel;

namespace StudentBase.MAUI.ViewModels
{
    public partial class SettingsPageViewModel(ICustomFieldRepository customFieldRepository, Func<object> openNewCustomFieldPage) : ViewModelBase
    {
        private readonly Func<object> _openNewCustomFieldPage = openNewCustomFieldPage;
        private readonly ICustomFieldRepository _customFieldRepository = customFieldRepository;

        [ObservableProperty]
        public partial ObservableCollection<CustomField> CustomFields { get; set; } = new();

        [ObservableProperty]
        public partial CustomField SelectedField { get; set; }


        [RelayCommand]
        private async Task DeleteAsync(CustomField? c)
        {
            try
            {
                if (c == null) return;
                await _customFieldRepository.DeleteAsync(c.Id);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task AddNewCustomFieldAsync()
        {
            var page = (Page)_openNewCustomFieldPage();
            await Shell.Current.Navigation.PushModalAsync(page);
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var list = await _customFieldRepository.GetAllAsync();
                if (list == null) return;
                CustomFields.Clear();
                foreach (var cf in list)
                {
                    CustomFields.Add(cf);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
