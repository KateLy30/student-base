using CommunityToolkit.Mvvm.ComponentModel;

namespace StudentBase.MAUI.ViewModels
{
    public abstract partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _hasError;
    }
}
