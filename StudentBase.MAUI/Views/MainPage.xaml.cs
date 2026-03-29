using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _mainPageViewModel;
    public MainPage(MainPageViewModel mainPageViewModel )
    {
        InitializeComponent();
        _mainPageViewModel = mainPageViewModel;
        BindingContext = _mainPageViewModel;
    }
    // подгрузка списка каждый раз при открытии окна
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _mainPageViewModel.LoadSummaries();
        await _mainPageViewModel.LoadAsync();
    }
}
