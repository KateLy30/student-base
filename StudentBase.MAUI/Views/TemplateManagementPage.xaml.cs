using StudentBase.MAUI.ViewModels;

namespace StudentBase.MAUI.Views;

public partial class TemplateManagementPage : ContentPage
{
	private readonly TemplateManagementViewModel _viewModel;
	public TemplateManagementPage(TemplateManagementViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

    protected override async void OnAppearing()
    {
		await _viewModel.LoadAsync();
    }
}