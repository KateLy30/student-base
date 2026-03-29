using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class StudentTransfersPage : ContentPage
{
	private readonly StudentTransferViewModel _studentTransferViewModel;
	public StudentTransfersPage(StudentTransferViewModel studentTransferViewModel)
	{
		InitializeComponent();
		_studentTransferViewModel = studentTransferViewModel;
		BindingContext = _studentTransferViewModel;
	}

    // подгрузка списка каждый раз при открытии окна
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _studentTransferViewModel.LoadAsync();
    }
}