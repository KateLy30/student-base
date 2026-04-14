using StudentBase.Domain;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using StudentBase.Domain.Entities.Dynamic;
using StudentBase.Domain.Repositories;

namespace StudentBase.MAUI.ViewModels
{
    public partial class NewCustomFieldViewModel(ICustomFieldRepository repository) : ViewModelBase
    {
        private readonly ICustomFieldRepository _repository = repository;
        private CustomField _field = new();

        [ObservableProperty]
        public partial string FieldName { get; set; }

        [ObservableProperty]
        public partial string DisplayName { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<FieldType> FieldTypeList { get; set; } = new ObservableCollection<FieldType>(Enum.GetValues<FieldType>().Cast<FieldType>());

        [ObservableProperty]
        public partial FieldType SelectedType { get; set; }

        [ObservableProperty]
        public partial string Title { get; set; } = "Добавление нового поля";


        [RelayCommand]
        private static async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            _field.FieldName = FieldName;
            _field.DisplayName = DisplayName;
            _field.FieldType = SelectedType;
            _field.CreatedAt = DateTime.Now;

            await _repository.CreateAsync(_field);

            await Shell.Current.Navigation.PopModalAsync();

        }

    }
}
