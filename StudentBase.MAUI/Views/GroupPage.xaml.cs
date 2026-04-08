using StudentBase.MAUI.ViewModels;
using System.Threading.Tasks;

namespace StudentBase.MAUI.Views;

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
		_groupPageViewModel.SearchText = null;
		await _groupPageViewModel.LoadAsync();
    }

}