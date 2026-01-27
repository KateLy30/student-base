using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI;

public partial class ProgramPage : ContentPage
{
	private readonly ProgramPageViewModel _programPageViewModel;
	public ProgramPage(ProgramPageViewModel programPageViewModel)
	{
		InitializeComponent();
		_programPageViewModel = programPageViewModel;
		BindingContext = _programPageViewModel;
    }

    // подгрузка списка каждый раз при открытии окна
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _programPageViewModel.LoadAsync();
    }
}