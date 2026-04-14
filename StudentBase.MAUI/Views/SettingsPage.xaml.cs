using StudentBase.MAUI.ViewModels;
using System.Threading.Tasks;

namespace StudentBase.MAUI.Views;

public partial class SettingsPage : ContentPage
{
	private readonly SettingsPageViewModel _viewModel;
	public SettingsPage(SettingsPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await _viewModel.LoadAsync();
    }
}