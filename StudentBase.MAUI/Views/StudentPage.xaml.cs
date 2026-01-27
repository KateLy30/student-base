using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI;
public partial class StudentPage : ContentPage
{
    private readonly StudentPageViewModel _studentPageViewModel;
    public StudentPage(StudentPageViewModel studentPageViewModel)
    {
        InitializeComponent();
        _studentPageViewModel = studentPageViewModel;
        BindingContext = _studentPageViewModel;
    }

    // подгрузка списка каждый раз при открытии окна
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _studentPageViewModel.LoadAsync();
    }
   
}