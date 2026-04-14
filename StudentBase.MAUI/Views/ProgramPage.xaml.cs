using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

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
        _programPageViewModel.SearchText = null;
        await _programPageViewModel.LoadAsync();
    }
}