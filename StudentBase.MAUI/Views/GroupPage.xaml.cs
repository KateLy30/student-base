
using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI;

public partial class GroupPage : ContentPage
{
	private readonly GroupPageViewModel _groupPageViewModel;
	public GroupPage(GroupPageViewModel groupPageViewModel)
	{
		InitializeComponent();
		_groupPageViewModel = groupPageViewModel;
		BindingContext = _groupPageViewModel;
	}

	 // подгрузка списка каждый раз при открытии окна
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _groupPageViewModel.LoadAsync();
    }

}